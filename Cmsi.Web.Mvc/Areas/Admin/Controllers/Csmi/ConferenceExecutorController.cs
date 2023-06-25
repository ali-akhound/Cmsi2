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
    public class ConferenceExecutorController : BaseController
    {
        #region ConferenceExecutor
        public ActionResult ConferenceExecutorManagement(string ConferenceID)
        {
            ViewBag.ConferenceID = ConferenceID;
            int conferenceID = int.Parse(ConferenceID);
            var context = new ApplicationDbContext();
            var selectedConference = context.Conferences
                .Where(item => item.ID.Equals(conferenceID))
                .Select(item => new { item.Title })
                .SingleOrDefault();
            ViewBag.GridID = "ConferenceExecutorManagementGrid";
            ViewBag.ConferenceName = selectedConference.Title;
            return PartialView("~/Areas/Admin/Views/Csmi/ConferenceExecutorManagement.cshtml");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Conference)]
        public ActionResult HtmlEditorPartial()
        {
            return new HtmlEditorPartialController().HtmlEditorView("HtmlEditor", "OnHtmlChangedChanged", "OnInit");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Conference)]
        public ActionResult GetConferenceExecutorPartial(string ConferenceID)
        {
            return new GridViewPartialController().GridViewPartial<ConferenceExecutorDetailViewModel>(Bind(ConferenceID), "ConferenceExecutor", "GetConferenceExecutorPartial", "ConferenceExecutorManagementGrid", "ID");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Conference)]
        public ActionResult ExportTo(int ActionType, string ConferenceID)
        {
            return new GridViewPartialController().ExportTo<ConferenceExecutorDetailViewModel>(Bind(ConferenceID), ActionType, "ConferenceExecutorManagementGrid");
        }
        List<ConferenceExecutorDetailViewModel> Bind(string ConferenceID)
        {
            var ConferenceExecutor = new List<ConferenceExecutorDetailViewModel>();
            var context = new ApplicationDbContext();
            var model = new object[0];
            int conferenceID = int.Parse(ConferenceID);
            var DbConferenceExecutors =
                 context.ConferenceExecutors
                .Include(confRef => confRef.Executor)
                .Include(confRef => confRef.Executor.RefrenceUser)
                .Include(confRef => confRef.Executor.RefrenceUser.Person)
                .Where(confRef => confRef.Conference.ID == conferenceID)
                .Select(confRef => new
                {
                    confRef.ID,
                    confRef.CreateDate,
                    confRef.Enable,
                    ExecutorName = confRef.Executor.RefrenceUser.Person.FirstName + " " + confRef.Executor.RefrenceUser.Person.LastName
                })
                .ToList().OrderByDescending(confRef => confRef.CreateDate);
            foreach (var item in DbConferenceExecutors)
            {
                ConferenceExecutor.Add(new ConferenceExecutorDetailViewModel()
                {
                    ID = item.ID,
                    ExecutorName = item.ExecutorName,
                    CreateDateConverted = CommonHelper.DateAndTimes.GetPersianDate(item.CreateDate),
                    Enable = item.Enable,

                });
            }
            if (ConferenceExecutor.Count() == 0)
            {
                ConferenceExecutor.Add(new ConferenceExecutorDetailViewModel());
            }
            return ConferenceExecutor;
        }
        [HttpPost]
        [AjaxValidateAntiForgeryTokenAttribute]
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Conference)]
        public ActionResult ActiveConferenceExecutor(string[] ids)
        {
            var context = new ApplicationDbContext();
            try
            {
                foreach (var id in ids)
                {
                    int CurrentID = int.Parse(id);
                    var selectedItem = context.ConferenceExecutors
                        .Include(item => item.Conference)
                        .Include(item => item.Executor)
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
        public ActionResult DeleteConferenceExecutor(string[] ids)
        {
            var context = new ApplicationDbContext();
            try
            {
                foreach (var id in ids)
                {
                    int ID = int.Parse(id);
                    var selectedItem = context.ConferenceExecutors
                        .Where(item => item.ID == ID).Single();
                    if (selectedItem != null)
                    {
                        context.ConferenceExecutors.Remove(selectedItem);
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
        public ActionResult ConferenceExecutor(string ConferenceID)
        {

            //var context = new ApplicationDbContext();
            //var selectedID = int.Parse(Request.QueryString["Conference"]);
            //var DbConferenceExecutors =
            //    context.ConferenceExecutors
            //    .Include(confRef => confRef.Executor)
            //    .Include(confRef => confRef.Executor.CreatorUser)
            //    .Where(confRef => confRef.Conference.ID == selectedID && confRef.Executor.Enable).ToList();
            //var Executors = new List<ExecutorSelective>();
            return PartialView("~/Areas/Admin/Views/Csmi/ConferenceExecutor.cshtml", new ConferenceExecutorViewModel()
            {
                ConferenceID = int.Parse(ConferenceID),
                Executors = new List<ExecutorSelective>(),
                ExecutorsFeed = new List<ExecutorSelective>(),
                ObjectState = ObjectState.Insert,
                Enable = true
            });
        }
        [HttpPost]
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Conference)]
        //[AjaxValidateAntiForgeryTokenAttribute]
        public ActionResult ConferenceExecutorFeed(Select2ConferenceExecutorViewModel Input)
        {
            var context = new ApplicationDbContext();
            var ConferenceExecutor = new List<ExecutorSelective>();
            int ConferenceID = int.Parse(Input.ConferenceID);
            var DBExecutors = context.Executors
                 .Include(Executor => Executor.RefrenceUser)
                 .Include(Executor => Executor.RefrenceUser.Person)
                 .Where(item =>
                    ((item.RefrenceUser.Person.FirstName + " " + item.RefrenceUser.Person.LastName).Contains(Input.Name) || string.IsNullOrEmpty(Input.Name))
                    && context.ConferenceExecutors.Count(ConfRef => ConfRef.Executor.ID == item.ID && ConfRef.Conference.ID == ConferenceID) == 0
                    && (item.Enable)
                 )
                 .Select(item => new { ID = item.ID, Name = item.RefrenceUser.Person.FirstName + " " + item.RefrenceUser.Person.LastName })
                 .Take(20)
                 .ToList();
            foreach (var item in DBExecutors)
            {
                ConferenceExecutor.Add(new ExecutorSelective() { ID = item.ID.ToString(), Name = item.Name });
            }
            return Json(ConferenceExecutor, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [AjaxValidateAntiForgeryTokenAttribute]
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Conference)]
        public ActionResult SubmitConferenceExecutor(ConferenceExecutorViewModel viewmodel)
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
                            foreach (var Executor in viewmodel.Executors)
                            {
                                var ExecutorID = int.Parse(Executor.ID);
                                if (context.ConferenceExecutors.Count(ConfExec => ConfExec.Conference.ID == selectedConference.ID && ConfExec.Executor.ID == ExecutorID) == 0)
                                {

                                    context.ConferenceExecutors.Add(new Core.Entities.ConferenceExecutor()
                                    {
                                        Conference = selectedConference,
                                        Executor = context.Executors.Where(item => item.ID == ExecutorID).SingleOrDefault(),
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