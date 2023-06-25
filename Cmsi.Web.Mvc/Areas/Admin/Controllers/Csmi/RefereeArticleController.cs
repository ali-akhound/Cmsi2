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
using AVA.UI.Helpers.FileUploadManagment;
using System.Web.Script.Serialization;
using AVA.UI.Helpers;
namespace AVA.Web.Mvc.Controllers.Admin
{
    //Optimized
    public class RefereeArticleController : BaseController
    {
        // GET: Admin/RefereeArticle
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.RefereeArticle)]
        public ActionResult RefereeArticle()
        {
            if (Request.QueryString["id"] == null || Request.QueryString["id"] == "undefined")
            {
                return PartialView("~/Areas/Admin/Views/Csmi/RefereeArticle.cshtml", new RefereeArticleViewModel() { ObjectState = ObjectState.Insert });
            }
            else
            {
                var context = new ApplicationDbContext();
                var selectedID = int.Parse(Request.QueryString["id"]);
                var DBrefStatuses = context.RefereeStatuses
                    .Where(refSatus => refSatus.Enable && refSatus.ID != (int)Enums.RefereeStatusType.Checking)
                    .Select(refSatus => new { refSatus.ID, refSatus.Name })
                    .ToList();
                List<RefereeStatusViewModel> refStatuses = new List<RefereeStatusViewModel>();
                foreach (var refStatus in DBrefStatuses)
                {
                    refStatuses.Add(new RefereeStatusViewModel()
                    {
                        ID = refStatus.ID,
                        Name = refStatus.Name
                    });
                }
                var DBArticlePresentTypes = context.ArticlePresentTypes
                    .Where(artPresentType => artPresentType.Enable && artPresentType.ID != (int)Enums.ArticlePresentType.Checking)
                    .Select(artPresentType => new { artPresentType.ID, artPresentType.Name })
                    .ToList();
                List<ArticlePresentTypeViewModel> ArticlePresentTypes = new List<ArticlePresentTypeViewModel>();
                foreach (var artPresentType in DBArticlePresentTypes)
                {
                    ArticlePresentTypes.Add(new ArticlePresentTypeViewModel()
                    {
                        ID = artPresentType.ID,
                        Name = artPresentType.Name
                    });
                }
                string CurrentID = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault().Id;
                List<RefereeQuestionAnswerViewModel> RefereeQuestionAnswers = new List<Models.RefereeQuestionAnswerViewModel>();
                var DBQuestions = context.RefereeQuestions
                    .Where(refQues => refQues.Enable)
                    .Select(refQues => new { refQues.ID, refQues.Question })
                    .ToList();
                foreach (var ques in DBQuestions)
                {
                    var dbAnswers = context.RefereeAnswers.
                        Where(ans => ans.Enable && ans.RefereeQuestion.ID == ques.ID).ToList();
                    List<AnswerViewModel> answers = new List<AnswerViewModel>();
                    foreach (var ans in dbAnswers)
                    {
                        answers.Add(new AnswerViewModel()
                        {
                            Answer = ans.Answer,
                            AnswerID = ans.ID
                        });
                    }
                    RefereeQuestionAnswers.Add(new RefereeQuestionAnswerViewModel()
                    {
                        RefereeQuestionID = ques.ID,
                        RefereeQuestionText = ques.Question,
                        Answers = answers,
                        SelectedAnswerID = answers.Count > 0 ? answers[0].AnswerID : 0
                    });
                }
                var selectedRefereeQuestionAnswer = context.
                 RefereeArticles
                .Include(refArt => refArt.Referee)
                .Include(refArt => refArt.Article)
                .Where(refArt =>
                    refArt.Article.ID.Equals(selectedID)
                    &&
                    !refArt.IsRead
                    &&
                    refArt.Referee.RefrenceUser.Id == CurrentID
                    &&
                    refArt.RefereeStatus.ID == (int)Enums.RefereeStatusType.Checking
                )
                .Select(refArt => new { refArt.ID, RefereeID = refArt.Referee.ID, ArticleID = refArt.Article.ID, refArt.AttachUrl })
                .SingleOrDefault();
                return PartialView("~/Areas/Admin/Views/Csmi/RefereeArticle.cshtml", new RefereeArticleViewModel()
                {

                    ID = selectedRefereeQuestionAnswer.ID,
                    RefereeID = selectedRefereeQuestionAnswer.RefereeID,
                    ArticleID = selectedRefereeQuestionAnswer.ArticleID,
                    SelectedPresentTypeID = (int)Enums.ArticlePresentType.Verbally,
                    SelectedRefereeStatuseID = (int)Enums.RefereeStatusType.Confirmed,
                    RefereeStatuses = refStatuses,
                    ArticlePresentTypes = ArticlePresentTypes,
                    RefereeQuestionAnswers = RefereeQuestionAnswers,
                    AttachUrl = string.IsNullOrEmpty(selectedRefereeQuestionAnswer.AttachUrl) ? "" : selectedRefereeQuestionAnswer.AttachUrl,
                    ObjectState = ObjectState.Update
                });
            }
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.RefereeArticle)]
        public ActionResult RefereeArticleManagement(int Receive)
        {
            var context = new ApplicationDbContext();
            var Statuses = context.ArticleStatuses.Where(status => status.Enable).Select(status => new { status.ID, status.Name }).ToList();
            List<ArticleStatusDprViewModel> ArticleSelectedStatus = new List<ArticleStatusDprViewModel>();
            ArticleSelectedStatus.Add(new ArticleStatusDprViewModel()
            {
                ID = -1,
                Name = "لطفا انتخاب کنید"
            });
            foreach (var status in Statuses)
            {
                ArticleSelectedStatus.Add(new ArticleStatusDprViewModel()
                {
                    ID = status.ID,
                    Name = status.Name
                });
            }
            var Categories = context.Categories.Where(cat => cat.TableName == "Conference" && (bool)cat.Enable).Select(cat => new { cat.ID, cat.Name }).ToList();
            List<ArticleCategoryDprViewModel> ArticleCategories = new List<ArticleCategoryDprViewModel>();
            ArticleCategories.Add(new ArticleCategoryDprViewModel()
            {
                ID = -1,
                Name = "لطفا انتخاب کنید"
            });
            foreach (var cat in Categories)
            {
                ArticleCategories.Add(new ArticleCategoryDprViewModel()
                {
                    ID = cat.ID,
                    Name = cat.Name
                });
            }
            ViewBag.GridID = "RefereeArticleManagementGrid";
            return PartialView("~/Areas/Admin/Views/Csmi/RefereeArticleManagement.cshtml", new ArticleFilterViewModel()
            {
                ArticleSelectedStatus = ArticleSelectedStatus,
                ArticleSelectedStatusID = -1,
                ArticleCategories = ArticleCategories,
                ArticleSelectedCategory = -1,
                SendArticleViewModel = new ArticleSendArticleViewModel() { Referees = new List<RefereeSelective>(), RefereesFeed = new List<RefereeSelective>() },
                IsReceive = Receive
            });
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.RefereeArticle)]
        public ActionResult GetRefereeArticlePartial(ArticleFilterViewModel vm)
        {
            return new GridViewPartialController().GridViewPartial<RefereeArticleViewModel>(Bind(vm), "RefereeArticle", "GetRefereeArticlePartial", "RefereeArticleManagementGrid", "ID");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.RefereeArticle)]
        public ActionResult ExportTo(int ActionType, ArticleFilterViewModel vm)
        {
            return new GridViewPartialController().ExportTo<RefereeArticleViewModel>(Bind(vm), ActionType, "RefereeArticleManagementGrid");
        }
        List<RefereeArticleViewModel> Bind(ArticleFilterViewModel vm)
        {
            DateTime? CreateDateFrom = CommonHelper.DateAndTimes.GetGregorianDate(vm.CreateDateFrom);
            DateTime? CreateDateTo = CommonHelper.DateAndTimes.GetGregorianDate(vm.CreateDateTo);

            var context = new ApplicationDbContext();
            var model = new object[0];

            //refArt.Referee.RefrenceUser == context.Users.Where(user => user.UserName == User.Identity.Name).FirstOrDefault()
            var DbArticles = context.RefereeArticles

                 .Include(refArt => refArt.Referee)
                 .Include(refArt => refArt.Referee.RefrenceUser)
                 .Include(refArt => refArt.Article.CreatorUser)
                 .Include(refArt => refArt.Article.CreatorUser.Person)
                 .Include(refArt => refArt.Article.ArticleStatus)
                 .Where(refArt =>
                 refArt.Referee.RefrenceUser.UserName == User.Identity.Name
                 && ((refArt.RefereeStatus.ID == (int)Enums.RefereeStatusType.Checking && vm.IsReceive == 1) || (refArt.RefereeStatus.ID != (int)Enums.RefereeStatusType.Checking && vm.IsReceive == 0))
                 && context.ConferenceArticles.Count(confArt => confArt.Article.ID == refArt.Article.ID && (bool)confArt.Conference.Enable && (bool)confArt.Conference.Visible) > 0
                 //&& ((refArt.Article.ArticleStatus.ID == (int)Enums.ArticleStatus.Checking && vm.IsReceive == 1) || (vm.IsReceive == 0))
                 && (refArt.Article.Title.Contains(vm.Title) || vm.Title == "" || vm.Title == null)
                 && (refArt.Article.EnglishTitle.Contains(vm.EnglishTitle) || vm.EnglishTitle == "" || vm.EnglishTitle == null)
                 && (refArt.Article.ArticleStatus.ID == vm.ArticleSelectedStatusID || vm.ArticleSelectedStatusID == -1 || vm.ArticleSelectedStatusID == 0)
                 && (refArt.Article.CreateDate >= CreateDateFrom.Value || CreateDateFrom == null)
                 && (refArt.Article.CreateDate <= CreateDateTo.Value || CreateDateTo == null))
                .Select(refArt => new
                {
                    Article = refArt.Article,
                    ArticleStatusName = refArt.Article.ArticleStatus.Name,
                    OwnerName = refArt.Article.CreatorUser.Person.FirstName + " " + refArt.Article.CreatorUser.Person.LastName,
                    OwnerEmail = refArt.Article.CreatorUser.Email,
                    OwnerCellphone = refArt.Article.CreatorUser.PhoneNumber,
                    refArt.Article.CreateDate
                })
                .ToList().OrderByDescending(item => item.CreateDate);

            var Articles = new List<RefereeArticleViewModel>();
            foreach (var item in DbArticles)
            {
                var RefereeArticle = context.RefereeArticles
                .Include(refArt => refArt.ArticlePresentType)
                .Include(refArt => refArt.RefereeStatus)
                .Include(refArt => refArt.Referee)
                .Include(refArt => refArt.Referee.RefrenceUser)
                .Include(refArt => refArt.Referee.RefrenceUser.Person)
                .Where(refArt => refArt.Article.ID == item.Article.ID)
                .Select(refArt => new
                {
                    Status = refArt.RefereeStatus.Name,
                    PresentType = refArt.ArticlePresentType.Name,
                    RefreeName = refArt.Referee.RefrenceUser.Person.FirstName + " " + refArt.Referee.RefrenceUser.Person.LastName
                })
                .ToList();
                RefereeArticleViewModel RefereeArticleVM = new RefereeArticleViewModel()
                {
                    ID = item.Article.ID,
                    Title = item.Article.Title,
                    CreateDateConverted = CommonHelper.DateAndTimes.GetPersianDate(item.Article.CreateDate),
                    Fields = string.Join(",", context.ArticleCategories.Include(ArtCat => ArtCat.Category).Where(ArtCat => ArtCat.Article.ID == item.Article.ID).Select(ArtCat => ArtCat.Category.Name)),
                    RefereeSendTimes = context.RefereeArticles.Count(refArt => refArt.Article.ID == item.Article.ID),
                    OwnerName = item.OwnerName,
                    OwnerEmail = item.OwnerEmail,
                    OwnerCellphone = item.OwnerCellphone,
                    ArticleStatusName = item.ArticleStatusName,
                    EnglishTitle = item.Article.EnglishTitle,
                    FileUrl = Url.Content(item.Article.FileUrl),
                    FileUrlText = "فایل",
                    Keywords = item.Article.Keywords,
                    Summary = item.Article.Summary,
                    Visit = item.Article.Visit,
                    Writers =
                    string.Join(",",
                     context.ArticleWriters
                    .Where(writer => writer.Article.ID == item.Article.ID)
                    .Select(writer => new { Name = writer.FirstName + " " + writer.LastName }).ToList()),
                    Enable = item.Article.Enable,
                };
                Articles.Add(RefereeArticleVM);
            }
            if (Articles.Count() == 0)
            {
                Articles.Add(new RefereeArticleViewModel());
            }
            return Articles;
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.RefereeArticle)]
        public ActionResult HtmlEditorPartial()
        {
            return new HtmlEditorPartialController().HtmlEditorView("HtmlEditor", "OnHtmlChangedChanged", "OnInit");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.RefereeArticle)]
        [AjaxValidateAntiForgeryTokenAttribute]
        public ActionResult SubmitRefereeArticle(string viewmodel)
        {
            try
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                RefereeArticleViewModel vm = jss.Deserialize<RefereeArticleViewModel>(viewmodel);
                try
                {
                    if (vm.ObjectState == ObjectState.Update)//Update
                    {
                        string Attach = "";
                        if (Request.Files.Count > 0)
                        {
                            var file = Request.Files[CommonHelper.ExposeProperty.GetPropertyInfo(vm, m => m.Attach).Name];

                            if (file != null && file.ContentLength > 0)
                            {
                                Attach = FileUploadManagment.UploadFile("~/assets/img/Attach/RefereeArticle/", "", file, FileUploadManagment.AppFileType.Document);
                                if (Attach == "")
                                {
                                    return ControllerHelper.ErrorResult("فرمت یا حجم فایل صحیح نمی باشد");
                                }
                            }
                        }
                        var context = new ApplicationDbContext();
                        if (context.RefereeArticles
                            .Include(refArt => refArt.Article)
                            .Include(refArt => refArt.Referee.RefrenceUser)
                            .Include(refArt => refArt.RefereeStatus)
                            .Count(refArt =>
                            refArt.Article.ID == vm.ArticleID
                            && refArt.Referee.RefrenceUser == context.Users.Where(user => user.UserName == User.Identity.Name).FirstOrDefault()
                            && (refArt.RefereeStatus.ID == (int)Enums.RefereeStatusType.Checking)) == 0)
                        {
                            return ControllerHelper.ErrorResult("شما قبلا برای این مقاله داوری را انجام داده اید");
                        }

                        var trans = context.Database.BeginTransaction();
                        try
                        {
                            foreach (var quesAns in vm.RefereeQuestionAnswers)
                            {
                                if (quesAns.SelectedAnswerID != 0)
                                {
                                    context.RefereeQuestionAnswers.Add(new RefereeQuestionAnswer()
                                    {
                                        RefereeAnswer = context.RefereeAnswers.Where(ans => ans.ID == quesAns.SelectedAnswerID).SingleOrDefault(),
                                        RefereeArticle = context.RefereeArticles.Where(refArt => refArt.ID == vm.ID).SingleOrDefault(),
                                        CreateDate = DateTime.Now,
                                        CreatorUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault()
                                    });
                                }
                            }
                            context.SaveChanges();
                            var selectedModel =
                                context.RefereeArticles
                                .Include(refArt => refArt.Article)
                                .Include(refArt => refArt.Article.ArticleWriters)
                                .Include(refArt => refArt.Article.CreatorUser.Person)
                                .Include(refArt => refArt.ArticlePresentType)
                                .Include(refArt => refArt.Referee)
                                .Include(refArt => refArt.RefereeStatus)
                                .Where(refArt => refArt.ID == vm.ID).Single();
                            if (selectedModel != null)
                            {
                                selectedModel.ArticlePresentType = context.ArticlePresentTypes.Where(presType => presType.ID == vm.SelectedPresentTypeID).SingleOrDefault();
                                selectedModel.RefereeStatus = context.RefereeStatuses.Where(refstatus => refstatus.ID == vm.SelectedRefereeStatuseID).SingleOrDefault();
                                selectedModel.IsRead = true;
                                selectedModel.LastModifyUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault();
                                selectedModel.LastModifyDate = DateTime.Now;
                                selectedModel.Explain = vm.Explain;
                                if (Attach != "")
                                    selectedModel.AttachUrl = Attach;
                            }
                            context.SaveChanges();
                            //var TotalConfirmVots = context.RefereeArticles.Count(refArt => refArt.Article.ID == viewmodel.ArticleID && refArt.RefereeStatus.ID == (int)Enums.RefereeStatusType.Confirmed);
                            //var TotalNotConfirmVots = context.RefereeArticles.Count(refArt => refArt.Article.ID == viewmodel.ArticleID && refArt.RefereeStatus.ID == (int)Enums.RefereeStatusType.NotConfirmed);
                            //var Article = context.Articles
                            //    .Where(art => art.ID == viewmodel.ArticleID).SingleOrDefault();
                            //if(TotalConfirmVots == 2)
                            //{
                            //    Article.ArticleStatus = context.ArticleStatuses.Where(status => status.ID == (int)Enums.ArticleStatus.RefereeConfirmed).SingleOrDefault();
                            //}
                            //else 
                            //if (TotalNotConfirmVots == 2)
                            //{
                            //    Article.ArticleStatus = context.ArticleStatuses.Where(status => status.ID == (int)Enums.ArticleStatus.RefereeNotConfirmed).SingleOrDefault();
                            //}
                            //context.SaveChanges();
                            trans.Commit();
                            //try
                            //{
                            //    EmailService.EmailResponse MailResult = EmailService.SendRefereeResult(
                            //               selectedModel.Article.CreatorUser.Person.FirstName,
                            //               selectedModel.Article.CreatorUser.Person.LastName,
                            //               selectedModel.Article.CreatorUser.UserName,
                            //               selectedModel.Article.CreatorUser.Email,
                            //               selectedModel.Article.Title,
                            //               selectedModel.RefereeStatus.Name + "<br />" + vm.Explain
                            //               );
                            //    foreach (var writer in selectedModel.Article.ArticleWriters)
                            //    {
                            //        EmailService.EmailResponse MailResultWriter = EmailService.SendRefereeResult(
                            //           writer.FirstName,
                            //           writer.LastName,
                            //           "",
                            //           writer.Email,
                            //           selectedModel.Article.Title,
                            //           selectedModel.RefereeStatus.Name + "<br />" + vm.Explain
                            //           );
                            //    }

                            //}
                            //catch (Exception)
                            //{

                            //}

                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
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
            catch (Exception ex)
            {
                return ControllerHelper.ErrorResult("بروز خطای سیستمی");
            }
        }

    }
}