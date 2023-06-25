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
    public class SocietyMemberController : BaseController
    {
        // GET: Admin/SocietyMember
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.SocietyMember)]
        public ActionResult SocietyMember()
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
                return PartialView("~/Areas/Admin/Views/Csmi/SocietyMember.cshtml", new SocietyMemberViewModel() { ObjectState = ObjectState.Insert, Languages = Languages, LanguageID = Languages[0].Value, PeriodID = int.Parse(Request.QueryString["PeriodID"]) });
            }
            else
            {

                var selectedID = int.Parse(Request.QueryString["id"]);
                var selectedSocietyMember = context.SocietyMembers
                    .Include(SocietyMembers => SocietyMembers.Language)
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
                        SocietyMemberPeriodID = item.SocietyMemberPeriod.ID,
                        item.CreateDate,
                    })
                    .SingleOrDefault();
                return PartialView("~/Areas/Admin/Views/Csmi/SocietyMember.cshtml", new SocietyMemberViewModel()
                {
                    ID = selectedSocietyMember.ID,
                    CreateDateConverted = CommonHelper.DateAndTimes.GetPersianDate(selectedSocietyMember.CreateDate),
                    Name = selectedSocietyMember.Name,
                    Family = selectedSocietyMember.Family,
                    DegreeLevel = selectedSocietyMember.DegreeLevel,
                    Position = selectedSocietyMember.Position,
                    Email = selectedSocietyMember.Email,
                    PeriodID = selectedSocietyMember.SocietyMemberPeriodID,
                    Languages = Languages,
                    LanguageID = selectedSocietyMember.LanguageID.ToString(),
                    PersonalImageUrl = string.IsNullOrEmpty(selectedSocietyMember.PersonalImageUrl) ? "" : Url.Content(selectedSocietyMember.PersonalImageUrl),
                    ResumeUrl = string.IsNullOrEmpty(selectedSocietyMember.ResumeUrl) ? "" : Url.Content(selectedSocietyMember.ResumeUrl),
                    ObjectState = ObjectState.Update
                });
            }
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.SocietyMember)]
        public ActionResult SocietyMemberManagement(int PeriodID)
        {
            ViewBag.GridID = "SocietyMemberManagementGrid";
            ViewBag.PeriodID = PeriodID;
            return PartialView("~/Areas/Admin/Views/Csmi/SocietyMemberManagement.cshtml");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.SocietyMember)]
        public ActionResult GetSocietyMemberPartial(int PeriodID)
        {
            return new GridViewPartialController().GridViewPartial<SocietyMemberViewModel>(Bind(PeriodID), "SocietyMember", "GetSocietyMemberPartial", "SocietyMemberManagementGrid", "ID");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.SocietyMember)]
        public ActionResult ExportTo(int ActionType, int PeriodID)
        {
            return new GridViewPartialController().ExportTo<SocietyMemberViewModel>(Bind(PeriodID), ActionType, "SocietyMemberManagementGrid");
        }
        List<SocietyMemberViewModel> Bind(int PeriodID)
        {
            var SocietyMembers = new List<SocietyMemberViewModel>();
            var context = new ApplicationDbContext();
            var model = new object[0];
            var DbSocietyMembers = context.SocietyMembers
                .Include(SocietyMember => SocietyMember.Language)
                .Where(SocietyMember => SocietyMember.SocietyMemberPeriod.ID == PeriodID)
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
                    SocietyMemberPeriodID = item.SocietyMemberPeriod.ID,
                    item.CreateDate,
                })
                .ToList().OrderByDescending(item => item.CreateDate);
            foreach (var item in DbSocietyMembers)
            {
                SocietyMembers.Add(new SocietyMemberViewModel()
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
                    PersonalImageUrl = item.PersonalImageUrl,
                    PeriodID = item.SocietyMemberPeriodID,
                    Enable=item.Enable,
                    LanguageName = item.LanguageName
                });
            }
            if (SocietyMembers.Count() == 0)
            {
                SocietyMembers.Add(new SocietyMemberViewModel());
            }
            return SocietyMembers;
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.SocietyMember)]
        public ActionResult HtmlEditorPartial()
        {
            return new HtmlEditorPartialController().HtmlEditorView("HtmlEditor", "OnHtmlChangedChanged", "OnInit");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.SocietyMember)]
        [AjaxValidateAntiForgeryTokenAttribute]
        public ActionResult SubmitSocietyMember(string viewmodel)
        {
            try
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                SocietyMemberViewModel vm = jss.Deserialize<SocietyMemberViewModel>(viewmodel);
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
                                PersonalImage = FileUploadManagment.UploadFile("~/assets/img/Attach/SocietyMember/", "", file, FileUploadManagment.AppFileType.Image);
                                if (PersonalImage == "")
                                {
                                    return ControllerHelper.ErrorResult("فرمت یا حجم فایل صحیح نمی باشد");
                                }
                            }
                            file = Request.Files[CommonHelper.ExposeProperty.GetPropertyInfo(vm, m => m.Resume).Name];

                            if (file != null && file.ContentLength > 0)
                            {
                                Resume = FileUploadManagment.UploadFile("~/assets/img/Attach/SocietyMember/", "", file, FileUploadManagment.AppFileType.Document);
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
                                context.SocietyMembers.Add(new SocietyMember()
                                {
                                    Name = vm.Name,
                                    Family = vm.Family,
                                    Email = vm.Email,
                                    Position = vm.Position,
                                    DegreeLevel = vm.DegreeLevel,
                                    ResumeUrl = Resume,
                                    PersonalImageUrl = PersonalImage,
                                    Language = context.Languages.Where(lang => lang.ID == langID).SingleOrDefault(),
                                    SocietyMemberPeriod = context.SocietyMemberPeriods.Where(period => period.ID == vm.PeriodID).SingleOrDefault(),
                                    Enable=true,
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
                            var selectedModel = context.SocietyMembers
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
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.SocietyMember)]
        public ActionResult DeleteSocietyMember(string[] ids)
        {
            var context = new ApplicationDbContext();
            var dbContextTransaction = context.Database.BeginTransaction();
            try
            {
                foreach (var id in ids)
                {
                    int ID = int.Parse(id);
                    var selectedItem = context.SocietyMembers
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
                        context.SocietyMembers.Remove(selectedItem);
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
        public ActionResult ActiveSocietyMember(string[] ids)
        {
            var context = new ApplicationDbContext();
            try
            {
                foreach (var id in ids)
                {
                    int CurrentID = int.Parse(id);
                    var selectedItem = context.SocietyMembers
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