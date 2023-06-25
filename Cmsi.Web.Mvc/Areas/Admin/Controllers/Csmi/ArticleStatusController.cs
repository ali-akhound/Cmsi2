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
using AVA.UI.Helpers;

namespace AVA.Web.Mvc.Controllers.Admin
{
    //Optimized
    public class ArticleStatusController : BaseController
    {
        // GET: Admin/ArticleStatus
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult ArticleStatus()
        {
            if (Request.QueryString["id"] == null || Request.QueryString["id"] == "undefined")
            {
                return PartialView("~/Areas/Admin/Views/Csmi/ArticleStatus.cshtml", new ArticleStatusViewModel() { ObjectState = ObjectState.Insert });
            }
            else
            {
                var context = new ApplicationDbContext();
                var selectedID = int.Parse(Request.QueryString["id"]);
                var selectedArticleStatus = context.ArticleStatuses
                    .Where(item => item.ID.Equals(selectedID))
                    .Select(item => new { item.ID, item.Name, item.Enable, item.CreateDate })
                    .SingleOrDefault();
                return PartialView("~/Areas/Admin/Views/Csmi/ArticleStatus.cshtml", new ArticleStatusViewModel()
                {
                    ID = selectedArticleStatus.ID,
                    Name = selectedArticleStatus.Name,
                    CreateDateConverted = CommonHelper.DateAndTimes.GetPersianDate(selectedArticleStatus.CreateDate),
                    Enable = selectedArticleStatus.Enable,
                    ObjectState = ObjectState.Update
                });
            }
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult ArticleStatusManagement()
        {
            ViewBag.GridID = "ArticleStatusManagementGrid";
            return PartialView("~/Areas/Admin/Views/Csmi/ArticleStatusManagement.cshtml");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult GetArticleStatusPartial()
        {
            return new GridViewPartialController().GridViewPartial<ArticleStatusViewModel>(Bind(), "ArticleStatus", "GetArticleStatusPartial", "ArticleStatusManagementGrid", "ID");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult ExportTo(int ActionType)
        {
            return new GridViewPartialController().ExportTo<ArticleStatusViewModel>(Bind(), ActionType, "PollManagementGrid");
        }
        List<ArticleStatusViewModel> Bind()
        {
            var ArticleStatuses = new List<ArticleStatusViewModel>();
            var context = new ApplicationDbContext();
            var model = new object[0];
            var DbArticleStatuss = context.ArticleStatuses
               .Select(item => new { item.ID, item.Name, item.Enable, item.CreateDate })
                .ToList().OrderByDescending(item => item.CreateDate);
            foreach (var item in DbArticleStatuss)
            {
                ArticleStatuses.Add(new ArticleStatusViewModel()
                {
                    ID = item.ID,
                    Name = item.Name,
                    CreateDateConverted = CommonHelper.DateAndTimes.GetPersianDate(item.CreateDate),
                    Enable = item.Enable,

                });
            }
            if (ArticleStatuses.Count() == 0)
            {
                ArticleStatuses.Add(new ArticleStatusViewModel());
            }
            return ArticleStatuses;
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult HtmlEditorPartial()
        {
            return new HtmlEditorPartialController().HtmlEditorView("HtmlEditor", "OnHtmlChangedChanged", "OnInit");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        [AjaxValidateAntiForgeryTokenAttribute]
        public ActionResult SubmitArticleStatus(ArticleStatusViewModel viewmodel)
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
                            context.ArticleStatuses
                            .Add(new ArticleStatus()
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
                            ArticleStatus selectedModel = context.ArticleStatuses.Where(item => item.ID == viewmodel.ID).Single();
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
        public ActionResult ActiveArticleStatus(string[] ids)
        {
            var context = new ApplicationDbContext();
            try
            {
                foreach (var id in ids)
                {
                    int CurrentID = int.Parse(id);
                    var selectedItem = context.ArticleStatuses
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
        //public ActionResult DeleteArticleStatus(string[] ids)
        //{
        //    var context = new ApplicationDbContext();
        //    try
        //    {
        //        foreach (var id in ids)
        //        {
        //            int ID = int.Parse(id);
        //            var selectedItem = context.ArticleStatuses
        //                .Where(item => item.ID == ID).Single();
        //            if (selectedItem != null)
        //            {
        //                context.ArticleStatuses.Remove(selectedItem);
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