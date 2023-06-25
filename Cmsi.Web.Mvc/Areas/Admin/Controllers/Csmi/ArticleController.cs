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
using static AVA.Web.Mvc.Models.BaseViewModel;
using System.IO;
using System.IO.Compression;
using AVA.UI.Helpers;
using AVA.UI.Helpers.FileUploadManagment;
using System.Web.Script.Serialization;

namespace AVA.Web.Mvc.Controllers.Admin
{

    //Optimized
    public class ArticleController : BaseController
    {
        // GET: Admin/Article
        [AuthorizeUser(AccessLevelIDs = new int[] { (int)ControllerHelper.SysModuleType.Article, (int)ControllerHelper.SysModuleType.ScientificSecretaryCircle })]
        public ActionResult Article()
        {
            if (Request.QueryString["id"] == null || Request.QueryString["id"] == "undefined")
            {
                return PartialView("~/Areas/Admin/Views/Csmi/Article.cshtml", new ArticleViewModel() { ObjectState = ObjectState.Insert });
            }
            else
            {
                var context = new ApplicationDbContext();
                #region Bind Article
                var selectedID = int.Parse(Request.QueryString["id"]);
                var selectedArticle = context.Articles
                    .Include(item => item.ArticleStatus)
                    .Where(item => item.ID.Equals(selectedID))
                    .Select(item => new
                    {
                        ArticleStatusName = item.ArticleStatus.Name,
                        item.ID,
                        item.Title,
                        item.EnglishTitle,
                        item.FileUrl,
                        item.FileWordUrl,
                        item.ShortImageUrl,
                        item.LongImageUrl,
                        item.Keywords,
                        item.Visible,
                        item.Visit,
                        item.Summary,
                        item.ArticleWriters,
                        item.Enable,
                        item.CreateDate,
                        LanguageID = item.Language.ID
                    })
                    .SingleOrDefault();
                var Languages = new List<DropDownVm>();
                var DbLanguages = context.Languages
                    .Select(lang => new { lang.ID, lang.Name })
                    .ToList();
                foreach (var item in DbLanguages)
                {
                    Languages.Add(new DropDownVm()
                    {
                        Text = item.Name,
                        Value = item.ID.ToString()
                    });
                }
                var Fields = new List<CheckBoxVm>();
                var DBcat = context.Categories
                    .Where(cat => cat.TableName == "Conference")
                    .Select(cat => new { cat.ID, cat.Name })
                    .ToList();

                foreach (var item in DBcat)
                {
                    Fields.Add(new CheckBoxVm()
                    {
                        IsChecked = context.ArticleCategories.Count(artCat => artCat.Article.ID == selectedID && artCat.Category.ID == item.ID) > 0 ? true : false,
                        Text = item.Name,
                        Value = item.ID.ToString()
                    });
                }
                #endregion
                #region Bind Writers
                var DBWriters = context.ArticleWriters
                      .Where(writer => writer.Article.ID == selectedArticle.ID)
                      .Select(writer => new
                      {
                          writer.ID,
                          writer.FirstName,
                          writer.LastName,
                          writer.Email,
                          writer.Cellphone,
                      }).ToList();
                List<WriterViewModel> writers = new List<WriterViewModel>();
                foreach (var writer in DBWriters)
                {
                    writers.Add(new WriterViewModel()
                    {
                        FirstName = writer.FirstName,
                        LastName = writer.LastName,
                        Cellphone = writer.Cellphone,
                        Email = writer.Email,
                        ID = writer.ID
                    });
                }
                #endregion
                #region Bind Referee Questions
                List<RefereeArticleResultViewModel> RefereeArticleResults = new List<RefereeArticleResultViewModel>();
                var DBRefereeArticles =
                    context.RefereeArticles
                        .Include(refArt => refArt.Referee.RefrenceUser.Person)
                        .Include(refArt => refArt.Article)
                        .Include(refArt => refArt.ArticlePresentType)
                        .Include(refArt => refArt.RefereeStatus)
                        .Where(refArt =>
                            refArt.Article.ID.Equals(selectedID)
                        )
                .Select(refArt => new
                {
                    RefereeArticleID = refArt.ID,
                    RefereeID = refArt.Referee.ID,
                    ArticleID = refArt.Article.ID,
                    RefereeFirstName = refArt.Referee.RefrenceUser.Person.FirstName,
                    RefereeLastName = refArt.Referee.RefrenceUser.Person.LastName,
                    Explain = refArt.Explain,
                    AttachUrl = refArt.AttachUrl,
                    RefereeStatus = refArt.RefereeStatus.Name,
                    ArticlePresentType = refArt.ArticlePresentType.Name,
                }).ToList();

                foreach (var refArt in DBRefereeArticles)
                {
                    var DBQuestions = context.RefereeQuestions
                      .Where(refQues => refQues.Enable)
                      .Select(refQues => new { refQues.ID, refQues.Question })
                      .ToList();
                    List<RefereeQuestionAnswerViewModel> RefereeQuestionAnswers = new List<Models.RefereeQuestionAnswerViewModel>();
                    foreach (var ques in DBQuestions)
                    {
                        var dbAnswers = context.RefereeAnswers.
                            Where(ans => ans.Enable && ans.RefereeQuestion.ID == ques.ID).ToList();
                        var SelectedAnswerID = context.RefereeQuestionAnswers
                             .Include(refQuesAns => refQuesAns.RefereeArticle)
                             .Include(refQuesAns => refQuesAns.RefereeAnswer.RefereeQuestion)
                             .Where(refQuesAns =>
                                 refQuesAns.RefereeArticle.ID == refArt.RefereeArticleID
                                 &&
                                 refQuesAns.RefereeAnswer.RefereeQuestion.ID == ques.ID
                             )
                             .Select(refQuesAns => refQuesAns.RefereeAnswer.ID).FirstOrDefault();
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
                            SelectedAnswerID = SelectedAnswerID != 0 ? SelectedAnswerID : 0
                        });
                    }

                    RefereeArticleResults.Add(new RefereeArticleResultViewModel()
                    {
                        RefereeArticleID = refArt.RefereeArticleID,
                        ArticleID = selectedID,
                        RefereeQuestionAnswers = RefereeQuestionAnswers,
                        RefereeName = refArt.RefereeFirstName,
                        RefereeFamily = refArt.RefereeLastName,
                        RefereeID = refArt.RefereeID,
                        Explain = refArt.Explain,
                        AttachUrl = string.IsNullOrEmpty(refArt.AttachUrl) ? "" : Url.Content(refArt.AttachUrl),
                        ArticlePresentType = refArt.ArticlePresentType,
                        RefereeStatuse = refArt.RefereeStatus,
                    });
                }


