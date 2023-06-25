using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AVA.UI.Helpers.Base;
using AVA.Core.Entities;
using AVA.UI.Helpers.Controller;
using AVA.UI.Helpers.CustomAttribute;
using AVA.Web.Mvc.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using AVA.UI.Helpers.MailSmsService;
using System.Threading.Tasks;
using AVA.Web.Mvc.Controllers.Admin;
using AVA.UI.Helpers.Common;
using AVA.Web.Mvc.Models.Admin;
using AVA.UI.Helpers;
using System.Web.Script.Serialization;

namespace AVA.Web.Mvc.Controllers
{
    public class CsmiController : BaseController
    {
        #region Poll
        public ActionResult Poll()
        {
            string CurrentLang = Request.Cookies["Lang"].Value;
            var contex = new ApplicationDbContext();
            var CurrentPoll = contex.Polls
                .Where(Poll =>
                (bool)Poll.Enable
                &&
                    Poll.StartDate <= DateTime.Now
                &&
                    DateTime.Now <= Poll.EndDate
                )
                .Select(Poll => new
                {
                    Poll.ID,
                }).SingleOrDefault();
            var questions = new List<PollQuestionViewModel>();
            if (CurrentPoll != null)
            {
                var CurrentPollID = CurrentPoll.ID;

                var DbQuestions = contex.PollQuestions
               .Include(Question => Question.PollAnswers)
               .Include(Question => Question.PollAnswers.Select(ans => ans.Language))
               .Include(Question => Question.Language)
               .Where(Question =>
                    (bool)Question.Enable
                    &&
                    Question.Poll.ID == CurrentPollID
                    &&
                    Question.Language.Value == CurrentLang
                   )
               .Select(Question => new
               {
                   Question.ID,
                   Question.Question,
                   PollAnswers = Question.PollAnswers.Where(ans => ans.Language.Value == CurrentLang)
               });
                foreach (var item in DbQuestions)
                {
                    var answers = new List<PollAnswerViewModel>();
                    foreach (var ans in item.PollAnswers)
                    {
                        answers.Add(new PollAnswerViewModel()
                        {
                            ID = ans.ID,
                            Answer = ans.Answer
                        });
                    }
                    questions.Add(
                       new PollQuestionViewModel()
                       {
                           ID = item.ID,
                           Question = item.Question,
                           Answers = answers,
                           SelectedAnswerID = answers[0].ID
                       }
                    );
                }
            }

            return PartialView("~/Views/CSMI/Poll.cshtml", new PollViewModel()
            {
                Questions = questions,
            });

        }
        [HttpPost]
        [AjaxValidateAntiForgeryTokenAttribute]
        public ActionResult SubmitPoll(PollViewModel viewmodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var context = new ApplicationDbContext();
                        if (User.Identity.IsAuthenticated)
                        {
                            var UserPoll = context.PollQuestionAnswers
                                .Include(AnsQues => AnsQues.CreatorUser)
                                .Where(AnsQues => AnsQues.CreatorUser.UserName == User.Identity.Name)
                                .FirstOrDefault();
                            if (UserPoll != null)
                            {
                                return ControllerHelper.ErrorResult("شما قبلا در نظرسنجی شرکت کرده اید");
                            }
                            foreach (var ques in viewmodel.Questions)
                            {
                                context.PollQuestionAnswers.Add(new PollQuestionAnswer()
                                {
                                    CreatorUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault(),
                                    PollAnswer = context.PollAnswers.Where(ans => ans.ID == ques.SelectedAnswerID).SingleOrDefault(),
                                    CreateDate = DateTime.Now
                                });
                            }
                        }
                        else
                        {
                            if (Request.Cookies["PCookie"] != null)
                            {
                                return ControllerHelper.ErrorResult("شما قبلا در نظرسنجی شرکت کرده اید");
                            }
                            foreach (var ques in viewmodel.Questions)
                            {
                                context.PollQuestionAnswers.Add(new PollQuestionAnswer()
                                {
                                    PollAnswer = context.PollAnswers.Where(ans => ans.ID == ques.SelectedAnswerID).SingleOrDefault(),
                                    CreateDate = DateTime.Now
                                });
                            }
                        }
                        context.SaveChanges();
                        Response.Cookies.Add(new HttpCookie("PCookie", "1") { Expires = DateTime.Now.AddYears(1) });
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
        #region BookList
        const int BookListPageSize = 5;
        [HttpGet]
        public ActionResult BookList(int pageNumber, string WriterName)
        {
            return PartialView("~/Views/CSMI/BookList.cshtml", BookListFeedGenerator(pageNumber, WriterName, Request.Cookies["Lang"].Value, "ASC"));
        }
        [HttpGet]
        public ActionResult BookListFeed(int pageNumber, string WriterName, string OrderType)
        {
            return Json(BookListFeedGenerator(pageNumber, WriterName, Request.Cookies["Lang"].Value, OrderType), JsonRequestBehavior.AllowGet);
        }
        public BookListViewModel BookListFeedGenerator(int pageNumber, string WriterName, string Language, string OrderType)
        {

            var contex = new ApplicationDbContext();
            //int totalRecords = contex.Books.Count();
            int skipRows = (pageNumber - 1) * BookListPageSize;
            var Book = contex.Books
                .Include(book => book.Language)
                .Where(book => (bool)book.Enable
                   && book.Language.Value == Language
                 )
                .OrderByDescending(book => book.CreateDate)
                .Select(book => new { book.ID, book.Name, book.Writer, book.Year, book.PrintPeriod, book.ImageUrl, book.CreateDate })
                .ToList();

            var BookList = new List<BookViewModel>();
            foreach (var item in Book)
            {
                BookList.Add(
                    new BookViewModel()
                    {
                        ID = item.ID,
                        Name = item.Name,
                        ImageUrl = item.ImageUrl != null ? Url.Content(item.ImageUrl) : Url.Content("~/assets/img/projects/book.jpg"),
                        Writer = item.Writer,
                        Year = item.Year,
                        PrintPeriod = item.PrintPeriod,
                        CreateDateConverted = CommonHelper.DateAndTimes.GetPersianDate(item.CreateDate),
                    }
                );
            }
            if (OrderType == "ASC")
            {
                return new BookListViewModel()
                {
                    Books = BookList.Where(book =>
                       (
                            book.Writer.Contains(WriterName)
                            || string.IsNullOrEmpty(WriterName)
                            || WriterName == "undefined")
                            || WriterName == "null"
                            || book.Name.Contains(WriterName)
                       )
                        .Skip(skipRows).Take(BookListPageSize)
                        .OrderBy(book => book.Name)
                        .ToList(),
                    pageNumber = pageNumber,
                    pageSize = BookListPageSize,
                    totalRecords = BookList.Count(book =>
                              book.Writer.Contains(WriterName)
                            || string.IsNullOrEmpty(WriterName)
                            || WriterName == "undefined"
                            || WriterName == "null"
                            || book.Name.Contains(WriterName)
                        ),
                    OrderType = OrderType
                };
            }
            else
            {
                return new BookListViewModel()
                {
                    Books = BookList.Where(book =>
                       (
                            book.Writer.Contains(WriterName)
                            || string.IsNullOrEmpty(WriterName)
                            || WriterName == "undefined")
                            || WriterName == "null"
                            || book.Name.Contains(WriterName)
                 )
                  .Skip(skipRows).Take(BookListPageSize)
                  .OrderByDescending(book => book.Name)
                  .ToList(),
                    pageNumber = pageNumber,
                    pageSize = BookListPageSize,
                    totalRecords = BookList.Count(book =>
                               book.Writer.Contains(WriterName)
                            || string.IsNullOrEmpty(WriterName)
                            || WriterName == "undefined"
                            || WriterName == "null"
                            || book.Name.Contains(WriterName)
                           ),
                    OrderType = OrderType
                };
            }
        }
        #endregion
        #region SocietyMembers
        public ActionResult SocietyMembers()
        {
            string CurrentLang = Request.Cookies["Lang"].Value;
            var contex = new ApplicationDbContext();
            var DBSocietyMembers = contex.SocietyMembers
                .Include(item => item.SocietyMemberPeriod)
                .Include(item => item.Language)
                .Where(item => item.Language.Value == CurrentLang && item.Enable)
                .Select(item => new
                {
                    item.ID,
                    item.Name,
                    item.Family,
                    item.Position,
                    item.PersonalImageUrl,
                    item.ResumeUrl,
                    item.Email,
                    item.DegreeLevel,
                    PeriodID = item.SocietyMemberPeriod.ID,
                    item.SocietyMemberPeriod.EndDate
                }).ToList();
            SocietyMemberListViewModel viewModel = new SocietyMemberListViewModel();
            viewModel.SocietyMembers = new List<SocietyMemberViewModel>();
            foreach (var member in DBSocietyMembers)
            {
                viewModel.SocietyMembers.Add(new SocietyMemberViewModel()
                {
                    ID = member.ID,
                    Name = member.Name,
                    Family = member.Family,
                    Email = member.Email == null ? "" : member.Email,
                    Position = member.Position == null ? "" : member.Position,
                    DegreeLevel = member.DegreeLevel == null ? "" : member.DegreeLevel,
                    PersonalImageUrl = member.PersonalImageUrl == null ? "~/assets/img/profile-green.png" : member.PersonalImageUrl,
                    ResumeUrl = member.ResumeUrl == null ? "" : member.ResumeUrl,
                    PeriodID = member.PeriodID

                });
            }
            viewModel.SocietyMemberGroupList = new List<SocietyMemberGroupViewModel>();
            var DBGroupList = contex.SocietyMemberPeriods
                .Select(item => new { item.ID, item.StartDate, item.EndDate })
                .ToList()
                .OrderByDescending(item => item.EndDate);
            foreach (var group in DBGroupList)
            {
                viewModel.SocietyMemberGroupList.Add(new SocietyMemberGroupViewModel()
                {
                    PeriodName = CommonHelper.DateAndTimes.GetPersianDate(group.StartDate).Split('/')[0] + "-" + CommonHelper.DateAndTimes.GetPersianDate(group.EndDate).Split('/')[0],
                    PeriodID = group.ID
                });
            }
            return PartialView("~/Views/CSMI/SocietyMembers.cshtml", viewModel);
        }
        #endregion 
        #region SocietyDirector
        public ActionResult SocietyDirector()
        {
            string CurrentLang = Request.Cookies["Lang"].Value;
            var contex = new ApplicationDbContext();
            var DBSocietyMembers = contex.SocietyExecutors
                .Include(item => item.Language)
                .Where(item => item.Language.Value == CurrentLang && item.Enable)
                .Select(item => new
                {
                    item.ID,
                    item.Name,
                    item.Family,
                    item.PersonalImageUrl,
                    item.ResumeUrl,
                    item.Position,
                    item.Email,
                    item.DegreeLevel,
                }).ToList();
            SocietyExecutorListViewModel viewModel = new SocietyExecutorListViewModel();
            viewModel.SocietyExecutors = new List<SocietyExecutorViewModel>();
            foreach (var member in DBSocietyMembers)
            {
                viewModel.SocietyExecutors.Add(new SocietyExecutorViewModel()
                {
                    ID = member.ID,
                    Name = member.Name,
                    Family = member.Family,
                    Email = member.Email == null ? "" : member.Email,
                    DegreeLevel = member.DegreeLevel == null ? "" : member.DegreeLevel,
                    PersonalImageUrl = member.PersonalImageUrl == null ? "~/assets/img/profile-green.png" : member.PersonalImageUrl,
                    ResumeUrl = member.ResumeUrl == null ? "" : member.ResumeUrl,
                    Position = member.Position
                });
            }
            return PartialView("~/Views/CSMI/SocietyDirector.cshtml", viewModel);
        }
        #endregion
        #region Gallary
        public ActionResult Gallary()
        {
            string CurrentLang = Request.Cookies["Lang"].Value;
            var contex = new ApplicationDbContext();
            var DBGalleryAlbums = contex.GalleryAlbums
                .Include(gallaryAlbum => gallaryAlbum.Language)
                .Include(gallaryAlbum => gallaryAlbum.GalleryImages)
                .Where(gallaryAlbum =>
                gallaryAlbum.Language.Value == CurrentLang
                &&
                (bool)gallaryAlbum.Enable
                )
                .Select(gallaryAlbum => new
                {
                    AlbumID = gallaryAlbum.ID,
                    AlbumName = gallaryAlbum.Name,
                    gallaryAlbum.CreateDate,
                    gallaryAlbum.GalleryImages
                })
                .OrderByDescending(gallaryAlbum => gallaryAlbum.CreateDate)
                .ToList();
            GalleryImageListViewModel viewmodel = new GalleryImageListViewModel();
            viewmodel.GalleryAlbums = new List<GalleryAlbumViewModel>();
            viewmodel.GalleryImages = new List<GalleryImageViewModel>();
            foreach (var group in DBGalleryAlbums)
            {
                var img = group.GalleryImages.OrderBy(image => image.Priority).FirstOrDefault();
                if (img != null)
                {
                    viewmodel.GalleryAlbums.Add(new GalleryAlbumViewModel()
                    {
                        ID = group.AlbumID,
                        Name = group.AlbumName
                    });
                    viewmodel.GalleryImages.Add(new GalleryImageViewModel()
                    {
                        ID = img.ID,
                        Name = img.Name,
                        ShortImageUrl = img.ShortImageUrl,
                        GalleryAlbumID = group.AlbumID
                    });
                }
            }
            return PartialView("~/Views/CSMI/Gallary.cshtml", viewmodel);
        }
        public ActionResult GallaryInfo(int AlbumID)
        {
            string CurrentLang = Request.Cookies["Lang"].Value;
            var contex = new ApplicationDbContext();
            var DBGalleryImage = contex.GalleryImages
                .Include(gallary => gallary.GalleryAlbum)
                .Include(gallary => gallary.GalleryAlbum.Language)
                .Where(gallary =>
                gallary.GalleryAlbum.Language.Value == CurrentLang
                &&
                (bool)gallary.GalleryAlbum.Enable
                &&
                (bool)gallary.Enable
                &&
                gallary.GalleryAlbum.ID == AlbumID
                )
                .Select(gallary => new
                {
                    gallary.ShortImageUrl,
                    gallary.LongImageUrl,
                    gallary.ID,
                    gallary.Name,
                    gallary.Priority,
                    AlbumID = gallary.GalleryAlbum.ID,
                    AlbumName = gallary.GalleryAlbum.Name,
                    AlbumDescription = gallary.GalleryAlbum.Description
                })
                .OrderBy(gallary => gallary.Priority)
                .ToList();
            GalleryImageListViewModel viewmodel = new GalleryImageListViewModel();
            viewmodel.GalleryAlbums = new List<GalleryAlbumViewModel>();
            viewmodel.GalleryImages = new List<GalleryImageViewModel>();
            bool IsAdded = false;
            foreach (var image in DBGalleryImage)
            {
                viewmodel.GalleryImages.Add(new GalleryImageViewModel()
                {
                    ID = image.ID,
                    Name = image.Name,
                    ShortImageUrl = image.ShortImageUrl,
                    LongImageUrl = image.LongImageUrl,
                    GalleryAlbumID = image.AlbumID
                });
                if (!IsAdded)
                {
                    viewmodel.GalleryAlbums.Add(new GalleryAlbumViewModel()
                    {
                        ID = image.AlbumID,
                        Name = image.AlbumName,
                        Description = image.AlbumDescription
                    });
                    IsAdded = true;
                }
            }

            return PartialView("~/Views/CSMI/GallaryInfo.cshtml", viewmodel);
            //var contex = new ApplicationDbContext();
            //var DBGalleryImage = contex.GalleryImages
            //    .Include(gallary => gallary.GalleryAlbum)
            //    .Where(gallary =>
            //        (bool)gallary.GalleryAlbum.Enable
            //        &&
            //        (bool)gallary.Enable
            //        &&
            //        gallary.ID == ID
            //    )
            //    .Select(gallary => new
            //    {
            //        gallary.LongImageUrl,
            //        gallary.ID,
            //        gallary.Name,
            //        gallary.Explain,
            //        AlbumID = gallary.GalleryAlbum.ID,
            //        AlbumName = gallary.GalleryAlbum.Name,
            //        gallary.CreateDate
            //    }).SingleOrDefault();
            //GalleryImageViewModel viewmodel = new GalleryImageViewModel()
            //{
            //    ID = DBGalleryImage.ID,
            //    LongImageUrl = DBGalleryImage.LongImageUrl,
            //    Explain = DBGalleryImage.Explain,
            //    Name = DBGalleryImage.Name,
            //    GalleryAlbumID = DBGalleryImage.AlbumID,
            //    GalleryAlbumName = DBGalleryImage.AlbumName,
            //    CreateDateConverted = CommonHelper.DateAndTimes.GetPersianDate(DBGalleryImage.CreateDate)
            //};

        }

        #endregion
        #region FAQ

        const int FAQListPageSize = 10;
        public ActionResult FAQ(int pageNumber)
        {
            return PartialView("~/Views/CSMI/FAQ.cshtml", FAQListFeedGenerator(pageNumber));
        }
        [HttpGet]
        public ActionResult FAQListFeed(int pageNumber)
        {
            return Json(FAQListFeedGenerator(pageNumber), JsonRequestBehavior.AllowGet);
        }
        public FAQListViewModel FAQListFeedGenerator(int pageNumber)
        {
            string CurrentLang = Request.Cookies["Lang"].Value;
            var contex = new ApplicationDbContext();
            FAQListViewModel viewmodel = new FAQListViewModel();
            int skipRows = (pageNumber - 1) * FAQListPageSize;
            var totalRecords = contex.FAQs
                .Count(faq =>
                    (bool)faq.Enable
                    &&
                    faq.Language.Value == CurrentLang
                    );

            var DBFAQs = contex.FAQs
              .Where(faq =>
              (bool)faq.Enable
                &&
                faq.Language.Value == CurrentLang
              )
              .OrderByDescending(faq => faq.Priority)
              .Select(faq => new { faq.ID, faq.Question, faq.Answer, faq.CreateDate })
              .Skip(skipRows)
              .Take(FAQListPageSize)
              .ToList();
            var FAQList = new List<FAQViewModel>();
            foreach (var item in DBFAQs)
            {
                FAQList.Add(
                    new FAQViewModel()
                    {
                        ID = item.ID,
                        Question = item.Question,
                        Answer = item.Answer,
                    }
                );
            }
            return new FAQListViewModel()
            {
                FAQs = FAQList,
                pageNumber = pageNumber,
                pageSize = FAQListPageSize,
                totalRecords = totalRecords
            };
        }
        #endregion
        #region ConferenceList
        public ActionResult ConferenceList()
        {
            List<ConferenceViewModel> conferences = new List<ConferenceViewModel>();
            var context = new ApplicationDbContext();
            var DBConferences = context.Conferences
                 .Where(conf =>
                     conf.EventDate < DateTime.Now
                     && (bool)conf.Visible
                 )
                 .Select(conf => new
                 {
                     conf.ID,
                     conf.Title,
                     conf.EventDate,
                     conf.PosterImageUrl,
                     conf.AttachTotalArticlesFileUrl
                 })
                 .ToList()
                 .OrderByDescending(conf => conf.EventDate);
            foreach (var conference in DBConferences)
            {
                conferences.Add(new ConferenceViewModel()
                {
                    ID = conference.ID,
                    Title = conference.Title,
                    EventDateConverted = CommonHelper.DateAndTimes.GetPersianDate(conference.EventDate),
                    Year = CommonHelper.DateAndTimes.GetPersianDate(conference.EventDate).Split('/')[0],
                    PosterImageUrl = Url.Content(conference.PosterImageUrl),
                    AttachTotalArticlesFileUrl = conference.AttachTotalArticlesFileUrl
                });
            }

            //var ConferenceGroups = conferences
            //.GroupBy(conf => conf.Year)
            //.Select(conf => new { conf.Key, Count = conf.Count() })
            //.OrderByDescending(conf => conf.Key);
            //var ConferenceGroupList = new List<ConferenceListViewModel>();
            //foreach (var item in ConferenceGroups)
            //{
            //    ConferenceGroupList.Add(new ConferenceListViewModel()
            //    {
            //         = item.Key,
            //        Count = item.Count
            //    });
            //}
            //return new NewsListViewModel()
            //{
            //    DataModel = newsList.Where(news => news.Year == year || string.IsNullOrEmpty(year) || year == "undefined").Skip(skipRows).Take(pageSize).ToList(),
            //    pageNumber = pageNumber,
            //    pageSize = pageSize,
            //    totalRecords = newsList.Count(news => news.Year == year || string.IsNullOrEmpty(year) || year == "undefined"),
            //    GroupList = newsGroupList,
            //};

            return PartialView("~/Views/CSMI/ConferenceList.cshtml", new ConferenceListViewModel() { DataModel = conferences });
        }
        #endregion
        #region HamayeshActive
        public ActionResult HamayeshActive()
        {
            var context = new ApplicationDbContext();
            var SelectedConf = context.Conferences
              .Where(conf => (bool)conf.Enable && (bool)conf.Visible /*&& conf.EventDate > DateTime.Now*/)
              .Select(conf => new
              {
                  conf.EventDate,
                  conf.SendStartDate,
                  conf.SendEndDate,
                  conf.EnglishTitle,
                  conf.Title,
                  conf.PosterImageUrl,
                  conf.AttachFileUrl,
                  conf.AttachAmayeshFileUrl,
                  conf.AttachPosterTemplateFileUrl,
                  conf.AttachPresentationHelpFileUrl,
                  conf.AttachPresentationPowerpointFileUrl,
                  conf.AttachChemistryPresentationProgramFileUrl,
                  conf.AttachPhysicsPresentationProgramFileUrl,
                  conf.AttachGeologyPresentationProgramFileUrl,
                  conf.AttachScientificCommitteeFileUrl,
                  conf.AttachExecutiveCommitteeFileUrl,
                  conf.AttachOpeningPlanUrl,
                  conf.AttachAttendingHelpUrl,
                  conf.TelegramUrl,
                  conf.Place,
                  conf.MobileTel,
                  conf.ID,
                  conf.Explain
              }).SingleOrDefault();
            CurrentFutureConferenceListViewModel viewModel = new CurrentFutureConferenceListViewModel();
            viewModel.CurrentConference = new ConferenceViewModel();
            if (SelectedConf != null)
            {
                viewModel.CurrentConference.EventDate = SelectedConf.EventDate.ToShortDateString();
                viewModel.CurrentConference.EventDateConverted = CommonHelper.DateAndTimes.GetPersianDate(SelectedConf.EventDate);
                viewModel.CurrentConference.SendStartDateConverted = CommonHelper.DateAndTimes.GetPersianDate(SelectedConf.SendStartDate);
                viewModel.CurrentConference.SendEndDateConverted = CommonHelper.DateAndTimes.GetPersianDate(SelectedConf.SendEndDate);
                viewModel.CurrentConference.Title = SelectedConf.Title;
                viewModel.CurrentConference.EnglishTitle = SelectedConf.EnglishTitle;
                viewModel.CurrentConference.PosterImageUrl = SelectedConf.PosterImageUrl;
                viewModel.CurrentConference.AttachFileUrl = SelectedConf.AttachFileUrl;
                viewModel.CurrentConference.AttachAmayeshFileUrl = SelectedConf.AttachAmayeshFileUrl;
                viewModel.CurrentConference.AttachPosterTemplateFileUrl = SelectedConf.AttachPosterTemplateFileUrl;
                viewModel.CurrentConference.AttachPresentationHelpFileUrl = SelectedConf.AttachPresentationHelpFileUrl;
                viewModel.CurrentConference.AttachPresentationPowerpointFileUrl = SelectedConf.AttachPresentationPowerpointFileUrl;
                viewModel.CurrentConference.AttachChemistryPresentationProgramFileUrl = SelectedConf.AttachChemistryPresentationProgramFileUrl;
                viewModel.CurrentConference.AttachPhysicsPresentationProgramFileUrl = SelectedConf.AttachPhysicsPresentationProgramFileUrl;
                viewModel.CurrentConference.AttachGeologyPresentationProgramFileUrl = SelectedConf.AttachGeologyPresentationProgramFileUrl;
                viewModel.CurrentConference.AttachScientificCommitteeFileUrl = SelectedConf.AttachScientificCommitteeFileUrl;
                viewModel.CurrentConference.AttachExecutiveCommitteeFileUrl = SelectedConf.AttachExecutiveCommitteeFileUrl;
                viewModel.CurrentConference.AttachOpeningPlanFileUrl = SelectedConf.AttachOpeningPlanUrl;
                viewModel.CurrentConference.AttachAttendingHelpFileUrl = SelectedConf.AttachAttendingHelpUrl;
                viewModel.CurrentConference.Place = SelectedConf.Place;
                viewModel.CurrentConference.TelegramUrl = SelectedConf.TelegramUrl;
                viewModel.CurrentConference.MobileTel = SelectedConf.MobileTel;
                viewModel.CurrentConference.ID = SelectedConf.ID;
                viewModel.CurrentConference.Explain = SelectedConf.Explain;
            }
            viewModel.FutureConferences = new List<ConferenceViewModel>();
            var DbFutureConfs = context.Conferences
              .Where(conf => !(bool)conf.Enable && (bool)conf.Visible && conf.EventDate > DateTime.Now)
              .OrderBy(conf => conf.EventDate)
              .Take(3);
            foreach (var conf in DbFutureConfs)
            {
                viewModel.FutureConferences.Add(new Models.ConferenceViewModel()
                {
                    EventDate = conf.EventDate.ToShortDateString(),
                    EventDateConverted = CommonHelper.DateAndTimes.GetPersianDate(conf.EventDate),
                    SendStartDateConverted = CommonHelper.DateAndTimes.GetPersianDate(conf.SendStartDate),
                    SendEndDateConverted = CommonHelper.DateAndTimes.GetPersianDate(conf.SendEndDate),
                    Title = conf.Title,
                    EnglishTitle = conf.EnglishTitle,
                    PosterImageUrl = conf.PosterImageUrl,
                    AttachFileUrl = conf.AttachFileUrl,
                    AttachAmayeshFileUrl = conf.AttachAmayeshFileUrl,
                    Place = conf.Place

                });
            }
            return PartialView("~/Views/CSMI/HamayeshActive.cshtml", viewModel);
        }
        #endregion 
        #region Article
        const int ArticlePageSize = 20;
        public ActionResult ArticleList(string ConferenceID, string FieldID, string PosterArticle)
        {
            string CurrentLang = Request.Cookies["Lang"].Value;
            int conferenceID = ConferenceID == "undefined" ? 0 : int.Parse(ConferenceID);
            bool ShowPosterArticle = PosterArticle == "undefined" ? false : PosterArticle == "1";
            var context = new ApplicationDbContext();
            var Dbcats = context.Categories
                .Include(cat => cat.CategoryNames)
                .Include(cat => cat.CategoryNames.Select(CatLang => CatLang.Language))
                .Where(
                    cat => cat.TableName == "Conference"
                    && (bool)cat.Enable
                )
                .Select(cat => new
                {
                    cat.ID,
                    cat.CategoryNames.Where(CatLang => CatLang.Language.Value == CurrentLang).FirstOrDefault().Name,
                    cat.CategoryNames.Where(CatLang => CatLang.Language.Value == CurrentLang).FirstOrDefault().Explain,
                    cat.IconName,
                })
                .ToList();
            List<Models.BaseViewModel.CheckBoxVm> Fields = new List<Models.BaseViewModel.CheckBoxVm>();
            var DbConference = context.Conferences
                 .Where(conf => conf.ID == conferenceID)
                 .Select(conf => new { conf.EventDate, conf.Title })
                 .FirstOrDefault();

            foreach (var cat in Dbcats)
            {
                if (!string.IsNullOrEmpty(FieldID) && FieldID != "undefined")
                {
                    Fields.Add(new Models.BaseViewModel.CheckBoxVm()
                    {
                        Text = cat.Name,
                        Value = cat.ID.ToString(),
                        IsChecked = cat.ID.ToString() == FieldID ? true : false,
                    });
                }
                else
                {
                    Fields.Add(new Models.BaseViewModel.CheckBoxVm()
                    {
                        Text = cat.Name,
                        Value = cat.ID.ToString(),
                        IsChecked = true,
                    });
                }
            }
            var ArticleList = ArticleListFeedGenerator(
                new ArticleClientQuickFilterViewModel()
                {
                    ConferenceID = conferenceID,
                    Fields = Fields,
                    ShowPosterArticle = ShowPosterArticle
                }, 1);
            if (DbConference != null)
            {
                ArticleList.ConferenceDate = CommonHelper.DateAndTimes.GetPersianDate(DbConference.EventDate);
                ArticleList.ConferenceName = DbConference.Title;
            }
            return PartialView("~/Views/CSMI/ArticleList.cshtml", ArticleList);
        }
        [AjaxValidateAntiForgeryToken]
        public ActionResult ArticleListPageChange(ArticleClientListViewModel viewmodel)
        {
            return new JsonResult()
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = ArticleListFeedGenerator(viewmodel.FilterForm, viewmodel.pageNumber)
            };
        }
        ArticleClientListViewModel ArticleListFeedGenerator(ArticleClientQuickFilterViewModel viewmodel, int pageNumber)
        {
            string CurrentLang = Request.Cookies["Lang"].Value;
            var SelectedCatIDs = viewmodel.Fields
                    .Where(field => field.IsChecked)
                    .Select(
                        field => int.Parse(field.Value)
                    );
            int skipRows = (pageNumber - 1) * ArticlePageSize;
            var context = new ApplicationDbContext();
            int totalRecords = context.Articles
                .Include(article => article.ConferenceArticles)
                .Include(article => article.ConferenceArticles.Select(ConfArt => ConfArt.Conference))
                .Include(article => article.ArticleCategories.Select(ArtCat => ArtCat.Category.CategoryNames.Select(catName => catName.Language)))
                .Include(article => article.ArticleWriters)
                .Include(article => article.CreatorUser.Person)
                .Where(article =>
                    (article.ConferenceArticles.Any(ConfArt => ConfArt.Article.ID == article.ID && ConfArt.Conference.ID == viewmodel.ConferenceID) || viewmodel.ConferenceID == 0)
                    &&
                    (
                        article.Title.Contains(viewmodel.Title)
                        ||
                        article.EnglishTitle.Contains(viewmodel.Title)
                        || viewmodel.Title == ""
                        || viewmodel.Title == null
                    )
                    &&
                    (
                        article.Keywords.Contains(viewmodel.Keyword)
                        || viewmodel.Keyword == ""
                        || viewmodel.Keyword == null
                    )
                    &&
                    (
                        article.ID == context.ArticleWriters.Where(writer => (writer.FirstName + " " + writer.LastName).Contains(viewmodel.Writer) && writer.Article.ID == article.ID).Select(writer => writer.Article.ID).FirstOrDefault()
                        ||
                        (article.CreatorUser.Person.FirstName + " " + article.CreatorUser.Person.LastName).Contains(viewmodel.Writer)
                        || viewmodel.Writer == ""
                        || viewmodel.Writer == null
                    )
                    &&
                    (
                        article.ArticleCategories.Any(ArtCat => SelectedCatIDs.Contains(ArtCat.Category.ID) && ArtCat.Article.ID == article.ID)
                    )
                     &&
                    (
                        (bool)article.Enable
                        &&
                        article.Published
                    )
                    &&
                    (
                         (article.ConferenceArticles.Any(ConfArt => ConfArt.Article.ID == article.ID && (ConfArt.Conference.Title.Contains(viewmodel.ConferenceName) || ConfArt.Conference.EnglishTitle.Contains(viewmodel.ConferenceName))) || viewmodel.ConferenceName == null || viewmodel.ConferenceName == "")
                    )
                    &&
                    (
                          (article.ArticlePresentType.ID == (int)Enums.ArticlePresentType.Poster && viewmodel.ShowPosterArticle && !string.IsNullOrEmpty(article.PosterUrl)) || !viewmodel.ShowPosterArticle
                    )
                )
                .Count();
            //↑↑↑↑↑↑↑↑↑ ↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓    realated to each other             
            var DBArticles = context.Articles
                .Include(article => article.ConferenceArticles)
                .Include(article => article.ConferenceArticles.Select(ConfArt => ConfArt.Conference))
                .Include(article => article.ArticleCategories.Select(ArtCat => ArtCat.Category.CategoryNames.Select(catName => catName.Language)))
                .Include(article => article.ArticleWriters)
                .Include(article => article.CreatorUser.Person)
                .Where(article =>
                    (article.ConferenceArticles.Any(ConfArt => ConfArt.Article.ID == article.ID && ConfArt.Conference.ID == viewmodel.ConferenceID) || viewmodel.ConferenceID == 0)
                    &&
                    (
                        article.Title.Contains(viewmodel.Title)
                        ||
                        article.EnglishTitle.Contains(viewmodel.Title)
                        || viewmodel.Title == ""
                        || viewmodel.Title == null
                    )
                    &&
                    (
                        article.Keywords.Contains(viewmodel.Keyword)
                        || viewmodel.Keyword == ""
                        || viewmodel.Keyword == null
                    )
                    &&
                    (
                        article.ID == context.ArticleWriters.Where(writer => (writer.FirstName + " " + writer.LastName).Contains(viewmodel.Writer) && writer.Article.ID == article.ID).Select(writer => writer.Article.ID).FirstOrDefault()
                        ||
                        (article.CreatorUser.Person.FirstName + " " + article.CreatorUser.Person.LastName).Contains(viewmodel.Writer)
                        || viewmodel.Writer == ""
                        || viewmodel.Writer == null
                    )
                    &&
                    (
                        article.ArticleCategories.Any(ArtCat => SelectedCatIDs.Contains(ArtCat.Category.ID) && ArtCat.Article.ID == article.ID)
                    )
                    &&
                    (
                        (bool)article.Enable
                        &&
                        article.Published
                    )
                    &&
                    (
                        (article.ConferenceArticles.Any(ConfArt => ConfArt.Article.ID == article.ID && (ConfArt.Conference.Title.Contains(viewmodel.ConferenceName) || ConfArt.Conference.EnglishTitle.Contains(viewmodel.ConferenceName))) || viewmodel.ConferenceName == null || viewmodel.ConferenceName == "")
                    )
                    &&
                    (
                          (article.ArticlePresentType.ID == (int)Enums.ArticlePresentType.Poster && viewmodel.ShowPosterArticle && !string.IsNullOrEmpty(article.PosterUrl)) || !viewmodel.ShowPosterArticle
                    )

                )
                .Select(article => new
                {
                    article.ID,
                    article.EnglishTitle,
                    article.Title,
                    article.ArticleWriters,
                    article.ArticlePresentType,
                    article.PosterUrl,
                    article.PresentTime,
                    article.PresentLocation,
                    article.CreatorUser.Person.FirstName,
                    article.CreatorUser.Person.LastName,
                    article.Keywords,
                    ArticleID = article.ID,
                    ConferenceTitle = article.ConferenceArticles.Select(ConfArt => ConfArt.Conference).FirstOrDefault().Title,
                    article.ArticleCategories,
                    CategoryNames = article.ArticleCategories.Select(artCat => artCat.Category.CategoryNames.Where(transCat => transCat.Language.Value == CurrentLang).Select(transName => transName)),
                    article.CreateDate,
                    article.LikeCnt
                })
              .OrderByDescending(ConfArt => ConfArt.CreateDate)
              .Skip(skipRows)
              .Take(ArticlePageSize)
              .ToList();

            List<ArticleViewModel> Articles = new List<ArticleViewModel>();
            foreach (var article in DBArticles)
            {

                Articles.Add(new ArticleViewModel()
                {
                    ID = article.ID,
                    Title = article.Title,
                    EnglishTitle = article.EnglishTitle,
                    OwnerName = article.FirstName + " " + article.LastName,
                    Keywords = article.Keywords,
                    PresentTime = string.IsNullOrEmpty(article.PresentTime.ToString()) ? "" : article.PresentTime.Value.ToString("HH:mm"),
                    PresentDate = string.IsNullOrEmpty(article.PresentTime.ToString()) ? "" : (CommonHelper.DateAndTimes.GetPersianDate(article.PresentTime.Value).Split('/')[2] + " " + CommonHelper.DateAndTimes.GetMonthName(article.PresentTime.Value)),
                    PresentLocation = article.PresentLocation,
                    PosterUrl = !string.IsNullOrEmpty(article.PosterUrl) ? Url.Content(article.PosterUrl) : "",
                    WriterNames =/* article.FirstName + " " + article.LastName + */string.Join(",", article.ArticleWriters.Select(ArtWriter => string.Format("{0} {1}", ArtWriter.FirstName, ArtWriter.LastName))),
                    ConferenceName = article.ConferenceTitle,
                    FieldNames = string.Join(",", article.CategoryNames.Select(name => name.Select(TransName => TransName.Name).FirstOrDefault()).ToList()),
                    LikeCnt = article.LikeCnt == null ? 0 : (int)article.LikeCnt,
                    //FieldNames = string.Join(",", context.ArticleCategories.Include(ArtCat => ArtCat.Category).Where(ArtCat => ArtCat.Article.ID == article.ArticleID).Select(ArtCat => ArtCat.Category.Name)),
                });
            }

            return new ArticleClientListViewModel()
            {
                Articles = Articles,
                FilterForm = viewmodel,
                pageSize = ArticlePageSize,
                pageNumber = pageNumber,
                totalRecords = totalRecords
            };
        }
        #region ArticleDetails
        public ActionResult ArticleDetails(int ArticleID)
        {
            string CurrentLang = Request.Cookies["Lang"].Value;
            var contex = new ApplicationDbContext();
            var SelectedItem = contex.Articles
                .Include(article => article.ArticleCategories)
                .Include(article => article.ArticleCategories.Select(ArtCat => ArtCat.Category))
                .Include(article => article.ArticleCategories.Select(ArtCat => ArtCat.Category.CategoryNames))
                .Include(article => article.ArticleCategories.Select(ArtCat => ArtCat.Category.CategoryNames.Select(catName => catName.Language)))
                .Include(article => article.ConferenceArticles.Select(confArt => confArt.Conference))
                .Include(article => article.ArticleWriters)
                .Include(article => article.CreatorUser.Person)
                .Where(article => article.ID == ArticleID && article.Enable == true && article.Visible == true && article.Published)
                .Select(article => new
                {
                    article.ID,
                    article.Title,
                    article.EnglishTitle,
                    article.ArticleWriters,
                    article.CreatorUser.Person.FirstName,
                    article.CreatorUser.Person.LastName,
                    article.Summary,
                    article.Keywords,
                    article.FileUrl,
                    article.Visit,
                    ConferenceName = article.ConferenceArticles.Select(confArt => confArt.Conference.Title).FirstOrDefault(),
                    ConferencePlace = article.ConferenceArticles.Select(confArt => confArt.Conference.Place).FirstOrDefault(),
                    ConferenceID = article.ConferenceArticles.Select(confArt => confArt.Conference.ID).FirstOrDefault(),
                    CategoryNames = article.ArticleCategories.Select(artCat => artCat.Category.CategoryNames.Where(transCat => transCat.Language.Value == CurrentLang).Select(transName => transName))
                })
                .FirstOrDefault();
            var DbArticle = contex.Articles.Where(article => article.ID == ArticleID).FirstOrDefault();
            DbArticle.Visit = DbArticle.Visit + 1;
            contex.SaveChanges();
            return PartialView("~/Views/CSMI/ArticleDetails.cshtml", new ArticleViewModel()
            {
                ID = SelectedItem.ID,
                Title = SelectedItem.Title,
                EnglishTitle = SelectedItem.EnglishTitle,
                Summary = SelectedItem.Summary,
                FieldNames = string.Join(",", SelectedItem.CategoryNames.Select(name => name.Select(TransName => TransName.Name).FirstOrDefault()).ToList()),
                WriterNames = /*SelectedItem.FirstName + " " + SelectedItem.LastName + " " + */string.Join(",", SelectedItem.ArticleWriters.Select(ArtWriter => string.Format("{0} {1}", ArtWriter.FirstName, ArtWriter.LastName))),
                FileUrl = Url.Content(SelectedItem.FileUrl),
                Keywords = SelectedItem.Keywords,
                ConferenceName = SelectedItem.ConferenceName,
                ConferencePlace = SelectedItem.ConferencePlace,
                ConferenceID = SelectedItem.ConferenceID,
                Visit = SelectedItem.Visit,
            });
        }
        [HttpPost]
        public ActionResult LikeArticle(int ArticleID)
        {
            try
            {
                var contex = new ApplicationDbContext();
                var SelectedArticle = contex.Articles.Where(article => article.ID == ArticleID).FirstOrDefault();
                SelectedArticle.LikeCnt = SelectedArticle.LikeCnt + 1;
                contex.SaveChanges();
                return ControllerHelper.SuccessResult("با موفقیت انجام شد");
            }
            catch (Exception)
            {
                return ControllerHelper.ErrorResult("بروز خطای سیستمی");
            }

        }
        #endregion
        #endregion
        #region AnnualProgram
        public ActionResult AnnualProgram()
        {
            return PartialView("~/Views/CSMI/AnnualProgram.cshtml");
        }
        #endregion
        #region ArticleWrite
        public ActionResult ArticleWrite()
        {
            return PartialView("~/Views/CSMI/ArticleWrite.cshtml");
        }
        #endregion
        #region HistoryCrystal
        public ActionResult HistoryCrystal()
        {
            return PartialView("~/Views/CSMI/HistoryCrystal.cshtml");
        }
        #endregion 
        #region PaperArbiterList
        public ActionResult PaperArbiterList()
        {
            return PartialView("~/Views/CSMI/PaperArbiterList.cshtml");
        }
        #endregion 
        #region GuideArticleSend
        public ActionResult GuideArticleSend()
        {
            return PartialView("~/Views/CSMI/GuideArticleSend.cshtml");
        }
        #endregion
        #region GuideArticleArbiter
        public ActionResult GuideArticleArbiter()
        {
            return PartialView("~/Views/CSMI/GuideArticleArbiter.cshtml");
        }
        #endregion
        #region GuideSitemap
        public ActionResult GuideSitemap()
        {
            return PartialView("~/Views/CSMI/GuideSitemap.cshtml");
        }
        #endregion
        #region SocietyBases
        public ActionResult SocietyBases()
        {
            return PartialView("~/Views/CSMI/SocietyBases.cshtml");
        }
        #endregion 
        #region SocietyCertificate
        public ActionResult SocietyCertificate()
        {
            return PartialView("~/Views/CSMI/SocietyCertificate.cshtml");
        }
        #endregion 
        #region SocietyChart
        public ActionResult SocietyChart()
        {
            return PartialView("~/Views/CSMI/SocietyChart.cshtml");
        }
        #endregion
        #region SocietyGoals
        public ActionResult SocietyGoals()
        {
            return PartialView("~/Views/CSMI/SocietyGoals.cshtml");
        }
        #endregion
        #region SocietyActivities
        public ActionResult SocietyActivities()
        {
            return PartialView("~/Views/CSMI/SocietyActivities.cshtml");
        }
        #endregion
        #region SocietyHistory
        public ActionResult SocietyHistory()
        {
            return PartialView("~/Views/CSMI/SocietyHistory.cshtml");
        }
        #endregion 
        #region SocientyRules
        public ActionResult SocientyRules()
        {
            return PartialView("~/Views/CSMI/SocientyRules.cshtml");
        }
        #endregion
        #region RegisterFeatures
        public ActionResult RegisterFeatures()
        {
            return PartialView("~/Views/CSMI/RegisterFeatures.cshtml");
        }
        #endregion
        #region RegisterHelp
        public ActionResult RegisterHelp()
        {
            return PartialView("~/Views/CSMI/RegisterHelp.cshtml");
        }
        #endregion
        #region Tarefe
        public ActionResult TarefeConference()
        {
            return PartialView("~/Views/CSMI/TarefeConference.cshtml");
        }
        public ActionResult TarefeSociety()
        {
            return PartialView("~/Views/CSMI/TarefeSociety.cshtml");
        }
        #endregion 
        #region MajalehBases
        public ActionResult MajalehBases()
        {
            return PartialView("~/Views/CSMI/MajalehBases.cshtml");
        }
        #endregion 
        #region MajalehMember
        public ActionResult MajalehMember()
        {
            return PartialView("~/Views/CSMI/MajalehMember.cshtml");
        }
        #endregion 
        #region MajalehAbout
        public ActionResult MajalehAbout()
        {
            return PartialView("~/Views/CSMI/MajalehAbout.cshtml");
        }
        #endregion
        #region Election
        public ActionResult ElectionResult()
        {
            var context = new ApplicationDbContext();
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "undefined")
            {
                var selectedID = int.Parse(Request.QueryString["id"]);
                var selectedElection = context.Elections.Where(item => item.ID.Equals(selectedID))
                    .Select(item => new
                    {
                        item.ID,
                        item.Name,
                        item.EndDate
                    })
                    .SingleOrDefault();
                var TotalCandidList = context.Candidates
                  .Include(candid => candid.CandidateType)
                  .Include(candid => candid.Election)
                  .Where(candid => candid.Election.ID == selectedElection.ID && candid.Enable && candid.CandidateType.ID == 3)
                  .Select(candid =>
                        new
                        {
                            ID = candid.ID,
                            Name = candid.FirstName + " " + candid.LastName
                        })
                  .ToList();
                var FinalTotalCandidList = TotalCandidList.Select(candid => new
                {
                    ID = candid.ID,
                    Name = candid.Name,
                    Count = context.UserVotes
                              .Include(userVote => userVote.Candidate.Election)
                              .Count(userVote => userVote.Candidate.ID == candid.ID)
                }).ToList().OrderByDescending(item => item.Count);
                var TotalInspectorList = context.Candidates
                .Include(candid => candid.CandidateType)
                .Include(candid => candid.Election)
                .Where(candid => candid.Election.ID == selectedElection.ID && candid.Enable && candid.CandidateType.ID == 4)
                .Select(candid =>
                      new
                      {
                          ID = candid.ID,
                          Name = candid.FirstName + " " + candid.LastName
                      })
                .ToList();
                var FinalTotalInspectorList = TotalInspectorList.Select(candid => new
                {
                    ID = candid.ID,
                    Name = candid.Name,
                    Count = context.UserVotes
                             .Include(userVote => userVote.Candidate.Election)
                             .Count(userVote => userVote.Candidate.ID == candid.ID)
                }).ToList().OrderByDescending(item => item.Count);

                if (selectedElection.EndDate.Date >= DateTime.Now.Date)
                    return PartialView("~/Views/Shared/Unauthorized.cshtml");
                else
                {
                    var jsonSerialiser = new JavaScriptSerializer();
                    return PartialView("~/Views/Csmi/ElectionResult.cshtml", new ElectionResultViewModel()
                    {
                        ID = selectedElection.ID,
                        ElectionName = selectedElection.Name,
                        CandidCategories = jsonSerialiser.Serialize(FinalTotalCandidList.Select(candid => candid.Name)),
                        CandidVoteCount = jsonSerialiser.Serialize(FinalTotalCandidList.Select(candid => candid.Count)),
                        InspectorCategories = jsonSerialiser.Serialize(TotalInspectorList.Select(candid => candid.Name)),
                        InspectorVoteCount = jsonSerialiser.Serialize(FinalTotalInspectorList.Select(candid => candid.Count)),
                    });
                }

            }
            else
            {
                return PartialView("~/Views/Csmi/ElectionResult.cshtml", new ElectionResultViewModel()
                {
                });
            }
        }
        #endregion
    }
}