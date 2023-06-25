using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AVA.Web.Mvc.Models.Admin;
using AVA.Core.Entities;
using AVA.UI.Helpers.Base;
using AVA.UI.Helpers.Common;
using System.Net;
using AVA.UI.Helpers.Controller;
using AVA.UI.Helpers.CustomAttribute;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using AVA.Web.Mvc.Models;
using AVA.UI.Helpers.FileUploadManagment;
using System.Web.Script.Serialization;
using static AVA.Web.Mvc.Models.Admin.BaseViewModel;

namespace AVA.Web.Mvc.Controllers.Admin
{
    //Optimized
    public class SocietyExecutorController : BaseController
    {
        // GET: Admin/SocietyExecutor
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.SocietyExecutor)]
        public ActionResult SocietyExecutor()
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
            if ((Request.QueryString["id"] == null || Request.QueryString["id"] == "undefined"))
            {
                return PartialView("~/Areas/Admin/Views/Csmi/SocietyExecutor.cshtml", new SocietyExecutorViewModel() { ObjectState = ObjectState.Insert, Languages = Languages, LanguageID = Languages[0].Value });
            }
            else
            {

                var selectedID = int.Parse(Request.QueryString["id"]);
                var selectedSocietyExecutor = context.SocietyExecutors
                    .Include(SocietyExecutors => SocietyExecutors.Language)
                    .Where(item => item.ID.Equals(selectedID))
                    .Select(item => new
                    {
                        item.ID,
                        item.DegreeLevel,
                        item.Email,
                        item.Family,
                        LanguageID = item.Language.ID,
                        item.Name,
                        item.PersonalImageUrl,
                        item.Position,
                        item.ResumeUrl,
                        item.CreateDate,
                    })
                    .SingleOrDefault();
                return PartialView("~/Areas/Admin/Views/Csmi/SocietyExecutor.cshtml", new SocietyExecutorViewModel()
                {
                    ID = selectedSocietyExecutor.ID,
                    CreateDateConverted = CommonHelper.DateAndTimes.GetPersianDate(selectedSocietyExecutor.CreateDate),
                    Name = selectedSocietyExecutor.Name,
                    Family = selectedSocietyExecutor.Family,
                    DegreeLevel = selectedSocietyExecutor.DegreeLevel,
                    Position = selectedSocietyExecutor.Position,
                    Email = selectedSocietyExecutor.Email,
                    Languages = Languages,
                    LanguageID = selectedSocietyExecutor.LanguageID.ToString(),
                    PersonalImageUrl = string.IsNullOrEmpty(selectedSocietyExecutor.PersonalImageUrl) ? "" : Url.Content(selectedSocietyExecutor.PersonalImageUrl),
                    ResumeUrl = string.IsNullOrEmpty(selectedSocietyExecutor.ResumeUrl) ? "" : Url.Content(selectedSocietyExecutor.ResumeUrl),
                    ObjectState = ObjectState.Update
                });
            }
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.SocietyExecutor)]
        public ActionResult SocietyExecutorManagement()
        {
            ViewBag.GridID = "SocietyExecutorManagementGrid";
            return PartialView("~/Areas/Admin/Views/Csmi/SocietyExecutorManagement.cshtml");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.SocietyExecutor)]
        public ActionResult GetSocietyExecutorPartial()
        {
            return new GridViewPartialController().GridViewPartial<SocietyExecutorViewModel>(Bind(), "SocietyExecutor", "GetSocietyExecutorPartial", "SocietyExecutorManagementGrid", "ID");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.SocietyExecutor)]
        public ActionResult ExportTo(int ActionType)
        {
            return new GridViewPartialController().ExportTo<SocietyExecutorViewModel>(Bind(), ActionType, "SocietyExecutorManagementGrid");
        }
        List<SocietyExecutorViewModel> Bind()
        {
            var SocietyExecutors = new List<SocietyExecutorViewModel>();
            var context = new ApplicationDbContext();
            var model = new object[0];
            var DbSocietyExecutors = context.SocietyExecutors
                .Include(SocietyExecutor => SocietyExecutor.Language)
                .Select(item => new
                {
                    item.ID,
                    item.DegreeLevel,
                    item.Email,
                    item.Family,
                    LanguageName = item.Language.Name,
                    item.Name,
                    item.PersonalImageUrl,
                    item.Position,
                    item.ResumeUrl,
                    item.Enable,
                    item.CreateDate,
                })
                .ToList().OrderByDescending(item => item.CreateDate);
            foreach (var item in DbSocietyExecutors)
            {
                SocietyExecutors.Add(new SocietyExecutorViewModel()
                {
                    ID = item.ID,
                    CreateDateConverted = CommonHelper.DateAndTimes.GetPersianDate(item.CreateDate),
                    Name = item.Name,
                    Family = item.Family,
                    DegreeLevel = item.DegreeLevel,
                    ResumeUrl = string.IsNullOrEmpty(item.ResumeUrl) ? "" : Url.Content(item.ResumeUrl),
                    ResumeUrlText = "فایل رزومه",
                    Position = item.Position,
                    Email = item.Email,
                    Enable=item.Enable,
                    PersonalImageUrl = item.PersonalImageUrl,
                    LanguageName = item.LanguageName
                });
            }
            if (SocietyExecutors.Count() == 0)
            {
                SocietyExecutors.Add(new SocietyExecutorViewModel());
            }
            return SocietyExecutors;
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.SocietyExecutor)]
        public ActionResult HtmlEditorPartial()
        {
            return new HtmlEditorPartialController().HtmlEditorView("HtmlEditor", "OnHtmlChangedChanged", "OnInit");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.SocietyExecutor)]
        [AjaxValidateAntiForgeryTokenAttribute]
        public ActionResult SubmitSocietyExecutor(string viewmodel)
        {
            try
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                SocietyExecutorViewModel vm = jss.Deserialize<SocietyExecutorViewModel>(viewmodel);
                if (ModelState.IsValid)
                {
                    try
                    {
                        string PersonalImage = "";
                        string Resume = "";
                        if (Request.Files.Count > 0)
                        {
                            var file = Request.Files[CommonHelper.ExposeProperty.GetPropertyInfo(vm, m => m.PersonalImage).Name];

                            if (file != null && file.ContentLength > 0)
                            {
                                PersonalImage = FileUploadManagment.UploadFile("~/assets/img/Attach/SocietyExecutor/", "", file, FileUploadManagment.AppFileType.Image);
                                if (PersonalImage == "")
                                {
                                    return ControllerHelper.ErrorResult("فرمت یا حجم فایل صحیح نمی باشد");
                                }
                            }
                            file = Request.Files[CommonHelper.ExposeProperty.GetPropertyInfo(vm, m => m.Resume).Name];

                            if (file != null && file.ContentLength > 0)
                            {
                                Resume = FileUploadManagment.UploadFile("~/assets/img/Attach/SocietyExecutor/", "", file, FileUploadManagment.AppFileType.Document);
                                if (Resume == "")
                                {
                                    return ControllerHelper.ErrorResult("فرمت یا حجم فایل صحیح نمی باشد");
                                }
                            }
                        }
                        if (vm.ObjectState == ObjectState.Insert)
                        {
                            var file = Request.Files[CommonHelper.ExposeProperty.GetPropertyInfo(vm, m => m.PersonalImage).Name];
                            if (file == null && vm.ObjectState == ObjectState.Insert)
                            {
                                return ControllerHelper.ErrorResult("تصویر پرسنلی را وارد کنید");
                            }
                            file = Request.Files[CommonHelper.ExposeProperty.GetPropertyInfo(vm, m => m.Resume).Name];
                            if (file == null && vm.ObjectState == ObjectState.Insert)
                            {
                                return ControllerHelper.ErrorResult(" فایل رزومه را وارد کنید");
                            }

                        }
                        int langID = int.Parse(vm.LanguageID);
                        if (vm.ObjectState == ObjectState.Insert) // insert
                        {

                            var context = new ApplicationDbContext();
                            try
                            {
                                context.SocietyExecutors.Add(new SocietyExecutor()
                                {
                                    Name = vm.Name,
                                    Family = vm.Family,
                                    Email = vm.Email,
                                    Position = vm.Position,
                                    DegreeLevel = vm.DegreeLevel,
                                    ResumeUrl = Resume,
                                    Enable = true,
                                    PersonalImageUrl = PersonalImage,
                                    Language = context.Languages.Where(lang => lang.ID == langID).SingleOrDefault(),
                                    CreateDate = DateTime.Now,
                                    CreatorUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault(),
                                });
                                context.SaveChanges();

                            }
                            catch (Exception ex)
                            {
                                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.Message);
                            }

                        }
                        else if (vm.ObjectState == ObjectState.Update)//Update
                        {
                            var context = new ApplicationDbContext();
                            var selectedModel = context.SocietyExecutors
                                .Where(item => item.ID == vm.ID).Single();
                            try
                            {
                                if (selectedModel != null)
                                {
                                    selectedModel.ID = vm.ID;
                                    selectedModel.Name = vm.Name;
                                    selectedModel.Family = vm.Family;
                                    selectedModel.Email = vm.Email;
                                    selectedModel.Position = vm.Position;
                                    selectedModel.DegreeLevel = vm.DegreeLevel;
                                    if (Resume != "")
                                    {
                                        selectedModel.ResumeUrl = Resume;
                                    }
                                    if (PersonalImage != "")
                                    {
                                        selectedModel.PersonalImageUrl = PersonalImage;
                                    }
                                    selectedModel.Language = context.Languages.Where(lang => lang.ID == langID).SingleOrDefault();
                                    selectedModel.LastModifyUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault();
                                    selectedModel.LastModifyDate = DateTime.Now;
                                }
                                context.SaveChanges();

                            }
                            catch (Exception ex)
                            {
                                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.Message);
                            }

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
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.SocietyExecutor)]
        public ActionResult DeleteSocietyExecutor(string[] ids)
        {
            var context = new ApplicationDbContext();
            var dbContextTransaction = context.Database.BeginTransaction();
            try
            {
                foreach (var id in ids)
                {
                    int ID = int.Parse(id);
                    var selectedItem = context.SocietyExecutors
                        .Where(item => item.ID == ID).Single();
                    if (selectedItem != null)
                    {
                        try
                        {
                            System.IO.File.Delete(selectedItem.ResumeUrl);
                        }
                        catch
                        {

                        }
                        try
                        {
                            System.IO.File.Delete(selectedItem.PersonalImageUrl);
                        }
                        catch
                        {

                        }
                        context.SocietyExecutors.Remove(selectedItem);
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
        [HttpPost]
        [AjaxValidateAntiForgeryTokenAttribute]
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Article)]
        public ActionResult ActiveSocietyExecutor(string[] ids)
        {
            var context = new ApplicationDbContext();
            try
            {
                foreach (var id in ids)
                {
                    int CurrentID = int.Parse(id);
                    var selectedItem = context.SocietyExecutors
                        .Where(item => item.ID == CurrentID).Single();
                    if (selectedItem != null)
                    {
                        selectedItem.Enable = !selectedItem.Enable;
                    }
                }
                context.SaveChanges();
                return ControllerHelper.SuccessResult("با موفقیت ثبت شد");
            }
            catch (Exception ex)
            {
                return ControllerHelper.ErrorResult("بروز خطای سیستمی");
            }


        }
    }
}