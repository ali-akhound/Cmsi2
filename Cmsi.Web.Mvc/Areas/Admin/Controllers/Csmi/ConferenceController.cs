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
using System.Web.Script.Serialization;
using AVA.UI.Helpers.FileUploadManagment;

namespace AVA.Web.Mvc.Controllers.Admin
{
    //Optimized
    public class ConferenceController : BaseController
    {
        #region Conference
        // GET: Admin/Conference
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Conference)]
        public ActionResult Conference()
        {
            if (Request.QueryString["id"] == null || Request.QueryString["id"] == "undefined")
            {

                var context = new ApplicationDbContext();
                List<AdminConferencePackageViewModel> ConferencePackages = new List<AdminConferencePackageViewModel>();
                var DBConferencePackageNames = context.PackageNameTranslations
                     .Include(PackTrans => PackTrans.PackageName)
                     .Include(PackTrans => PackTrans.Language)
                     .Where(PackTrans =>
                        (bool)PackTrans.PackageName.Enable
                        //&&
                        //(
                        //    PackTrans.PackageName.PackageType.ID == (int)Enums.PackageType.RegisterConferencePackage
                        //    ||
                        //    PackTrans.PackageName.PackageType.ID == (int)Enums.PackageType.ConferenceFeaturePackage
                        //)
                        &&
                        PackTrans.Language.Value == "fa"
                     )
                     .Select(PackTrans => new
                     {
                         PackTrans.PackageName.ID,
                         PackTrans.Name
                     })
                     .ToList();
                foreach (var item in DBConferencePackageNames)
                {
                    ConferencePackages.Add(new AdminConferencePackageViewModel()
                    {
                        ID = item.ID,
                        Name = item.Name,
                        Price = 0,
                    });
                }

                return PartialView("~/Areas/Admin/Views/Csmi/Conference.cshtml", new ConferenceViewModel()
                {
                    ObjectState = ObjectState.Insert,
                    AdminConferencePackage = ConferencePackages
                });
            }
            else
            {
                var ConferenceID = int.Parse(Request.QueryString["id"]);
                var context = new ApplicationDbContext();
                List<AdminConferencePackageViewModel> ConferencePackages = new List<AdminConferencePackageViewModel>();
                var DBConferencePackages = context.ConferencePackages
                     .Include(ConfPack => ConfPack.PackageName)
                     .Include(ConfPack => ConfPack.PackageName.PackageNameTranslations)
                     .Include(ConfPack => ConfPack.PackageName.PackageNameTranslations.Select(trans => trans.Language))
                     .Where(
                            ConfPack => ConfPack.PackageName.PackageNameTranslations.Any(trans => trans.Language.Value == "fa")
                            //&&
                            //(
                            //    ConfPack.PackageName.PackageType.ID == (int)Enums.PackageType.RegisterConferencePackage
                            //    ||
                            //    ConfPack.PackageName.PackageType.ID == (int)Enums.PackageType.ConferenceFeaturePackage
                            //)
                            &&
                            ConfPack.Conference.ID == ConferenceID
                           )
                     .Select(ConfPack => new
                     {
                         ConfPack.PackageName.ID,
                         Name = ConfPack.PackageName.PackageNameTranslations.FirstOrDefault().Name,
                         ConfPack.Price
                     })
                     .ToList();
                foreach (var item in DBConferencePackages)
                {
                    ConferencePackages.Add(new AdminConferencePackageViewModel()
                    {
                        ID = item.ID,
                        Name = item.Name,
                        Price = item.Price,
                    });
                }
                var selectedID = int.Parse(Request.QueryString["id"]);
                var selectedConference = context.Conferences
                    .Where(item => item.ID.Equals(selectedID))
                    .Select(item => new
                    {
                        item.ID,
                        item.Title,
                        item.Enable,
                        item.CreateDate,
                        item.SendStartDate,
                        item.SendEndDate,
                        item.EventDate,
                        item.EnglishTitle,
                        item.PosterImageUrl,
                        item.AttachFileUrl,
                        item.AttachPresentationHelpFileUrl,
                        item.AttachPresentationPowerpointFileUrl,
                        item.AttachPosterTemplateFileUrl,
                        item.AttachChemistryPresentationProgramFileUrl,
                        item.AttachGeologyPresentationProgramFileUrl,
                        item.AttachPhysicsPresentationProgramFileUrl,
                        item.AttachAmayeshFileUrl,
                        item.AttachScientificCommitteeFileUrl,
                        item.AttachExecutiveCommitteeFileUrl,
                        item.AttachTotalArticlesFileUrl,
                        item.Place,
                        item.TelegramUrl,
                        item.Explain
                        //item.PublishPrice
                    })
                    .SingleOrDefault();
                return PartialView("~/Areas/Admin/Views/Csmi/Conference.cshtml", new ConferenceViewModel()
                {
                    ID = selectedConference.ID,
                    Title = selectedConference.Title,
                    EnglishTitle = selectedConference.EnglishTitle,
                    CreateDateConverted = CommonHelper.DateAndTimes.GetPersianDate(selectedConference.CreateDate),
                    SendStartDateConverted = CommonHelper.DateAndTimes.GetPersianDate(selectedConference.SendStartDate),
                    SendEndDateConverted = CommonHelper.DateAndTimes.GetPersianDate(selectedConference.SendEndDate),
                    EventDateConverted = CommonHelper.DateAndTimes.GetPersianDate(selectedConference.EventDate),
                    AdminConferencePackage = ConferencePackages,
                    EventDate = selectedConference.EventDate.ToShortDateString(),
                    Place = selectedConference.Place,
                    PosterImageUrl = selectedConference.PosterImageUrl,
                    AttachFileUrl = selectedConference.AttachFileUrl,
                    AttachPresentationHelpFileUrl = selectedConference.AttachPresentationHelpFileUrl,
                    AttachPresentationPowerpointFileUrl = selectedConference.AttachPresentationPowerpointFileUrl,
                    AttachPosterTemplateFileUrl = selectedConference.AttachPosterTemplateFileUrl,
                    AttachChemistryPresentationProgramFileUrl = selectedConference.AttachChemistryPresentationProgramFileUrl,
                    AttachGeologyPresentationProgramFileUrl = selectedConference.AttachGeologyPresentationProgramFileUrl,
                    AttachPhysicsPresentationProgramFileUrl = selectedConference.AttachPhysicsPresentationProgramFileUrl,
                    AttachAmayeshFileUrl = selectedConference.AttachAmayeshFileUrl,
                    AttachExecutiveCommitteeFileUrl = selectedConference.AttachExecutiveCommitteeFileUrl,
                    AttachScientificCommitteeFileUrl = selectedConference.AttachScientificCommitteeFileUrl,
                    AttachTotalArticlesFileUrl=selectedConference.AttachTotalArticlesFileUrl,
                    Explain = selectedConference.Explain,
                    TelegramUrl = selectedConference.TelegramUrl,
                    //PublishPrice = selectedConference.PublishPrice,
                    Enable = selectedConference.Enable,
                    ObjectState = ObjectState.Update
                }); ;
            }
        }
        [AuthorizeUser(AccessLevelIDs = new int[] { (int)ControllerHelper.SysModuleType.Conference, (int)ControllerHelper.SysModuleType.ScientificSecretaryCircle })]
        public ActionResult ConferenceManagement()
        {
            ViewBag.GridID = "ConferenceManagementGrid";
            return PartialView("~/Areas/Admin/Views/Csmi/ConferenceManagement.cshtml");
        }
        [AuthorizeUser(AccessLevelIDs = new int[] { (int)ControllerHelper.SysModuleType.Conference, (int)ControllerHelper.SysModuleType.ScientificSecretaryCircle })]
        public ActionResult GetConferencePartial()
        {
            return new GridViewPartialController().GridViewPartial<ConferenceViewModel>(Bind(), "Conference", "GetConferencePartial", "ConferenceManagementGrid", "ID");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Conference)]
        public ActionResult ExportTo(int ActionType)
        {
            return new GridViewPartialController().ExportTo<ConferenceViewModel>(Bind(), ActionType, "ConferenceManagementGrid");
        }
        List<ConferenceViewModel> Bind()
        {
            var Conferences = new List<ConferenceViewModel>();
            var context = new ApplicationDbContext();
            var model = new object[0];
            var DbConferences = context.Conferences
                .Select(item => new
                {
                    item.ID,
                    item.Title,
                    item.Enable,
                    item.Visible,
                    item.CreateDate,
                    item.SendStartDate,
                    item.SendEndDate,
                    item.EventDate,
                    //item.PublishPrice
                })
                .ToList().OrderByDescending(item => item.CreateDate);
            foreach (var item in DbConferences)
            {
                var conf = new ConferenceViewModel()
                {
                    ID = item.ID,
                    Title = item.Title,
                    CreateDateConverted = CommonHelper.DateAndTimes.GetPersianDate(item.CreateDate),
                    SendStartDateConverted = CommonHelper.DateAndTimes.GetPersianDate(item.SendStartDate),
                    SendEndDateConverted = CommonHelper.DateAndTimes.GetPersianDate(item.SendEndDate),
                    EventDateConverted = CommonHelper.DateAndTimes.GetPersianDate(item.EventDate),
                    ArticleCount = context.ConferenceArticles.Where(confArt => confArt.Conference.ID == item.ID).Count(),
                    Fields = string.Join(",", context.ConferenceCategories.Include(confCat => confCat.Category.Name).Where(confCat => confCat.Conference.ID == item.ID).Select(confCat => confCat.Category.Name)),
                    //PublishPrice = item.PublishPrice,
                    Enable = item.Enable,
                    Visible = item.Visible,

                };
                var AccessArea = new AVA.UI.Helpers.CustomAttribute.AuthorizeUserAttribute() { AccessLevelID = (int)ControllerHelper.SysModuleType.Managment };
                if (!AccessArea.HasAccess(new HttpContextWrapper(System.Web.HttpContext.Current)))
                {
                    if (context.ConferenceScientificSecretaries
                        .Include(ConfScien => ConfScien.ScientificSecretary.RefrenceUser)
                        .Count(ConfScien => ConfScien.Conference.ID == item.ID
                        &&
                        ConfScien.ScientificSecretary.RefrenceUser.UserName == User.Identity.Name
                        ) > 0
                        ||
                        context.ConferenceExecutors
                        .Include(ConfExec => ConfExec.Executor.RefrenceUser)
                        .Count(ConfExec => ConfExec.Conference.ID == item.ID
                        &&
                        ConfExec.Executor.RefrenceUser.UserName == User.Identity.Name
                        ) > 0
                    )
                    {
                        Conferences.Add(conf);
                    }
                }
                else
                {
                    Conferences.Add(conf);
                }
            }
            if (Conferences.Count() == 0)
            {
                Conferences.Add(new ConferenceViewModel());
            }
            return Conferences;
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Conference)]
        public ActionResult HtmlEditorPartial()
        {
            return new HtmlEditorPartialController().HtmlEditorView("HtmlEditor", "OnHtmlChangedChanged", "OnInit");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Conference)]
        [AjaxValidateAntiForgeryTokenAttribute]
        [ValidateInput(false)]
        public ActionResult SubmitConference(string viewmodel)
        {
            try
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                ConferenceViewModel vm = jss.Deserialize<ConferenceViewModel>(viewmodel);
                if (ModelState.IsValid)
                {
                    try
                    {
                        string PosterImage = "";
                        string AttachFile = "";
                        string AttachPresentationHelp = "";
                        string AttachPresentationPowerpoint = "";
                        string AttachPosterTemplateFile = "";
                        string AttachPhysicsPresentationProgramFile = "";
                        string AttachChemistryPresentationProgramFile = "";
                        string AttachGeologyPresentationProgramFile = "";
                        string AttachAmayeshFile = "";
                        string AttachScientificCommitteeFile = "";
                        string AttachExecutiveCommitteeFile = "";
                        string AttachOpeningPlanFile = "";
                        string AttachAttendingHelpFile = "";
                        string AttachTotalArticlesFile = "";
                        if (Request.Files.Count > 0)
                        {
                            var file = Request.Files[CommonHelper.ExposeProperty.GetPropertyInfo(vm, m => m.PosterImage).Name];

                            if (file != null && file.ContentLength > 0)
                            {
                                PosterImage = FileUploadManagment.UploadFile("~/assets/img/Attach/Conference/", "", file, FileUploadManagment.AppFileType.Image);
                                if (PosterImage == "")
                                {
                                    return ControllerHelper.ErrorResult("فرمت یا حجم فایل پوستر همایش صحیح نمی باشد");
                                }
                            }
                            file = Request.Files[CommonHelper.ExposeProperty.GetPropertyInfo(vm, m => m.AttachPresentationHelpFile).Name];

                            if (file != null && file.ContentLength > 0)
                            {
                                AttachPresentationHelp = FileUploadManagment.UploadFile("~/assets/img/Attach/Conference/", "", file, FileUploadManagment.AppFileType.Document, "", 5000000);
                                if (AttachPresentationHelp == "")
                                {
                                    return ControllerHelper.ErrorResult("فرمت یا حجم فایل راهنمای ارائه سخنرانی همایش صحیح نمی باشد");
                                }
                            }
                            file = Request.Files[CommonHelper.ExposeProperty.GetPropertyInfo(vm, m => m.AttachPresentationPowerpointFile).Name];

                            if (file != null && file.ContentLength > 0)
                            {
                                AttachPresentationPowerpoint = FileUploadManagment.UploadFile("~/assets/img/Attach/Conference/", "", file, FileUploadManagment.AppFileType.Document, "", 5000000);
                                if (AttachPresentationPowerpoint == "")
                                {
                                    return ControllerHelper.ErrorResult("فرمت یا حجم فایل فرمت پاورپوینت همایش صحیح نمی باشد");
                                }
                            }
                            file = Request.Files[CommonHelper.ExposeProperty.GetPropertyInfo(vm, m => m.AttachPosterTemplateFile).Name];

                            if (file != null && file.ContentLength > 0)
                            {
                                AttachPosterTemplateFile = FileUploadManagment.UploadFile("~/assets/img/Attach/Conference/", "", file, FileUploadManagment.AppFileType.Document, "", 5000000);
                                if (AttachPosterTemplateFile == "")
                                {
                                    return ControllerHelper.ErrorResult("فرمت یا حجم فایل فرمت پوستر همایش صحیح نمی باشد");
                                }
                            }

                            file = Request.Files[CommonHelper.ExposeProperty.GetPropertyInfo(vm, m => m.AttachPhysicsPresentationProgramFile).Name];

                            if (file != null && file.ContentLength > 0)
                            {
                                AttachPhysicsPresentationProgramFile = FileUploadManagment.UploadFile("~/assets/img/Attach/Conference/", "", file, FileUploadManagment.AppFileType.Document, "", 5000000);
                                if (AttachPhysicsPresentationProgramFile == "")
                                {
                                    return ControllerHelper.ErrorResult("فرمت یا حجم فایل برنامه سخنرانی همایش گروه فیزیک نمی باشد");
                                }
                            }
                            file = Request.Files[CommonHelper.ExposeProperty.GetPropertyInfo(vm, m => m.AttachGeologyPresentationProgramFile).Name];

                            if (file != null && file.ContentLength > 0)
                            {
                                AttachGeologyPresentationProgramFile = FileUploadManagment.UploadFile("~/assets/img/Attach/Conference/", "", file, FileUploadManagment.AppFileType.Document, "", 5000000);
                                if (AttachGeologyPresentationProgramFile == "")
                                {
                                    return ControllerHelper.ErrorResult("فرمت یا حجم فایل پیوست برنامه سخنرانی همایش گروه زمین شناسی صحیح نمی باشد");
                                }
                            }
                            file = Request.Files[CommonHelper.ExposeProperty.GetPropertyInfo(vm, m => m.AttachChemistryPresentationProgramFile).Name];

                            if (file != null && file.ContentLength > 0)
                            {
                                AttachChemistryPresentationProgramFile = FileUploadManagment.UploadFile("~/assets/img/Attach/Conference/", "", file, FileUploadManagment.AppFileType.Document, "", 5000000);
                                if (AttachChemistryPresentationProgramFile == "")
                                {
                                    return ControllerHelper.ErrorResult("فرمت یا حجم فایل پیوست برنامه سخنرانی همایش گروه شیمی صحیح نمی باشد");
                                }
                            }
                            file = Request.Files[CommonHelper.ExposeProperty.GetPropertyInfo(vm, m => m.AttachFile).Name];

                            if (file != null && file.ContentLength > 0)
                            {
                                AttachFile = FileUploadManagment.UploadFile("~/assets/img/Attach/Conference/", "", file, FileUploadManagment.AppFileType.Document, "", 5000000);
                                if (AttachFile == "")
                                {
                                    return ControllerHelper.ErrorResult("فرمت یا حجم فایل پیوست همایش صحیح نمی باشد");
                                }
                            }
                            file = Request.Files[CommonHelper.ExposeProperty.GetPropertyInfo(vm, m => m.AttachAmayeshFile).Name];

                            if (file != null && file.ContentLength > 0)
                            {
                                AttachAmayeshFile = FileUploadManagment.UploadFile("~/assets/img/Attach/Conference/", "", file, FileUploadManagment.AppFileType.Document, "", 10000000);
                                if (AttachAmayeshFile == "")
                                {
                                    return ControllerHelper.ErrorResult("فرمت یا حجم فایل پیوست آمایش مقاله صحیح نمی باشد");
                                }
                            }
                            file = Request.Files[CommonHelper.ExposeProperty.GetPropertyInfo(vm, m => m.AttachScientificCommitteeFile).Name];

                            if (file != null && file.ContentLength > 0)
                            {
                                AttachScientificCommitteeFile = FileUploadManagment.UploadFile("~/assets/img/Attach/Conference/", "", file, FileUploadManagment.AppFileType.Document, "", 10000000);
                                if (AttachScientificCommitteeFile == "")
                                {
                                    return ControllerHelper.ErrorResult("فرمت یا حجم فایل پیوست کمیته علمی صحیح نمی باشد");
                                }
                            }
                            file = Request.Files[CommonHelper.ExposeProperty.GetPropertyInfo(vm, m => m.AttachExecutiveCommitteeFile).Name];

                            if (file != null && file.ContentLength > 0)
                            {
                                AttachExecutiveCommitteeFile = FileUploadManagment.UploadFile("~/assets/img/Attach/Conference/", "", file, FileUploadManagment.AppFileType.Document, "", 10000000);
                                if (AttachExecutiveCommitteeFile == "")
                                {
                                    return ControllerHelper.ErrorResult("فرمت یا حجم فایل پیوست کمیته علمی صحیح نمی باشد");
                                }
                            }
                            file = Request.Files[CommonHelper.ExposeProperty.GetPropertyInfo(vm, m => m.AttachOpeningPlanFile).Name];

                            if (file != null && file.ContentLength > 0)
                            {
                                AttachOpeningPlanFile = FileUploadManagment.UploadFile("~/assets/img/Attach/Conference/", "", file, FileUploadManagment.AppFileType.Document, "", 10000000);
                                if (AttachOpeningPlanFile == "")
                                {
                                    return ControllerHelper.ErrorResult("فرمت یا حجم فایل پیوست برنامه افتتاحیه صحیح نمی باشد");
                                }
                            }
                            file = Request.Files[CommonHelper.ExposeProperty.GetPropertyInfo(vm, m => m.AttachAttendingHelpFile).Name];

                            if (file != null && file.ContentLength > 0)
                            {
                                AttachAttendingHelpFile = FileUploadManagment.UploadFile("~/assets/img/Attach/Conference/", "", file, FileUploadManagment.AppFileType.Compress, "", 10000000);
                                if (AttachAttendingHelpFile == "")
                                {
                                    return ControllerHelper.ErrorResult("فرمت یا حجم فایل راهنمای شرکت در همایش صحیح نمی باشد");
                                }
                            }
                            file = Request.Files[CommonHelper.ExposeProperty.GetPropertyInfo(vm, m => m.AttachTotalArticlesFile).Name];

                            if (file != null && file.ContentLength > 0)
                            {
                                AttachTotalArticlesFile = FileUploadManagment.UploadFile("~/assets/img/Attach/Conference/", "", file, FileUploadManagment.AppFileType.Document, "", 30000000);
                                if (AttachTotalArticlesFile == "")
                                {
                                    return ControllerHelper.ErrorResult("فرمت یا حجم فایل پیوست مقالات همایش صحیح نمی باشد");
                                }
                            }

                            
                        }
                        if (vm.ObjectState == ObjectState.Insert) // insert
                        {
                            var file = Request.Files[CommonHelper.ExposeProperty.GetPropertyInfo(vm, m => m.AttachFile).Name];
                            if (file == null && vm.ObjectState == ObjectState.Insert)
                            {
                                return ControllerHelper.ErrorResult("فایل پیوست همایش را وارد کنید");
                            }
                            var context = new ApplicationDbContext();
                            var trans = context.Database.BeginTransaction();
                            try
                            {
                                DateTime? startDate = CommonHelper.DateAndTimes.GetGregorianDate(vm.SendStartDateConverted);
                                DateTime? endDate = CommonHelper.DateAndTimes.GetGregorianDate(vm.SendEndDateConverted);
                                DateTime? eventDate = CommonHelper.DateAndTimes.GetGregorianDate(vm.EventDateConverted);
                                var conferance = new Conference()
                                {
                                    Title = vm.Title,
                                    EnglishTitle = vm.EnglishTitle,
                                    Place = vm.Place,
                                    SendStartDate = startDate != null ? (DateTime)startDate : DateTime.Now,
                                    SendEndDate = endDate != null ? (DateTime)endDate : DateTime.Now,
                                    EventDate = endDate != null ? (DateTime)eventDate : DateTime.Now,
                                    PosterImageUrl = PosterImage,
                                    AttachFileUrl = AttachFile,
                                    AttachPresentationHelpFileUrl = AttachPresentationHelp,
                                    AttachPresentationPowerpointFileUrl = AttachPresentationPowerpoint,
                                    AttachPosterTemplateFileUrl = AttachPosterTemplateFile,
                                    AttachChemistryPresentationProgramFileUrl = AttachChemistryPresentationProgramFile,
                                    AttachGeologyPresentationProgramFileUrl = AttachGeologyPresentationProgramFile,
                                    AttachPhysicsPresentationProgramFileUrl = AttachPhysicsPresentationProgramFile,
                                    AttachAmayeshFileUrl = AttachAmayeshFile,
                                    AttachScientificCommitteeFileUrl = AttachScientificCommitteeFile,
                                    AttachExecutiveCommitteeFileUrl = AttachExecutiveCommitteeFile,
                                    AttachOpeningPlanUrl = AttachOpeningPlanFile,
                                    AttachAttendingHelpUrl = AttachAttendingHelpFile,
                                    AttachTotalArticlesFileUrl= AttachTotalArticlesFile,
                                    TelegramUrl = vm.TelegramUrl,
                                    Explain=vm.Explain,
                                    //PublishPrice = viewmodel.PublishPrice,
                                    CreateDate = DateTime.Now,
                                    Enable = false,
                                    Visible = false,
                                    CreatorUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault(),
                                    MobileTel = vm.MobileTel,
                                };
                                context.Conferences.Add(conferance);
                                foreach (var pack in vm.AdminConferencePackage)
                                {
                                    context.ConferencePackages.Add(new ConferencePackage()
                                    {
                                        Conference = conferance,
                                        PackageName = context.PackageNames.Where(Pack => Pack.ID == pack.ID).SingleOrDefault(),
                                        Price = pack.Price,
                                        CreateDate = DateTime.Now,
                                        CreatorUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault(),
                                    });
                                }
                                context.SaveChanges();
                                trans.Commit();
                            }
                            catch (Exception ex)
                            {
                                trans.Rollback();
                                return ControllerHelper.ErrorResult("بروز خطای سیستمی");
                            }
                        }
                        else if (vm.ObjectState == ObjectState.Update)//Update
                        {
                            var context = new ApplicationDbContext();
                            var trans = context.Database.BeginTransaction();
                            try
                            {
                                Conference selectedModel = context.Conferences.Where(item => item.ID == vm.ID).Single();
                                if (selectedModel != null)
                                {
                                    DateTime? startDate = CommonHelper.DateAndTimes.GetGregorianDate(vm.SendStartDateConverted);
                                    DateTime? endDate = CommonHelper.DateAndTimes.GetGregorianDate(vm.SendEndDateConverted);
                                    DateTime? eventDate = CommonHelper.DateAndTimes.GetGregorianDate(vm.EventDateConverted);
                                    selectedModel.Title = vm.Title;
                                    selectedModel.EnglishTitle = vm.EnglishTitle;
                                    selectedModel.Place = vm.Place;
                                    selectedModel.SendStartDate = startDate != null ? (DateTime)startDate : DateTime.Now;
                                    selectedModel.SendEndDate = endDate != null ? (DateTime)endDate : DateTime.Now;
                                    selectedModel.EventDate = eventDate != null ? (DateTime)eventDate : DateTime.Now;
                                    if (PosterImage != "")
                                    {
                                        selectedModel.PosterImageUrl = PosterImage;
                                    }
                                    if (AttachFile != "")
                                    {
                                        selectedModel.AttachFileUrl = AttachFile;
                                    }
                                    if (AttachPresentationHelp != "")
                                    {
                                        selectedModel.AttachPresentationHelpFileUrl = AttachPresentationHelp;
                                    }
                                    if (AttachPresentationPowerpoint != "")
                                    {
                                        selectedModel.AttachPresentationPowerpointFileUrl = AttachPresentationPowerpoint;
                                    }
                                    if (AttachPosterTemplateFile != "")
                                    {
                                        selectedModel.AttachPosterTemplateFileUrl = AttachPosterTemplateFile;
                                    }
                                    if (AttachChemistryPresentationProgramFile != "")
                                    {
                                        selectedModel.AttachChemistryPresentationProgramFileUrl = AttachChemistryPresentationProgramFile;
                                    }
                                    if (AttachGeologyPresentationProgramFile != "")
                                    {
                                        selectedModel.AttachGeologyPresentationProgramFileUrl = AttachGeologyPresentationProgramFile;
                                    }
                                    if (AttachPhysicsPresentationProgramFile != "")
                                    {
                                        selectedModel.AttachPhysicsPresentationProgramFileUrl = AttachPhysicsPresentationProgramFile;
                                    }
                                    if (AttachAmayeshFile != "")
                                    {
                                        selectedModel.AttachAmayeshFileUrl = AttachAmayeshFile;
                                    }
                                    if (AttachScientificCommitteeFile != "")
                                    {
                                        selectedModel.AttachScientificCommitteeFileUrl = AttachScientificCommitteeFile;
                                    }
                                    if (AttachExecutiveCommitteeFile != "")
                                    {
                                        selectedModel.AttachExecutiveCommitteeFileUrl = AttachExecutiveCommitteeFile;
                                    }
                                    if (AttachExecutiveCommitteeFile != "")
                                    {
                                        selectedModel.AttachExecutiveCommitteeFileUrl = AttachExecutiveCommitteeFile;
                                    }
                                    if (AttachOpeningPlanFile != "")
                                    {
                                        selectedModel.AttachOpeningPlanUrl = AttachOpeningPlanFile;
                                    }
                                    if (AttachAttendingHelpFile != "")
                                    {
                                        selectedModel.AttachAttendingHelpUrl = AttachAttendingHelpFile;
                                    }
                                    if (AttachTotalArticlesFile != "")
                                    {
                                        selectedModel.AttachTotalArticlesFileUrl = AttachTotalArticlesFile;
                                    }
                                    
                                    selectedModel.TelegramUrl = vm.TelegramUrl;
                                    //selectedModel.PublishPrice = viewmodel.PublishPrice;
                                    selectedModel.LastModifyUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault();
                                    selectedModel.LastModifyDate = DateTime.Now;
                                    selectedModel.MobileTel = vm.MobileTel;
                                    selectedModel.Explain = vm.Explain;
                                }
                                context.SaveChanges();
                                foreach (var pack in vm.AdminConferencePackage)
                                {
                                    var selectedConfPack = context.ConferencePackages
                                        .Include(ConfPack => ConfPack.Conference)
                                        .Include(ConfPack => ConfPack.PackageName)
                                        .Where(ConfPack => ConfPack.PackageName.ID == pack.ID && ConfPack.Conference.ID == vm.ID).SingleOrDefault();
                                    selectedConfPack.Price = pack.Price;
                                    selectedConfPack.LastModifyUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault();
                                    selectedConfPack.LastModifyDate = DateTime.Now;
                                }
                                context.SaveChanges();
                                trans.Commit();
                            }
                            catch (Exception ex)
                            {
                                trans.Rollback();
                                return ControllerHelper.ErrorResult("بروز خطای سیستمی");
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
        //[HttpPost]
        //[AjaxValidateAntiForgeryTokenAttribute]
        //[AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Conference)]
        //public ActionResult ActiveConference(string[] ids)
        //{
        //    var context = new ApplicationDbContext();
        //    try
        //    {
        //        foreach (var id in ids)
        //        {
        //            int CurrentID = int.Parse(id);
        //            var selectedItem = context.Conferences
        //                .Where(item => item.ID == CurrentID).Single();
        //            if (selectedItem != null)
        //            {
        //                selectedItem.Enable = !selectedItem.Enable;
        //            }
        //        }
        //        context.SaveChanges();
        //        return ControllerHelper.SuccessResult("با موفقیت ثبت شد");
        //    }
        //    catch (Exception ex)
        //    {
        //        return ControllerHelper.ErrorResult("بروز خطای سیستمی");
        //    }


        //}
        [HttpPost]
        [AjaxValidateAntiForgeryTokenAttribute]
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Conference)]
        public ActionResult VisibleConference(string[] ids)
        {
            var context = new ApplicationDbContext();
            var trans = context.Database.BeginTransaction();
            try
            {
                foreach (var id in ids)
                {
                    int CurrentID = int.Parse(id);
                    var selectedItem = context.Conferences
                        .Where(item => item.ID == CurrentID).Single();
                    if (selectedItem != null)
                    {
                        selectedItem.Visible = !selectedItem.Visible;
                    }
                }
                context.SaveChanges();
                //if (context.Conferences.Count(conf => (bool)conf.Visible && (bool)conf.Enable) == 0)
                //{
                //    trans.Rollback();
                //    return ControllerHelper.ErrorResult("حداقل یک همایش باید در سیستم فعال و قابل نمایش باشد");
                //}
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
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Conference)]
        public ActionResult ActiveConference(string[] ids)
        {
            var context = new ApplicationDbContext();
            var trans = context.Database.BeginTransaction();
            try
            {
                context.Conferences.ToList().ForEach(conf => conf.Enable = false);
                foreach (var id in ids)
                {
                    int CurrentID = int.Parse(id);
                    var selectedItem = context.Conferences
                        .Where(item => item.ID == CurrentID).Single();
                    if (selectedItem != null)
                    {
                        if (selectedItem.EventDate.Date < DateTime.Now.Date && selectedItem.Enable == false)
                        {
                            trans.Rollback();
                            return ControllerHelper.ErrorResult("همایش انتخابی شما سپری شده است");
                        }
                        selectedItem.Enable = true;
                        selectedItem.Visible = true;
                    }
                }
                context.SaveChanges();
                if (context.Conferences.Count(conf => conf.Visible && conf.Enable) == 0)
                {
                    trans.Rollback();
                    return ControllerHelper.ErrorResult("حداقل یک همایش باید در سیستم فعال و قابل نمایش باشد");
                }
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
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult DeleteConference(string[] ids)
        {
            var context = new ApplicationDbContext();
            try
            {
                foreach (var id in ids)
                {
                    int ID = int.Parse(id);
                    var selectedItem = context.Conferences
                        .Where(item => item.ID == ID).Single();
                    if (selectedItem != null)
                    {
                        context.ConferencePackages.RemoveRange(context.ConferencePackages.Where(confPack => confPack.Conference.ID == ID));
                        context.Conferences.Remove(selectedItem);
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
        #endregion


    }
}