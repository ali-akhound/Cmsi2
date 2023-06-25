using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using AVA.Web.Mvc.Models.Admin;
using AVA.Core.Entities;
using AVA.UI.Helpers.Base;
using AVA.UI.Helpers.Common;
using System.Net;
using AVA.UI.Helpers.Controller;
using AVA.UI.Helpers.CustomAttribute;
using System.Web.Script.Serialization;
using AVA.UI.Helpers.FileUploadManagment;
using static AVA.Web.Mvc.Models.BaseViewModel;

namespace AVA.Web.Mvc.Controllers.Admin
{
    //Optimized

    [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Election)]
    public class ElectionController : BaseController
    {
        // GET: Admin/Election
        public ActionResult Election()
        {
            var context = new ApplicationDbContext();
            if (Request.QueryString["id"] == null || Request.QueryString["id"] == "undefined")
            {
                return PartialView("~/Areas/Admin/Views/Csmi/Election.cshtml", new ElectionViewModel() { ObjectState = ObjectState.Insert });
            }
            else
            {
                var selectedID = int.Parse(Request.QueryString["id"]);
                var selectedElection = context.Elections.Where(item => item.ID.Equals(selectedID))
                    .Select(item => new
                    {
                        item.ID,
                        item.Name,
                        item.StartDate,
                        item.EndDate,
                        item.ElectionPosterUrl,
                        item.ElectionAttachUrl,
                        item.CreateDate,
                    })
                    .SingleOrDefault();
                return PartialView("~/Areas/Admin/Views/Csmi/Election.cshtml", new ElectionViewModel()
                {
                    ID = selectedElection.ID,
                    Name = selectedElection.Name,
                    StartDateConverted = CommonHelper.DateAndTimes.GetPersianDate(selectedElection.StartDate),
                    EndDateConverted = CommonHelper.DateAndTimes.GetPersianDate(selectedElection.EndDate),
                    ElectionPosterUrl = !string.IsNullOrEmpty(selectedElection.ElectionPosterUrl) ? Url.Content(selectedElection.ElectionPosterUrl) : selectedElection.ElectionPosterUrl,
                    ElectionAttachUrl = !string.IsNullOrEmpty(selectedElection.ElectionAttachUrl) ? Url.Content(selectedElection.ElectionAttachUrl) : selectedElection.ElectionAttachUrl,
                    ObjectState = ObjectState.Update
                });
            }
        }
        public ActionResult ElectionManagement()
        {
            ViewBag.GridID = "ElectionManagementGrid";
            return PartialView("~/Areas/Admin/Views/Csmi/ElectionManagement.cshtml");
        }
        public ActionResult ElectionVoterManagement(int id)
        {
            var context = new ApplicationDbContext();
            ViewBag.GridID = "ElectionVoterManagementGrid";
            ViewBag.ElectionID = id;
            ViewBag.ElectionName = context.Elections.Where(item => item.ID == id).Select(item => item.Name).FirstOrDefault();
            return PartialView("~/Areas/Admin/Views/Csmi/ElectionVoterManagement.cshtml");
        }
        public ActionResult GetElectionPartial()
        {
            return new GridViewPartialController().GridViewPartial<ElectionViewModel>(Bind(), "Election", "GetElectionPartial", "ElectionManagementGrid", "ID");
        }
        public ActionResult GetElectionVoterPartial(int ElectionID)
        {
            return new GridViewPartialController().GridViewPartial<ElectionVoterViewModel>(ElectionVoterBind(ElectionID), "Election", "GetElectionVoterPartial", "ElectionVoterManagementGrid", "ID");
        }
        public ActionResult ExportTo(int ActionType)
        {
            return new GridViewPartialController().ExportTo<ElectionViewModel>(Bind(), ActionType, "ElectionManagementGrid");
        }
        public ActionResult ExportElectionVoterTo(int ActionType, int ElectionID)
        {
            return new GridViewPartialController().ExportTo<ElectionVoterViewModel>(ElectionVoterBind(ElectionID), ActionType, "ElectionVoterManagementGrid");
        }
        List<ElectionViewModel> Bind()
        {
            var ElectionList = new List<ElectionViewModel>();
            var context = new ApplicationDbContext();
            var model = new object[0];
            var DbElection = context.Elections
                .Select(item => new
                {
                    item.ID,
                    item.Name,
                    item.StartDate,
                    item.EndDate,
                    item.CreateDate,
                    item.Enable,
                })
            .ToList()
            .OrderByDescending(item => item.CreateDate);
            foreach (var item in DbElection)
            {
                ElectionList.Add(new ElectionViewModel()
                {
                    ID = item.ID,
                    StartDateConverted = CommonHelper.DateAndTimes.GetPersianDate(item.StartDate),
                    EndDateConverted = CommonHelper.DateAndTimes.GetPersianDate(item.EndDate),
                    Name = item.Name,
                    Enable = item.Enable,
                    CreateDateConverted = CommonHelper.DateAndTimes.GetPersianDate(item.CreateDate)
                });
            }
            if (ElectionList.Count() == 0)
            {
                ElectionList.Add(new ElectionViewModel());
            }
            return ElectionList;
        }
        List<ElectionVoterViewModel> ElectionVoterBind(int ElectionID)
        {
            var ElectionVoterList = new List<ElectionVoterViewModel>();
            var context = new ApplicationDbContext();
            var model = new object[0];
            var DbVoters = context.UserVotes
                .Include(item => item.CreatorUser)
                .Include(item => item.CreatorUser.Person)
                .Where(item => item.Candidate.Election.ID == ElectionID)
                .Select(item => new
                {
                    item.CreatorUser.UserName,
                    item.CreatorUser.Person.FirstName,
                    item.CreatorUser.Person.LastName,
                    item.CreatorUser.Person.Email
                }).Distinct().ToList();
            foreach (var item in DbVoters)
            {
                ElectionVoterList.Add(new ElectionVoterViewModel()
                {
                    Email = item.Email,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    UserName = item.UserName
                    //CreateDateConverted = CommonHelper.DateAndTimes.GetPersianDate(item.CreateDate)
                });
            }
            if (ElectionVoterList.Count() == 0)
            {
                ElectionVoterList.Add(new ElectionVoterViewModel());
            }
            return ElectionVoterList;
        }
        [HttpPost]
        [AjaxValidateAntiForgeryTokenAttribute]
        public ActionResult SubmitElection(string viewmodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        JavaScriptSerializer jss = new JavaScriptSerializer();
                        ElectionViewModel vm = jss.Deserialize<ElectionViewModel>(viewmodel);
                        string PosterPic = "";
                        string AttachPic = "";
                        if (Request.Files.Count > 0)
                        {
                            var file = Request.Files[CommonHelper.ExposeProperty.GetPropertyInfo(vm, m => m.ElectionPoster).Name];
                            if (file != null && file.ContentLength > 0)
                            {
                                PosterPic = FileUploadManagment.UploadFile("~/assets/img/Attach/Election/", "", file, FileUploadManagment.AppFileType.Image);
                                if (PosterPic == "")
                                {
                                    return ControllerHelper.ErrorResult("فرمت یا حجم فایل عکس پوستر صحیح نمی باشد");
                                }
                            }
                            file = Request.Files[CommonHelper.ExposeProperty.GetPropertyInfo(vm, m => m.ElectionAttach).Name];
                            if (file != null && file.ContentLength > 0)
                            {
                                AttachPic = FileUploadManagment.UploadFile("~/assets/img/Attach/Election/", "", file, FileUploadManagment.AppFileType.Image);
                                if (AttachPic == "")
                                {
                                    return ControllerHelper.ErrorResult("فرمت یا حجم فایل عکس پیوست صحیح نمی باشد");
                                }
                            }
                        }
                        DateTime? StartDate = CommonHelper.DateAndTimes.GetGregorianDate(vm.StartDateConverted);
                        DateTime? EndDate = CommonHelper.DateAndTimes.GetGregorianDate(vm.EndDateConverted);
                        if (StartDate == null || EndDate == null)
                        {
                            return ControllerHelper.ErrorResult("تاریخ شروع یا تاریخ پایان اشتباه است");
                        }
                        if (StartDate > EndDate)
                        {
                            return ControllerHelper.ErrorResult("تاریخ شروع یا تاریخ پایان اشتباه است");
                        }
                        var context = new ApplicationDbContext();
                        if (vm.ObjectState == ObjectState.Insert)
                        {
                            if (PosterPic == "")
                            {
                                return ControllerHelper.ErrorResult("لطفا عکس پوستر را انتخاب نمایید");
                            }
                            context.Elections.Add(new Election()
                            {
                                Name = vm.Name,
                                StartDate = (DateTime)StartDate,
                                EndDate = (DateTime)EndDate,
                                Enable = false,
                                ElectionPosterUrl = PosterPic,
                                ElectionAttachUrl = AttachPic,
                                CreateDate = DateTime.Now,
                                CreatorUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault(),
                            });
                        }
                        else
                        {
                            Election selectedModel = context.Elections.Where(item => item.ID == vm.ID).Single();
                            if (selectedModel != null)
                            {

                                selectedModel.Name = vm.Name;
                                if (context.UserVotes.Count(userVote => userVote.Candidate.Election.ID == selectedModel.ID) <= 0)
                                {
                                    selectedModel.StartDate = (DateTime)StartDate;
                                    selectedModel.EndDate = (DateTime)EndDate;
                                }
                                selectedModel.LastModifyUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault();
                                selectedModel.LastModifyDate = DateTime.Now;
                                if (PosterPic != "")
                                    selectedModel.ElectionPosterUrl = PosterPic;
                                if (AttachPic != "")
                                    selectedModel.ElectionAttachUrl = AttachPic;
                            }
                        }
                        context.SaveChanges();
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
                return ControllerHelper.ErrorResult("بروز خطای سیستمی" + ex.Message);
            }
        }
        [HttpPost]
        [AjaxValidateAntiForgeryTokenAttribute]
        public ActionResult ActiveElection(string[] ids)
        {
            var context = new ApplicationDbContext();
            var trans = context.Database.BeginTransaction();
            try
            {
                context.Elections.ToList().ForEach(conf => conf.Enable = false);
                foreach (var id in ids)
                {
                    int CurrentID = int.Parse(id);
                    var selectedItem = context.Elections
                        .Where(item => item.ID == CurrentID).Single();
                    if (selectedItem != null)
                    {
                        selectedItem.Enable = true;
                    }
                }
                context.SaveChanges();
                trans.Commit();
                return ControllerHelper.SuccessResult("با موفقیت ثبت شد");
            }
            catch (Exception ex)
            {
                return ControllerHelper.ErrorResult("بروز خطای سیستمی");
            }

        }
        [HttpPost]
        [AjaxValidateAntiForgeryTokenAttribute]
        public ActionResult DeleteElection(string[] ids)
        {
            var context = new ApplicationDbContext();
            var dbContextTransaction = context.Database.BeginTransaction();
            try
            {
                foreach (var id in ids)
                {
                    int ID = int.Parse(id);
                    var selectedItem = context.Elections
                        .Where(item => item.ID == ID).Single();
                    if (selectedItem != null)
                    {

                        context.Elections.Remove(selectedItem);
                    }

                }
                context.SaveChanges();
                dbContextTransaction.Commit();
                return ControllerHelper.SuccessResult("با موفقیت ثبت شد");
            }
            catch (Exception ex)
            {
                dbContextTransaction.Rollback();
                return ControllerHelper.ErrorResult("بروز خطای سیستمی");
            }


        }
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
                    return PartialView("~/Areas/Admin/Views/Shared/UnauthorizedWithMessage.cshtml", new UnauthorizedViewModel()
                    {
                        Message = "به دلیل پایان نیافتن مهلت انتخابات قادر به رویت نتیجه انتخابات نیستید",
                        ReturnUrl = "ElectionManagement"
                    });
                else
                {
                    var jsonSerialiser = new JavaScriptSerializer();
                    return PartialView("~/Areas/Admin/Views/Csmi/ElectionResult.cshtml", new ElectionResultViewModel()
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
                return PartialView("~/Areas/Admin/Views/Csmi/ElectionResult.cshtml", new ElectionResultViewModel()
                {
                });
            }
        }
        public ActionResult ElectionVoterDiagram()
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
                var TotalVoteList = context.UserVotes
                  .Where(userVote => userVote.Candidate.Election.ID == selectedElection.ID && userVote.Candidate.Enable)
                  .Select(userVote =>
                        new
                        {
                            userVote.CreateDate,
                            userVote.CreatorUser.Id
                        })
                   .Distinct()
                  .ToList();
                var FinalTotalVoteList = TotalVoteList.Select(item => new { item.CreateDate.Date, item.Id }).Distinct().ToList();
                var diagramData = FinalTotalVoteList
                      .GroupBy(item => new { item.Date })
                      .Select(item =>
                             new
                             {
                                 Date = CommonHelper.DateAndTimes.GetPersianDate(item.Key.Date),
                                 Count = item.Count()
                             })
                      .OrderBy(item => item.Date);
                var jsonSerialiser = new JavaScriptSerializer();
                return PartialView("~/Areas/Admin/Views/Csmi/ElectionVoterDiagram.cshtml", new ElectionVoterDiagramViewModel()
                {
                    ID = selectedElection.ID,
                    ElectionName = selectedElection.Name,
                    DateCategories = jsonSerialiser.Serialize(diagramData.Select(votes => votes.Date)),
                    VoteCount = jsonSerialiser.Serialize(diagramData.Select(votes => votes.Count)),
                });
            }
            else
            {
                return PartialView("~/Areas/Admin/Views/Csmi/ElectionVoterDiagram.cshtml", new ElectionVoterDiagramViewModel()
                {
                });
            }
        }
    }
}