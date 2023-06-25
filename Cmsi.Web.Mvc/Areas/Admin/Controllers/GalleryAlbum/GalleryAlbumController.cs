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

namespace AVA.Web.Mvc.Controllers.Admin
{
    //Optimized
    public class GalleryAlbumController : BaseController
    {
        // GET: Admin/GalleryAlbum
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.GalleryAlbum)]
        public ActionResult GalleryAlbum()
        {
            if (Request.QueryString["id"] == null || Request.QueryString["id"] == "undefined")
            {
                return PartialView("~/Areas/Admin/Views/GalleryAlbum/GalleryAlbum.cshtml", new GalleryAlbumViewModel() { ObjectState = ObjectState.Insert });
            }
            else
            {
                var context = new ApplicationDbContext();
                var selectedID = int.Parse(Request.QueryString["id"]);
                var selectedGalleryAlbum = context.GalleryAlbums.Where(item => item.ID.Equals(selectedID))
                    .Select(item => new
                    {
                        item.ID,
                        item.Name,
                        item.Description,
                        item.CreateDate
                    })
                    .SingleOrDefault();
                return PartialView("~/Areas/Admin/Views/GalleryAlbum/GalleryAlbum.cshtml", new GalleryAlbumViewModel()
                {
                    ID = selectedGalleryAlbum.ID,
                    CreateDateConverted = CommonHelper.DateAndTimes.GetPersianDate(selectedGalleryAlbum.CreateDate),
                    Name = selectedGalleryAlbum.Name,
                    Description = selectedGalleryAlbum.Description,
                    ObjectState = ObjectState.Update
                });
            }
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.GalleryAlbum)]
        public ActionResult GalleryAlbumManagement()
        {
            ViewBag.GridID = "GalleryAlbumManagementGrid";
            return PartialView("~/Areas/Admin/Views/GalleryAlbum/GalleryAlbumManagement.cshtml");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.GalleryAlbum)]
        public ActionResult GetGalleryAlbumPartial()
        {     
            return new GridViewPartialController().GridViewPartial<GalleryAlbumViewModel>(BindGalleryAlbum(), "GalleryAlbum", "GetGalleryAlbumPartial", "GalleryAlbumManagementGrid", "ID");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.GalleryAlbum)]
        public ActionResult ExportTo(int ActionType)
        {
            return new GridViewPartialController().ExportTo<GalleryAlbumViewModel>(BindGalleryAlbum(), ActionType, "GalleryAlbumManagementGrid");
        }
        List<GalleryAlbumViewModel> BindGalleryAlbum()
        {
            var GalleryAlbumList = new List<GalleryAlbumViewModel>();
            var context = new ApplicationDbContext();
            var model = new object[0];
            var DbGalleryAlbum = context.GalleryAlbums
                .Select(item => new
                {
                    item.ID,
                    item.Name,
                    item.Description,
                    item.Language,
                    item.CreateDate,
                    item.Enable
                })
            .ToList()
            .OrderByDescending(item => item.CreateDate);
            foreach (var item in DbGalleryAlbum)
            {
                GalleryAlbumList.Add(new GalleryAlbumViewModel()
                {
                    ID = item.ID,
                    Name = item.Name,
                    Description = item.Description,
                    CreateDateConverted = CommonHelper.DateAndTimes.GetPersianDate(item.CreateDate)
                });
            }
            if (GalleryAlbumList.Count() == 0)
            {
                GalleryAlbumList.Add(new GalleryAlbumViewModel());
            }
            return GalleryAlbumList;
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.GalleryAlbum)]
        public ActionResult HtmlEditorPartial()
        {
            return new HtmlEditorPartialController().HtmlEditorView("HtmlEditor", "OnHtmlChangedChanged", "OnInit");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.GalleryAlbum)]
        [HttpPost]
        [AjaxValidateAntiForgeryTokenAttribute]
        public ActionResult SubmitGalleryAlbum(GalleryAlbumViewModel viewmodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        if (viewmodel.ObjectState == ObjectState.Insert) // insert
                        {
                            var context = new ApplicationDbContext();
                            context.GalleryAlbums.Add(new GalleryAlbum()
                            {
                                Name = viewmodel.Name,
                                Description = viewmodel.Description,
                                Enable = true,
                                CreateDate = DateTime.Now,
                                CreatorUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault(),
                            });
                            context.SaveChanges();

                        }
                        else if (viewmodel.ObjectState == ObjectState.Update)//Update
                        {
                            var context = new ApplicationDbContext();
                            GalleryAlbum selectedModel = context.GalleryAlbums.Where(item => item.ID == viewmodel.ID).Single();
                            if (selectedModel != null)
                            {
                                selectedModel.Name = viewmodel.Name;
                                selectedModel.Description = viewmodel.Description;
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
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.GalleryAlbum)]
        public ActionResult ActiveGalleryAlbum(string[] ids)
        {
            var context = new ApplicationDbContext();
            try
            {
                foreach (var id in ids)
                {
                    int CurrentID = int.Parse(id);
                    var selectedItem = context.GalleryAlbums.Where(item => item.ID == CurrentID).Single();
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
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.GalleryAlbum)]
        public ActionResult DeleteGalleryAlbum(string[] ids)
        {
            var context = new ApplicationDbContext();
            var dbContextTransaction = context.Database.BeginTransaction();
            try
            {
                foreach (var id in ids)
                {
                    int ID = int.Parse(id);
                    var selectedItem = context.GalleryAlbums
                        .Where(item => item.ID == ID).Single();
                    if (selectedItem != null)
                    {
                        context.GalleryAlbums.Remove(selectedItem);
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