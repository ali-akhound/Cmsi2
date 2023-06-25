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
using System.Web.Script.Serialization;
using AVA.UI.Helpers.FileUploadManagment;

namespace AVA.Web.Mvc.Controllers.Admin
{
    //Optimized
    public class ConferenceCompanionController : BaseController
    {
        // GET: Admin/ConferenceCompanion
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.ConferenceCompanion)]
        public ActionResult ConferenceCompanion()
        {
            if (Request.QueryString["id"] == null || Request.QueryString["id"] == "undefined")
            {
                return PartialView("~/Areas/Admin/Views/Csmi/ConferenceCompanion.cshtml", new CompanionViewModel() { ObjectState = ObjectState.Insert });
            }
            else
            {
                var context = new ApplicationDbContext();
                var selectedID = int.Parse(Request.QueryString["id"]);
                var selectedConferenceCompanion = context.ConferenceCompanions
                    .Where(item => item.ID.Equals(selectedID))
                    .Select(item => new
                    {
                        item.ID,
                        item.Email,
                        item.FirstName,
                        item.LastName,
                        item.University,
                        item.FieldOfStudy,
                        item.PayReceiptUrl,
                        item.UniversityCardUrl,
                        item.Degree,
                        item.BornDate,
                        item.Cellphone,
                        item.Melicode,
                        item.PersonalImageUrl,
                        ConferenceID = item.Conference.ID,
                        item.CreateDate,
                    })
                    .SingleOrDefault();
                return PartialView("~/Areas/Admin/Views/Csmi/ConferenceCompanion.cshtml", new CompanionViewModel()
                {
                    ID = selectedConferenceCompanion.ID,
                    CreateDateConverted = CommonHelper.DateAndTimes.GetPersianDate(selectedConferenceCompanion.CreateDate),
                    Email = selectedConferenceCompanion.Email,
                    FirstName = selectedConferenceCompanion.FirstName,
                    LastName = selectedConferenceCompanion.LastName,
                    University = selectedConferenceCompanion.University,
                    FieldOfStudy = selectedConferenceCompanion.FieldOfStudy,
                    BornDate = CommonHelper.DateAndTimes.GetPersianDate(selectedConferenceCompanion.BornDate),
                    Degree = selectedConferenceCompanion.Degree,
                    Cellphone = selectedConferenceCompanion.Cellphone,
                    Melicode = selectedConferenceCompanion.Melicode,
                    UniversityCardFileImageUrl = Url.Content(selectedConferenceCompanion.UniversityCardUrl),
                    PayReceiptFileImageUrl = Url.Content(selectedConferenceCompanion.PayReceiptUrl),
                    ConferenceID = selectedConferenceCompanion.ConferenceID,
                    //ImageUrl = selectedConferenceCompanion.PersonalImageUrl,
                    //Enable = selectedConferenceCompanion.Enable,
                    ObjectState = ObjectState.Update
                });
            }
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.ConferenceCompanion)]
        public ActionResult ConferenceCompanionManagement(int ConferenceID, string UserID = "0")
        {
            ViewBag.GridID = "ConferenceCompanionManagementGrid";
            ViewBag.ConferenceID = ConferenceID;
            ViewBag.UserID = UserID;
            return PartialView("~/Areas/Admin/Views/Csmi/ConferenceCompanionManagement.cshtml");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.ConferenceCompanion)]
        public ActionResult GetConferenceCompanionPartial(int ConferenceID, string UserID = "0")
        {
            return new GridViewPartialController().GridViewPartial<CompanionViewModel>(Bind(ConferenceID, UserID), "ConferenceCompanion", "GetConferenceCompanionPartial", "ConferenceCompanionManagementGrid", "ID");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.ConferenceCompanion)]
        public ActionResult ExportTo(int ActionType, int ConferenceID, string UserID = "0")
        {
            return new GridViewPartialController().ExportTo<CompanionViewModel>(Bind(ConferenceID, UserID), ActionType, "ConferenceCompanionManagementGrid");
        }
        List<CompanionViewModel> Bind(int ConferenceID, string UserID)
        {
            var ConferenceCompanions = new List<CompanionViewModel>();
            var context = new ApplicationDbContext();
            var model = new object[0];
            var DbConferenceCompanions = context.ConferenceCompanions
                .Include(ConferenceCompanion => ConferenceCompanion.ConferencePackage.PackageName.PackageNameTranslations)
                .Include(ConferenceCompanion => ConferenceCompanion.CreatorUser)
                .Where(ConferenceCompanion => ConferenceCompanion.Conference.ID == ConferenceID
                    &&
                    ConferenceCompanion.ConferencePackage.PackageName.PackageNameTranslations.Any(trans => trans.Language.Value == "fa")
                    &&
                    (ConferenceCompanion.CreatorUser.Id == UserID || UserID == "0" || UserID == "undefined")
                )
                .Select(item => new
                {
                    item.ID,
                    item.Email,
                    PackageName = item.ConferencePackage.PackageName.PackageNameTranslations.FirstOrDefault().Name,
                    item.UniversityCardUrl,
                    item.PayReceiptUrl,
                    item.FirstName,
                    item.LastName,
                    item.Cellphone,
                    item.CreateDate,
                    item.CreatorUser,
                })
                .OrderByDescending(item => item.CreateDate)
                .ToList();
            foreach (var item in DbConferenceCompanions)
            {
                ConferenceCompanions.Add(new CompanionViewModel()
                {
                    ID = item.ID,
                    CreateDateConverted = CommonHelper.DateAndTimes.GetPersianDate(item.CreateDate),
                    Email = item.Email,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    SelectedPackage = item.PackageName,
                    Cellphone = item.Cellphone,
                    Attach =
                            item.UniversityCardUrl != "~/assets/img/profile-green.png" ? Url.Content(item.UniversityCardUrl) :
                            item.PayReceiptUrl != "~/assets/img/profile-green.png" ? Url.Content(item.PayReceiptUrl) : "",
                    AttachText = "پیوست"
                });
            }
            if (ConferenceCompanions.Count() == 0)
            {
                ConferenceCompanions.Add(new CompanionViewModel());
            }
            return ConferenceCompanions;
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.ConferenceCompanion)]
        public ActionResult HtmlEditorPartial()
        {
            return new HtmlEditorPartialController().HtmlEditorView("HtmlEditor", "OnHtmlChangedChanged", "OnInit");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        [AjaxValidateAntiForgeryTokenAttribute]
        public ActionResult SubmitConferenceCompanion(string viewmodel)
        {
            try
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                CompanionViewModel vm = jss.Deserialize<CompanionViewModel>(viewmodel);
                if (ModelState.IsValid)
                {
                    try
                    {
                        string PayReceiptFileImage = "";
                        string UniversityCardFileImage = "";
                        if (Request.Files.Count > 0)
                        {
                            var file = Request.Files[CommonHelper.ExposeProperty.GetPropertyInfo(vm, m => m.PayReceiptFileImage).Name];

                            if (file != null && file.ContentLength > 0)
                            {
                                PayReceiptFileImage = FileUploadManagment.UploadFile("~/assets/img/Attach/ConferenceCompanion/", "", file, FileUploadManagment.AppFileType.Image);
                                if (PayReceiptFileImage == "")
                                {
                                    return ControllerHelper.ErrorResult("فرمت یا حجم فایل صحیح نمی باشد");
                                }
                            }
                            file = Request.Files[CommonHelper.ExposeProperty.GetPropertyInfo(vm, m => m.UniversityCardFileImage).Name];

                            if (file != null && file.ContentLength > 0)
                            {
                                UniversityCardFileImage = FileUploadManagment.UploadFile("~/assets/img/Attach/ConferenceCompanion/", "", file, FileUploadManagment.AppFileType.Document);
                                if (UniversityCardFileImage == "")
                                {
                                    return ControllerHelper.ErrorResult("فرمت یا حجم فایل صحیح نمی باشد");
                                }
                            }
                        }
                        if (vm.ObjectState == ObjectState.Insert)
                        {
                            var file = Request.Files[CommonHelper.ExposeProperty.GetPropertyInfo(vm, m => m.PayReceiptFileImage).Name];
                            if (file == null && vm.ObjectState == ObjectState.Insert)
                            {
                                return ControllerHelper.ErrorResult("تصویر پرداخت را وارد کنید");
                            }
                            file = Request.Files[CommonHelper.ExposeProperty.GetPropertyInfo(vm, m => m.UniversityCardFileImage).Name];
                            if (file == null && vm.ObjectState == ObjectState.Insert)
                            {
                                return ControllerHelper.ErrorResult(" تصویر کارت دانشجویی را وارد کنید");
                            }
                            var context = new ApplicationDbContext();
                            context.ConferenceCompanions.Add(new ConferenceCompanion()
                            {
                                FirstName = vm.FirstName,
                                LastName = vm.LastName,
                                Email = vm.Email,
                                University = vm.University,
                                FieldOfStudy = vm.FieldOfStudy,
                                Degree = vm.Degree,
                                Cellphone = vm.Cellphone,
                                BornDate = CommonHelper.DateAndTimes.GetGregorianDate(vm.BornDate),
                                UniversityCardUrl = UniversityCardFileImage,
                                PayReceiptUrl = PayReceiptFileImage,
                                Conference = context.Conferences.Where(conf => conf.ID == vm.ConferenceID).SingleOrDefault(),
                                CreateDate = DateTime.Now,
                                CreatorUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault()
                            });
                        }
                        else if (vm.ObjectState == ObjectState.Update)//Update
                        {
                            var context = new ApplicationDbContext();
                            var selectedModel = context.ConferenceCompanions
                                .Include(item => item.Conference)
                                .Include(item => item.ConferencePackage)
                                .Where(item => item.ID == vm.ID).Single();
                            try
                            {

                                if (selectedModel != null)
                                {
                                    selectedModel.ID = vm.ID;
                                    selectedModel.FirstName = vm.FirstName;
                                    selectedModel.LastName = vm.LastName;
                                    selectedModel.Email = vm.Email;
                                    selectedModel.University = vm.University;
                                    selectedModel.FieldOfStudy = vm.FieldOfStudy;
                                    selectedModel.Degree = vm.Degree;
                                    selectedModel.Cellphone = vm.Cellphone;
                                    selectedModel.BornDate = CommonHelper.DateAndTimes.GetGregorianDate(vm.BornDate);
                                    if (UniversityCardFileImage != "")
                                    {
                                        selectedModel.UniversityCardUrl = UniversityCardFileImage;
                                    }
                                    if (PayReceiptFileImage != "")
                                    {
                                        selectedModel.PayReceiptUrl = PayReceiptFileImage;
                                    }
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
        //[HttpPost]
        //[AjaxValidateAntiForgeryTokenAttribute]
        //[AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.ConferenceCompanion)]
        //public ActionResult ActiveConferenceCompanion(string[] ids)
        //{
        //    var context = new ApplicationDbContext();
        //    try
        //    {
        //        foreach (var id in ids)
        //        {
        //            int CurrentID = int.Parse(id);
        //            var selectedItem = context.ConferenceCompanions
        //                .Where(item => item.ID == CurrentID).Single();
        //            if (selectedItem != null)
        //            {
        //                selectedItem.Enable = !selectedItem.Enable;
        //            }
        //        }
        //        context.SaveChanges();
        //        return ControllerHelper.SuccessResult("با موفقیت ثبت شد");
        //    }
        //    catch (Exception ex)
        //    {
        //        return ControllerHelper.ErrorResult("بروز خطای سیستمی");
        //    }


        //}
        [HttpPost]
        [AjaxValidateAntiForgeryTokenAttribute]
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.ConferenceCompanion)]
        public ActionResult DeleteConferenceCompanion(string[] ids)
        {
            var context = new ApplicationDbContext();
            var dbContextTransaction = context.Database.BeginTransaction();
            try
            {
                foreach (var id in ids)
                {
                    int ID = int.Parse(id);
                    var selectedItem = context.ConferenceCompanions
                        .Where(item => item.ID == ID).Single();
                    if (selectedItem != null)
                    {
                        context.ConferenceCompanions.Remove(selectedItem);
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