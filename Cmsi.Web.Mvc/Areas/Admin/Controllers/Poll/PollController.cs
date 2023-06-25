using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using AVA.Web.Mvc.Models.Admin;
using AVA.Core.Entities;
using AVA.UI.Helpers.Common;
using AVA.UI.Helpers.Controller;
using AVA.UI.Helpers.CustomAttribute;
using DevExpress.Web.Mvc;

namespace AVA.Web.Mvc.Controllers.Admin
{
    //Optimized
    public class PollController : BaseController
    {
        // GET: Admin/Poll
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Poll)]
        public ActionResult PollManagement()
        {
            ViewBag.GridID = "PollManagementGrid";
            return PartialView("~/Areas/Admin/Views/Poll/PollManagement.cshtml");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Poll)]
        public ActionResult GetPollPartial()
        {
            return new GridViewPartialController().GridViewPartial<PollQuestionViewModel>(Bind(), "Poll", "GetPollPartial", "PollManagementGrid", "ID");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Poll)]
        public ActionResult ExportTo(int ActionType)
        {
            return new GridViewPartialController().ExportTo<PollQuestionViewModel>(Bind(), ActionType, "PollManagementGrid");
        }
        List<PollQuestionViewModel> Bind()
        {
            var context = new ApplicationDbContext();
            var model = new object[0];
            var DbPollQuestions = context.PollQuestions
                .Include(ques => ques.PollAnswers)
                .Where(ques =>
                 (bool)ques.Poll.Enable
                &&
                    ques.Poll.StartDate <= DateTime.Now
                &&
                    DateTime.Now <= ques.Poll.EndDate

                )
                .Select(item => new
                {
                    item.ID,
                    item.Question,
                    item.PollAnswers,
                    item.Priority,
                    item.CreateDate
                })
            .ToList()
            .OrderByDescending(item => item.Priority);
            var PollQuestions = new List<PollQuestionViewModel>();
            var DbQuesAns = context.PollQuestionAnswers
                .Where(quesAns =>
                 (bool)quesAns.PollAnswer.PollQuestion.Poll.Enable
                &&
                    quesAns.PollAnswer.PollQuestion.Poll.StartDate <= DateTime.Now
                &&
                    DateTime.Now <= quesAns.PollAnswer.PollQuestion.Poll.EndDate
                ).ToList();
            foreach (var item in DbPollQuestions)
            {
                var PollQuestion = new PollQuestionViewModel()
                {
                    ID = item.ID,
                    Question = item.Question,
                    CreateDateConverted = CommonHelper.DateAndTimes.GetPersianDate(item.CreateDate)
                };
                foreach (var ans in item.PollAnswers)
                {
                    PollQuestion.Result += ans.Answer + " تعداد: " + DbQuesAns.Count(quesAns => quesAns.PollAnswer.ID == ans.ID) + " ";
                }
                PollQuestions.Add(PollQuestion);
            }
            if (PollQuestions.Count() == 0)
            {
                PollQuestions.Add(new PollQuestionViewModel());
            }
            return PollQuestions;
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Poll)]
        public ActionResult HtmlEditorPartial()
        {
            return new HtmlEditorPartialController().HtmlEditorView("HtmlEditor", "OnHtmlChangedChanged", "OnInit");
        }
    }
}