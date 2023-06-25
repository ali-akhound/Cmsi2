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
    public class ConferenceScientificSecretaryController : BaseController
    {
        #region ConferenceScientificSecretary
        public ActionResult ConferenceScientificSecretaryManagement(string ConferenceID)
        {
            ViewBag.ConferenceID = ConferenceID;
            int conferenceID = int.Parse(ConferenceID);
            var context = new ApplicationDbContext();
            var selectedConference = context.Conferences
                .Where(item => item.ID.Equals(conferenceID))
                .Select(item => new { item.Title })
                .SingleOrDefault();
            ViewBag.GridID = "ConferenceScientificSecretaryManagementGrid";
            ViewBag.ConferenceName = selectedConference.Title;
            return PartialView("~/Areas/Admin/Views/Csmi/ConferenceScientificSecretaryManagement.cshtml");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Conference)]
        public ActionResult HtmlEditorPartial()
        {
            return new HtmlEditorPartialController().HtmlEditorView("HtmlEditor", "OnHtmlChangedChanged", "OnInit");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Conference)]
        public ActionResult GetConferenceScientificSecretaryPartial(string ConferenceID)
        {
            return new GridViewPartialController().GridViewPartial<ConferenceScientificSecretaryDetailViewModel>(Bind(ConferenceID), "ConferenceScientificSecretary", "GetConferenceScientificSecretaryPartial", "ConferenceScientificSecretaryManagementGrid", "ID");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Conference)]
        public ActionResult ExportTo(int ActionType, string ConferenceID)
        {
            return new GridViewPartialController().ExportTo<ConferenceScientificSecretaryDetailViewModel>(Bind(ConferenceID), ActionType, "ConferenceScientificSecretaryManagementGrid");
        }
        List<ConferenceScientificSecretaryDetailViewModel> Bind(string ConferenceID)
        {
            var ConferenceScientificSecretary = new List<ConferenceScientificSecretaryDetailViewModel>();
            var context = new ApplicationDbContext();
            var model = new object[0];
            int conferenceID = int.Parse(ConferenceID);
            var DbConferenceScientificSecretarys =
                 context.ConferenceScientificSecretaries
                .Include(confRef => confRef.ScientificSecretary)
                .Include(confRef => confRef.ScientificSecretary.RefrenceUser)
                .Include(confRef => confRef.ScientificSecretary.RefrenceUser.Person)
                .Where(confRef => confRef.Conference.ID == conferenceID)
                .Select(confRef => new
                {
                    confRef.ID,
                    confRef.CreateDate,
                    confRef.Enable,
                    ScientificSecretaryName = confRef.ScientificSecretary.RefrenceUser.Person.FirstName + " " + confRef.ScientificSecretary.RefrenceUser.Person.LastName
                })
                .ToList().OrderByDescending(confRef => confRef.CreateDate);
            foreach (var item in DbConferenceScientificSecretarys)
            {
                ConferenceScientificSecretary.Add(new ConferenceScientificSecretaryDetailViewModel()
                {
                    ID = item.ID,
                    ScientificSecretaryName = item.ScientificSecretaryName,
                    CreateDateConverted = CommonHelper.DateAndTimes.GetPersianDate(item.CreateDate),
                    Enable = item.Enable,

                });
            }
            if (ConferenceScientificSecretary.Count() == 0)
            {
                ConferenceScientificSecretary.Add(new ConferenceScientificSecretaryDetailViewModel());
            }
            return ConferenceScientificSecretary;
        }
        [HttpPost]
        [AjaxValidateAntiForgeryTokenAttribute]
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Conference)]
        public ActionResult ActiveConferenceScientificSecretary(string[] ids)
        {
            var context = new ApplicationDbContext();
            try
            {
                foreach (var id in ids)
                {
                    int CurrentID = int.Parse(id);
                    var selectedItem = context.ConferenceScientificSecretaries
                        .Include(item => item.Conference)
                        .Include(item => item.ScientificSecretary)
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
                return ControllerHelper.ErrorResult("بروز خطای سیستمی");
            }


        }
        [HttpPost]
        [AjaxValidateAntiForgeryTokenAttribute]
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Conference)]
        public ActionResult DeleteConferenceScientificSecretary(string[] ids)
        {
            var context = new ApplicationDbContext();
            try
            {
                foreach (var id in ids)
                {
                    int ID = int.Parse(id);
                    var selectedItem = context.ConferenceScientificSecretaries
                        .Where(item => item.ID == ID).Single();
                    if (selectedItem != null)
                    {
                        context.ConferenceScientificSecretaries.Remove(selectedItem);
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

        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Conference)]
        public ActionResult ConferenceScientificSecretary(string ConferenceID)
        {

            //var context = new ApplicationDbContext();
            //var selectedID = int.Parse(Request.QueryString["Conference"]);
            //var DbConferenceScientificSecretarys =
            //    context.ConferenceScientificSecretarys
            //    .Include(confRef => confRef.ScientificSecretary)
            //    .Include(confRef => confRef.ScientificSecretary.CreatorUser)
            //    .Where(confRef => confRef.Conference.ID == selectedID && confRef.ScientificSecretary.Enable).ToList();
            //var ScientificSecretarys = new List<ScientificSecretarySelective>();
            return PartialView("~/Areas/Admin/Views/Csmi/ConferenceScientificSecretary.cshtml", new ConferenceScientificSecretaryViewModel()
            {
                ConferenceID = int.Parse(ConferenceID),
                ScientificSecretaries = new List<ScientificSecretarySelective>(),
                ScientificSecretariesFeed = new List<ScientificSecretarySelective>(),
                ObjectState = ObjectState.Insert,
                Enable = true
            });
        }
        [HttpPost]
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Conference)]
        //[AjaxValidateAntiForgeryTokenAttribute]
        public ActionResult ConferenceScientificSecretaryFeed(Select2ConferenceScientificSecretaryViewModel Input)
        {
            var context = new ApplicationDbContext();
            var ConferenceScientificSecretary = new List<ScientificSecretarySelective>();
            int ConferenceID = int.Parse(Input.ConferenceID);
            var DBScientificSecretarys = context.ScientificSecretaries
                 .Include(ScientificSecretary => ScientificSecretary.RefrenceUser)
                 .Include(ScientificSecretary => ScientificSecretary.RefrenceUser.Person)
                 .Where(item =>
                    ((item.RefrenceUser.Person.FirstName + " " + item.RefrenceUser.Person.LastName).Contains(Input.Name) || string.IsNullOrEmpty(Input.Name))
                    && context.ConferenceScientificSecretaries.Count(ConfRef => ConfRef.ScientificSecretary.ID == item.ID && ConfRef.Conference.ID == ConferenceID) == 0
                    && (item.Enable)
                 )
                 .Select(item => new { ID = item.ID, Name = item.RefrenceUser.Person.FirstName + " " + item.RefrenceUser.Person.LastName })
                 .Take(20)
                 .ToList();
            foreach (var item in DBScientificSecretarys)
            {
                ConferenceScientificSecretary.Add(new ScientificSecretarySelective() { ID = item.ID.ToString(), Name = item.Name });
            }
            return Json(ConferenceScientificSecretary, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [AjaxValidateAntiForgeryTokenAttribute]
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Conference)]
        public ActionResult SubmitConferenceScientificSecretary(ConferenceScientificSecretaryViewModel viewmodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        if (viewmodel.ObjectState == ObjectState.Insert)//Update
                        {

                            var context = new ApplicationDbContext();
                            var selectedConference = context.Conferences.Where(item => item.ID.Equals(viewmodel.ConferenceID)).SingleOrDefault();
                            foreach (var ScientificSecretary in viewmodel.ScientificSecretaries)
                            {
                                var ScientificSecretaryID = int.Parse(ScientificSecretary.ID);
                                if (context.ConferenceScientificSecretaries.Count(confSec => confSec.ScientificSecretary.ID == ScientificSecretaryID && confSec.Conference.ID == viewmodel.ConferenceID) == 0)
                                {
                                    context.ConferenceScientificSecretaries.Add(new Core.Entities.ConferenceScientificSecretary()
                                    {
                                        Conference = selectedConference,
                                        ScientificSecretary = context.ScientificSecretaries.Where(item => item.ID == ScientificSecretaryID).SingleOrDefault(),
                                        Enable = true,
                                        CreateDate = DateTime.Now,
                                        CreatorUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault(),
                                    });
                                }
                                else
                                {
                                    return ControllerHelper.ErrorResult("کاربر قبلا برای این همایش تعریف شده شده است");
                                }
                            }
                            context.SaveChanges();
                        }
                    }
                    catch (Exception ex)
                    {
                        return ControllerHelper.ErrorResult("بروز خطای سیستمی");
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

        #endregion
    }
}