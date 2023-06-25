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
using AVA.Web.Mvc.Areas.Admin.Models.Base;
using System.Web.Script.Serialization;
using AVA.UI.Helpers.FileUploadManagment;
using AVA.UI.Helpers.MailSmsService;

namespace AVA.Web.Mvc.Controllers.Admin
{
    //Optimized

    [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Vote)]
    public class VoteController : BaseController
    {
        // GET: Admin/Vote
        public ActionResult Vote()
        {
            var context = new ApplicationDbContext();
            var vm = new VoteViewModel();
            var SelectedElection = context.Elections.Where(election => election.Enable && election.EndDate >= DateTime.Now).FirstOrDefault();
            var DbCandidateTypes = context.CandidateTypes.Where(candidateType => candidateType.Enable).ToList();
            foreach (var candidateType in DbCandidateTypes)
            {
                vm.CandidateTypes.Add(new DropDownVm() { Text = candidateType.Name, Value = candidateType.ID.ToString() });
            }
            if (SelectedElection != null)
            {
                var candidates = context.Candidates
                    .Include(candidate => candidate.CandidateType)
                    .Where(candidate => candidate.Enable && candidate.Election.ID == SelectedElection.ID).ToList();
                foreach (var item in candidates)
                {
                    vm.UserVotes.Add(new UserVoteViewModel()
                    {
                        Candidate = new CandidateViewModel()
                        {
                            ID = item.ID,
                            FirstName = item.FirstName,
                            LastName = item.LastName,
                            PersonPicUrl = !string.IsNullOrEmpty(item.PersonPic) ? Url.Content(item.PersonPic) : item.PersonPic,
                            Explain = item.Explain,
                            SelectedCandidateTypeName = item.CandidateType.Name,
                            SelectedCandidateTypeID = item.CandidateType.ID.ToString(),
                        },
                        IsChecked = false
                    });
                }
            }
            //foreach (var item in DbVoteTypes)
            //{
            //    vm.VoteTypes.Add(new DropDownVm() { Text = item.Name, Value = item.ID.ToString() });
            //}
            //vm.ObjectState = ObjectState.Insert;
            //if (Request.QueryString["ElectionID"] != null)
            //{
            //    vm.ElectionID = int.Parse(Request.QueryString["ElectionID"]);
            //    vm.ElectionName = context.Elections.Where(item => item.ID == vm.ElectionID).FirstOrDefault().Name;
            //}
            return PartialView("~/Areas/Admin/Views/Csmi/Vote.cshtml", vm);

        }
        [HttpPost]
        [AjaxValidateAntiForgeryTokenAttribute]
        public ActionResult SubmitVote(VoteViewModel vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var context = new ApplicationDbContext();
                        var SelectedElection = context.Elections.Where(election => election.Enable && election.EndDate >= DateTime.Now).FirstOrDefault();
                        if(SelectedElection!=null)
                        {
                            var cnt = context.UserVotes.Count(userVote => userVote.CreatorUser.UserName == User.Identity.Name && userVote.Candidate.Election.ID == SelectedElection.ID);
                            var currentuser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault();
                            var userVoteToken = context.UserVoteTokens.Where(userToken => userToken.Election.ID == SelectedElection.ID && userToken.CreatorUser.Id == currentuser.Id).FirstOrDefault();
                            if (cnt == 0)
                            {
                                if (vm.ActivationCode == userVoteToken.Token)
                                {
                                    int candidCnt = 0;
                                    int inspectorCnt = 0;
                                    var userVotes = new List<UserVote>();
                                    foreach (var candidate in vm.UserVotes)
                                    {
                                        var DbCandidate = context.Candidates
                                            .Include(candid => candid.CandidateType)
                                            .Where(candid => candid.ID == candidate.Candidate.ID).SingleOrDefault();
                                        if (candidate.IsChecked)
                                        {
                                            if (DbCandidate.CandidateType.ID == 3)
                                            {
                                                candidCnt++;
                                            }
                                            if (DbCandidate.CandidateType.ID == 4)
                                            {
                                                inspectorCnt++;
                                            }
                                            userVotes.Add(new UserVote()
                                            {
                                                Candidate = context.Candidates.Where(candid => candid.ID == candidate.Candidate.ID).SingleOrDefault(),
                                                CreatorUser = currentuser,
                                                CreateDate = DateTime.Now,
                                            });
                                        }
                                    }
                                    if (candidCnt <= 5 && inspectorCnt <= 1)
                                    {
                                        context.UserVotes.AddRange(userVotes);
                                        context.SaveChanges();
                                    }
                                    else
                                    {
                                        return ControllerHelper.ErrorResult("اطلاعات ارسالی اشتباه است");
                                    }
                                }
                                else
                                {
                                    return ControllerHelper.ErrorResult("کد فعال سازی اشتباه است");
                                }
                            }
                            else
                            {
                                return ControllerHelper.ErrorResult("شما قبلا در رای گیری شرکت کرده اید");
                            }
                        }
                        else
                        {
                            return ControllerHelper.ErrorResult("زمان انتخابات به پایان رسیده است");
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
                return ControllerHelper.ErrorResult("بروز خطای سیستمی" + ex.Message);
            }
        }
        [HttpPost]
        [AjaxValidateAntiForgeryTokenAttribute]
        public ActionResult SendActivationCode()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var context = new ApplicationDbContext();
                        var SelectedElection = context.Elections.Where(election => election.Enable && election.EndDate >= DateTime.Now).FirstOrDefault();
                        var cnt = context.UserVotes.Count(userVote => userVote.CreatorUser.UserName == User.Identity.Name && userVote.Candidate.Election.ID == SelectedElection.ID);
                        var currentuser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault();
                        var userVoteToken = context.UserVoteTokens.Where(userToken => userToken.Election.ID == SelectedElection.ID && userToken.CreatorUser.Id == currentuser.Id).FirstOrDefault();
                        string token = new Random().Next(100000, 999999).ToString();
                        if (cnt == 0)
                        {
                            if (userVoteToken == null)
                            {
                                userVoteToken = new UserVoteToken()
                                {
                                    CreatorUser = currentuser,
                                    CreateDate = DateTime.Now,
                                    Election = SelectedElection,
                                    Token = token
                                };
                                context.UserVoteTokens.Add(userVoteToken);
                            }
                            else
                            {
                                userVoteToken.Token = token;
                                userVoteToken.LastModifyDate = DateTime.Now;
                            }
                            context.SaveChanges();
                            EmailService.EmailResponse MailResult = EmailService.SendActivationCode(currentuser.Id, token);
                            if (MailResult.Code == (int)EmailService.EmailEnum.Success)
                            {
                                return ControllerHelper.SuccessResult("ثبت نام با موفقیت انجام شد. لطفا برای فعال سازی اکانت به ایمیل فعال سازی که برای شما ارسال شده است مراجعه فرمایید.در صورت نبودن ایمیل اسپم لیست خود را بررسی بفرمایید");
                            }
                            else
                                return ControllerHelper.ErrorResult("بروز خطای سیستمی در ارسال ایمیل فعال سازی");
                        }
                        else
                        {
                            return ControllerHelper.ErrorResult("شما قبلا در رای گیری شرکت کرده اید");
                        }
                    }
                    catch (Exception ex)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.Message);
                    }
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
    }
}