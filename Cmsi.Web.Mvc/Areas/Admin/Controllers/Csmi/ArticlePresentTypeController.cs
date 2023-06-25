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

namespace AVA.Web.Mvc.Controllers.Admin
{
    //Optimized

    public class ArticlePresentTypeController : BaseController
    {
        // GET: Admin/ArticlePresentType
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult ArticlePresentType()
        {
            if (Request.QueryString["id"] == null || Request.QueryString["id"] == "undefined")
            {
                return PartialView("~/Areas/Admin/Views/Csmi/ArticlePresentType.cshtml", new ArticlePresentTypeViewModel() { ObjectState = ObjectState.Insert });
            }
            else
            {
                var context = new ApplicationDbContext();
                var selectedID = int.Parse(Request.QueryString["id"]);
                var selectedArticlePresentType = context.ArticlePresentTypes
                    .Where(item => item.ID.Equals(selectedID))
                    .Select(item => new { item.ID, item.Name, item.Enable, item.CreateDate })
                    .SingleOrDefault();
                return PartialView("~/Areas/Admin/Views/Csmi/ArticlePresentType.cshtml", new ArticlePresentTypeViewModel()
                {
                    ID = selectedArticlePresentType.ID,
                    Name = selectedArticlePresentType.Name,
                    CreateDateConverted = CommonHelper.DateAndTimes.GetPersianDate(selectedArticlePresentType.CreateDate),
                    Enable = selectedArticlePresentType.Enable,
                    ObjectState = ObjectState.Update
                });
            }
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult ArticlePresentTypeManagement()
        {
            ViewBag.GridID = "ArticlePresentTypeManagementGrid";
            return PartialView("~/Areas/Admin/Views/Csmi/ArticlePresentTypeManagement.cshtml");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult GetArticlePresentTypePartial()
        {            
            return new GridViewPartialController().GridViewPartial<ArticlePresentTypeViewModel>(Bind(), "ArticlePresentType", "GetArticlePresentTypePartial", "ArticlePresentTypeManagementGrid", "ID");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult ExportTo(int ActionType)
        {
            return new GridViewPartialController().ExportTo<ArticlePresentTypeViewModel>(Bind(), ActionType, "ArticlePresentTypeManagementGrid");
        }
        List<ArticlePresentTypeViewModel> Bind()
        {
            var ArticlePresentTypes = new List<ArticlePresentTypeViewModel>();
            var context = new ApplicationDbContext();
            var model = new object[0];
            var DbArticlePresentTypes = context.ArticlePresentTypes
                .Select(item => new { item.ID, item.Name, item.Enable, item.CreateDate })
                .OrderByDescending(item => item.CreateDate)
                .ToList();
            foreach (var item in DbArticlePresentTypes)
            {
                ArticlePresentTypes.Add(new ArticlePresentTypeViewModel()
                {
                    ID = item.ID,
                    Name = item.Name,
                    CreateDateConverted = CommonHelper.DateAndTimes.GetPersianDate(item.CreateDate),
                    Enable = item.Enable,

                });
            }
            if (ArticlePresentTypes.Count() == 0)
            {
                ArticlePresentTypes.Add(new ArticlePresentTypeViewModel());
            }
            return ArticlePresentTypes;
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult HtmlEditorPartial()
        {
            return new HtmlEditorPartialController().HtmlEditorView("HtmlEditor", "OnHtmlChangedChanged", "OnInit");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        [AjaxValidateAntiForgeryTokenAttribute]
        public ActionResult SubmitArticlePresentType(ArticlePresentTypeViewModel viewmodel)
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
                            context.ArticlePresentTypes
                            .Add(new ArticlePresentType()
                            {
                                Name = viewmodel.Name,
                                Enable = true,
                                CreateDate = DateTime.Now,
                                CreatorUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault(),

                            });
                            context.SaveChanges();
                        }
                        else if (viewmodel.ObjectState == ObjectState.Update)//Update
                        {
                            var context = new ApplicationDbContext();
                            ArticlePresentType selectedModel = context.ArticlePresentTypes.Where(item => item.ID == viewmodel.ID).Single();
                            if (selectedModel != null)
                            {
                                selectedModel.Name = viewmodel.Name;
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
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult ActiveArticlePresentType(string[] ids)
        {
            var context = new ApplicationDbContext();
            try
            {
                foreach (var id in ids)
                {
                    int CurrentID = int.Parse(id);
                    var selectedItem = context.ArticlePresentTypes
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
        //[HttpPost]
        //[AjaxValidateAntiForgeryTokenAttribute]
        //[AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        //public ActionResult DeleteArticlePresentType(string[] ids)
        //{
        //    var context = new ApplicationDbContext();
        //    try
        //    {
        //        foreach (var id in ids)
        //        {
        //            int ID = int.Parse(id);
        //            var selectedItem = context.ArticlePresentTypes
        //                .Where(item => item.ID == ID).Single();
        //            if (selectedItem != null)
        //            {
        //                context.ArticlePresentTypes.Remove(selectedItem);
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
    }
}