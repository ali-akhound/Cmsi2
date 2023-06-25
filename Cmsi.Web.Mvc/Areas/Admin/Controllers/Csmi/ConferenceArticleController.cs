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

namespace AVA.Web.Mvc.Controllers.Admin
{
    public class ConferenceArticleController : BaseController
    {
        #region ConferenceArticle
        public ActionResult ConferenceArticleManagement(string ConferenceID)
        {
            ViewBag.ConferenceID = ConferenceID;
            int conferenceID = int.Parse(ConferenceID);
            var context = new ApplicationDbContext();
            var selectedConference = context.Conferences.Where(item => item.ID.Equals(conferenceID)).FirstOrDefault();
            ViewBag.GridID = "ArticleManagementGrid";
            ViewBag.ConferenceName = selectedConference.Title;
            return PartialView("~/Areas/Admin/Views/Csmi/ArticleManagement.cshtml");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Article)]
        public ActionResult HtmlEditorPartial()
        {
            return new HtmlEditorPartialController().HtmlEditorView("HtmlEditor", "OnHtmlChangedChanged", "OnInit");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Conference)]
        public ActionResult GetConferenceArticlePartial(string ConferenceID)
        {
            var ConferenceArticle = new List<ConferenceArticleDetailViewModel>();
            var context = new ApplicationDbContext();
            var model = new object[0];
            int conferenceID = int.Parse(ConferenceID);
            var DbConferenceArticles =
                 context.ConferenceArticles
                .Include(confRef => confRef.Referee)
                .Include(confRef => confRef.Referee.RefrenceUser)
                .Where(confRef => confRef.Conference.ID == conferenceID)
                .ToList().OrderByDescending(confRef => confRef.CreateDate);
            foreach (var item in DbConferenceArticles)
            {
                ConferenceArticle.Add(new ConferenceArticleDetailViewModel()
                {
                    ID = item.ID,
                    RefereeName = item.Referee.RefrenceUser.FirstName + " " + item.Referee.RefrenceUser.LastName,
                    CreateDateConverted = CommonHelper.DateAndTimes.GetPersianDate(item.CreateDate),
                    Enable = item.Enable,

                });
            }
            if (ConferenceArticle.Count() == 0)
            {
                ConferenceArticle.Add(new ConferenceArticleDetailViewModel());
            }
            return new GridViewPartialController().GridViewPartial<ConferenceArticleDetailViewModel>(ConferenceArticle, "ConferenceArticle", "GetConferenceArticlePartial", "ConferenceArticleManagementGrid", "ID");
        }
        [HttpPost]
        [AjaxValidateAntiForgeryTokenAttribute]
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Conference)]
        public ActionResult ActiveConferenceArticle(string[] ids)
        {
            var context = new ApplicationDbContext();
            try
            {
                foreach (var id in ids)
                {
                    int CurrentID = int.Parse(id);
                    var selectedItem = context.ConferenceArticles
                        .Include(item => item.Conference)
                        .Include(item => item.Referee)
                        .Include(item => item.CreatorUser)
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
                return ControllerHelper.ServerErrorResult(ex);
            }


        }
        [HttpPost]
        [AjaxValidateAntiForgeryTokenAttribute]
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult DeleteConferenceArticle(string[] ids)
        {
            var context = new ApplicationDbContext();
            try
            {
                foreach (var id in ids)
                {
                    int ID = int.Parse(id);
                    var selectedItem = context.ConferenceArticles
                        .Where(item => item.ID == ID).Single();
                    if (selectedItem != null)
                    {
                        context.ConferenceArticles.Remove(selectedItem);
                    }

                }
                context.SaveChanges();
                return ControllerHelper.SuccessResult("با موفقیت ثبت شد");
            }
            catch (Exception ex)
            {
                return ControllerHelper.ServerErrorResult(ex);
            }


        }

        #endregion
    }
}