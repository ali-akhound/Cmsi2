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

namespace AVA.Web.Mvc.Controllers.Admin
{
    //Optimized
    public class CandidateController : BaseController
    {
        // GET: Admin/Candidate
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Candidate)]
        public ActionResult Candidate()
        {
            var context = new ApplicationDbContext();
            var DbCandidateTypes = context.CandidateTypes.Where(candidate => candidate.Enable).ToList();
            var vm = new CandidateViewModel();
            foreach (var item in DbCandidateTypes)
            {
                vm.CandidateTypes.Add(new DropDownVm() { Text = item.Name, Value = item.ID.ToString() });
            }
            if (Request.QueryString["id"] == null || Request.QueryString["id"] == "undefined")
            {
                vm.ObjectState = ObjectState.Insert;
                if (Request.QueryString["ElectionID"] != null)
                {
                    vm.ElectionID = int.Parse(Request.QueryString["ElectionID"]);
                    vm.ElectionName = context.Elections.Where(item => item.ID == vm.ElectionID).FirstOrDefault().Name;
                }
                return PartialView("~/Areas/Admin/Views/Csmi/Candidate.cshtml", vm);
            }
            else
            {

                var selectedID = int.Parse(Request.QueryString["id"]);
                var selectedCandidate = context.Candidates
                    .Include(item => item.Election)
                    .Include(item => item.CandidateType)
                    .Where(item => item.ID.Equals(selectedID))
                    .Select(item => new
                    {
                        item.ID,
                        item.FirstName,
                        item.LastName,
                        item.PersonPic,
                        item.Explain,
                        item.CreateDate,
                        item.Degree,
                        item.FieldOfStudy,
                        item.University,
                        item.ResumeUrl,
                        ElectionID = item.Election.ID,
                        ElectionName = item.Election.Name,
                        CandidateTypeID = item.CandidateType.ID
                    })
                    .SingleOrDefault();
                vm = new CandidateViewModel()
                {
                    ID = selectedCandidate.ID,
                    FirstName = selectedCandidate.FirstName,
                    LastName = selectedCandidate.LastName,
                    PersonPicUrl = !string.IsNullOrEmpty(selectedCandidate.PersonPic) ? Url.Content(selectedCandidate.PersonPic) : selectedCandidate.PersonPic,
                    ResumeUrl = !string.IsNullOrEmpty(selectedCandidate.ResumeUrl) ? Url.Content(selectedCandidate.ResumeUrl) : selectedCandidate.ResumeUrl,
                    Explain = selectedCandidate.Explain,
                    ElectionName = selectedCandidate.ElectionName,
                    ElectionID = selectedCandidate.ElectionID,
                    Degree = selectedCandidate.Degree,
                    FieldOfStudy=selectedCandidate.FieldOfStudy,
                    University = selectedCandidate.University,
                    CandidateTypes = vm.CandidateTypes,
                    SelectedCandidateTypeID = selectedCandidate.CandidateTypeID.ToString(),
                    ObjectState = ObjectState.Update
                };

                return PartialView("~/Areas/Admin/Views/Csmi/Candidate.cshtml", vm);
            }
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Candidate)]
        public ActionResult CandidateManagement(int ElectionID)
        {

            var context = new ApplicationDbContext();
            ViewBag.GridID = "CandidateManagementGrid";
            ViewBag.ElectionID = ElectionID;
            ViewBag.ElectionName = context.Elections.Where(item => item.ID == ElectionID).FirstOrDefault().Name;
            return PartialView("~/Areas/Admin/Views/Csmi/CandidateManagement.cshtml");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Candidate)]
        public ActionResult GetCandidatePartial(int ElectionID)
        {
            ViewBag.ElectionID = ElectionID;
            return new GridViewPartialController().GridViewPartial<CandidateViewModel>(Bind(ElectionID), "Candidate", "GetCandidatePartial", "CandidateManagementGrid", "ID");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Candidate)]
        public ActionResult ExportTo(int ActionType, int ElectionID)
        {
            return new GridViewPartialController().ExportTo<CandidateViewModel>(Bind(ElectionID), ActionType, "CandidateManagementGrid");
        }
        List<CandidateViewModel> Bind(int ElectionID)
        {
            var CandidateList = new List<CandidateViewModel>();
            var context = new ApplicationDbContext();
            var model = new object[0];
            var DbCandidate = context.Candidates
                .Include(item =>item.CandidateType)
                .Where(item => item.Election.ID == ElectionID)
                .Select(item => new
                {
                    item.ID,
                    item.FirstName,
                    item.LastName,
                    item.PersonPic,
                    item.Explain,
                    item.CreateDate,
                    item.Enable,
                    CandidateTypeName=item.CandidateType.Name
                })
            .ToList()
            .OrderByDescending(item => item.CreateDate);
            foreach (var item in DbCandidate)
            {
                CandidateList.Add(new CandidateViewModel()
                {
                    ID = item.ID,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    PersonPicUrl = item.PersonPic,
                    Explain = item.Explain,
                    Enable = item.Enable,
                    SelectedCandidateTypeName= item.CandidateTypeName,
                    CreateDateConverted = CommonHelper.DateAndTimes.GetPersianDate(item.CreateDate)
                });
            }
            if (CandidateList.Count() == 0)
            {
                CandidateList.Add(new CandidateViewModel());
            }
            return CandidateList;
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Candidate)]
        [HttpPost]
        [AjaxValidateAntiForgeryTokenAttribute]
        [ValidateInput(false)]
        public ActionResult SubmitCandidate(string viewmodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        JavaScriptSerializer jss = new JavaScriptSerializer();
                        CandidateViewModel vm = jss.Deserialize<CandidateViewModel>(viewmodel);
                        string PersonPic = "";
                        string Resume = "";
                        if (Request.Files.Count > 0)
                        {
                            var file = Request.Files[CommonHelper.ExposeProperty.GetPropertyInfo(vm, m => m.PersonPic).Name];
                            if (file != null && file.ContentLength > 0)
                            {
                                PersonPic = FileUploadManagment.UploadFile("~/assets/img/Attach/Candidate/", "", file, FileUploadManagment.AppFileType.Image);
                                if (PersonPic == "")
                                {
                                    return ControllerHelper.ErrorResult("فرمت یا حجم فایل عکس پرسنلی صحیح نمی باشد");
                                }
                            }
                            file = Request.Files[CommonHelper.ExposeProperty.GetPropertyInfo(vm, m => m.Resume).Name];
                            if (file != null && file.ContentLength > 0)
                            {
                                Resume = FileUploadManagment.UploadFile("~/assets/img/Attach/Candidate/", "", file, FileUploadManagment.AppFileType.Document);
                                if (Resume == "")
                                {
                                    return ControllerHelper.ErrorResult("فرمت یا حجم فایل رزومه صحیح نمی باشد");
                                }
                            }
                        }
                        var context = new ApplicationDbContext();
                        var DbElection = context.Elections.Where(item => item.ID == vm.ElectionID).FirstOrDefault();                       
                        int SelectedCandidateTypeID = int.Parse(vm.SelectedCandidateTypeID);
                        if (vm.ObjectState == ObjectState.Insert)
                        {
                            if(PersonPic=="")
                            {
                                return ControllerHelper.ErrorResult("لطفا عکس پرسنلی را انتخاب نمایید");
                            }
                            if (Resume == "")
                            {
                                return ControllerHelper.ErrorResult("لطفا رزومه را انتخاب نمایید");
                            }
                            context.Candidates.Add(new Candidate()
                            {
                                FirstName = vm.FirstName,
                                LastName = vm.LastName,
                                Explain = vm.Explain,
                                PersonPic = PersonPic,
                                ResumeUrl=Resume,
                                Enable = true,
                                CreateDate = DateTime.Now,
                                Degree=vm.Degree,
                                University=vm.University,
                                FieldOfStudy=vm.FieldOfStudy,
                                Election = DbElection,
                                CandidateType = context.CandidateTypes.Where(type => type.ID == SelectedCandidateTypeID).SingleOrDefault(),
                                CreatorUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault(),
                            });
                            if (DateTime.Now >= DbElection.StartDate)
                            {
                                return ControllerHelper.ErrorResult("به دلیل شروع شدن انتخابات قادر به افزودن کاندید نیستید");
                            }
                        }
                        else
                        {
                            Candidate selectedModel = context.Candidates.Where(item => item.ID == vm.ID).Single();
                            if (selectedModel != null)
                            {
                                selectedModel.FirstName = vm.FirstName;
                                selectedModel.LastName = vm.LastName;
                                selectedModel.Explain = vm.Explain;
                                selectedModel.Degree = vm.Degree;
                                selectedModel.University = vm.University;
                                selectedModel.FieldOfStudy = vm.FieldOfStudy;
                                if (Resume != "")
                                    selectedModel.ResumeUrl = Resume;
                                if (PersonPic != "")
                                    selectedModel.PersonPic = PersonPic;
                                selectedModel.CandidateType = context.CandidateTypes.Where(type => type.ID == SelectedCandidateTypeID).SingleOrDefault();
                                selectedModel.LastModifyUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault();
                                selectedModel.LastModifyDate = DateTime.Now;
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
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Candidate)]
        public ActionResult ActiveCandidate(string[] ids)
        {
            var context = new ApplicationDbContext();
            try
            {
                foreach (var id in ids)
                {
                    int CurrentID = int.Parse(id);
                    var selectedItem = context.Candidates.Where(item => item.ID == CurrentID).Single();
                    if (selectedItem != null)
                        selectedItem.Enable = !selectedItem.Enable;
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
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Candidate)]
        public ActionResult DeleteCandidate(string[] ids)
        {
            var context = new ApplicationDbContext();
            var dbContextTransaction = context.Database.BeginTransaction();
            try
            {
                foreach (var id in ids)
                {
                    int ID = int.Parse(id);
                    var selectedItem = context.Candidates
                        .Where(item => item.ID == ID).Single();
                    if (selectedItem != null)
                    {
                        try
                        {
                            System.IO.File.Delete(selectedItem.PersonPic);
                        }
                        catch (Exception)
                        {

                        }
                        context.Candidates.Remove(selectedItem);
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
    }
}