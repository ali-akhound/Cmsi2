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
using AVA.UI.Helpers.MailSmsService;

namespace AVA.Web.Mvc.Controllers.Admin
{
    //Optimized
    public class ConferenceRefereeController : BaseController
    {
        #region ConferenceReferee
        public ActionResult ConferenceRefereeManagement(string ConferenceID)
        {
            ViewBag.ConferenceID = ConferenceID;
            int conferenceID = int.Parse(ConferenceID);
            var context = new ApplicationDbContext();
            var selectedConference = context.Conferences
                .Where(item => item.ID.Equals(conferenceID))
                .Select(item => new { item.Title })
                .SingleOrDefault();
            ViewBag.GridID = "ConferenceRefereeManagementGrid";
            ViewBag.ConferenceName = selectedConference.Title;
            return PartialView("~/Areas/Admin/Views/Csmi/ConferenceRefereeManagement.cshtml");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Conference)]
        public ActionResult HtmlEditorPartial()
        {
            return new HtmlEditorPartialController().HtmlEditorView("HtmlEditor", "OnHtmlChangedChanged", "OnInit");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Conference)]
        public ActionResult GetConferenceRefereePartial(string ConferenceID)
        {
            var ConferenceReferee = new List<ConferenceRefereeDetailViewModel>();
            var context = new ApplicationDbContext();
            var model = new object[0];
            int conferenceID = int.Parse(ConferenceID);
            var DbConferenceReferees =
                 context.ConferenceReferees
                .Include(confRef => confRef.Referee)
                .Include(confRef => confRef.Referee.RefrenceUser)
                .Include(confRef => confRef.Referee.RefrenceUser.Person)
                .Where(confRef => confRef.Conference.ID == conferenceID)
                .Select(confRef => new { confRef.ID, confRef.CreateDate, confRef.Enable, RefereeName = confRef.Referee.RefrenceUser.Person.FirstName + " " + confRef.Referee.RefrenceUser.Person.LastName })
                .ToList().OrderByDescending(confRef => confRef.CreateDate);
            foreach (var item in DbConferenceReferees)
            {
                ConferenceReferee.Add(new ConferenceRefereeDetailViewModel()
                {
                    ID = item.ID,
                    RefereeName = item.RefereeName,
                    CreateDateConverted = CommonHelper.DateAndTimes.GetPersianDate(item.CreateDate),
                    Enable = item.Enable,

                });
            }
            if (ConferenceReferee.Count() == 0)
            {
                ConferenceReferee.Add(new ConferenceRefereeDetailViewModel());
            }
            return new GridViewPartialController().GridViewPartial<ConferenceRefereeDetailViewModel>(ConferenceReferee, "ConferenceReferee", "GetConferenceRefereePartial", "ConferenceRefereeManagementGrid", "ID");
        }
        [HttpPost]
        [AjaxValidateAntiForgeryTokenAttribute]
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Conference)]
        public ActionResult ActiveConferenceReferee(string[] ids)
        {
            var context = new ApplicationDbContext();
            try
            {
                foreach (var id in ids)
                {
                    int CurrentID = int.Parse(id);
                    var selectedItem = context.ConferenceReferees
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
                return ControllerHelper.ErrorResult("بروز خطای سیستمی");
            }


        }
        [HttpPost]
        [AjaxValidateAntiForgeryTokenAttribute]
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Conference)]
        public ActionResult DeleteConferenceReferee(string[] ids)
        {
            var context = new ApplicationDbContext();
            try
            {
                foreach (var id in ids)
                {
                    int ID = int.Parse(id);
                    var selectedItem = context.ConferenceReferees
                        .Where(item => item.ID == ID).SingleOrDefault();
                    if (selectedItem != null)
                    {
                        context.ConferenceReferees.Remove(selectedItem);
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
        public ActionResult ConferenceReferee(string ConferenceID)
        {

            //var context = new ApplicationDbContext();
            //var selectedID = int.Parse(Request.QueryString["Conference"]);
            //var DbConferenceReferees =
            //    context.ConferenceReferees
            //    .Include(confRef => confRef.Referee)
            //    .Include(confRef => confRef.Referee.CreatorUser)
            //    .Where(confRef => confRef.Conference.ID == selectedID && confRef.Referee.Enable).ToList();
            //var Referees = new List<RefereeSelective>();
            return PartialView("~/Areas/Admin/Views/Csmi/ConferenceReferee.cshtml", new ConferenceRefereeViewModel()
            {
                ConferenceID = int.Parse(ConferenceID),
                Referees = new List<RefereeSelective>(),
                RefereesFeed = new List<RefereeSelective>(),
                ObjectState = ObjectState.Insert,
                Enable = true
            });
        }
        [HttpPost]
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Conference)]
        //[AjaxValidateAntiForgeryTokenAttribute]
        public ActionResult ConferenceRefereeFeed(Select2ConferenceRefereeViewModel Input)
        {
            var context = new ApplicationDbContext();
            var ConferenceReferee = new List<RefereeSelective>();
            int ConferenceID = int.Parse(Input.ConferenceID);
            var DBReferees = context.Referees
                 .Include(referee => referee.RefrenceUser)
                 .Include(referee => referee.RefrenceUser.Person)
                 .Where(item =>
                 ((item.RefrenceUser.Person.FirstName + " " + item.RefrenceUser.Person.LastName).Contains(Input.Name) || string.IsNullOrEmpty(Input.Name))
                 && context.ConferenceReferees.Count(ConfRef => ConfRef.Referee.ID == item.ID && ConfRef.Conference.ID == ConferenceID) == 0
                 && (item.Enable))
                 .Select(item => new { item.ID, Name = item.RefrenceUser.Person.FirstName + " " + item.RefrenceUser.Person.LastName })
                 .Take(20)
                 .ToList();
            foreach (var item in DBReferees)
            {
                ConferenceReferee.Add(new RefereeSelective() { ID = item.ID, Name = item.Name });
            }
            return Json(ConferenceReferee, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [AjaxValidateAntiForgeryTokenAttribute]
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Conference)]
        public ActionResult SubmitConferenceReferee(ConferenceRefereeViewModel viewmodel)
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
                            foreach (var referee in viewmodel.Referees)
                            {
                                var refereeObj = context.Referees
                                    .Include(item => item.RefrenceUser)
                                    .Where(item => item.ID == referee.ID).SingleOrDefault();
                                if (context.ConferenceReferees.Count(ConfRef => ConfRef.Conference.ID == selectedConference.ID && ConfRef.Referee.ID == refereeObj.ID) == 0)
                                {
                                    context.ConferenceReferees.Add(new Core.Entities.ConferenceReferee()
                                    {
                                        Conference = selectedConference,
                                        Referee = refereeObj,
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