                #endregion
                return PartialView("~/Areas/Admin/Views/Csmi/Article.cshtml", new ArticleViewModel()
                {
                    ID = selectedArticle.ID,
                    Title = selectedArticle.Title,
                    CreateDateConverted = CommonHelper.DateAndTimes.GetPersianDate(selectedArticle.CreateDate),
                    ArticleStatusName = selectedArticle.ArticleStatusName,
                    EnglishTitle = selectedArticle.EnglishTitle,
                    FileUrl = selectedArticle.FileUrl,
                    FileWordUrl = selectedArticle.FileWordUrl,
                    Keywords = selectedArticle.Keywords,
                    ArticleFileUrl = selectedArticle.FileUrl,
                    LongImageUrl = selectedArticle.LongImageUrl,
                    ShortImageUrl = selectedArticle.ShortImageUrl,
                    Summary = selectedArticle.Summary,
                    Visit = selectedArticle.Visit,
                    Writers = writers,
                    WriterNames = string.Join(",",
                     context.ArticleWriters
                    .Where(writer => writer.Article.ID == selectedArticle.ID)
                    .Select(writer => new { Name = writer.FirstName + " " + writer.LastName }).ToList()),
                    RefereeArticleResults = RefereeArticleResults,
                    Enable = selectedArticle.Enable,
                    Languages = Languages,
                    ArticleLanguageSelectedID = selectedArticle.LanguageID.ToString(),
                    Fields = Fields,
                    ConferenceID = context.ConferenceArticles.Where(confArt => confArt.Article.ID == selectedID).Select(confArt => confArt.Conference.ID).FirstOrDefault(),
                    ObjectState = ObjectState.Update
                });
            }
        }
        [AuthorizeUser(AccessLevelIDs = new int[] { (int)ControllerHelper.SysModuleType.Article, (int)ControllerHelper.SysModuleType.ScientificSecretaryCircle })]
        public ActionResult ArticleManagement(int ConferenceID)
        {
            ViewBag.Conference = ConferenceID;
            var context = new ApplicationDbContext();
            var selectedConference = context.Conferences.Where(item => item.ID.Equals(ConferenceID)).Select(item => new { item.Title }).SingleOrDefault();
            ViewBag.GridID = "ArticleManagementGrid";
            if (ConferenceID != -1)
                ViewBag.ConferenceName = selectedConference.Title;
            else
                ViewBag.ConferenceName = "";
            var Statuses = context.ArticleStatuses.Where(status => status.Enable && status.ID != (int)Enums.ArticleStatus.ResendByUser)
                .Select(status => new { status.ID, status.Name }).ToList();
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
            List<DropDownVm> ArticlePresentTypes = new List<DropDownVm>();
            var DbArticlePresentTypes = context.ArticlePresentTypes
                .Where(present => present.Enable)
                .Select(present => new
                {
                    present.ID,
                    present.Name
                })
                .ToList();
            foreach (var presentType in DbArticlePresentTypes)
            {
                ArticlePresentTypes.Add(new Models.BaseViewModel.DropDownVm()
                {
                    Text = presentType.Name,
                    Value = presentType.ID.ToString()
                });
            }
            List<DropDownVm> ArticleStatuses = new List<DropDownVm>();
            var DbStatuses = context.ArticleStatuses
                .Where(Status => Status.Enable && Status.ID != (int)Enums.ArticleStatus.Checking && Status.ID != (int)Enums.ArticleStatus.ResendByUser)
                .Select(Status => new
                {
                    Status.ID,
                    Status.Name
                })
                .ToList();
            foreach (var status in DbStatuses)
            {
                ArticleStatuses.Add(new Models.BaseViewModel.DropDownVm()
                {
                    Text = status.Name,
                    Value = status.ID.ToString()
                });
            }

            return PartialView("~/Areas/Admin/Views/Csmi/ArticleManagement.cshtml", new ArticleFilterViewModel()
            {
                ArticleSelectedStatus = ArticleSelectedStatus,
                ConferenceID = ConferenceID,
                SendArticleViewModel = new ArticleSendArticleViewModel() { ConferenceID = ConferenceID, Referees = new List<RefereeSelective>(), RefereesFeed = new List<RefereeSelective>() },
                ArticleSelectedStatusID = -1,
                ArticleCategories = ArticleCategories,
                ArticleSelectedCategory = -1,
                ArticleStatusTypeViewModel = new ArticleStatusTypeViewModel() { ArticlePresentTypes = ArticlePresentTypes, SelectedPresentTypeID = ArticlePresentTypes[1].Value, ArticleStatuses = ArticleStatuses, SelectedStatusID = ArticleStatuses[0].Value },
            });
        }
        [HttpPost]
        [AuthorizeUser(AccessLevelIDs = new int[] { (int)ControllerHelper.SysModuleType.Article, (int)ControllerHelper.SysModuleType.ScientificSecretaryCircle })]
        public ActionResult ConferenceRefereeFeed(Select2ConferenceRefereeViewModel Input)
        {
            var context = new ApplicationDbContext();
            var ConferenceReferee = new List<RefereeSelective>();
            int ConferenceID = int.Parse(Input.ConferenceID);
            foreach (var item in context.Referees
                .Include(referee => referee.RefrenceUser)
                .Include(referee => referee.RefrenceUser.Person)
                .Where(item =>
                ((item.RefrenceUser.Person.FirstName + " " + item.RefrenceUser.Person.LastName).Contains(Input.Name) || string.IsNullOrEmpty(Input.Name))
                && context.ConferenceReferees.Count(ConfRef => ConfRef.Referee.ID == item.ID && ConfRef.Conference.ID == ConferenceID) > 0
                && (item.Enable))
                .Select(item => new { item.ID, Name = item.RefrenceUser.Person.FirstName + " " + item.RefrenceUser.Person.LastName })
                .Take(20)
                .ToList()
                )
            {
                ConferenceReferee.Add(new RefereeSelective() { ID = item.ID, Name = item.Name });
            }
            return Json(ConferenceReferee, JsonRequestBehavior.AllowGet);
        }
        [AuthorizeUser(AccessLevelIDs = new int[] { (int)ControllerHelper.SysModuleType.Article, (int)ControllerHelper.SysModuleType.ScientificSecretaryCircle })]
        public ActionResult GetArticlePartial(ArticleFilterViewModel vm)
        {
            return new GridViewPartialController().GridViewPartial<ArticleViewModel>(Bind(vm), "Article", "GetArticlePartial", "ArticleManagementGrid", "ID");
        }
        [AuthorizeUser(AccessLevelIDs = new int[] { (int)ControllerHelper.SysModuleType.Article, (int)ControllerHelper.SysModuleType.ScientificSecretaryCircle })]
        public ActionResult ExportTo(int ActionType, ArticleFilterViewModel vm)
        {
            return new GridViewPartialController().ExportTo<ArticleViewModel>(Bind(vm), ActionType, "ArticleManagementGrid");
        }
        List<ArticleViewModel> Bind(ArticleFilterViewModel vm)
        {
            DateTime? CreateDateFrom = CommonHelper.DateAndTimes.GetGregorianDate(vm.CreateDateFrom);
            DateTime? CreateDateTo = CommonHelper.DateAndTimes.GetGregorianDate(vm.CreateDateTo);
            var Articles = new List<ArticleViewModel>();
            var context = new ApplicationDbContext();
            // Should check Conference ID
            var model = new object[0];
            var DbArticles = context.ConferenceArticles
                .Include(confArt => confArt.Article)
                .Include(confArt => confArt.Article.CreatorUser)
                .Include(confArt => confArt.Article.CreatorUser.Person)
                .Include(confArt => confArt.Article.ArticleStatus)
                .Include(confArt => confArt.Article.ArticlePresentType)
                .Where(confArt =>
                confArt.Conference.ID == vm.ConferenceID || vm.ConferenceID == -1
                &&
                (confArt.Article.Title.Contains(vm.Title) || vm.Title == "" || vm.Title == null)
                &&
                (confArt.Article.EnglishTitle.Contains(vm.EnglishTitle) || vm.EnglishTitle == "" || vm.EnglishTitle == null)
                &&
                (confArt.Article.ArticleStatus.ID == vm.ArticleSelectedStatusID || vm.ArticleSelectedStatusID == -1 || vm.ArticleSelectedStatusID == 0)
                &&
                (confArt.Article.CreateDate >= CreateDateFrom.Value || CreateDateFrom == null)
                &&
                (confArt.Article.CreateDate <= CreateDateTo.Value || CreateDateTo == null)
                )
               .Select(conf => new
               {
                   Article = conf.Article,
                   ArticleStatusName = conf.Article.ArticleStatus.Name,
                   ArticlePresentTypeName = conf.Article.ArticlePresentType.Name,
                   ArticleStatusID = conf.Article.ArticleStatus.ID,
                   OwnerName = conf.Article.CreatorUser.Person.FirstName + " " + conf.Article.CreatorUser.Person.LastName,
                   OwnerEmail = conf.Article.CreatorUser.Email,
                   OwnerCellphone = conf.Article.CreatorUser.PhoneNumber,
                   VipStartDate = conf.Article.CreatorUser.VipStartDate,
                   VipEndDate = conf.Article.CreatorUser.VipEndDate,
                   conf.CreateDate
               })
               .OrderByDescending(confArt => confArt.Article.CreateDate)
               .ToList();

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
                    StatusName = refArt.RefereeStatus.Name,
                    PresentType = refArt.ArticlePresentType.Name,
                    RefreeName = refArt.Referee.RefrenceUser.Person.FirstName + " " + refArt.Referee.RefrenceUser.Person.LastName
                })
                .ToList();
                ArticleViewModel ArticleVm = new ArticleViewModel()
                {
                    ID = item.Article.ID,
                    Title = item.Article.Title + "<br />عنوان انگلیسی:" + item.Article.EnglishTitle + "<br /> نام نویسنده:" + item.OwnerName,
                    RegisterationStatus = (item.VipStartDate != null ? CommonHelper.DateAndTimes.GetPersianDate(item.VipStartDate) : "") + "-" + (item.VipEndDate != null ? CommonHelper.DateAndTimes.GetPersianDate(item.VipEndDate) : ""),
                    CreateDateConverted = CommonHelper.DateAndTimes.GetPersianDate(item.Article.CreateDate),
                    FieldNames = string.Join(",", context.ArticleCategories.Include(ArtCat => ArtCat.Category).Where(ArtCat => ArtCat.Article.ID == item.Article.ID).Select(ArtCat => ArtCat.Category.Name)),
                    OwnerName = item.OwnerName,
                    OwnerEmail = item.OwnerEmail,
                    OwnerCellphone = item.OwnerCellphone,
                    ArticleStatusName = item.ArticleStatusName,
                    ArticlePresentTypeName = item.ArticlePresentTypeName,
                    ArticleStatusID = item.ArticleStatusID,
                    EnglishTitle = item.Article.EnglishTitle,
                    FileUrl = Url.Content(item.Article.FileUrl),
                    FileUrlText = "فایل",
                    FileWordUrl = !string.IsNullOrEmpty(item.Article.FileWordUrl) ? Url.Content(item.Article.FileWordUrl) : "",
                    FileWordUrlText = "فایل",
                    Keywords = item.Article.Keywords,
                    LongImageUrl = item.Article.LongImageUrl,
                    ShortImageUrl = item.Article.ShortImageUrl,
                    Summary = item.Article.Summary,
                    Visit = item.Article.Visit,
                    WriterNames = string.Join(",",
                    context.ArticleWriters
                    .Where(writer => writer.Article.ID == item.Article.ID)
                    .Select(writer => (string)(writer.FirstName + " " + writer.LastName + "_ایمیل:" + writer.Email + "_همراه:" + writer.Cellphone + (writer.IsMainWriter ? "_:نویسنده مسئول" : ""))).ToList()),
                    Enable = item.Article.Enable,
                    Published = item.Article.Published
                };
                foreach (var referee in RefereeArticle)
                {
                    //check Referee
                    ArticleVm.RefereeState += "نام داور:" + referee.RefreeName + "--وضعیت:" + referee.StatusName + "--نحوه ارائه:" + referee.PresentType + "<br />";
                }
                Articles.Add(ArticleVm);
            }
            if (Articles.Count() == 0)
            {
                Articles.Add(new ArticleViewModel());
            }

            return Articles;
        }
        [HttpPost]
        [AjaxValidateAntiForgeryTokenAttribute]
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Article)]
        public ActionResult ActiveArticle(string[] ids)
        {
            var context = new ApplicationDbContext();
            try
            {
                foreach (var id in ids)
                {
                    int CurrentID = int.Parse(id);
                    var selectedItem = context.Articles
                        .Where(item => item.ID == CurrentID).Single();
                    if (selectedItem != null)
                    {
                        selectedItem.Enable = !selectedItem.Enable;
                        selectedItem.Visible = selectedItem.Published = selectedItem.Enable;
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
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Article)]
        public ActionResult PublishArticle(string[] ids)
        {
            var context = new ApplicationDbContext();
            try
            {
                foreach (var id in ids)
                {
                    int CurrentID = int.Parse(id);
                    var selectedItem = context.Articles
                        .Include(item => item.ArticleStatus)
                        .Where(item => item.ID == CurrentID).Single();
                    if (selectedItem != null)
                    {
                        if (selectedItem.ArticleStatus.ID == (int)Enums.ArticleStatus.Confirmed)
                        {
                            selectedItem.Published = !selectedItem.Published;
                            selectedItem.Visible = selectedItem.Published;
                        }
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
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.ScientificSecretaryCircle)]
        [AjaxValidateAntiForgeryTokenAttribute]
        public ActionResult SendArticle(ArticleSendArticleViewModel vm)
        {
            var context = new ApplicationDbContext();
            var trans = context.Database.BeginTransaction();
            try
            {
                var refArray = vm.Referees.Select(r => r.ID).ToList();
                var DBReferees = context.ConferenceReferees
                     .Include(confref => confref.Referee.RefrenceUser.Person)
                     .Where(confref => confref.Conference.ID == vm.ConferenceID && refArray.Contains(confref.Referee.ID))
                     .Select(confref => new { confref.Referee, confref.Referee.RefrenceUser.Person, confref.Referee.RefrenceUser.Email, confref.Referee.RefrenceUser.UserName }).ToList();
                int ConfirmCnt = 0;
                int NotConfirmCnt = 0;
                foreach (var articleID in vm.selectedArticleIDs)
                {
                    foreach (var referee in DBReferees)
                    {
                        var Article = context.Articles
                            .Include(art => art.ArticleStatus)
                            .Include(art => art.CreatorUser.Person)
                            .Where(art => art.ID == articleID).SingleOrDefault();
                        if (Article.CreatorUser.VipEndDate != null)
                        {
                            //return ControllerHelper.ErrorResult("عضویت شما در انجمن به پایان رسیده است لطفا عضویت خود را تمدید کنید");
                            if (Article.CreatorUser.VipEndDate > DateTime.Now)
                            {
                                //return ControllerHelper.ErrorResult("عضویت شما در انجمن به پایان رسیده است لطفا عضویت خود را تمدید کنید");
                                var cnt = context.RefereeArticles.Where(refArt =>
                                refArt.Article.ID == articleID
                                && refArt.Referee.ID == referee.Referee.ID
                                && (
                                    refArt.RefereeStatus.ID == (int)Enums.RefereeStatusType.Checking ||
                                    refArt.RefereeStatus.ID == (int)Enums.RefereeStatusType.Confirmed ||
                                    (refArt.RefereeStatus.ID == (int)Enums.RefereeStatusType.NotConfirmed && !refArt.IsRead)
                                   )).Count();
                                if (
                                    cnt == 0
                                    &&
                                    Article.ArticleStatus.ID != (int)Enums.ArticleStatus.Confirmed
                                    &&
                                    Article.ArticleStatus.ID != (int)Enums.ArticleStatus.RefereeConfirmed
                                    )
                                {
                                    var RefArticle = new RefereeArticle()
                                    {
                                        Referee = referee.Referee,
                                        Article = Article,
                                        IsRead = false,
                                        ArticlePresentType = context.ArticlePresentTypes.Where(PresentType => PresentType.ID == (int)Enums.ArticlePresentType.Checking).SingleOrDefault(),
                                        RefereeStatus = context.RefereeStatuses.Where(status => status.ID == (int)Enums.ArticlePresentType.Checking).SingleOrDefault(),
                                        CreateDate = DateTime.Now,
                                        CreatorUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault()
                                    };
                                    context.RefereeArticles.Add(RefArticle);
                                    ConfirmCnt++;
                                }
                            }
                            else
                            {
                                NotConfirmCnt++;
                            }
                        }
                        else
                        {
                            NotConfirmCnt++;
                        }

                    }
                }
                context.SaveChanges();
                trans.Commit();
                foreach (var referee in DBReferees)
                {
                    var cnt = context.RefereeArticles
                        .Where(refArt =>
                       refArt.Referee.ID == referee.Referee.ID
                       &&
                       !refArt.IsRead
                       ).Count();
                    EmailService.SendRefereeNotification(referee.Person.FirstName, referee.Person.LastName, referee.UserName, cnt.ToString(), referee.Email);
                }
                if (NotConfirmCnt > 0)
                    return ControllerHelper.SuccessResult("ارسال " + ConfirmCnt.ToString() + " با موفقیت انجام شد " + "---- ارسال " + NotConfirmCnt.ToString() + " با به علت عدم پرداخت حق عضویت انجام نشد");
                else
                    return ControllerHelper.SuccessResult("ارسال " + ConfirmCnt.ToString() + " با موفقیت انجام شد ");
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return ControllerHelper.ErrorResult("بروز خطای سیستمی");
            }
        }
        [HttpPost]
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.ScientificSecretaryCircle)]
        public ActionResult SetArticlePresentType(string viewModel)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            ArticleStatusTypeViewModel vm = jss.Deserialize<ArticleStatusTypeViewModel>(viewModel);
            var context = new ApplicationDbContext();
            var trans = context.Database.BeginTransaction();
            try
            {
                foreach (var articleID in vm.selectedArticleIDs)
                {
                    string PosterImage = "";
                    if (vm.SelectedPresentTypeID == ((int)Enums.ArticlePresentType.Poster).ToString())
                    {
                        if (Request.Files.Count > 0)
                        {
                            var file = Request.Files[CommonHelper.ExposeProperty.GetPropertyInfo(vm, m => m.PosterFile).Name];

                            if (file != null && file.ContentLength > 0)
                            {
                                PosterImage = FileUploadManagment.UploadFile("~/assets/img/Attach/Article/", "", file, FileUploadManagment.AppFileType.Image, "", 2048000);
                                if (PosterImage == "")
                                {
                                    return ControllerHelper.ErrorResult("فرمت یا حجم فایل پوستر مقاله صحیح نمی باشد");
                                }
                            }
                            else
                            {
                                return ControllerHelper.ErrorResult("لطفا فایل پوستر را انتخاب نمایید");
                            }
                        }
                    }
                    int PresentTypeID = int.Parse(vm.SelectedPresentTypeID);
                    int StatusTypeID = int.Parse(vm.SelectedStatusID);
                    var selectedArticle = context.Articles
                        .Include(article => article.CreatorUser.Person)
                        .Include(article => article.ArticlePresentType)
                        .Include(article => article.ArticleWriters)
                        .Include(article => article.ArticleStatus)
                        .Where(article => article.ID == articleID).SingleOrDefault();
                    selectedArticle.ArticlePresentType = context.ArticlePresentTypes.Where(presentType => presentType.ID == PresentTypeID).SingleOrDefault();
                    selectedArticle.ArticleStatus = context.ArticleStatuses.Where(statusType => statusType.ID == StatusTypeID).SingleOrDefault();
                    selectedArticle.ArticlePresentTypeExplain = vm.Explain;
                    if (vm.SelectedPresentTypeID == ((int)Enums.ArticlePresentType.Poster).ToString())
                    {
                        if (!string.IsNullOrEmpty(vm.PresentDate) && !string.IsNullOrEmpty(vm.PresentTime))
                        {
                            selectedArticle.PresentTime = CommonHelper.DateAndTimes.GetGregorianDate(vm.PresentDate);
                            var splittedTime = vm.PresentTime.Trim().Split(':');
                            TimeSpan ts = new TimeSpan(int.Parse(splittedTime[0]), int.Parse(splittedTime[1]), 0);
                            selectedArticle.PresentTime = selectedArticle.PresentTime.Value.Date + ts;
                        }
                        selectedArticle.PresentLocation = vm.PresentLocation;
                        selectedArticle.PosterUrl = PosterImage;
                        selectedArticle.Published = true;
                        selectedArticle.Visible = true;
                    }
                    context.SaveChanges();
                    trans.Commit();
                    try
                    {
                        EmailService.EmailResponse MailResult = EmailService.SendRefereePeresentTypeResult(
                                   selectedArticle.CreatorUser.Person.FirstName,
                                   selectedArticle.CreatorUser.Person.LastName,
                                   selectedArticle.CreatorUser.UserName,
                                   selectedArticle.CreatorUser.Email,
                                   selectedArticle.Title,
                                   selectedArticle.ArticlePresentType.Name,
                                   selectedArticle.ArticleStatus.Name
                                   );
                        foreach (var writer in selectedArticle.ArticleWriters)
                        {
                            EmailService.EmailResponse MailResultWriter = EmailService.SendRefereePeresentTypeResult(
                               writer.FirstName,
                               writer.LastName,
                               "",
                               writer.Email,
                               selectedArticle.Title,
                               selectedArticle.ArticlePresentType.Name,
                               selectedArticle.ArticleStatus.Name
                               );
                        }

                    }
                    catch (Exception)
                    {

                    }

                }
                return ControllerHelper.SuccessResult("با موفقیت ثبت شد");
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return ControllerHelper.ErrorResult("بروز خطای سیستمی " + ex.Message);
            }
        }
        [HttpPost]
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.ScientificSecretaryCircle)]
        public ActionResult GetArchivedArticles(int ConferenceID)
        {
            try
            {
                var context = new ApplicationDbContext();
                var dbArticleFiles = context.ConferenceArticles
                     .Include(confArt => confArt.Article)
                     .Where(confArt =>
                             //confArt.Article.ArticleStatus.ID == (int)Enums.ArticleStatus.Confirmed
                             //&&
                             confArt.Conference.ID == ConferenceID
                         )
                     .Select(confArt => confArt.Article.FileUrl).ToList();
                string FileName = Guid.NewGuid().ToString() + ".zip";
                using (var fileStream = new FileStream(Path.GetTempPath() + FileName, FileMode.CreateNew))
                {
                    using (var archive = new ZipArchive(fileStream, ZipArchiveMode.Create, true))
                    {
                        foreach (var file in dbArticleFiles)
                        {
                            var _file = new FileInfo(Server.MapPath(file));
                            var pdfBytes = System.IO.File.ReadAllBytes(_file.FullName);
                            var zipArchiveEntry = archive.CreateEntry(_file.Name, CompressionLevel.NoCompression);
                            using (var zipStream = zipArchiveEntry.Open())
                                zipStream.Write(pdfBytes, 0, pdfBytes.Length);
                        }

                    }
                    fileStream.Close();
                }
                return File(System.IO.File.ReadAllBytes(Path.GetTempPath() + FileName), System.Net.Mime.MediaTypeNames.Application.Zip, FileName);
            }
            catch (Exception ex)
            {
                return ControllerHelper.ErrorResult("بروز خطای سیستمی");
            }
        }

    }
}