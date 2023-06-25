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
using Microsoft.AspNet.Identity;
using AVA.Web.Mvc.Models;
using System.Web.Script.Serialization;
using AVA.UI.Helpers.FileUploadManagment;

namespace AVA.Web.Mvc.Controllers.Admin
{
    //Optimized
    public class GalleryImageController : BaseController
    {
        // GET: Admin/GalleryImage
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.GalleryImage)]
        public ActionResult GalleryImage()
        {
            if (Request.QueryString["id"] == null || Request.QueryString["id"] == "undefined" )
            {
                return PartialView("~/Areas/Admin/Views/GalleryImage/GalleryImage.cshtml", new GalleryImageViewModel() { ObjectState = ObjectState.Insert, GalleryAlbumID = int.Parse(Request.QueryString["AlbumID"]) });
            }
            else
            {
                var context = new ApplicationDbContext();
                var selectedID = int.Parse(Request.QueryString["id"]);
                var selectedGalleryImage = context.GalleryImages.Where(item => item.ID.Equals(selectedID))
                    .Select(item => new
                    {
                        item.ID,
                        item.Name,
                        item.Explain,
                        item.Height,
                        item.ImageLink,
                        item.Priority,
                        item.Width,
                        item.LongImageUrl,
                        item.ShortImageUrl,
                        GalleryAlbumID = item.GalleryAlbum.ID,
                        item.CreateDate
                    })
                    .SingleOrDefault();
                return PartialView("~/Areas/Admin/Views/GalleryImage/GalleryImage.cshtml", new GalleryImageViewModel()
                {
                    ID = selectedGalleryImage.ID,
                    CreateDateConverted = CommonHelper.DateAndTimes.GetPersianDate(selectedGalleryImage.CreateDate),
                    Name = selectedGalleryImage.Name,
                    Explain = selectedGalleryImage.Explain,
                    Height = selectedGalleryImage.Height,
                    ImageLink = selectedGalleryImage.ImageLink,
                    Priority = selectedGalleryImage.Priority,
                    Width = selectedGalleryImage.Width,
                    LongImageUrl = string.IsNullOrEmpty(selectedGalleryImage.LongImageUrl) ? "" : Url.Content(selectedGalleryImage.LongImageUrl),
                    ShortImageUrl = string.IsNullOrEmpty(selectedGalleryImage.ShortImageUrl) ? "" : Url.Content(selectedGalleryImage.ShortImageUrl),
                    GalleryAlbumID = selectedGalleryImage.GalleryAlbumID,
                    ObjectState = ObjectState.Update
                });
            }
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.GalleryImage)]
        public ActionResult GalleryImageManagement(int AlbumID)
        {
            ViewBag.GridID = "GalleryImageManagementGrid";
            ViewBag.AlbumID = AlbumID;
            return PartialView("~/Areas/Admin/Views/GalleryImage/GalleryImageManagement.cshtml");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.GalleryImage)]
        public ActionResult GetGalleryImagePartial(int AlbumID)
        {
            return new GridViewPartialController().GridViewPartial<GalleryImageViewModel>(Bind(AlbumID), "GalleryImage", "GetGalleryImagePartial", "GalleryImageManagementGrid", "ID");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.GalleryImage)]
        public ActionResult ExportTo(int ActionType, int AlbumID)
        {
            return new GridViewPartialController().ExportTo<GalleryImageViewModel>(Bind(AlbumID), ActionType, "GalleryImageManagementGrid");
        }
        List<GalleryImageViewModel> Bind(int AlbumID)
        {
            var GalleryImageList = new List<GalleryImageViewModel>();
            var context = new ApplicationDbContext();
            var model = new object[0];
            var DbGalleryImage = context.GalleryImages
                .Where(item => item.GalleryAlbum.ID == AlbumID)
                .Select(item => new
                {
                    item.ID,
                    item.Name,
                    item.Explain,
                    item.Height,
                    item.ImageLink,
                    item.Priority,
                    item.Width,
                    item.LongImageUrl,
                    item.ShortImageUrl,
                    GalleryAlbumID = item.GalleryAlbum.ID,
                    GalleryAlbumName = item.GalleryAlbum.Name,
                    item.CreateDate,
                    item.Enable
                })
            .ToList()
            .OrderByDescending(item => item.CreateDate);
            foreach (var selectedGalleryImage in DbGalleryImage)
            {
                GalleryImageList.Add(new GalleryImageViewModel()
                {
                    ID = selectedGalleryImage.ID,
                    Name = selectedGalleryImage.Name,
                    Explain = selectedGalleryImage.Explain,
                    Height = selectedGalleryImage.Height,
                    ImageLink = selectedGalleryImage.ImageLink,
                    Priority = selectedGalleryImage.Priority,
                    Width = selectedGalleryImage.Width,
                    LongImageUrl = selectedGalleryImage.LongImageUrl,
                    ShortImageUrl = selectedGalleryImage.ShortImageUrl,
                    GalleryAlbumID = selectedGalleryImage.GalleryAlbumID,
                    GalleryAlbumName = selectedGalleryImage.GalleryAlbumName,
                    Enable = selectedGalleryImage.Enable,
                    CreateDateConverted = CommonHelper.DateAndTimes.GetPersianDate(selectedGalleryImage.CreateDate)
                });
            }
            if (GalleryImageList.Count() == 0)
            {
                GalleryImageList.Add(new GalleryImageViewModel());
            }
            return GalleryImageList;
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.GalleryImage)]
        public ActionResult HtmlEditorPartial()
        {
            return new HtmlEditorPartialController().HtmlEditorView("HtmlEditor", "OnHtmlChangedChanged", "OnInit");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.GalleryImage)]
        [HttpPost]
        [AjaxValidateAntiForgeryTokenAttribute]
        public ActionResult SubmitGalleryImage(string viewmodel)
        {
            try
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                GalleryImageViewModel vm = jss.Deserialize<GalleryImageViewModel>(viewmodel);
                if (ModelState.IsValid)
                {
                    try
                    {
                        string SmallImage = "";
                        string LongImage = "";
                        if (Request.Files.Count > 0)
                        {
                            var file = Request.Files[CommonHelper.ExposeProperty.GetPropertyInfo(vm, m => m.SmallImage).Name];

                            if (file != null && file.ContentLength > 0)
                            {
                                SmallImage = FileUploadManagment.UploadFile("~/assets/img/Attach/Gallery/", "", file, FileUploadManagment.AppFileType.Image);
                                if (SmallImage == "")
                                {
                                    return ControllerHelper.ErrorResult("فرمت یا حجم فایل صحیح نمی باشد");
                                }
                            }
                            file = Request.Files[CommonHelper.ExposeProperty.GetPropertyInfo(vm, m => m.LongImage).Name];

                            if (file != null && file.ContentLength > 0)
                            {
                                LongImage = FileUploadManagment.UploadFile("~/assets/img/Attach/Gallery/", "", file, FileUploadManagment.AppFileType.Document);
                                if (LongImage == "")
                                {
                                    return ControllerHelper.ErrorResult("فرمت یا حجم فایل صحیح نمی باشد");
                                }
                            }
                        }
                        if (vm.ObjectState == ObjectState.Insert)
                        {
                            var file = Request.Files[CommonHelper.ExposeProperty.GetPropertyInfo(vm, m => m.SmallImage).Name];
                            if (file == null && vm.ObjectState == ObjectState.Insert)
                            {
                                return ControllerHelper.ErrorResult("تصویر پرسنلی را وارد کنید");
                            }
                            file = Request.Files[CommonHelper.ExposeProperty.GetPropertyInfo(vm, m => m.LongImage).Name];
                            if (file == null && vm.ObjectState == ObjectState.Insert)
                            {
                                return ControllerHelper.ErrorResult(" فایل رزومه را وارد کنید");
                            }

                        }
                        if (vm.ObjectState == ObjectState.Insert) // insert
                        {
                            var context = new ApplicationDbContext();
                            context.GalleryImages.Add(new GalleryImage()
                            {
                                Name = vm.Name,
                                Explain = vm.Explain,
                                Height = vm.Height,
                                ImageLink = vm.ImageLink,
                                Priority = vm.Priority,
                                Width = vm.Width,
                                ShortImageUrl = SmallImage,
                                LongImageUrl = LongImage,
                                GalleryAlbum = context.GalleryAlbums.Where(Album => Album.ID == vm.GalleryAlbumID).SingleOrDefault(),
                                Enable = true,
                                CreateDate = DateTime.Now,
                                CreatorUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault(),
                            });
                            context.SaveChanges();

                        }
                        else if (vm.ObjectState == ObjectState.Update)//Update
                        {
                            var context = new ApplicationDbContext();
                            GalleryImage selectedModel = context.GalleryImages.Where(item => item.ID == vm.ID).Single();
                            if (selectedModel != null)
                            {
                                selectedModel.Name = vm.Name;
                                selectedModel.Explain = vm.Explain;
                                selectedModel.Height = vm.Height;
                                selectedModel.ImageLink = vm.ImageLink;
                                selectedModel.Priority = vm.Priority;
                                selectedModel.Width = vm.Width;
                                if (SmallImage != "")
                                {
                                    selectedModel.ShortImageUrl = SmallImage;
                                }
                                if (LongImage != "")
                                {
                                    selectedModel.LongImageUrl = LongImage;
                                }
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
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.GalleryImage)]
        public ActionResult ActiveGalleryImage(string[] ids)
        {
            var context = new ApplicationDbContext();
            try
            {
                foreach (var id in ids)
                {
                    int CurrentID = int.Parse(id);
                    var selectedItem = context.GalleryImages.Where(item => item.ID == CurrentID).Single();
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
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.GalleryImage)]
        public ActionResult DeleteGalleryImage(string[] ids)
        {
            var context = new ApplicationDbContext();
            var dbContextTransaction = context.Database.BeginTransaction();
            try
            {
                foreach (var id in ids)
                {
                    int ID = int.Parse(id);
                    var selectedItem = context.GalleryImages
                        .Where(item => item.ID == ID).Single();
                    if (selectedItem != null)
                    {
                        try
                        {
                            System.IO.File.Delete(selectedItem.ShortImageUrl);
                        }
                        catch (Exception)
                        {

                        }
                        try
                        {
                            System.IO.File.Delete(selectedItem.LongImageUrl);
                        }
                        catch (Exception)
                        {

                        }
                        context.GalleryImages.Remove(selectedItem);
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