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
    public class RefereeQuestionController : BaseController
    {
        // GET: Admin/RefereeQuestion
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult RefereeQuestion()
        {
            if (Request.QueryString["id"] == null || Request.QueryString["id"] == "undefined")
            {
                return PartialView("~/Areas/Admin/Views/Csmi/RefereeQuestion.cshtml", new RefereeQuestionViewModel()
                {
                    ObjectState = ObjectState.Insert,
                    RefereeAnswers = new List<RefereeAnswerViewModel>(),
                    ActiveEditMode = false
                });
            }
            else
            {
                var context = new ApplicationDbContext();
                var selectedID = int.Parse(Request.QueryString["id"]);
                var DBAnswers = context.RefereeAnswers
                     .Include(Ans => Ans.RefereeQuestion)
                     .Where(Ans => Ans.RefereeQuestion.ID.Equals(selectedID))
                     .Select(Ans => new {
                         Ans.ID,
                         Ans.RefereeQuestion.Question,
                         QuestionID=Ans.RefereeQuestion.ID,
                         Ans.Answer,
                         Ans.Priority,
                         Ans.Enable,
                     })
                     .ToList();
                var RefereeQuestionVw = new RefereeQuestionViewModel();
                RefereeQuestionVw.RefereeAnswers = new List<RefereeAnswerViewModel>();
                RefereeQuestionVw.Question = DBAnswers[0].Question;
                RefereeQuestionVw.ID = DBAnswers[0].QuestionID;
                RefereeQuestionVw.ObjectState = ObjectState.Update;
                RefereeQuestionVw.ActiveEditMode = false;
                foreach (var item in DBAnswers)
                {
                    RefereeQuestionVw.RefereeAnswers.Add(new RefereeAnswerViewModel()
                    {
                        ID = item.ID,
                        Answer = item.Answer,
                        Priority = item.Priority,
                        Enable = item.Enable
                    });

                }
                return PartialView("~/Areas/Admin/Views/Csmi/RefereeQuestion.cshtml", RefereeQuestionVw);
            }
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult RefereeQuestionManagement()
        {
            ViewBag.GridID = "RefereeQuestionManagementGrid";
            return PartialView("~/Areas/Admin/Views/Csmi/RefereeQuestionManagement.cshtml");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult GetRefereeQuestionPartial()
        {
            return new GridViewPartialController().GridViewPartial<RefereeQuestionViewModel>(Bind(), "RefereeQuestion", "GetRefereeQuestionPartial", "RefereeQuestionManagementGrid", "ID");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult ExportTo(int ActionType)
        {
            return new GridViewPartialController().ExportTo<RefereeQuestionViewModel>(Bind(), ActionType, "PollManagementGrid");
        }
        List<RefereeQuestionViewModel> Bind()
        {
            var RefereeQuestions = new List<RefereeQuestionViewModel>();
            var context = new ApplicationDbContext();
            var model = new object[0];
            var DbRefereeQuestions = context.RefereeQuestions
                .Select(ques => new { ques.ID, ques.Question, ques.Enable, ques.Priority, ques.CreateDate })
                .ToList().OrderByDescending(item => item.CreateDate);
            foreach (var item in DbRefereeQuestions)
            {
                RefereeQuestions.Add(new RefereeQuestionViewModel()
                {
                    ID = item.ID,
                    Question = item.Question,
                    Priority = item.Priority,
                    CreateDateConverted = CommonHelper.DateAndTimes.GetPersianDate(item.CreateDate),
                    Enable = item.Enable,

                });
            }
            if (RefereeQuestions.Count() == 0)
            {
                RefereeQuestions.Add(new RefereeQuestionViewModel());
            }
            return RefereeQuestions;
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult HtmlEditorPartial()
        {
            return new HtmlEditorPartialController().HtmlEditorView("HtmlEditor", "OnHtmlChangedChanged", "OnInit");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        [AjaxValidateAntiForgeryTokenAttribute]
        public ActionResult SubmitRefereeQuestion(RefereeQuestionViewModel viewmodel)
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
                            var dbContextTransaction = context.Database.BeginTransaction();
                            try
                            {
                                var question = new RefereeQuestion()
                                {
                                    Question = viewmodel.Question,
                                    Priority = viewmodel.Priority,
                                    Enable = true,
                                    CreateDate = DateTime.Now,
                                    CreatorUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault(),

                                };
                                context.RefereeQuestions.Add(question);
                                context.SaveChanges();
                                foreach (var ans in viewmodel.RefereeAnswers)
                                {
                                    context.RefereeAnswers
                                    .Add(new RefereeAnswer
                                    {
                                        RefereeQuestion = context.RefereeQuestions.Where(ques => ques.ID == question.ID).SingleOrDefault(),
                                        Answer = ans.Answer,
                                        Priority = ans.Priority,
                                        Enable = true,
                                        CreateDate = DateTime.Now,
                                        CreatorUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault(),

                                    });
                                }
                                context.SaveChanges();
                                dbContextTransaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                dbContextTransaction.Rollback();
                                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.Message);
                            }

                        }
                        else if (viewmodel.ObjectState == ObjectState.Update)//Update
                        {
                            var context = new ApplicationDbContext();
                            var dbContextTransaction = context.Database.BeginTransaction();
                            try
                            {
                                RefereeQuestion selectedModel = context.RefereeQuestions.Where(item => item.ID == viewmodel.ID).Single();
                                if (selectedModel != null)
                                {
                                    selectedModel.Question = viewmodel.Question;
                                    selectedModel.Priority = viewmodel.Priority;
                                    selectedModel.LastModifyUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault();
                                    selectedModel.LastModifyDate = DateTime.Now;
                                }
                                context.SaveChanges();
                                dbContextTransaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                dbContextTransaction.Rollback();
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
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult SubmitRefereeAnswer(RefereeAnswerViewModel viewmodel)
        {

            try
            {
                var context = new ApplicationDbContext();
                try
                {

                    if (viewmodel.ObjectState == ObjectState.Insert)
                    {
                        var model = new RefereeAnswer
                        {
                            RefereeQuestion = context.RefereeQuestions.Where(ques => ques.ID == viewmodel.QuestionID).SingleOrDefault(),
                            Answer = viewmodel.Answer,
                            Priority = viewmodel.Priority,
                            Enable = viewmodel.Enable == null ? false : (bool)viewmodel.Enable,
                            CreateDate = DateTime.Now,
                            CreatorUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault(),

                        };
                        context.RefereeAnswers
                        .Add(model);
                        context.SaveChanges();
                        RefereeAnswer Vm = new RefereeAnswer()
                        {
                            ID = model.ID,
                            Answer = model.Answer,
                            Priority = model.Priority,
                        };
                        return new JsonResult()
                        {
                            Data = new { Type = "Success", Data = Vm }
                        };
                    }
                    else
                    {
                        var selectedAnswer = context.RefereeAnswers
                            .Include(answ => answ.RefereeQuestion)
                            .Where(answ => answ.ID == viewmodel.ID).SingleOrDefault();
                        selectedAnswer.Answer = viewmodel.Answer;
                        selectedAnswer.Priority = viewmodel.Priority;
                        selectedAnswer.Enable = viewmodel.Enable == null ? false : (bool)viewmodel.Enable;
                        selectedAnswer.LastModifyDate = DateTime.Now;
                        selectedAnswer.LastModifyUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault();
                        context.SaveChanges();
                        RefereeAnswer Vm = new RefereeAnswer()
                        {
                            ID = selectedAnswer.ID,
                            Answer = selectedAnswer.Answer,
                            Priority = selectedAnswer.Priority,
                            Enable = selectedAnswer.Enable
                        };
                        return new JsonResult()
                        {
                            Data = new { Type = "Success", Data = Vm }
                        };
                    }
                }
                catch (Exception ex)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.Message);
                }

            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpPost]
        [AjaxValidateAntiForgeryTokenAttribute]
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult ActiveRefereeQuestion(string[] ids)
        {
            var context = new ApplicationDbContext();
            try
            {
                foreach (var id in ids)
                {
                    int CurrentID = int.Parse(id);
                    var selectedItem = context.RefereeQuestions
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
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult DeleteRefereeQuestion(string[] ids)
        {
            var context = new ApplicationDbContext();
            try
            {
                foreach (var id in ids)
                {
                    int ID = int.Parse(id);
                    var selectedItem = context.RefereeQuestions
                        .Where(item => item.ID == ID).Single();
                    if (selectedItem != null)
                    {
                        context.RefereeQuestions.Remove(selectedItem);
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
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult DeleteRefereeAnswer(string[] ids)
        {
            var context = new ApplicationDbContext();
            try
            {
                foreach (var id in ids)
                {
                    int ID = int.Parse(id);
                    var selectedItem = context.RefereeAnswers
                        .Where(item => item.ID == ID).Single();
                    if (selectedItem != null)
                    {
                        context.RefereeAnswers.Remove(selectedItem);
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
    }
}