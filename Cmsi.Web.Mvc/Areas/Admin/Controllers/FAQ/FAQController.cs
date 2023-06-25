using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AVA.Web.Mvc.Models.Admin;
using AVA.Core.Entities;
using AVA.UI.Helpers.Base;
using AVA.UI.Helpers.Common;
using System.Net;
using AVA.UI.Helpers.Controller;
using AVA.UI.Helpers.CustomAttribute;
using static AVA.Web.Mvc.Models.Admin.BaseViewModel;

namespace AVA.Web.Mvc.Controllers.Admin
{
    //Optimized
    public class FAQController : BaseController
    {
        // GET: Admin/FAQ
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.FAQ)]
        public ActionResult FAQ()
        {
            var context = new ApplicationDbContext();
            List<DropDownVm> Languages = new List<DropDownVm>();
            var Dblanguage = context.Languages
                .Select(lang => new
                {
                    lang.ID,
                    lang.Name
                })
                .ToList();
            foreach (var lang in Dblanguage)
            {
                Languages.Add(new DropDownVm()
                {
                    Text = lang.Name,
                    Value = lang.ID.ToString()
                });
            }
            if (Request.QueryString["id"] == null || Request.QueryString["id"] == "undefined")
            {
                return PartialView("~/Areas/Admin/Views/FAQ/FAQ.cshtml", new FAQViewModel() { ObjectState = ObjectState.Insert,Languages= Languages, LanguageID = Languages[0].Value });
            }
            else
            {
                var selectedID = int.Parse(Request.QueryString["id"]);
                var selectedFAQ = context.FAQs.Where(item => item.ID.Equals(selectedID))
                    .Select(item => new
                    {
                        item.ID,
                        item.Answer,
                        item.Question,
                        item.Priority,
                        item.Language,
                        item.CreateDate
                    })
                    .SingleOrDefault();
                return PartialView("~/Areas/Admin/Views/FAQ/FAQ.cshtml", new FAQViewModel()
                {
                    ID = selectedFAQ.ID,
                    CreateDateConverted = CommonHelper.DateAndTimes.GetPersianDate(selectedFAQ.CreateDate),
                    Answer = selectedFAQ.Answer,
                    Question = selectedFAQ.Question,
                    Priority = selectedFAQ.Priority,
                    LanguageID = selectedFAQ.Language.ID.ToString(),
                    LanguageName = selectedFAQ.Language.Name,
                    Languages= Languages,
                    ObjectState = ObjectState.Update
                });
            }
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.FAQ)]
        public ActionResult FAQManagement()
        {
            ViewBag.GridID = "FAQManagementGrid";
            return PartialView("~/Areas/Admin/Views/FAQ/FAQManagement.cshtml");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.FAQ)]
        public ActionResult GetFAQPartial()
        {
            return new GridViewPartialController().GridViewPartial<FAQViewModel>(Bind(), "FAQ", "GetFAQPartial", "FAQManagementGrid", "ID");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.FAQ)]
        public ActionResult ExportTo(int ActionType)
        {
            return new GridViewPartialController().ExportTo<FAQViewModel>(Bind(), ActionType, "FAQManagementGrid");
        }
        List<FAQViewModel> Bind()
        {
            var FAQList = new List<FAQViewModel>();
            var context = new ApplicationDbContext();
            var model = new object[0];
            var DbFAQ = context.FAQs
                .Select(item => new
                {
                    item.ID,
                    item.Answer,
                    item.Question,
                    item.Priority,
                    item.Language,
                    item.CreateDate,
                    item.Enable
                })
            .ToList()
            .OrderByDescending(item => item.CreateDate);
            foreach (var item in DbFAQ)
            {
                FAQList.Add(new FAQViewModel()
                {
                    ID = item.ID,
                    Answer = item.Answer,
                    Question = item.Question,
                    Priority = item.Priority,
                    LanguageID = item.Language.ID.ToString(),
                    LanguageName = item.Language.Name,
                    Enable = item.Enable,
                    CreateDateConverted = CommonHelper.DateAndTimes.GetPersianDate(item.CreateDate)
                });
            }
            if (FAQList.Count() == 0)
            {
                FAQList.Add(new FAQViewModel());
            }
            return FAQList;
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.FAQ)]
        public ActionResult HtmlEditorPartial()
        {
            return new HtmlEditorPartialController().HtmlEditorView("HtmlEditor", "OnHtmlChangedChanged", "OnInit");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.FAQ)]
        [HttpPost]
        [AjaxValidateAntiForgeryTokenAttribute]
        public ActionResult SubmitFAQ(FAQViewModel viewmodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        int langID = int.Parse(viewmodel.LanguageID);
                        if (viewmodel.ObjectState == ObjectState.Insert) // insert
                        {
                            var context = new ApplicationDbContext();
                            context.FAQs.Add(new FAQ()
                            {
                                Answer = viewmodel.Answer,
                                Question = viewmodel.Question,
                                Priority = viewmodel.Priority,
                                Language = context.Languages.Where(lang => lang.ID == langID).SingleOrDefault(),
                                Enable = true,
                                CreateDate = DateTime.Now,
                                CreatorUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault(),
                            });
                            context.SaveChanges();

                        }
                        else if (viewmodel.ObjectState == ObjectState.Update)//Update
                        {
                            var context = new ApplicationDbContext();
                            FAQ selectedModel = context.FAQs.Where(item => item.ID == viewmodel.ID).Single();
                            if (selectedModel != null)
                            {
                                selectedModel.Answer = viewmodel.Answer;
                                selectedModel.Question = viewmodel.Question;
                                selectedModel.Priority = viewmodel.Priority;
                                selectedModel.Language = context.Languages.Where(lang => lang.ID == langID).SingleOrDefault();
                                selectedModel.LastModifyUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault();
                                selectedModel.LastModifyDate = DateTime.Now;
                            }
                            context.SaveChanges();
                        }
                    }
                    catch (Exception ex)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.Message);
                    }
                    return ControllerHelper.SuccessResult("با موفقیت ثبت شد");
                }
                else
                {
                    return ControllerHelper.FormNotValid(ModelState);
                }

            }
            catch (Exception ex)
            {
                //return ControllerHelper.ErrorResult("بروز خطای سیستمی");
                return ControllerHelper.ErrorResult("بروز خطای سیستمی");
            }
        }
        [HttpPost]
        [AjaxValidateAntiForgeryTokenAttribute]
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.FAQ)]
        public ActionResult ActiveFAQ(string[] ids)
        {
            var context = new ApplicationDbContext();
            try
            {
                foreach (var id in ids)
                {
                    int CurrentID = int.Parse(id);
                    var selectedItem = context.FAQs.Where(item => item.ID == CurrentID).Single();
                    if (selectedItem != null)
                        selectedItem.Enable = !selectedItem.Enable;
                }
                context.SaveChanges();
                return ControllerHelper.SuccessResult("با موفقیت ثبت شد");
            }
            catch (Exception ex)
            {
                return ControllerHelper.ErrorResult("بروز خطای سیستمی");
            }


        }
        [HttpPost]
        [AjaxValidateAntiForgeryTokenAttribute]
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.FAQ)]
        public ActionResult DeleteFAQ(string[] ids)
        {
            var context = new ApplicationDbContext();
            var dbContextTransaction = context.Database.BeginTransaction();
            try
            {
                foreach (var id in ids)
                {
                    int ID = int.Parse(id);
                    var selectedItem = context.FAQs
                        .Where(item => item.ID == ID).Single();
                    if (selectedItem != null)
                    {
                        context.FAQs.Remove(selectedItem);
                    }

                }
                context.SaveChanges();
                dbContextTransaction.Commit();
                return ControllerHelper.SuccessResult("با موفقیت ثبت شد");
            }
            catch (Exception ex)
            {
                dbContextTransaction.Rollback();
                return ControllerHelper.ErrorResult("بروز خطای سیستمی");
            }


        }
    }
}