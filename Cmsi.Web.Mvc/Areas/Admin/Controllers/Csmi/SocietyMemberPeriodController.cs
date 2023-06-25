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
    public class SocietyMemberPeriodController : BaseController
    {
        // GET: Admin/SocietyMemberPeriod
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.SocietyMemberPeriod)]
        public ActionResult SocietyMemberPeriod()
        {
            var context = new ApplicationDbContext();
            List<DropDownVm> language = new List<DropDownVm>();
            var Dblanguage = context.Languages
                .Select(lang => new
                {
                    lang.ID,
                    lang.Name
                })
                .ToList();
            foreach (var lang in Dblanguage)
            {
                language.Add(new DropDownVm()
                {
                    Text = lang.Name,
                    Value = lang.ID.ToString()
                });
            }
            if (Request.QueryString["id"] == null || Request.QueryString["id"] == "undefined")
            {
                return PartialView("~/Areas/Admin/Views/Csmi/SocietyMemberPeriod.cshtml", new SocietyMemberPeriodViewModel() { ObjectState = ObjectState.Insert});
            }
            else
            {

                var selectedID = int.Parse(Request.QueryString["id"]);
                var selectedSocietyMemberPeriod = context.SocietyMemberPeriods
                    .Where(item => item.ID.Equals(selectedID))
                    .Select(item => new
                    {
                        item.ID,
                        item.StartDate,
                        item.EndDate,
                        item.CreateDate,
                    })
                    .SingleOrDefault();
                return PartialView("~/Areas/Admin/Views/Csmi/SocietyMemberPeriod.cshtml", new SocietyMemberPeriodViewModel()
                {
                    ID = selectedSocietyMemberPeriod.ID,
                    CreateDateConverted = CommonHelper.DateAndTimes.GetPersianDate(selectedSocietyMemberPeriod.CreateDate),
                    StartDate = CommonHelper.DateAndTimes.GetPersianDate(selectedSocietyMemberPeriod.StartDate),
                    EndDate = CommonHelper.DateAndTimes.GetPersianDate(selectedSocietyMemberPeriod.EndDate),
                    ObjectState = ObjectState.Update
                });
            }
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.SocietyMemberPeriod)]
        public ActionResult SocietyMemberPeriodManagement()
        {
            ViewBag.GridID = "SocietyMemberPeriodManagementGrid";
            return PartialView("~/Areas/Admin/Views/Csmi/SocietyMemberPeriodManagement.cshtml");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.SocietyMemberPeriod)]
        public ActionResult GetSocietyMemberPeriodPartial()
        {
            return new GridViewPartialController().GridViewPartial<SocietyMemberPeriodViewModel>(Bind(), "SocietyMemberPeriod", "GetSocietyMemberPeriodPartial", "SocietyMemberPeriodManagementGrid", "ID");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Poll)]
        public ActionResult ExportTo(int ActionType)
        {
            return new GridViewPartialController().ExportTo<SocietyMemberPeriodViewModel>(Bind(), ActionType, "PollManagementGrid");
        }
        List<SocietyMemberPeriodViewModel> Bind() {
            var SocietyMemberPeriods = new List<SocietyMemberPeriodViewModel>();
            var context = new ApplicationDbContext();
            var model = new object[0];
            var DbSocietyMemberPeriods = context.SocietyMemberPeriods
                .Select(item => new
                {
                    item.ID,
                    item.StartDate,
                    item.EndDate,
                    item.CreateDate,
                })
                .ToList().OrderByDescending(item => item.CreateDate);
            foreach (var item in DbSocietyMemberPeriods)
            {
                SocietyMemberPeriods.Add(new SocietyMemberPeriodViewModel()
                {
                    ID = item.ID,
                    CreateDateConverted = CommonHelper.DateAndTimes.GetPersianDate(item.CreateDate),
                    StartDate = CommonHelper.DateAndTimes.GetPersianDate(item.StartDate),
                    EndDate = CommonHelper.DateAndTimes.GetPersianDate(item.EndDate),

                });
            }
            if (SocietyMemberPeriods.Count() == 0)
            {
                SocietyMemberPeriods.Add(new SocietyMemberPeriodViewModel());
            }
            return SocietyMemberPeriods;
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.SocietyMemberPeriod)]
        public ActionResult HtmlEditorPartial()
        {
            return new HtmlEditorPartialController().HtmlEditorView("HtmlEditor", "OnHtmlChangedChanged", "OnInit");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.SocietyMemberPeriod)]
        [AjaxValidateAntiForgeryTokenAttribute]
        public ActionResult SubmitSocietyMemberPeriod(SocietyMemberPeriodViewModel viewmodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        DateTime? StartDate = CommonHelper.DateAndTimes.GetGregorianDate(viewmodel.StartDate);
                        DateTime? EndDate = CommonHelper.DateAndTimes.GetGregorianDate(viewmodel.EndDate);
                        if (viewmodel.ObjectState == ObjectState.Insert) // insert
                        {

                            var context = new ApplicationDbContext();
                            try
                            {
                                context.SocietyMemberPeriods.Add(new SocietyMemberPeriod()
                                {
                                    StartDate = StartDate != null ? (DateTime)StartDate : DateTime.Now,
                                    EndDate = EndDate != null ? (DateTime)EndDate : DateTime.Now,
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
                        else if (viewmodel.ObjectState == ObjectState.Update)//Update
                        {
                            var context = new ApplicationDbContext();
                            var selectedModel = context.SocietyMemberPeriods
                                .Where(item => item.ID == viewmodel.ID).Single();
                            var dbContextTransaction = context.Database.BeginTransaction();
                            try
                            {
                                if (selectedModel != null)
                                {
                                    selectedModel.StartDate = StartDate != null ? (DateTime)StartDate : DateTime.Now;
                                    selectedModel.EndDate = EndDate != null ? (DateTime)EndDate : DateTime.Now;
                                   
                                    selectedModel.LastModifyUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault();
                                    selectedModel.LastModifyDate = DateTime.Now;
                                }
                                context.SaveChanges();
                                dbContextTransaction.Commit();

                            }
                            catch (Exception ex)
                            {
                                dbContextTransaction.Rollback();
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
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.SocietyMemberPeriod)]
        public ActionResult DeleteSocietyMemberPeriod(string[] ids)
        {
            var context = new ApplicationDbContext();
            var dbContextTransaction = context.Database.BeginTransaction();
            try
            {
                foreach (var id in ids)
                {
                    int ID = int.Parse(id);
                    var selectedItem = context.SocietyMemberPeriods
                        .Where(item => item.ID == ID).Single();
                    if (selectedItem != null)
                    {
                        context.SocietyMemberPeriods.Remove(selectedItem);
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