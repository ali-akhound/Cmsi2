using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Web.Security;
using AVA.UI.Helpers.Common;
using System.Web.Script.Serialization;
using AVA.UI.Helpers.FileUploadManagment;
using AVA.UI.Helpers;
using static AVA.Web.Mvc.Models.BaseViewModel;
using static AVA.UI.Helpers.Controller.ControllerHelper;
namespace AVA.Web.Mvc.Controllers
{

    public class HomeController : BaseController
    {
        #region  Home
        public ActionResult Index()
        {
            //Session["BankPrecheck"] = RequestUnpack();
            return View();
        }
        public ActionResult _Home()
        {
            string CurrentLang = Request.Cookies["Lang"].Value;
            var context = new ApplicationDbContext();
            var SelectedConf = context.Conferences
                 .Where(conf => (bool)conf.Enable && (bool)conf.Visible /*&& conf.EventDate > DateTime.Now*/)
                 .Select(conf => new { conf.EventDate, conf.SendStartDate, conf.SendEndDate, conf.PosterImageUrl, conf.Place, conf.Title, conf.EnglishTitle }).SingleOrDefault();
            //var slides = contex.Sliders.Where(slide => slide.Enable).Select(slide => new { slide.ID, slide.Context, slide.Image, slide.Priority }).OrderBy(slide => slide.Priority);
            //List<SliderViewModel> slidesVm = new List<SliderViewModel>();
            //foreach (var item in slides)
            //{
            //    slidesVm.Add(new SliderViewModel()
            //    {
            //        ID = item.ID,
            //        Context = item.Context,
            //        Image = item.Image
            //    });
            //}
            DefaultViewModel defaultViewModel = new DefaultViewModel();
            if (SelectedConf != null)
            {
                defaultViewModel.ConferenceEventDate = SelectedConf.EventDate.Date;
                defaultViewModel.ConferenceTitle = SelectedConf.Title;
                defaultViewModel.ConferencePlace = SelectedConf.Place;
                defaultViewModel.ConferenceEnglishTitle = SelectedConf.EnglishTitle;
                defaultViewModel.ConferenceEventDatePersian = CommonHelper.DateAndTimes.GetPersianDate(SelectedConf.EventDate);
                defaultViewModel.ConferenceSendStartDate = CommonHelper.DateAndTimes.GetPersianDate(SelectedConf.SendStartDate);
                defaultViewModel.ConferenceSendEndDate = CommonHelper.DateAndTimes.GetPersianDate(SelectedConf.SendEndDate);
                defaultViewModel.PosterImageUrl = SelectedConf.PosterImageUrl;
            }
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
            defaultViewModel.ConferenceCategories = new List<ConferenceCategoryViewModel>();
            foreach (var cat in Dbcats)
            {
                defaultViewModel.ConferenceCategories.Add(new ConferenceCategoryViewModel()
                {
                    ID = cat.ID,
                    Name = cat.Name,
                    Explain = cat.Explain,
                    IconName = cat.IconName
                });

            }
            var SelectedElection = context.Elections.Where(election => election.Enable).FirstOrDefault();
            if (SelectedElection != null)
            {
                if (SelectedElection.EndDate.Date >= DateTime.Now.Date || (DateTime.Now.Subtract(SelectedElection.EndDate).TotalDays < 30 && DateTime.Now.Subtract(SelectedElection.EndDate).TotalDays > 0))
                {
                    defaultViewModel.Election.ID = SelectedElection.ID;
                    defaultViewModel.Election.Name = SelectedElection.Name;
                    defaultViewModel.Election.PosterImageUrl = SelectedElection.ElectionPosterUrl;
                    defaultViewModel.Election.StartDate = SelectedElection.StartDate;
                    defaultViewModel.Election.EndDate = SelectedElection.EndDate;
                    defaultViewModel.Election.StartDateConverted = CommonHelper.DateAndTimes.GetPersianDate(SelectedElection.StartDate);
                    defaultViewModel.Election.EndDateConverted = CommonHelper.DateAndTimes.GetPersianDate(SelectedElection.EndDate);
                    if (!string.IsNullOrEmpty(SelectedElection.ElectionAttachUrl))
                        defaultViewModel.Election.AttachUrl = Url.Content(SelectedElection.ElectionAttachUrl);
                }
            }
            return PartialView("~/Views/Home/Home.cshtml", defaultViewModel);
        }
        public ActionResult _Header()
        {
            if (User.Identity.IsAuthenticated)
            {
                var context = new ApplicationDbContext();
                var selectedUser = context.Users
                     .Include(user => user.Person)
                     .Where(user => user.UserName == User.Identity.Name).SingleOrDefault();
                ViewBag.FirstName = selectedUser.Person.FirstName;
                ViewBag.LastName = selectedUser.Person.LastName;
            }
            return PartialView("~/Views/Shared/_Header.cshtml");
        }
        public ActionResult About()
        {
            string CurrentLang = Request.Cookies["Lang"].Value;
            var contex = new ApplicationDbContext();
            var pages = contex.DynamicPageContents
                .Include(page => page.DynamicPage)
                .Include(page => page.Language)
                .Where(page => page.DynamicPage.ID == 6 || page.DynamicPage.ID == 7 && page.Language.Value == CurrentLang)
                .Select(page => new { page.Context, page.ID })
                .ToList();
            return PartialView("~/Views/Home/About.cshtml", new AboutUsViewModel()
            {
                AboutUsText = pages.Where(page => page.ID == 6).SingleOrDefault().Context,
                SupportText = pages.Where(page => page.ID == 7).SingleOrDefault().Context
            });
        }
        public ActionResult DynamicPage(int id)
        {
            string CurrentLang = Request.Cookies["Lang"].Value;
            var contex = new ApplicationDbContext();
            var selectedPage = contex.DynamicPageContents
                .Include(page => page.DynamicPage)
                .Include(page => page.Language)
                .Where(page => page.DynamicPage.ID == id && page.Language.Value == CurrentLang)
                .Select(page => new
                {
                    page.Context,
                    page.ID,
                    page.Keyword,
                    page.Description,
                    page.Title
                }).SingleOrDefault();
            if (selectedPage != null)
                return PartialView("~/Views/Home/DynamicPage.cshtml", new Models.Admin.DynamicPageViewModel()
                {
                    Context = selectedPage.Context,
                    Keyword = selectedPage.Keyword,
                    Description = selectedPage.Description,
                    Title = selectedPage.Title,
                });
            else
            {
                return PartialView("~/Views/Home/DynamicPage.cshtml", new Models.Admin.DynamicPageViewModel());

            }
        }
        public ActionResult DynamicPageBlockUI(int id)
        {

            string CurrentLang = Request.Cookies["Lang"].Value;
            var contex = new ApplicationDbContext();
            var selectedPage = contex.DynamicPageContents
                .Include(page => page.DynamicPage)
                .Include(page => page.Language)
                .Where(page => page.DynamicPage.ID == id && page.Language.Value == CurrentLang)
                .Select(page => new
                {
                    page.Context,
                    page.ID,
                    page.Keyword,
                    page.Description,
                    page.Title
                }).SingleOrDefault();
            if (selectedPage != null)
                return PartialView("~/Views/Home/DynamicPageBlockUI.cshtml", new Models.Admin.DynamicPageViewModel()
                {
                    Context = selectedPage.Context,
                    Keyword = selectedPage.Keyword,
                    Description = selectedPage.Description,
                    Title = selectedPage.Title,
                });
            else
            {
                return PartialView("~/Views/Home/DynamicPageBlockUI.cshtml", new Models.Admin.DynamicPageViewModel());

            }
        }
        public ActionResult SiteMapAction()
        {
            return PartialView("~/Views/Home/SiteMap.cshtml");
        }
        public ActionResult Error()
        {
            return PartialView("~/Views/Home/Error.cshtml");
        }
        #region Functions

        string referenceId
        {
            get
            {
                if (Session["referenceId"] != null)
                    return Session["referenceId"].ToString();
                else
                    return "";
            }
            set
            {
                Session["referenceId"] = value;
            }
        }
        string paymentId
        {
            get
            {
                if (Session["paymentId"] != null)
                    return Session["paymentId"].ToString();
                else
                    return "";
            }
            set
            {
                Session["paymentId"] = value;
            }
        }
        string resultCode
        {
            get
            {
                if (Session["resultCode"] != null)
                    return Session["resultCode"].ToString();
                else
                    return "";
            }
            set
            {
                Session["resultCode"] = value;
            }
        }
        string token
        {
            get
            {
                if (Session["token"] != null)
                    return Session["token"].ToString();
                else
                    return "";
            }
            set
            {
                Session["token"] = value;
            }
        }
        string merchantId
        {
            get
            {
                if (Session["merchantId"] != null)
                    return Session["merchantId"].ToString();
                else
                    return "";
            }
            set
            {
                Session["merchantId"] = value;
            }
        }
        string amount
        {
            get
            {
                if (Session["amount"] != null)
                    return Session["amount"].ToString();
                else
                    return "";
            }
            set
            {
                Session["amount"] = value;
            }
        }
        string StatusMsg
        {
            get
            {
                if (Session["StatusMsg"] != null)
                    return Session["StatusMsg"].ToString();
                else
                    return "";
            }
            set
            {
                Session["StatusMsg"] = value;
            }
        }
        private bool RequestUnpack()
        {
            if (RequestFeildIsEmpty()) return false;

            referenceId = Request.Form["referenceId"].ToString();
            paymentId = Request.Form["paymentId"].ToString();
            resultCode = Request.Form["resultCode"].ToString();
            token = Request.Form["token"].ToString();
            merchantId = Request.Form["merchantId"].ToString();
            amount = Request.Form["amount"].ToString();
            return true;
        }
        private bool RequestFeildIsEmpty()
        {
            if (Request.Form["resultCode"] != null && Request.Form["referenceId"] != null && Request.Form["resultCode"] != null)
            {
                if (Request.Form["resultCode"].ToString().Equals(string.Empty))
                {
                    StatusMsg = "خريد شما توسط بانک تاييد شده است اما رسيد ديجيتالي شما تاييد نگشت! مشکلي در فرايند رزرو خريد شما پيش آمده است";
                    return true;
                }

                if (Request.Form["referenceId"].ToString().Equals(string.Empty) && Request.Form["resultCode"].ToString().Equals(string.Empty))
                {
                    StatusMsg = "فرايند انتقال وجه با موفقيت انجام شده است اما فرايند تاييد رسيد ديجيتالي با خطا مواجه گشت";
                    return true;
                }

                if (Request.Form["paymentId"].ToString().Equals(string.Empty) && Request.Form["resultCode"].ToString().Equals(string.Empty))
                {
                    StatusMsg = "خطا در برقرار ارتباط با بانک";
                    return true;
                }
                return false;
            }
            else
                return true;
        }
        private string TransactionChecking(long i)
        {
            switch (i)
            {
                case 100:
                    return "تراکنش با موفقیت انجام شد";
                case 110:
                    return "انصراف دارنده کارت";
                case 120:
                    return "موجودی حساب کافی نیست";
                case 130:
                    return "اطلاعات کارت اشتباه است";
                case 131:
                    return "رمز کارت اشتباه است";
                case 132:
                    return "کارت مسدود شده است";
                case 133:
                    return "کارت منقضی شده است";
                case 140:
                    return "زمان مورد نظر به پایان رسیده است";
                case 150:
                    return "خطای داخلی بانک";
                case 160:
                    return "خطا در اطلاعات CVV2 یا تاریخ انقضا";
                case 166:
                    return "بانک صادر کننده کارت شما مجوز انجان تراکنش را صادر نکرده است";
                case 200:
                    return "مبلغ تراکنش بیشتر از سقف مجاز برای هر تراکنش می باشد";
                case 201:
                    return "مبلغ تراکنش بیشتر از سقف مجاز در روز می باشد";
                case 202:
                    return "مبلغ تراکنش بیشتر از سقف مجاز در ماه می باشد";
                case -20:
                    return "وجود کاراکتر های غیر مجاز در درخواست";
                case -30:
                    return "ترتکنش قبلا برگشت خورده است";
                case -50:
                    return "طول رشته درخواست غیر مجاز است";
                case -51:
                    return "خطا در درخواست";
                case -80:
                    return "تراکنش مورد نظر یافت نشد";
                case -81:
                    return "خطای داخلی بانک";
                case -90:
                    return "تراکنش قبلا تایید شده است";
                default:
                    return "";
            }
        }
        #endregion
        public ActionResult ResultBank(int id, string tbl)
        {
            var context = new ApplicationDbContext();
            var Amount = context.OrderItems
                        .Include(orderItem => orderItem.ConferencePackage)
                        .Where(orderItem => orderItem.Order.ID == id)
                        .Select(orderItem => orderItem.Count * orderItem.ConferencePackage.Price)
                        .DefaultIfEmpty(0)
                        .Sum();
            string StatusMsg = "";
            if ((bool)Session["BankPrecheck"])
            {
                try
                {
                    long vertificationResultCode;
                    bool vertificationResultSpecified;
                    if (!string.IsNullOrEmpty(resultCode) && resultCode == "100")
                    {

                        ir.shaparak.ikc.Verify.Verify VerifyService = new ir.shaparak.ikc.Verify.Verify();
                        VerifyService.KicccPaymentsVerification(token, merchantId, Session["referenceId"].ToString(), "22338240992352910814917221751200141041845518824222260", out vertificationResultCode, out vertificationResultSpecified);
                        if (vertificationResultCode > 0)
                        {
                            bool CheckPrice = false;
                            if (int.Parse(Session["amount"].ToString()) == (int)Amount)
                                CheckPrice = true;

                            if (CheckPrice)
                            {
                                StatusMsg = "تراکنش با موفقیت انجام شد";
                                var OrderObj = context.Orders
                                 .Include(order => order.Conference)
                                 .Include(order => order.OrderItems)
                                 .Include(order => order.OrderStatus)
                                 .Include(order => order.CreatorUser)
                                 .Where(order => order.ID == id).SingleOrDefault();
                                var invoiceObj = context.Invoices.Where(
                                  invoice =>
                                  invoice.TableName == "Order"
                                  && invoice.DocumentID == id
                                  && invoice.TableName == tbl
                                  ).SingleOrDefault();
                                var trans = context.Database.BeginTransaction();
                                if (invoiceObj != null)
                                {
                                    if ((long)invoiceObj.Amount == vertificationResultCode)
                                    {
                                        try
                                        {
                                            invoiceObj.DigitalCode = referenceId;
                                            invoiceObj.LastModifyDate = DateTime.Now;
                                            OrderObj.OrderStatus = context.OrderStatuses.Where(status => status.ID == (int)Enums.OrderStatus.Confirmed).SingleOrDefault();
                                            context.SaveChanges();
                                            trans.Commit();
                                        }
                                        catch (Exception)
                                        {
                                            trans.Rollback();
                                            StatusMsg = "بروز خطای سیستمی";
                                        }
                                    }
                                }
                            }
                            else
                            {
                                StatusMsg = "مقدار پرداخت شده با مبلغ صورت وضعیت برابر نیست لطفا پرداخت را مجددا انجام دهید" + vertificationResultCode + "-" + ((int)Amount).ToString();
                            }

                        }
                        else
                        {
                            StatusMsg = TransactionChecking(vertificationResultCode);
                        }
                    }
                    else
                    {
                        //StatusLbl.Text = "تراکنش از طرف بانک تایید نگردید لطفا مجددا پرداخت را انجام دهید";
                        StatusMsg = TransactionChecking(Convert.ToInt32(resultCode));
                    }
                }
                catch { }
            }
            return PartialView("~/Views/Home/ResultBank.cshtml", new InvoiceOnlineResultViewModel()
            {
                paymentId = paymentId,
                referenceId = referenceId,
                StatusMsg = StatusMsg

            });
        }
        #endregion
        #region ContactUs
        public ActionResult ContactUs()
        {
            return PartialView("~/Views/Home/ContactUs.cshtml", new Models.Admin.ContactUsViewModel() { ObjectState = ObjectState.Insert });
        }
        [HttpPost]
        [AjaxValidateAntiForgeryTokenAttribute]
        public ActionResult SubmitContactUs(Models.Admin.ContactUsViewModel viewmodel)
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
                            context.ContactUses.Add(new ContactUs()
                            {
                                Name = viewmodel.Name,
                                Family = viewmodel.Family,
                                Email = viewmodel.Email,
                                Subject = viewmodel.Subject,
                                Text = viewmodel.Text,
                                TelNumber = viewmodel.TelNumber,
                                CreateDate = DateTime.Now
                            });
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
        #region NewsFeed
        [HttpPost]
        [AjaxValidateAntiForgeryTokenAttribute]
        public ActionResult SubmitNewsFeed(NewsFeedViewModel viewmodel)
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
                            if (context.EmailNotifyTables.Where(notify => notify.Email == viewmodel.NewsFeedEmail).Count() > 0)
                            {
                                return ControllerHelper.ErrorResult("ایمیل قبلا ثبت شده است");
                            }
                            context.EmailNotifyTables.Add(new EmailNotifyTable()
                            {
                                Email = viewmodel.NewsFeedEmail,
                                CreateDate = DateTime.Now
                            });
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
        #region Login/Register
        #region Register
        public ActionResult Signup()
        {
            var context = new ApplicationDbContext();
            var DBProvinces = context.Provinces.Select(item => new { item.Name, item.ID }).ToList();
            var defaultProvinceID = DBProvinces[0].ID;
            var DBCities = context.Cities.Where(item => item.Province.ID == defaultProvinceID).Select(item => new { item.Name, item.ID }).ToList();
            List<DropDownVm> Provinces = new List<DropDownVm>();
            foreach (var province in DBProvinces)
            {
                Provinces.Add(new DropDownVm() { Text = province.Name, Value = province.ID.ToString() });
            }
            List<DropDownVm> Cities = new List<DropDownVm>();
            foreach (var city in DBCities)
            {
                Cities.Add(new DropDownVm() { Text = city.Name, Value = city.ID.ToString() });
            }
            return PartialView("~/Views/Home/Signup.cshtml", new RegisterMembershipVm()
            {
                CityFeeds = Cities,
                ProvinceFeeds = Provinces,
                selectedProvinceID = defaultProvinceID.ToString(),
                selectedCityID = DBCities[0].ID.ToString(),
                Degree = "فوق دیپلم",
                sex = true,
                ObjectState = ObjectState.Insert
            });
        }
        public ActionResult SubmitUser(string viewModel)
        {
            try
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                RegisterMembershipVm vm = jss.Deserialize<RegisterMembershipVm>(viewModel);
                if (ModelState.IsValid)
                {
                    try
                    {
                        if (vm.ObjectState == ObjectState.Insert) // insert
                        {

                            var context = new ApplicationDbContext();
                            var user = new ApplicationUser();
                            if (context.Users.Where(item => item.Email == vm.Email && item.EmailConfirmed).Count() > 0)
                            {
                                return ControllerHelper.ErrorResult("ایمیل قبلا ثبت شده است");
                            }
                            else if (context.Users.Where(item => item.Email == vm.Email && !item.EmailConfirmed).Count() > 0)
                            {
                                user = context.Users.Where(item => item.Email == vm.Email).SingleOrDefault();
                                var code = UserManager.GenerateEmailConfirmationToken(user.Id);
                                var callbackUrl = Url.Action(
                                 "Index", "Home",
                                 new { },
                                 protocol: Request.Url.Scheme) + "/UI/ConfirmEmail?userId=" + user.Id + "&code=" + code;

                                EmailService.EmailResponse MailResult = EmailService.SendWelcome(user.Id, callbackUrl);
                                if (MailResult.Code == (int)EmailService.EmailEnum.Success)
                                {
                                    return ControllerHelper.SuccessResult("ثبت نام با موفقیت انجام شد. لطفا برای فعال سازی اکانت به ایمیل فعال سازی که برای شما ارسال شده است مراجعه فرمایید.در صورت نبودن ایمیل اسپم لیست خود را بررسی بفرمایید");
                                }
                                else
                                    return ControllerHelper.ErrorResult("بروز خطای سیستمی در ارسال ایمیل فعال سازی");
                            }
                            else
                            {
                                string ProfilePic = "";
                                string MeliCardUrl = "";
                                string UniversityCardUrl = "";
                                if (Request.Files.Count > 0)
                                {
                                    var file = Request.Files[CommonHelper.ExposeProperty.GetPropertyInfo(vm, m => m.PersonalFileImage).Name];

                                    if (file != null && file.ContentLength > 0)
                                    {
                                        ProfilePic = FileUploadManagment.UploadFile("~/assets/img/Attach/UserPic/", "", file, FileUploadManagment.AppFileType.Image);
                                        if (ProfilePic == "")
                                        {
                                            return ControllerHelper.ErrorResult("فرمت یا حجم فایل صحیح نمی باشد");
                                        }
                                    }
                                    file = Request.Files[CommonHelper.ExposeProperty.GetPropertyInfo(vm, m => m.MeliCardFileImage).Name];

                                    if (file != null && file.ContentLength > 0)
                                    {
                                        MeliCardUrl = FileUploadManagment.UploadFile("~/assets/img/Attach/UserPic/", "", file, FileUploadManagment.AppFileType.Image);
                                        if (MeliCardUrl == "")
                                        {
                                            return ControllerHelper.ErrorResult("فرمت یا حجم فایل صحیح نمی باشد");
                                        }
                                    }
                                    file = Request.Files[CommonHelper.ExposeProperty.GetPropertyInfo(vm, m => m.UniversityCardFileImage).Name];

                                    if (file != null && file.ContentLength > 0)
                                    {
                                        UniversityCardUrl = FileUploadManagment.UploadFile("~/assets/img/Attach/UserPic/", "", file, FileUploadManagment.AppFileType.Image);
                                        if (UniversityCardUrl == "")
                                        {
                                            return ControllerHelper.ErrorResult("فرمت یا حجم فایل صحیح نمی باشد");
                                        }
                                    }
                                }
                                var trans = context.Database.BeginTransaction();
                                try
                                {
                                    //ApplicationUser selectedModel = context.Users
                                    //    .Include(item => item.Person)
                                    //    .Where(item => item.UserName == User.Identity.Name).Single();
                                    //if (selectedModel != null)
                                    //{
                                    //    selectedModel.Person.FirstName = viewmodel.FirstName;
                                    //    selectedModel.Person.LastName = viewmodel.LastName;
                                    //    selectedModel.Person.Cellphone = viewmodel.PhoneNumber;
                                    //    selectedModel.PhoneNumber = viewmodel.PhoneNumber;
                                    //    selectedModel.Email = viewmodel.Email;
                                    //    selectedModel.Person.BornDate = CommonHelper.DateAndTimes.GetGregorianDate(viewmodel.BornDate);
                                    //    selectedModel.Person.Degree = viewmodel.Degree;
                                    //    selectedModel.Person.Melicode = viewmodel.Melicode;
                                    //    selectedModel.Person.University = viewmodel.University;
                                    //    selectedModel.Person.FieldOfStudy = viewmodel.FieldOfStudy;
                                    //    selectedModel.Person.sex = viewmodel.sex;
                                    //}
                                    var SelectedProvinceID = int.Parse(vm.selectedProvinceID);
                                    var SelectedCityID = int.Parse(vm.selectedCityID);

                                    Person person = context.Persons.Add(new Person()
                                    {
                                        FirstName = vm.FirstName,
                                        LastName = vm.LastName,
                                        EnglishFirstName = vm.EnglishFirstName,
                                        EnglishLastName = vm.EnglishLastName,
                                        Email = vm.Email,
                                        Cellphone = vm.PhoneNumber,
                                        BornDate = CommonHelper.DateAndTimes.GetGregorianDate(vm.BornDate),
                                        Degree = vm.Degree,
                                        Melicode = vm.Melicode,
                                        University = vm.University,
                                        FieldOfStudy = vm.FieldOfStudy,
                                        sex = vm.sex,
                                        Address = vm.Address,
                                        Province = context.Provinces.Where(p => p.ID == SelectedProvinceID).SingleOrDefault(),
                                        City = context.Cities.Where(c => c.ID == SelectedCityID).SingleOrDefault(),
                                        CreateDate = DateTime.Now,
                                        CreatorUser = context.Users.Where(item => item.UserName == User.Identity.Name).SingleOrDefault()
                                    });
                                    if (ProfilePic != "")
                                    {
                                        person.PersonalImageUrl = ProfilePic;
                                    }
                                    if (MeliCardUrl != "")
                                    {
                                        person.MeliCardUrl = MeliCardUrl;
                                    }
                                    if (UniversityCardUrl != "")
                                    {
                                        person.UniversityCardUrl = UniversityCardUrl;
                                    }
                                    context.SaveChanges();
                                    user = new ApplicationUser
                                    {
                                        Email = vm.Email,
                                        PhoneNumber = vm.PhoneNumber,
                                        UserName = vm.Email,
                                        CreateDate = DateTime.Now,
                                        PasswordHash = new ApplicationUserManager(new UserStore<ApplicationUser>()).PasswordHasher.HashPassword(vm.Password),
                                        SecurityStamp = Guid.NewGuid().ToString("D"),
                                        Person = person,
                                        Enable = true,
                                        EmailConfirmed = false,

                                        //CreatorUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault(),
                                    };
                                    context.Users.Add(user);
                                    context.SaveChanges();
                                    trans.Commit();
                                }
                                catch (Exception ex)
                                {
                                    trans.Rollback();
                                    return ControllerHelper.ErrorResult("بروز خطای سیستمی");
                                }
                                var code = UserManager.GenerateEmailConfirmationToken(user.Id);
                                var callbackUrl = Url.Action(
                                 "Index", "Home",
                                 new { },
                                 protocol: Request.Url.Scheme) + "/UI/ConfirmEmail?userId=" + user.Id + "&code=" + code;

                                EmailService.EmailResponse MailResult = EmailService.SendWelcome(user.Id, callbackUrl);
                                if (MailResult.Code == (int)EmailService.EmailEnum.Success)
                                {
                                    return ControllerHelper.SuccessResult("ثبت نام با موفقیت انجام شد. لطفا برای فعال سازی اکانت به ایمیل فعال سازی که برای شما ارسال شده است مراجعه فرمایید.در صورت نبودن ایمیل اسپم لیست خود را بررسی بفرمایید");
                                }
                                else
                                    return ControllerHelper.ErrorResult("بروز خطای سیستمی در ارسال ایمیل فعال سازی");
                            }
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
                return ControllerHelper.ErrorResult("بروز خطای سیستمی");
            }
        }
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return PartialView("~/Views/Home/Error.cshtml");
            }
            code = code.Replace(" ", "+");
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return PartialView(result.Succeeded ? "~/Views/Home/ConfirmEmail.cshtml" : "~/Views/Home/Error.cshtml");
        }


        #endregion
        #region Login
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return PartialView("~/Views/Home/UserProfile/Index.cshtml", BindHomeProfile());
            }
            return PartialView("~/Views/Home/Login.cshtml", new LoginMembershipViewModel()
            {
            });
        }
        [HttpPost]
        [AjaxValidateAntiForgeryTokenAttribute]
        public ActionResult SignIn(LoginMembershipViewModel viewmodel)
        {
            if (User.Identity.IsAuthenticated)
            {
                return PartialView("~/Views/Home/UserProfile/Profile.cshtml");
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return PartialView("~/Views/Home/UserProfile/Profile.cshtml", viewmodel);
                }

                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, change to shouldLockout: true                
                var result = SignInManager.PasswordSignIn(viewmodel.Username, viewmodel.Password, viewmodel.RememberMe, shouldLockout: false);
                switch (result)
                {
                    case SignInStatus.Success:
                        {
                            var contex = new ApplicationDbContext();
                            var CurrentUser = contex.Users.Include(user => user.Person).Where(user => user.UserName == viewmodel.Username).SingleOrDefault();
                            foreach (var role in CurrentUser.Roles)
                            {
                                string RoleName = contex.Roles.Where(mrole => mrole.Id == role.RoleId).SingleOrDefault().Name;
                                UserManager.AddToRole(CurrentUser.Id, RoleName);
                            }
                            return ControllerHelper.SuccessResult(new { Name = CurrentUser.Person.FirstName, Family = CurrentUser.Person.LastName }, "1");
                        }
                    case SignInStatus.LockedOut:
                        return PartialView("Lockout");
                    case SignInStatus.RequiresVerification:
                        return RedirectToAction("SendCode", new { ReturnUrl = "", RememberMe = viewmodel.RememberMe });
                    case SignInStatus.Failure:
                    default:
                        ModelState.AddModelError("", "نام کاربری یا رمز عبور اشتباه است");
                        return ControllerHelper.ModelStateInvalidResult(ModelState);
                }


            }
        }
        [HttpPost]
        public ActionResult SignOut()
        {
            base.AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            FormsAuthentication.SignOut();
            Session.Abandon();
            return ControllerHelper.SuccessResult("1");
        }
        #endregion
        #region ForgetPass
        public ActionResult PasswordRecovery()
        {
            return PartialView("~/Views/Home/PasswordRecovery.cshtml", new PasswordRecoveryMembershipViewModel());
        }
        public ActionResult ForgetPass(PasswordRecoveryMembershipViewModel viewmodel)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var context = new ApplicationDbContext();
                        var user = new ApplicationUser();
                        if (context.Users.Where(item => item.Email == viewmodel.Email).Count() > 0)
                        {
                            user = context.Users.Where(item => item.Email == viewmodel.Email).Single();
                            var code = UserManager.GeneratePasswordResetToken(user.Id);
                            //var result = UserManager.ConfirmEmail(user.Id, code);
                            //code = System.Web.HttpUtility.UrlEncode(code);
                            var callbackUrl = Url.Action(
                             "Index", "Home",
                             new { },
                             protocol: Request.Url.Scheme) + "/UI/ResetPassword?id=" + user.Id + "&code=" + code;

                            EmailService.EmailResponse MailResult = EmailService.SendForgetPassMail(user.Id, callbackUrl);
                            if (MailResult.Code == (int)EmailService.EmailEnum.Success)
                            {
                                return ControllerHelper.SuccessResult("لطفا برای ریست پسورد به ایمیلی که برای شما ارسال شده است مراجعه فرمایید.در صورت نبودن ایمیل اسپم لیست خود را بررسی بفرمایید");
                            }
                            else
                                return ControllerHelper.ErrorResult("بروز خطای سیستمی در ارسال ایمیل فعال سازی");
                        }
                        else
                        {
                            return ControllerHelper.ErrorResult("ایمیل وارد شده در سیستم ثبت نشده است");
                        }
                    }
                    catch (Exception ex)
                    {
                        return ControllerHelper.ErrorResult("بروز خطای سیستمی");
                    }
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
        [AllowAnonymous]
        public ActionResult ResetPassword(string id, string code)
        {
            try
            {
                // Verifies the parameters of the password reset request
                // True if the token is valid for the specified user, false if the token is invalid or has expired
                // By default, the generated tokens are single-use and expire in 1 day
                code = code.Replace(" ", "+");
                if (UserManager.VerifyUserToken(id, "ResetPassword", code))
                {
                    // If the password request is valid, displays the password reset form
                    var model = new ResetPasswordViewModel
                    {
                        UserId = id,
                        Code = code
                    };

                    return PartialView("~/Views/Home/ResetPassword.cshtml", model);
                }

                // If the password request is invalid, returns a view informing the user
                return PartialView("~/Views/Home/Error.cshtml");
            }
            catch (InvalidOperationException)
            {
                // An InvalidOperationException occurs if a user with the given ID is not found
                // Returns a view informing the user that the password reset request is not valid
                return PartialView("~/Views/Home/Error.cshtml");
            }
            // return code == null ? PartialView("~/Views/Home/Error.cshtml") : PartialView("~/Views/Home/ResetPassword.cshtml",new ResetPasswordViewModel() {Code=code });
        }
        [HttpPost]
        [AllowAnonymous]
        [AjaxValidateAntiForgeryTokenAttribute]
        public ActionResult ResetPasswordAction(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("~/Views/Home/ResetPassword.cshtml", model);
            }
            var context = new ApplicationDbContext();
            ApplicationUser user = context.Users.Where(item => item.Email == model.Email).Single();
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return PartialView("~/Views/Home/Login.cshtml");
            }
            else
            {
                user.PasswordHash = new ApplicationUserManager(new UserStore<ApplicationUser>()).PasswordHasher.HashPassword(model.Password);
            }
            try
            {
                context.SaveChanges();
                return ControllerHelper.SuccessResult("رمز عبور با موفقیت تغییر یافت");
            }
            catch (Exception)
            {
                return ControllerHelper.ErrorResult("بروز خطای سیستمی");
            }

        }
        #endregion
        #endregion
        #region User Profile
        #region Profile
        EditMembershipVm ProfileBinding()
        {
            var context = new ApplicationDbContext();
            ApplicationUser selectedUser =
                context.Users
                .Include(user => user.Person)
                .Include(user => user.Person.City)
                .Include(user => user.Person.Province)
                .Where(user => user.UserName == User.Identity.Name).SingleOrDefault();

            var DBProvinces = context.Provinces.Select(item => new { item.Name, item.ID }).ToList();
            var defaultProvinceID = selectedUser.Person.Province == null ? DBProvinces[0].ID : selectedUser.Person.Province.ID;
            var DBCities = context.Cities.Where(item => item.Province.ID == defaultProvinceID).Select(item => new { item.Name, item.ID }).ToList();
            List<DropDownVm> Provinces = new List<DropDownVm>();
            foreach (var province in DBProvinces)
            {
                Provinces.Add(new DropDownVm() { Text = province.Name, Value = province.ID.ToString() });
            }
            List<DropDownVm> Cities = new List<DropDownVm>();
            foreach (var city in DBCities)
            {
                Cities.Add(new DropDownVm() { Text = city.Name, Value = city.ID.ToString() });
            }
            return new EditMembershipVm()
            {
                ID = selectedUser.Id,
                FirstName = selectedUser.Person.FirstName,
                LastName = selectedUser.Person.LastName,
                EnglishFirstName = selectedUser.Person.EnglishFirstName == null ? "" : selectedUser.Person.EnglishFirstName,
                EnglishLastName = selectedUser.Person.EnglishLastName == null ? "" : selectedUser.Person.EnglishLastName,
                Email = selectedUser.Email,
                PhoneNumber = selectedUser.Person.Cellphone,
                sex = selectedUser.Person.sex,
                BornDate = CommonHelper.DateAndTimes.GetPersianDate(selectedUser.Person.BornDate),
                Degree = selectedUser.Person.Degree,
                Melicode = selectedUser.Person.Melicode,
                University = selectedUser.Person.University,
                FieldOfStudy = selectedUser.Person.FieldOfStudy,
                CityFeeds = Cities,
                ProvinceFeeds = Provinces,
                selectedCityID = selectedUser.Person.City == null ? Cities[0].Value : selectedUser.Person.City.ID.ToString(),
                selectedProvinceID = selectedUser.Person.Province == null ? Provinces[0].Value : selectedUser.Person.Province.ID.ToString(),
                Address = selectedUser.Person.Address,
                MeliCardUrl = Url.Content(string.IsNullOrEmpty(selectedUser.Person.MeliCardUrl) ? MvcApplication.DefaultImageUrl : selectedUser.Person.MeliCardUrl),
                PersonalImageUrl = Url.Content(string.IsNullOrEmpty(selectedUser.Person.PersonalImageUrl) ? MvcApplication.DefaultImageUrl : selectedUser.Person.PersonalImageUrl),
                UniversityCardUrl = Url.Content(string.IsNullOrEmpty(selectedUser.Person.UniversityCardUrl) ? MvcApplication.DefaultImageUrl : selectedUser.Person.UniversityCardUrl),

                ObjectState = ObjectState.Update
            };
        }
        ProfileViewModel BindHomeProfile()
        {
            var context = new ApplicationDbContext();
            var selectedUser = context.Users
                 .Include(user => user.Person)
                 .Where(user => user.UserName == User.Identity.Name).SingleOrDefault();
            int TotalArticles = context.ConferenceArticles
              .Include(conf => conf.Article)
              .Where(conf =>
                  conf.Article.CreatorUser.UserName == User.Identity.Name
              )
             .Count();
            int AcceptedArticles = context.ConferenceArticles
             .Include(conf => conf.Article)
             .Where(conf =>
                 conf.Article.CreatorUser.UserName == User.Identity.Name
                 &&
                 conf.Article.ArticleStatus.ID == (int)Enums.ArticleStatus.Confirmed
             )
            .Count();
            int RejectedArticles = context.ConferenceArticles
             .Include(conf => conf.Article)
             .Where(conf =>
                 conf.Article.CreatorUser.UserName == User.Identity.Name
                 &&
                 conf.Article.ArticleStatus.ID == (int)Enums.ArticleStatus.RefereeNotConfirmed
             )
            .Count();
            return new ProfileViewModel()
            {
                Name = selectedUser.Person.FirstName,
                Family = selectedUser.Person.LastName,
                ImageUrl = string.IsNullOrEmpty(selectedUser.Person.PersonalImageUrl) ? "~/assets/img/UserImage/Add_user_icon.png" : selectedUser.Person.PersonalImageUrl,
                VipEndDate = CommonHelper.DateAndTimes.GetPersianDate(selectedUser.VipEndDate),
                TotalArticles = TotalArticles,
                AcceptedArticles = AcceptedArticles,
                RejectedArticles = RejectedArticles
            };
        }
        ResponseResult submitEditUser(EditMembershipVm vm)
        {

            try
            {
                var context = new ApplicationDbContext();
                ApplicationUser selectedModel = context.Users
                    .Include(item => item.Person)
                    .Where(item => item.UserName == User.Identity.Name).Single();
                string ProfilePic = "";
                string MeliCardUrl = "";
                string UniversityCardUrl = "";
                if (Request.Files.Count > 0)
                {
                    var file = Request.Files[CommonHelper.ExposeProperty.GetPropertyInfo(vm, m => m.PersonalFileImage).Name];

                    if (file != null && file.ContentLength > 0)
                    {
                        ProfilePic = FileUploadManagment.UploadFile("~/assets/img/Attach/UserPic/", "", file, FileUploadManagment.AppFileType.Image);
                        if (ProfilePic == "")
                        {
                            return new ResponseResult() { IsSuccess = false, ResultMessage = "فرمت یا حجم فایل عکس پرسنلی صحیح نمی باشد" };
                        }
                    }
                    if ((file == null || file.ContentLength == 0) && (string.IsNullOrEmpty(selectedModel.Person.PersonalImageUrl) || selectedModel.Person.PersonalImageUrl == MvcApplication.DefaultImageUrl))
                    {
                        return new ResponseResult() { IsSuccess = false, ResultMessage = "عکس پرسنلی ضروری است" };
                    }
                    file = Request.Files[CommonHelper.ExposeProperty.GetPropertyInfo(vm, m => m.MeliCardFileImage).Name];

                    if (file != null && file.ContentLength > 0)
                    {
                        MeliCardUrl = FileUploadManagment.UploadFile("~/assets/img/Attach/UserPic/", "", file, FileUploadManagment.AppFileType.Image);
                        if (MeliCardUrl == "")
                        {
                            return new ResponseResult() { IsSuccess = false, ResultMessage = "فرمت یا حجم فایل کارت ملی صحیح نمی باشد" };
                        }
                    }
                    if ((file == null || file.ContentLength == 0) && (string.IsNullOrEmpty(selectedModel.Person.MeliCardUrl) || selectedModel.Person.MeliCardUrl == MvcApplication.DefaultImageUrl))
                    {
                        return new ResponseResult() { IsSuccess = false, ResultMessage = "کارت ملی ضروری است" };
                    }
                    //file = Request.Files[CommonHelper.ExposeProperty.GetPropertyInfo(vm, m => m.UniversityCardFileImage).Name];

                    //if (file != null && file.ContentLength > 0)
                    //{
                    //    UniversityCardUrl = FileUploadManagment.UploadFile("~/assets/img/Attach/UserPic/", "", file, FileUploadManagment.AppFileType.Image);
                    //    if (UniversityCardUrl == "")
                    //    {
                    //        return new ResponseResult() { IsSuccess = false, ResultMessage = "فرمت یا حجم فایل صحیح نمی باشد" };
                    //    }
                    //}
                }
                else
                {
                    if ((string.IsNullOrEmpty(selectedModel.Person.PersonalImageUrl) || selectedModel.Person.PersonalImageUrl == MvcApplication.DefaultImageUrl))
                    {
                        return new ResponseResult() { IsSuccess = false, ResultMessage = "عکس پرسنلی ضروری است" };
                    }
                    if ((string.IsNullOrEmpty(selectedModel.Person.MeliCardUrl) || selectedModel.Person.MeliCardUrl == MvcApplication.DefaultImageUrl))
                    {
                        return new ResponseResult() { IsSuccess = false, ResultMessage = "کارت ملی ضروری است" };
                    }

                }

                if (vm.ObjectState == ObjectState.Update)//Update
                {
                    var SelectedProvinceID = int.Parse(vm.selectedProvinceID);
                    var SelectedCityID = int.Parse(vm.selectedCityID);
                    selectedModel.Id = vm.ID;
                    selectedModel.Person.FirstName = vm.FirstName;
                    selectedModel.Person.LastName = vm.LastName;
                    selectedModel.Person.EnglishFirstName = vm.EnglishFirstName;
                    selectedModel.Person.EnglishLastName = vm.EnglishLastName;
                    selectedModel.Person.Cellphone = vm.PhoneNumber;
                    selectedModel.PhoneNumber = vm.PhoneNumber;
                    selectedModel.Email = vm.Email;
                    selectedModel.Person.BornDate = CommonHelper.DateAndTimes.GetGregorianDate(vm.BornDate);
                    selectedModel.Person.Degree = vm.Degree;
                    selectedModel.Person.Melicode = vm.Melicode;
                    selectedModel.Person.University = vm.University;
                    selectedModel.Person.FieldOfStudy = vm.FieldOfStudy;
                    selectedModel.Person.sex = vm.sex;
                    selectedModel.Person.Address = vm.Address;
                    selectedModel.Person.Province = context.Provinces.Where(p => p.ID == SelectedProvinceID).SingleOrDefault();
                    selectedModel.Person.City = context.Cities.Where(c => c.ID == SelectedCityID).SingleOrDefault();
                    if (selectedModel.Person.PersonalImageUrl != ProfilePic && ProfilePic != "")
                    {
                        try
                        {
                            if (selectedModel.Person.PersonalImageUrl != MvcApplication.DefaultImageUrl)
                                System.IO.File.Delete(Server.MapPath(selectedModel.Person.PersonalImageUrl));
                        }
                        catch (Exception)
                        {

                        }
                    }
                    if (selectedModel.Person.MeliCardUrl != MeliCardUrl && MeliCardUrl != "")
                    {
                        try
                        {
                            if (selectedModel.Person.MeliCardUrl != MvcApplication.DefaultImageUrl)
                                System.IO.File.Delete(Server.MapPath(selectedModel.Person.MeliCardUrl));
                        }
                        catch (Exception)
                        {

                        }
                    }
                    if (selectedModel.Person.UniversityCardUrl != UniversityCardUrl && UniversityCardUrl != "")
                    {
                        try
                        {
                            if (selectedModel.Person.UniversityCardUrl != MvcApplication.DefaultImageUrl)
                                System.IO.File.Delete(Server.MapPath(selectedModel.Person.UniversityCardUrl));
                        }
                        catch (Exception)
                        {

                        }
                    }
                    if (ProfilePic != "")
                        selectedModel.Person.PersonalImageUrl = ProfilePic;
                    if (MeliCardUrl != "")
                        selectedModel.Person.MeliCardUrl = MeliCardUrl;
                    //selectedModel.Person.UniversityCardUrl = UniversityCardUrl == "" ? MvcApplication.DefaultImageUrl : UniversityCardUrl;
                    selectedModel.Person.LastModifyUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault();
                    selectedModel.Person.LastModifyDate = DateTime.Now;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                return new ResponseResult() { IsSuccess = false, ResultMessage = "بروز خطای سیستمی" };
            }
            return new ResponseResult() { IsSuccess = true, ResultMessage = "با موفقیت ثبت شد" };

        }
        [UIAuthorizeUser]
        public ActionResult ProfileIndex()
        {
            var context = new ApplicationDbContext();
            var selectedUser = context.Users
                 .Include(user => user.Person)
                 .Where(user => user.UserName == User.Identity.Name).SingleOrDefault();
            int TotalArticles = context.ConferenceArticles
              .Include(conf => conf.Article)
              .Where(conf =>
                  conf.Article.CreatorUser.UserName == User.Identity.Name
              )
             .Count();
            return PartialView("~/Views/Home/UserProfile/Index.cshtml", new ProfileViewModel()
            {
                Name = selectedUser.Person.FirstName,
                Family = selectedUser.Person.LastName,
                ImageUrl = string.IsNullOrEmpty(selectedUser.Person.PersonalImageUrl) ? "~/assets/img/UserImage/Add_user_icon.png" : selectedUser.Person.PersonalImageUrl,
                VipEndDate = CommonHelper.DateAndTimes.GetPersianDate(selectedUser.VipEndDate),
                TotalArticles = TotalArticles
            });
        }

        [UIAuthorizeUser]
        public ActionResult HomeProfile()
        {
            return PartialView("~/Views/Home/UserProfile/HomeProfile.cshtml", BindHomeProfile());
        }
        [UIAuthorizeUser]
        public ActionResult ChangePassAction()
        {
            return PartialView("~/Views/Home/UserProfile/ChangePass.cshtml");
        }

        [UIAuthorizeUser]
        public ActionResult ProfileForm()
        {
            return PartialView("~/Views/Home/UserProfile/Profile.cshtml", ProfileBinding());
        }
        [UIAuthorizeUser]
        [AjaxValidateAntiForgeryTokenAttribute]
        [HttpPost]
        public ActionResult EditUser(string viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    EditMembershipVm vm = jss.Deserialize<EditMembershipVm>(viewModel);
                    var result = submitEditUser(vm);
                    if (result.IsSuccess)
                        return ControllerHelper.SuccessResult("با موفقیت ثبت شد");
                    return ControllerHelper.ErrorResult(result.ResultMessage);
                }
                else
                {
                    return ControllerHelper.FormNotValid(ModelState);
                }

            }
            catch (Exception ex)
            {
                return ControllerHelper.ErrorResult("بروز خطای سیستمی");
            }
        }
        [UIAuthorizeUser]
        [AjaxValidateAntiForgeryTokenAttribute]
        public ActionResult UserChangePass(Models.PasswordChangeMembershipViewModel viewmodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var context = new ApplicationDbContext();
                        //User.Identity.Name
                        ApplicationUser selectedModel = context.Users.Where(item => item.UserName == User.Identity.Name).Single();
                        selectedModel.PasswordHash = new ApplicationUserManager(new UserStore<ApplicationUser>()).PasswordHasher.HashPassword(viewmodel.Password);
                        context.SaveChanges();
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
        [HttpPost]
        public ActionResult ProvinceFeed(string ProvinceID)
        {
            var defaultProvinecID = int.Parse(ProvinceID);
            var context = new ApplicationDbContext();
            var DBCities = context.Cities.Where(item => item.Province.ID == defaultProvinecID).Select(item => new { item.Name, item.ID }).ToList();
            List<DropDownVm> Cities = new List<DropDownVm>();
            foreach (var city in DBCities)
            {
                Cities.Add(new DropDownVm() { Text = city.Name, Value = city.ID.ToString() });
            }
            return Json(Cities, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Articles
        const int ArticlePageSize = 20;
        [UIAuthorizeUser]
        public ActionResult ArticleListFeed(int pageNumber)
        {
            return Json(ArticleListFeedGenerator(pageNumber), JsonRequestBehavior.AllowGet);
        }
        ArticleListViewModel ArticleListFeedGenerator(int pageNumber)
        {
            string CurrentLang = Request.Cookies["Lang"].Value;
            var contex = new ApplicationDbContext();
            int skipRows = (pageNumber - 1) * ArticlePageSize;

            var context = new ApplicationDbContext();
            var model = new object[0];
            int totalRecords = context.ConferenceArticles
                .Include(conf => conf.Article)
                .Where(conf =>
                    conf.Article.CreatorUser.UserName == User.Identity.Name
                )
               .Count();
            //↑↑↑↑↑↑↑↑↑ ↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓    realated to each other
            var DbArticles = context.ConferenceArticles
                .Include(conf => conf.Article)
                .Include(conf => conf.Article.CreatorUser)
                .Include(conf => conf.Article.ArticleStatus)
                .Include(conf => conf.Article.ArticlePresentType)
                .Where(conf =>
                    conf.Article.CreatorUser.UserName == User.Identity.Name
                )
               .Select(conf => new
               {
                   Article = conf.Article,
                   ArticleStatusName = conf.Article.ArticleStatus.Name,
                   ArticlePresentTypeName = conf.Article.ArticlePresentType.Name,
                   ArticlePresentTypeExplain = conf.Article.ArticlePresentTypeExplain,
                   ArticleStatusID = conf.Article.ArticleStatus.ID,
                   conf.CreateDate
               })
               .OrderByDescending(conf => conf.Article.CreateDate)
               .Skip(skipRows).Take(ArticlePageSize)
               .ToList();


            var Articles = new List<ArticleListItemViewModel>();
            foreach (var item in DbArticles)
            {
                //var RefereeArticle = context.RefereeArticles
                //.Include(refArt => refArt.RefereeStatus)
                //.Include(refArt => refArt.Referee)
                //.Include(refArt => refArt.Referee.RefrenceUser)
                //.Include(refArt => refArt.Referee.RefrenceUser.Person)
                //.Where(refArt => refArt.Article.ID == item.Article.ID)
                //.Select(refArt => new
                //{
                //    StatusName = refArt.RefereeStatus.Name,
                //    RefreeName = refArt.Referee.RefrenceUser.Person.FirstName + " " + refArt.Referee.RefrenceUser.Person.LastName
                //})
                //.ToList();
                ArticleListItemViewModel ArticleVm = new ArticleListItemViewModel()
                {
                    ID = item.Article.ID,
                    Title = item.Article.Title,
                    ArticleStatusName = item.ArticleStatusName,
                    ArticlePresentTypeName = item.ArticlePresentTypeName,
                    ArticlePresentTypeNameExpain = item.ArticlePresentTypeExplain,
                    CreateDateConverted = CommonHelper.DateAndTimes.GetPersianDate(item.Article.CreateDate),
                    Fields = string.Join(",",
                    context.ArticleCategories
                    .Include(ArtCat => ArtCat.Category)
                    .Include(ArtCat => ArtCat.Category.CategoryNames)
                    .Include(ArtCat => ArtCat.Category.CategoryNames.Select(CatLang => CatLang.Language))
                    .Where(ArtCat => ArtCat.Article.ID == item.Article.ID)
                    .Select(
                        ArtCat =>
                         ArtCat.Category.CategoryNames.Where(CatLang => CatLang.Language.Value == CurrentLang).FirstOrDefault().Name
                        )
                    ),

                };
                //foreach (var referee in RefereeArticle)
                //{
                //    //check Referee
                //    ArticleVm.RefereeState += "نام داور:" + referee.RefreeName + "<br /> وضعیت:" + referee.StatusName /*+ "--نحوه ارائه:" + referee.PresentType*/ + "<br />";
                //}
                ArticleVm.RefereeState = "";
                Articles.Add(ArticleVm);
            }

            return new ArticleListViewModel()
            {
                DataModel = Articles,
                pageNumber = pageNumber,
                pageSize = ArticlePageSize,
                totalRecords = totalRecords
            };
        }
        [UIAuthorizeUser]
        public ActionResult ArticleList(int pageNumber)
        {
            return PartialView("~/Views/Home/UserProfile/ArticleList.cshtml", ArticleListFeedGenerator(pageNumber));
        }
        [UIAuthorizeUser]
        public ActionResult TemplateArticle()
        {
            return PartialView("~/Views/Home/UserProfile/TemplateArticle.cshtml");
        }
        [UIAuthorizeUser]
        public ActionResult Article()
        {
            var context = new ApplicationDbContext();
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

            if (Request.QueryString["id"] == null || Request.QueryString["id"] == "undefined")
            {
                var Fields = new List<CheckBoxVm>();
                var DBcat = context.Categories
                    .Where(cat => cat.TableName == "Conference")
                    .Select(cat => new { cat.ID, cat.Name })
                    .ToList();

                foreach (var item in DBcat)
                {
                    Fields.Add(new CheckBoxVm()
                    {
                        IsChecked = false,
                        Text = item.Name,
                        Value = item.ID.ToString()
                    });
                }
                var selectedUser = context.Users
                     .Include(user => user.Person)
                     .Where(user => user.UserName == User.Identity.Name).SingleOrDefault();
                if (selectedUser.VipEndDate == null)
                {
                    ViewBag.VipEnabled = false;
                }
                else if (selectedUser.VipEndDate < DateTime.Now)
                {
                    ViewBag.VipEnabled = false;
                }
                else
                {
                    ViewBag.VipEnabled = true;
                }
                return PartialView("~/Views/Home/UserProfile/Article.cshtml", new ArticleViewModel()
                {
                    ObjectState = ObjectState.Insert,
                    Fields = Fields,
                    Languages = Languages,
                    ArticleLanguageSelectedID = Languages[0].Value,
                    AllowEdit = true,
                    ArticlePresentTypeID = 1
                });
            }
            else
            {
                var selectedUser = context.Users
                       .Include(user => user.Person)
                       .Where(user => user.UserName == User.Identity.Name).SingleOrDefault();
                if (selectedUser.VipEndDate == null)
                {
                    ViewBag.VipEnabled = false;
                }
                else if (selectedUser.VipEndDate < DateTime.Now)
                {
                    ViewBag.VipEnabled = false;
                }
                else
                {
                    ViewBag.VipEnabled = true;
                }
                var selectedID = int.Parse(Request.QueryString["id"]);
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
                var selectedArticle = context.Articles
                  .Include(item => item.ArticleStatus)
                  .Include(item => item.ArticlePresentType)
                  .Where(item =>
                    item.ID.Equals(selectedID)
                    &&
                    item.CreatorUser.Id == context.Users.Where(user => user.UserName == User.Identity.Name).FirstOrDefault().Id
                  )
                  .Select(item => new
                  {
                      ArticleStatusName = item.ArticleStatus.Name,
                      ArticleStatusID = item.ArticleStatus.ID,
                      ArticlePresentTypeID = item.ArticlePresentType.ID,
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
                      LanguageID = item.Language.ID,
                      item.CreateDate
                  })
                  .SingleOrDefault();
                var DbWriters = context.ArticleWriters
                    .Where(artWriters =>
                        artWriters.Article.ID == selectedArticle.ID
                    )
                    .ToList();
                List<WriterViewModel> Writers = new List<WriterViewModel>();
                foreach (var writer in DbWriters)
                {
                    Writers.Add(new WriterViewModel()
                    {
                        FirstName = writer.FirstName,
                        LastName = writer.LastName,
                        Email = writer.Email,
                        Cellphone = writer.Cellphone,
                        IsMainWriter = writer.IsMainWriter
                    });
                }
                return PartialView("~/Views/Home/UserProfile/Article.cshtml", new ArticleViewModel()
                {
                    ID = selectedArticle.ID,
                    Title = selectedArticle.Title,
                    CreateDateConverted = CommonHelper.DateAndTimes.GetPersianDate(selectedArticle.CreateDate),
                    EnglishTitle = selectedArticle.EnglishTitle,
                    ArticleFileUrl = selectedArticle.FileUrl,
                    FileWordUrl = selectedArticle.FileWordUrl,
                    Keywords = selectedArticle.Keywords,
                    Summary = selectedArticle.Summary,
                    Visit = selectedArticle.Visit,
                    Enable = selectedArticle.Enable,
                    Writers = Writers,
                    Fields = Fields,
                    Languages = Languages,
                    ArticlePresentTypeID = selectedArticle.ArticlePresentTypeID,
                    ArticleLanguageSelectedID = selectedArticle.LanguageID.ToString(),
                    AllowEdit = selectedArticle.ArticleStatusID == (int)Enums.ArticleStatus.RefereeNotConfirmed || selectedArticle.ArticleStatusID == (int)Enums.ArticleStatus.TechnicalProblem || (selectedArticle.ArticleStatusID == (int)Enums.ArticleStatus.Confirmed && selectedArticle.ArticlePresentTypeID == (int)Enums.ArticlePresentType.Poster),
                    ObjectState = ObjectState.Update
                });
            }
            //ArticleViewModel ArticleVm = new ArticleViewModel()
            //{
            //    ID = item.Article.ID,
            //    Title = item.Article.Title,
            //    CreateDateConverted = CommonHelper.DateAndTimes.GetPersianDate(item.Article.CreateDate),
            //    Fields = string.Join(",", context.ArticleCategories.Include(ArtCat => ArtCat.Category).Where(ArtCat => ArtCat.Article.ID == item.Article.ID).Select(ArtCat => ArtCat.Category.Name)),
            //    ArticleStatusName = item.ArticleStatusName,
            //    ArticleStatusID = item.ArticleStatusID,
            //    EnglishTitle = item.Article.EnglishTitle,
            //    FileUrl = Url.Content(item.Article.FileUrl),
            //    FileUrlText = "فایل",
            //    Keywords = item.Article.Keywords,
            //    Summary = item.Article.Summary,
            //    Visit = item.Article.Visit,
            //    Writers = item.Article.Writers,
            //    Enable = item.Article.Enable,
            //};
        }
        [UIAuthorizeUser]
        [AjaxValidateAntiForgeryTokenAttribute]
        [HttpPost]
        public ActionResult SubmitArticle(string viewModel)
        {
            try
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                ArticleViewModel vm = jss.Deserialize<ArticleViewModel>(viewModel);
                if (ModelState.IsValid)
                {
                    var context = new ApplicationDbContext();
                    var selectedUser = context.Users
                         .Include(user => user.Person)
                         .Where(user => user.UserName == User.Identity.Name).SingleOrDefault();
                    try
                    {
                        string ArticleFile = "";
                        string ArticleWordFile = "";
                        string ArticlePosterFile = "";
                        if (context.Articles.Count(art => art.Title == vm.Title) > 0 && vm.ObjectState == ObjectState.Insert)
                        {
                            return ControllerHelper.ErrorResult("مقاله ای با این عنوان قبلا در سیستم ثبت شده است");
                        }
                        if (vm.Fields.Count(field => field.IsChecked == true) == 0)
                        {
                            return ControllerHelper.ErrorResult("لطفا زمینه ی مقاله را انتخاب کنید");
                        }
                        if (vm.Writers == null)
                        {
                            return ControllerHelper.ErrorResult("لطفا حداقل یک نویسنده برای مقاله تعریف کنید");
                        }
                        if (vm.Writers.Count == 0)
                        {
                            return ControllerHelper.ErrorResult("لطفا حداقل یک نویسنده برای مقاله تعریف کنید");
                        }
                        var trans = context.Database.BeginTransaction();
                        var LanguageID = int.Parse(vm.ArticleLanguageSelectedID);
                        try
                        {
                            var SelectedConf = context.Conferences.Where(conf => (bool)conf.Enable && (bool)conf.Visible).SingleOrDefault();
                            if (SelectedConf != null)
                            {
                                int CurrentConfID = context.Conferences.Where(con => (bool)con.Enable && (bool)con.Visible).SingleOrDefault().ID;
                                if (SelectedConf.SendEndDate.Date < DateTime.Now.Date && vm.ObjectState == ObjectState.Insert)
                                {
                                    return ControllerHelper.ErrorResult("زمان ارسال مقاله به پایان رسیده است");
                                }
                                if (Request.Files.Count > 0)
                                {
                                    var file = Request.Files[CommonHelper.ExposeProperty.GetPropertyInfo(vm, m => m.ArticleFile).Name];

                                    if (file != null && file.ContentLength > 0)
                                    {
                                        ArticleFile = FileUploadManagment.UploadFile("~/assets/img/Attach/Article/", "", file, FileUploadManagment.AppFileType.Document, ".pdf", 10485760);
                                        if (ArticleFile == "")
                                        {
                                            return ControllerHelper.ErrorResult("فرمت یا حجم فایل صحیح نمی باشد");
                                        }
                                    }
                                    file = Request.Files[CommonHelper.ExposeProperty.GetPropertyInfo(vm, m => m.ArticleWordFile).Name];
                                    if (file != null && file.ContentLength > 0)
                                    {
                                        ArticleWordFile = FileUploadManagment.UploadFile("~/assets/img/Attach/Article/", "", file, FileUploadManagment.AppFileType.Document, ".docx", 10485760);
                                        if (ArticleWordFile == "")
                                        {
                                            return ControllerHelper.ErrorResult("فرمت یا حجم فایل صحیح نمی باشد.فرمت قابل قبول docx می باشد");
                                        }
                                    }
                                    file = Request.Files[CommonHelper.ExposeProperty.GetPropertyInfo(vm, m => m.PosterFile).Name];
                                    if (file != null && file.ContentLength > 0)
                                    {
                                        ArticlePosterFile = FileUploadManagment.UploadFile("~/assets/img/Attach/Article/", "", file, FileUploadManagment.AppFileType.Image, "", 5242880);
                                        if (ArticlePosterFile == "")
                                        {
                                            return ControllerHelper.ErrorResult("فرمت یا حجم فایل صحیح نمی باشد.فرمت قابل قبول jpg می باشد");
                                        }
                                    }
                                }
                                else
                                {
                                    return ControllerHelper.ErrorResult("لطفا فایل مقاله را انتخاب کنید");
                                }
                                if (vm.ObjectState == ObjectState.Insert) // insert
                                {
                                    if (ArticleFile == "")
                                    {
                                        return ControllerHelper.ErrorResult("فایل مقاله بارگذاری نشده است");
                                    }
                                    var article = new Article()
                                    {
                                        ArticleStatus = context.ArticleStatuses.Where(state => state.ID == (int)Enums.ArticleStatus.Checking).SingleOrDefault(),
                                        ArticlePresentType = context.ArticlePresentTypes.Where(state => state.ID == (int)Enums.ArticlePresentType.Checking).SingleOrDefault(),
                                        Enable = true,
                                        Visible = false,
                                        Published = false,
                                        FileUrl = ArticleFile,
                                        FileWordUrl = ArticleWordFile,
                                        EnglishTitle = vm.EnglishTitle,
                                        Title = vm.Title,
                                        Summary = vm.Summary,
                                        Visit = 0,
                                        Keywords = vm.Keywords,
                                        Language = context.Languages.Where(lang => lang.ID == LanguageID).SingleOrDefault(),
                                        CreateDate = DateTime.Now,
                                        CreatorUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault()
                                    };
                                    context.Articles.Add(article);
                                    foreach (var field in vm.Fields)
                                    {
                                        if (field.IsChecked)
                                        {
                                            var CatID = int.Parse(field.Value);
                                            context.ArticleCategories.Add(new ArticleCategory()
                                            {
                                                Article = article,
                                                Enable = true,
                                                Category = context.Categories.Where(cat => cat.ID == CatID).SingleOrDefault(),
                                                CreateDate = DateTime.Now,
                                                CreatorUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault()
                                            });
                                        }
                                    }
                                    var person = new Person();
                                    foreach (var writer in vm.Writers)
                                    {
                                        context.ArticleWriters.Add(new ArticleWriter()
                                        {
                                            Article = article,
                                            FirstName = writer.FirstName,
                                            LastName = writer.LastName,
                                            Cellphone = writer.Cellphone,
                                            Email = writer.Email,
                                            IsMainWriter = writer.IsMainWriter,
                                            CreateDate = DateTime.Now,
                                            CreatorUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault()
                                        });
                                    }
                                    context.ConferenceArticles.Add(new ConferenceArticle()
                                    {
                                        Article = article,
                                        Conference = SelectedConf,
                                        Enable = true,
                                        CreateDate = DateTime.Now,
                                        CreatorUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault()
                                    });
                                }
                                else if (vm.ObjectState == ObjectState.Update)
                                {
                                    var selectedArticle = context.Articles
                                      .Include(item => item.ArticleStatus)
                                      .Include(item => item.ArticlePresentType)
                                      .Where(item =>
                                        item.ID.Equals(vm.ID)
                                        &&
                                        item.CreatorUser.Id == context.Users.Where(user => user.UserName == User.Identity.Name).FirstOrDefault().Id
                                        &&
                                        (item.ArticleStatus.ID == (int)Enums.ArticleStatus.RefereeNotConfirmed || item.ArticleStatus.ID == (int)Enums.ArticleStatus.TechnicalProblem || (item.ArticleStatus.ID == (int)Enums.ArticleStatus.Confirmed && item.ArticlePresentType.ID == (int)Enums.ArticlePresentType.Poster))
                                      )
                                      .SingleOrDefault();
                                    if (selectedArticle != null)
                                    {
                                        if (selectedArticle.ArticleStatus.ID != (int)Enums.ArticleStatus.Confirmed)
                                        {
                                            selectedArticle.ArticleStatus = context.ArticleStatuses.Where(state => state.ID == (int)Enums.ArticleStatus.ResendByUser).SingleOrDefault();
                                            if (ArticleFile != "")
                                                selectedArticle.FileUrl = ArticleFile;
                                            if (ArticleWordFile != "")
                                                selectedArticle.FileWordUrl = ArticleWordFile;
                                            selectedArticle.EnglishTitle = vm.EnglishTitle;
                                            selectedArticle.Title = vm.Title;
                                            selectedArticle.Summary = vm.Summary;
                                            selectedArticle.Keywords = vm.Keywords;
                                            selectedArticle.Language = context.Languages.Where(lang => lang.ID == LanguageID).SingleOrDefault();
                                            context.ArticleCategories.RemoveRange(context.ArticleCategories.Where(artCat => artCat.Article.ID == selectedArticle.ID));
                                            context.ArticleWriters.RemoveRange(context.ArticleWriters.Where(artWriter => artWriter.Article.ID == selectedArticle.ID));
                                            foreach (var field in vm.Fields)
                                            {
                                                if (field.IsChecked)
                                                {
                                                    var CatID = int.Parse(field.Value);
                                                    context.ArticleCategories.Add(new ArticleCategory()
                                                    {
                                                        Article = selectedArticle,
                                                        Enable = true,
                                                        Category = context.Categories.Where(cat => cat.ID == CatID).SingleOrDefault(),
                                                        CreateDate = DateTime.Now,
                                                        CreatorUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault()
                                                    });
                                                }
                                            }
                                            var person = new Person();
                                            foreach (var writer in vm.Writers)
                                            {
                                                context.ArticleWriters.Add(new ArticleWriter()
                                                {
                                                    Article = selectedArticle,
                                                    FirstName = writer.FirstName,
                                                    LastName = writer.LastName,
                                                    Cellphone = writer.Cellphone,
                                                    Email = writer.Email,
                                                    CreateDate = DateTime.Now,
                                                    CreatorUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault()
                                                });
                                            }
                                        }
                                        if (selectedArticle.ArticlePresentType.ID == (int)Enums.ArticlePresentType.Poster)
                                        {
                                            if (ArticlePosterFile != "")
                                                selectedArticle.PosterUrl = ArticlePosterFile;
                                            else
                                            {
                                                return ControllerHelper.ErrorResult("لطفا فایل پوستر مقاله را وارد نمایید");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        return ControllerHelper.ErrorResult("شما قادر به ویرایش این مقاله نیستید");
                                    }
                                }
                                context.SaveChanges();
                                trans.Commit();
                                try
                                {
                                    if (vm.ObjectState == ObjectState.Insert) // insert
                                    {
                                        EmailService.EmailResponse MailResult = EmailService.SendSubmitArticle(
                                            selectedUser.Person.FirstName,
                                            selectedUser.Person.LastName,
                                            selectedUser.UserName,
                                            selectedUser.Email,
                                            vm.Title);
                                        foreach (var writer in vm.Writers)
                                        {
                                            EmailService.EmailResponse MailResultWriter = EmailService.SendSubmitArticle(
                                               writer.FirstName,
                                               writer.LastName,
                                               "",
                                               writer.Email,
                                               vm.Title);
                                        }
                                    }
                                    if (vm.ObjectState == ObjectState.Update) // Update
                                    {
                                        EmailService.EmailResponse MailResult = EmailService.SendEditArticle(
                                              selectedUser.Person.FirstName,
                                              selectedUser.Person.LastName,
                                              selectedUser.UserName,
                                              selectedUser.Email,
                                              vm.Title);
                                        foreach (var writer in vm.Writers)
                                        {
                                            EmailService.EmailResponse MailResultWriter = EmailService.SendEditArticle(
                                               writer.FirstName,
                                               writer.LastName,
                                               "",
                                               writer.Email,
                                               vm.Title);
                                        }
                                    }

                                }
                                catch (Exception)
                                {
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            return ControllerHelper.ErrorResult("بروز خطای سیستمی" + ex.Message);
                        }

                    }
                    catch (Exception ex)
                    {
                        return ControllerHelper.ErrorResult("بروز خطای سیستمی" + ex.Message);
                    }
                    if (selectedUser.VipEndDate == null)
                    {
                        return ControllerHelper.ErrorResult("با موفقیت ثبت شد.توجه داشته باشید که در صورت عدم عضویت شما در انجمن این مقاله به چرخه داوری ارسال نمی شود");
                    }
                    if (selectedUser.VipEndDate < DateTime.Now)
                    {
                        return ControllerHelper.ErrorResult("با موفقیت ثبت شد.توجه داشته باشید که در صورت عدم عضویت شما در انجمن این مقاله به چرخه داوری ارسال نمی شود");
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
        #region Companion

        [UIAuthorizeUser]
        public ActionResult RegisterCompanion()
        {
            string CurrentLang = Request.Cookies["Lang"].Value;
            var context = new ApplicationDbContext();
            var viewModel = new CompanionViewModel();
            var Packages = context.ConferencePackages
                .Include(ConfPack => ConfPack.Conference)
                .Include(ConfPack => ConfPack.PackageName)
                .Include(ConfPack => ConfPack.PackageName.PackageNameTranslations)
                .Include(ConfPack => ConfPack.PackageName.PackageNameTranslations.Select(trans => trans.Language))
                .Where(ConfPack =>
                (bool)ConfPack.Conference.Enable &&
                (bool)ConfPack.Conference.Visible &&
                ConfPack.PackageName.PackageType.ID == (int)Enums.PackageType.RegisterConferencePackage &&
                ConfPack.PackageName.PackageNameTranslations.Any(trans => trans.Language.Value == CurrentLang)
                )
                .Select(ConfPack => new
                {
                    ConfPack.ID,
                    ConfPack.Price,
                    PackageID = ConfPack.PackageName.ID,
                    PackageName = ConfPack.PackageName.PackageNameTranslations.FirstOrDefault().Name,
                })
               .ToList();
            //*****************************************************//
            var RegisterUserPackage = Packages.Where(pack => pack.PackageID == (int)Enums.ConferencePackageName.RegisterUserPackage).SingleOrDefault();
            viewModel.RegisterUserPackage = new ConferencePackageViewModel()
            {
                ID = RegisterUserPackage.ID.ToString(),
                Price = (int)RegisterUserPackage.Price,
                Name = RegisterUserPackage.PackageName
            };
            //*****************************************************//
            var UniversityUserPackage = Packages.Where(pack => pack.PackageID == (int)Enums.ConferencePackageName.UniversityUserPackage).SingleOrDefault();
            viewModel.UniversityUserPackage = new ConferencePackageViewModel
            {
                ID = UniversityUserPackage.ID.ToString(),
                Price = (int)UniversityUserPackage.Price,
                Name = UniversityUserPackage.PackageName
            };
            //*****************************************************//
            var RegularUserPackage = Packages.Where(pack => pack.PackageID == (int)Enums.ConferencePackageName.RegularUserPackage).SingleOrDefault();
            viewModel.RegularUserPackage = new ConferencePackageViewModel
            {
                ID = RegularUserPackage.ID.ToString(),
                Price = (int)RegularUserPackage.Price,
                Name = RegularUserPackage.PackageName
            };

            //*****************************************************//
            var SecondArticlePackage = Packages.Where(pack => pack.PackageID == (int)Enums.ConferencePackageName.SecondArticlePackage).SingleOrDefault();
            viewModel.SecondArticlePackage = new ConferencePackageViewModel
            {
                ID = SecondArticlePackage.ID.ToString(),
                Price = (int)SecondArticlePackage.Price,
                Name = SecondArticlePackage.PackageName
            };
            //*****************************************************//
            viewModel.SelectedPackage = viewModel.RegisterUserPackage.ID.ToString();
            viewModel.AllowEdit = true;
            return PartialView("~/Views/Home/UserProfile/RegisterCompanion.cshtml", viewModel);
        }
        [UIAuthorizeUser]
        [AjaxValidateAntiForgeryTokenAttribute]
        [HttpPost]
        public ActionResult SubmitRegisterCompanion(string viewModel)
        {
            try
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                CompanionViewModel vm = jss.Deserialize<CompanionViewModel>(viewModel);
                if (ModelState.IsValid)
                {
                    try
                    {
                        string PayReceiptFileImage = "";
                        string UniversityCardUrl = "";
                        var context = new ApplicationDbContext();
                        var trans = context.Database.BeginTransaction();
                        var person = new Person();
                        try
                        {
                            if (context.Conferences.Where(con => (bool)con.Enable && (bool)con.Visible).SingleOrDefault() != null)
                            {
                                int CurrentConfID = context.Conferences.Where(con => (bool)con.Enable && (bool)con.Visible).SingleOrDefault().ID;
                                if (context.ConferenceCompanions
                                    .Count(ConfComp =>
                                            ConfComp.Melicode == vm.Melicode
                                            &&
                                           ConfComp.Conference.ID == CurrentConfID) == 0)
                                {
                                    if (Request.Files.Count > 0)
                                    {
                                        var file = Request.Files[CommonHelper.ExposeProperty.GetPropertyInfo(vm, m => m.UniversityCardFileImage).Name];

                                        if (file != null && file.ContentLength > 0)
                                        {
                                            UniversityCardUrl = FileUploadManagment.UploadFile("~/assets/img/Attach/ConferenceCompanion/", "", file, FileUploadManagment.AppFileType.Image);
                                            if (UniversityCardUrl == "")
                                            {
                                                return ControllerHelper.ErrorResult("فرمت یا حجم فایل صحیح نمی باشد");
                                            }

                                        }
                                        file = Request.Files[CommonHelper.ExposeProperty.GetPropertyInfo(vm, m => m.PayReceiptFileImage).Name];
                                        if (file != null && file.ContentLength > 0)
                                        {
                                            PayReceiptFileImage = FileUploadManagment.UploadFile("~/assets/img/Attach/ConferenceCompanion/", "", file, FileUploadManagment.AppFileType.Image);
                                            if (PayReceiptFileImage == "")
                                            {
                                                return ControllerHelper.ErrorResult("فرمت یا حجم فایل صحیح نمی باشد");
                                            }
                                        }
                                    }
                                    //if (context.Persons.Count(p => p.Melicode == vm.Person.Melicode) == 0)
                                    //{
                                    //    person = context.Persons.Add(new Person()
                                    //    {
                                    //        FirstName = vm.Person.FirstName,
                                    //        LastName = vm.Person.LastName,
                                    //        Melicode = vm.Person.Melicode,
                                    //        University = vm.Person.University,
                                    //        Degree = vm.Person.Degree,
                                    //        FieldOfStudy = vm.Person.FieldOfStudy,
                                    //        Email = vm.Person.Email,
                                    //        BornDate = CommonHelper.DateAndTimes.GetGregorianDate(vm.Person.BornDate),
                                    //        UniversityCardUrl = UniversityCardUrl == "" ? MvcApplication.DefaultImageUrl : UniversityCardUrl,
                                    //        PayReceiptUrl = PayReceiptFileImage == "" ? MvcApplication.DefaultImageUrl : PayReceiptFileImage,
                                    //        CreateDate = DateTime.Now,
                                    //        CreatorUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault()
                                    //    });
                                    //}
                                    //else
                                    //{
                                    //    person = context.Persons.Where(p => p.Melicode == vm.Person.Melicode).SingleOrDefault();
                                    //    person.UniversityCardUrl = UniversityCardUrl == "" ? person.UniversityCardUrl : UniversityCardUrl;
                                    //    person.PayReceiptUrl = PayReceiptFileImage == "" ? person.PayReceiptUrl : PayReceiptFileImage;
                                    //    person.LastModifyUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault();
                                    //    person.LastModifyDate = DateTime.Now;

                                    //}
                                    //context.SaveChanges();
                                    int SelectedPackage = int.Parse(vm.SelectedPackage);
                                    context.ConferenceCompanions.Add(new ConferenceCompanion()
                                    {
                                        Conference = context.Conferences.Where(con => (bool)con.Enable && (bool)con.Visible).SingleOrDefault(),
                                        FirstName = vm.FirstName,
                                        LastName = vm.LastName,
                                        Melicode = vm.Melicode,
                                        University = vm.University,
                                        Degree = vm.Degree,
                                        FieldOfStudy = vm.FieldOfStudy,
                                        Cellphone = vm.Cellphone,
                                        Email = vm.Email,
                                        BornDate = CommonHelper.DateAndTimes.GetGregorianDate(vm.BornDate),
                                        UniversityCardUrl = UniversityCardUrl == "" ? MvcApplication.DefaultImageUrl : UniversityCardUrl,
                                        PayReceiptUrl = PayReceiptFileImage == "" ? MvcApplication.DefaultImageUrl : PayReceiptFileImage,
                                        ConferencePackage = context.ConferencePackages.Where(ConfPack => ConfPack.ID == SelectedPackage).SingleOrDefault(),
                                        CreatorUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault(),
                                        CreateDate = DateTime.Now,

                                    });
                                    context.SaveChanges();
                                    trans.Commit();
                                }
                                else
                                {
                                    trans.Rollback();
                                    return ControllerHelper.ErrorResult("مشخصات همراه شما قبلا برای این همایش ثبت شده است");

                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            return ControllerHelper.ErrorResult("بروز خطای سیستمی");
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
        [UIAuthorizeUser]
        [AjaxValidateAntiForgeryTokenAttribute]
        [HttpPost]
        public ActionResult CheckMeliCode(string MeliCode)
        {
            try
            {
                var context = new ApplicationDbContext();
                var SelectedPerson = context.Persons.Where(p => p.Melicode == MeliCode).SingleOrDefault();
                PersonViewModel person = new PersonViewModel();
                person.ID = 0;
                person.AllowEdit = true;
                if (SelectedPerson != null)
                {
                    person.ID = SelectedPerson.ID;
                    person.BornDate = CommonHelper.DateAndTimes.GetPersianDate(SelectedPerson.BornDate);
                    person.Degree = SelectedPerson.Degree;
                    person.Email = SelectedPerson.Email;
                    person.FirstName = SelectedPerson.FirstName;
                    person.LastName = SelectedPerson.LastName;
                    person.Melicode = SelectedPerson.Melicode;
                    person.PersonalFileImageUrl = SelectedPerson.PersonalImageUrl;
                    person.University = SelectedPerson.University;
                    person.AllowEdit = false;
                }
                return Json(person, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {

                return ControllerHelper.ErrorResult("بروز خطای سیستمی");
            }
        }


        const int CompanionPageSize = 20;
        [UIAuthorizeUser]
        public ActionResult CompanionListFeed(int pageNumber)
        {
            return Json(CompanionListFeedGenerator(pageNumber), JsonRequestBehavior.AllowGet);
        }
        [UIAuthorizeUser]
        public ActionResult CompanionList(int pageNumber)
        {
            return PartialView("~/Views/Home/UserProfile/CompanionList.cshtml", CompanionListFeedGenerator(pageNumber));
        }

        CompanionListViewModel CompanionListFeedGenerator(int pageNumber)
        {
            string CurrentLang = Request.Cookies["Lang"].Value;
            var context = new ApplicationDbContext();
            int skipRows = (pageNumber - 1) * CompanionPageSize;
            var CompanionViewModels = new List<CompanionViewModel>();
            int CurrentConfID = 0;
            int totalRecords = 0;
            if (context.Conferences.Where(con => (bool)con.Enable && (bool)con.Visible).SingleOrDefault() != null)
            {
                CurrentConfID = context.Conferences.Where(con => (bool)con.Enable && (bool)con.Visible).SingleOrDefault().ID;
                totalRecords = context.ConferenceCompanions
                    .Include(confCom => confCom.CreatorUser)
                    .Where(confCom =>
                        confCom.CreatorUser.UserName == User.Identity.Name
                        &&
                        confCom.Conference.ID == CurrentConfID
                    )
                   .Count();
                //↑↑↑↑↑↑↑↑↑ ↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓    realated to each other

                var DbCompanions = context.ConferenceCompanions
                    .Include(confCom => confCom.CreatorUser)
                    .Include(confCom => confCom.ConferencePackage)
                    .Include(confCom => confCom.ConferencePackage.PackageName)
                    .Include(confCom => confCom.ConferencePackage.PackageName.PackageNameTranslations)
                    .Include(confCom => confCom.ConferencePackage.PackageName.PackageNameTranslations.Select(trans => trans.Language))
                    .Where(confCom =>
                        confCom.CreatorUser.UserName == User.Identity.Name
                        &&
                        confCom.Conference.ID == CurrentConfID
                        &&
                         confCom.ConferencePackage.PackageName.PackageNameTranslations.Any(trans => trans.Language.Value == CurrentLang)
                    )
                    .Select(confCom => new
                    {
                        confCom.ID,
                        confCom.FirstName,
                        confCom.LastName,
                        confCom.CreateDate,
                        PackageName = confCom.ConferencePackage.PackageName.PackageNameTranslations.FirstOrDefault().Name,
                        PackagePrice = confCom.ConferencePackage.Price
                    })
                    .OrderByDescending(confCom => confCom.CreateDate)
                   .Skip(skipRows).Take(CompanionPageSize)
                   .ToList();
                foreach (var item in DbCompanions)
                {
                    CompanionViewModels.Add(new CompanionViewModel()
                    {
                        ID = item.ID,
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        CreateDateConverted = CommonHelper.DateAndTimes.GetPersianDate(item.CreateDate),
                        SelectedPackageObject = new ConferencePackageViewModel()
                        {
                            Name = item.PackageName,
                            Price = item.PackagePrice
                        }

                    });
                }


            }
            return new CompanionListViewModel()
            {
                DataModel = CompanionViewModels,
                pageNumber = pageNumber,
                pageSize = CompanionPageSize,
                totalRecords = totalRecords
            };


        }
        [HttpPost]
        [AjaxValidateAntiForgeryTokenAttribute]
        [UIAuthorizeUser]
        public ActionResult DeleteCompanion(string[] ids)
        {
            var context = new ApplicationDbContext();
            var dbContextTransaction = context.Database.BeginTransaction();
            try
            {
                foreach (var id in ids)
                {
                    int ID = int.Parse(id);
                    var selectedItem = context.ConferenceCompanions
                        .Where(item => item.ID == ID).Single();
                    if (selectedItem != null)
                    {
                        context.ConferenceCompanions.Remove(selectedItem);
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
        #endregion
        #region PayConference
        [UIAuthorizeUser]
        public ActionResult PayConference(int OrderID = 0)
        {
            string CurrentLang = Request.Cookies["Lang"].Value;
            PayConferenceViewModel payConferenceViewModel = new PayConferenceViewModel();
            payConferenceViewModel.ConferencePackages = new List<ConferencePackageViewModel>();
            var context = new ApplicationDbContext();
            var SelectedConf = context.Conferences.Where(con => (bool)con.Enable && (bool)con.Visible).SingleOrDefault();
            if (SelectedConf != null)
            {
                var DbConferencePackages = context.ConferencePackages
                    .Include(confPack => confPack.PackageName)
                    .Include(confPack => confPack.PackageName.PackageNameTranslations)
                    .Include(ConfPack => ConfPack.PackageName.PackageNameTranslations.Select(trans => trans.Language))
                    .Where(confPack =>
                        confPack.Conference.ID == SelectedConf.ID
                        &&
                        confPack.PackageName.PackageType.ID == (int)Enums.PackageType.ConferenceFeaturePackage
                        &&
                        confPack.PackageName.PackageNameTranslations.Any(trans => trans.Language.Value == CurrentLang)
                    )
                    .Select(confPack => new
                    {
                        confPack.ID,
                        confPack.Price,
                        confPack.PackageName.PackageNameTranslations.FirstOrDefault().Name
                    })
                    .ToList();
                var ComPrice = context.ConferenceCompanions
                    .Include(confCom => confCom.ConferencePackage)
                    .Where(confCom =>
                         confCom.ConferencePackage.Conference.ID == SelectedConf.ID
                         &&
                         confCom.CreatorUser == context.Users.Where(user => user.UserName == User.Identity.Name).FirstOrDefault()
                    )
                    .Select(confCom => confCom.ConferencePackage.Price)
                    .DefaultIfEmpty(0)
                    .Sum();
                payConferenceViewModel.CompanionPackage = new ConferencePackageViewModel()
                {
                    Name = "هزینه بسته های انتخاب شده",
                    Price = (int)ComPrice
                };
                foreach (var pack in DbConferencePackages)
                {
                    var OrderCnt = context.OrderItems.Where(orderItem => orderItem.Order.ID == OrderID && orderItem.ConferencePackage.ID == pack.ID).SingleOrDefault();
                    payConferenceViewModel.ConferencePackages.Add(new ConferencePackageViewModel()
                    {
                        ID = pack.ID.ToString(),
                        Name = pack.Name,
                        Price = (int)pack.Price,
                        Count = OrderID == 0 ? 0 : (OrderCnt != null ? OrderCnt.Count : 0)
                    });
                }
                if (OrderID != 0)
                {
                    var CurrentOrder = context.Orders
                        .Include(order => order.OrderStatus)
                        .Where(order => order.ID == OrderID).SingleOrDefault();
                    payConferenceViewModel.Enable = CurrentOrder.OrderStatus.ID == (int)Enums.OrderStatus.PreInvoice | CurrentOrder.OrderStatus.ID == (int)Enums.OrderStatus.NotConfirmed;
                }
                else
                {
                    payConferenceViewModel.Enable = true;
                }
            }
            else
            {
                payConferenceViewModel.Enable = false;
            }
            return PartialView("~/Views/Home/UserProfile/PayConference.cshtml", payConferenceViewModel);
        }
        [UIAuthorizeUser]
        [AjaxValidateAntiForgeryTokenAttribute]
        [HttpPost]
        public ActionResult SubmitPayConference(PayConferenceViewModel viewModel)
        {
            try
            {

                var context = new ApplicationDbContext();
                var trans = context.Database.BeginTransaction();
                try
                {
                    var SelectedConf = context.Conferences.Where(con => (bool)con.Enable && (bool)con.Visible).SingleOrDefault();
                    var UserOrder = context.Orders
                          .Include(order => order.CreatorUser)
                          .Include(order => order.OrderItems)
                          .Where(order =>
                              order.CreatorUser.UserName == User.Identity.Name
                              &&
                              order.OrderStatus.ID == (int)Enums.OrderStatus.PreInvoice
                          ).SingleOrDefault();

                    if (UserOrder != null)
                    {
                        context.OrderItems.RemoveRange(UserOrder.OrderItems);
                        context.Orders.Remove(UserOrder);
                        var invoiceObj = context.Invoices.Where(invoice =>
                              invoice.TableName == "Order"
                              &&
                              invoice.DocumentID == UserOrder.ID
                           ).SingleOrDefault();
                        if (invoiceObj != null)
                            context.Invoices.Remove(invoiceObj);
                        context.SaveChanges();
                    }
                    UserOrder = context.Orders.Add(new Order()
                    {
                        Conference = SelectedConf,
                        OrderStatus = context.OrderStatuses.Where(status => status.ID == (int)Enums.OrderStatus.PreInvoice).SingleOrDefault(),
                        CreateDate = DateTime.Now,
                        CreatorUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault(),

                    });
                    var Companions = context.ConferenceCompanions
                   .Include(confCom => confCom.ConferencePackage)
                   .Include(confCom => confCom.CreatorUser)
                   .Where(confCom =>
                        confCom.ConferencePackage.Conference.ID == SelectedConf.ID
                        &&
                        !confCom.Enable
                        &&
                        confCom.CreatorUser.UserName == User.Identity.Name
                   )
                   .Select(confCom =>
                        new
                        {
                            confCom.ConferencePackage,
                            confCom.ConferencePackage.PackageName
                        }
                   ).ToList();
                    if (Companions.Count > 0)
                    {
                        foreach (var pack in Companions.GroupBy(cmp => cmp.ConferencePackage.ID).Select(g => new { ID = g.Key, Count = g.Count() }))
                        {
                            if (pack.Count > 0)
                            {
                                var order = new OrderItem()
                                {
                                    Order = UserOrder,
                                    Count = pack.Count,
                                    ConferencePackage = context.ConferencePackages
                                            .Include(dbpack => dbpack.Conference)
                                            .Where(dbpack => dbpack.ID == pack.ID).SingleOrDefault(),
                                    CreateDate = DateTime.Now,
                                    CreatorUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault(),

                                };
                                context.OrderItems.Add(order);
                            }
                        }

                    }
                    foreach (var pack in viewModel.ConferencePackages)
                    {
                        var PackID = int.Parse(pack.ID);
                        if (pack.Count > 0)
                        {
                            var order = new OrderItem()
                            {
                                Order = UserOrder,
                                Count = pack.Count,
                                ConferencePackage = context.ConferencePackages
                                        .Include(dbpack => dbpack.Conference)
                                        .Where(dbpack => dbpack.ID == PackID).SingleOrDefault(),
                                CreateDate = DateTime.Now,
                                CreatorUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault(),

                            };
                            context.OrderItems.Add(order);
                        }
                    }

                    context.SaveChanges();
                    trans.Commit();
                    return ControllerHelper.SuccessResult(UserOrder.ID.ToString());
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    return ControllerHelper.ErrorResult(ex.Message/*"بروز خطای سیستمی"*/);
                }

            }
            catch (Exception ex)
            {
                //return ControllerHelper.ErrorResult("بروز خطای سیستمی");
                return ControllerHelper.ErrorResult("بروز خطای سیستمی");
            }
        }
        #endregion
        #region PayVip
        [UIAuthorizeUser]
        public ActionResult PayVip(int OrderID = 0)
        {
            string CurrentLang = Request.Cookies["Lang"].Value;
            PayVipViewModel PayVipViewModel = new PayVipViewModel();
            PayVipViewModel.OtherPackages = new List<VipPackageNameViewModel>();
            PayVipViewModel.RegisterPackages = new List<VipPackageNameViewModel>();
            var context = new ApplicationDbContext();
            var DBCompanyPackageNames = context.PackageNames
                .Include(Pack => Pack.PackageNameTranslations)
                .Include(Pack => Pack.PackageNameTranslations.Select(trans => trans.Language))
                .Where(Pack =>
                    Pack.PackageNameTranslations.Any(trans => trans.Language.Value == CurrentLang)
                    &&
                    (
                        Pack.PackageType.ID == (int)Enums.PackageType.SocietyPackageFeature
                        ||
                        Pack.PackageType.ID == (int)Enums.PackageType.SocietyPackage
                    )
                    &&
                    (bool)Pack.Enable
                )
                .Select(conPack => new
                {
                    conPack.ID,
                    conPack.Price,
                    conPack.PackageNameTranslations.FirstOrDefault().Name,
                    PackageTypeID = conPack.PackageType.ID
                })
                .ToList();
            foreach (var pack in DBCompanyPackageNames)
            {
                if (pack.PackageTypeID == (int)Enums.PackageType.SocietyPackage)
                {
                    PayVipViewModel.RegisterPackages.Add(new VipPackageNameViewModel()
                    {
                        ID = pack.ID,
                        Name = pack.Name,
                        Price = (int)pack.Price,
                        Count = 0
                    });
                }
                else
                {
                    PayVipViewModel.OtherPackages.Add(new VipPackageNameViewModel()
                    {
                        ID = pack.ID,
                        Name = pack.Name,
                        Price = (int)pack.Price,
                        Count = 0,
                    });
                }
            }
            if (OrderID != 0)
            {
                var CurrentOrder = context.SocietyVipOrders
                    .Include(order => order.PackageName)
                    .Include(order => order.OrderStatus)
                    .Where(order => order.ID == OrderID).SingleOrDefault();
                PayVipViewModel.SelectedRegisterPackageCount = CurrentOrder.Count;
                PayVipViewModel.ID = CurrentOrder.ID;
                PayVipViewModel.SelectedRegisterPackageIndex = PayVipViewModel.RegisterPackages.IndexOf(PayVipViewModel.RegisterPackages.Where(pack => pack.ID == CurrentOrder.PackageName.ID).SingleOrDefault());
                if (CurrentOrder.OrderStatus.ID == (int)Enums.OrderStatus.NotConfirmed || CurrentOrder.OrderStatus.ID == (int)Enums.OrderStatus.PreInvoice)
                    PayVipViewModel.AllowEdit = true;
                else
                    PayVipViewModel.AllowEdit = false;
            }
            else
            {
                PayVipViewModel.AllowEdit = true;
            }
            PayVipViewModel.MembershipVm = ProfileBinding();
            return PartialView("~/Views/Home/UserProfile/PayVip.cshtml", PayVipViewModel);
        }

        [UIAuthorizeUser]
        [AjaxValidateAntiForgeryTokenAttribute]
        [HttpPost]
        public ActionResult SubmitPayVip(string viewModel)
        {
            try
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                PayVipViewModel vm = jss.Deserialize<PayVipViewModel>(viewModel);
                string UniversityCardUrl = "";
                var context = new ApplicationDbContext();
                var trans = context.Database.BeginTransaction();
                var result = submitEditUser(vm.MembershipVm);
                if (!result.IsSuccess)
                    return ControllerHelper.ErrorResult(result.ResultMessage);
                try
                {

                    if (vm.RegisterPackages[vm.SelectedRegisterPackageIndex].ID == (int)Enums.VipPackageType.University_Student)
                    {
                        if (Request.Files.Count > 0)
                        {
                            var file = Request.Files[CommonHelper.ExposeProperty.GetPropertyInfo(vm, m => m.UniversityCardFileImage).Name];

                            if (file != null && file.ContentLength > 0)
                            {
                                UniversityCardUrl = FileUploadManagment.UploadFile("~/assets/img/Attach/Pay/", "", file, FileUploadManagment.AppFileType.Image);
                                if (UniversityCardUrl == "")
                                {
                                    return ControllerHelper.ErrorResult("فرمت یا حجم فایل صحیح نمی باشد");
                                }
                            }
                        }
                        else
                        {
                            return ControllerHelper.ErrorResult("فایل تصویر کارت دانشجویی انتخاب نشده است");
                        }
                    }
                    var UserOrder = context.SocietyVipOrders
                          .Include(order => order.CreatorUser)
                          .Where(order =>
                              order.CreatorUser.UserName == User.Identity.Name
                              &&
                              (order.OrderStatus.ID == (int)Enums.OrderStatus.PreInvoice || (order.OrderStatus.ID == (int)Enums.OrderStatus.NotConfirmed && order.ID == vm.ID))
                          ).SingleOrDefault();

                    if (UserOrder != null)
                    {
                        context.SocietyVipOrders.Remove(UserOrder);
                        var invoiceObj = context.Invoices.Where(invoice =>
                              invoice.TableName == "SocietyVipOrder"
                              &&
                              invoice.DocumentID == UserOrder.ID
                           ).SingleOrDefault();
                        if (invoiceObj != null)
                            context.Invoices.Remove(invoiceObj);
                        context.SaveChanges();
                    }
                    int PackID = vm.RegisterPackages[vm.SelectedRegisterPackageIndex].ID;
                    UserOrder = context.SocietyVipOrders.Add(new SocietyVipOrder()
                    {
                        PackageName = context.PackageNames
                                            .Where(dbpack => dbpack.ID == PackID).SingleOrDefault(),
                        Count = vm.SelectedRegisterPackageCount,
                        OrderStatus = context.OrderStatuses.Where(status => status.ID == (int)Enums.OrderStatus.PreInvoice).SingleOrDefault(),
                        UniversityCardUrl = UniversityCardUrl,
                        CreateDate = DateTime.Now,
                        CreatorUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault(),

                    });
                    context.SaveChanges();
                    trans.Commit();
                    return ControllerHelper.SuccessResult(UserOrder.ID.ToString());
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    return ControllerHelper.ErrorResult(ex.Message/*"بروز خطای سیستمی"*/);
                }

            }
            catch (Exception ex)
            {
                //return ControllerHelper.ErrorResult("بروز خطای سیستمی");
                return ControllerHelper.ErrorResult("بروز خطای سیستمی");
            }
        }
        #endregion
        #region Pay
        [UIAuthorizeUser]
        public ActionResult Pay(int OrderID, string Type)
        {
            var context = new ApplicationDbContext();
            var DbInvoiceTypes = context.InvoiceTypes
                 .Select(invoiceType => new { invoiceType.ID, invoiceType.Name });
            List<CheckBoxVm> PayTypes = new List<CheckBoxVm>();
            foreach (var item in DbInvoiceTypes)
            {
                PayTypes.Add(new CheckBoxVm()
                {
                    IsChecked = false,
                    Text = item.Name,
                    Value = item.ID.ToString(),
                });
            }
            decimal amount = 0;
            if (Type == "Order")
            {
                amount = context.OrderItems
                             .Include(orderItem => orderItem.ConferencePackage)
                             .Where(orderItem => orderItem.Order.ID == OrderID)
                             .Select(orderItem => orderItem.Count * orderItem.ConferencePackage.Price)
                             .DefaultIfEmpty(0)
                             .Sum();
            }
            if (Type == "SocietyVipOrder")
            {
                amount = context.SocietyVipOrders
                          .Include(order => order.PackageName)
                          .Where(order => order.ID == OrderID)
                          .Select(order => order.Count * order.PackageName.Price)
                          .DefaultIfEmpty(0)
                          .Sum();
            }
            return PartialView("~/Views/Home/UserProfile/Pay.cshtml", new InvoiceViewModel()
            {
                OrderID = OrderID,
                PayTypes = PayTypes,
                SelectedPayType = PayTypes[0].Value,
                amount = amount,
                merchantId = "A3E3",
                token = "",
                TableName = Type,
                ObjectState = ObjectState.Insert
            });
        }
        [UIAuthorizeUser]
        [HttpPost]
        [AjaxValidateAntiForgeryTokenAttribute]
        public ActionResult SubmitPay(string viewmodel)
        {

            try
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                InvoiceViewModel vm = jss.Deserialize<InvoiceViewModel>(viewmodel);
                try
                {
                    CommonHelper.DateAndTimes.GetGregorianDate(vm.Cash2BankPayDate);
                }
                catch (Exception ex)
                {
                    return ControllerHelper.ErrorResult("فرمت تاریخ واریزی صحیح نیست");
                }
                var context = new ApplicationDbContext();
                var trans = context.Database.BeginTransaction();
                try
                {
                    if (ModelState.IsValid)
                    {

                        var invoiceObj = context.Invoices.Where(
                           invoice =>
                           invoice.TableName == vm.TableName
                           && invoice.DocumentID == vm.OrderID
                           ).SingleOrDefault();
                        if (invoiceObj != null)
                        {
                            context.Invoices.Remove(invoiceObj);
                            context.SaveChanges();
                        }
                        if ((int)Enums.InvoiceType.CashBankPay == int.Parse(vm.SelectedPayType))
                        {

                            string Cash2BankPayFileUrl = "";
                            if (Request.Files.Count > 0)
                            {
                                var file = Request.Files[CommonHelper.ExposeProperty.GetPropertyInfo(vm, m => m.Cash2BankPayFile).Name];
                                if (file != null && file.ContentLength > 0)
                                {
                                    Cash2BankPayFileUrl = FileUploadManagment.UploadFile("~/assets/img/Attach/Pay/", "", file, FileUploadManagment.AppFileType.Image);
                                    if (Cash2BankPayFileUrl == "")
                                    {
                                        return ControllerHelper.ErrorResult("فرمت یا حجم فایل صحیح نمی باشد");
                                    }
                                }
                            }
                            if (vm.ObjectState == ObjectState.Insert)
                            {
                                if (Cash2BankPayFileUrl == "")
                                    return ControllerHelper.ErrorResult("فایل پیوست را انتخاب نمایید");
                            }
                            decimal Amount = 0;
                            if (vm.TableName == "Order")
                            {
                                var CurrentOrder = context.Orders.Where(
                                  order => order.ID == vm.OrderID
                                  &&
                                  (
                                    order.OrderStatus.ID == (int)Enums.OrderStatus.PreInvoice
                                    ||
                                    order.OrderStatus.ID == (int)Enums.OrderStatus.NotConfirmed
                                  )
                                  )
                               .SingleOrDefault();
                                CurrentOrder.OrderStatus = context.OrderStatuses.Where(orderStatus => orderStatus.ID == (int)Enums.OrderStatus.Checking).SingleOrDefault();
                                Amount = context.OrderItems
                                    .Include(orderItem => orderItem.ConferencePackage)
                                    .Where(orderItem => orderItem.Order.ID == vm.OrderID)
                                    .Select(orderItem => orderItem.Count * orderItem.ConferencePackage.Price)
                                    .DefaultIfEmpty(0)
                                    .Sum();
                            }
                            if (vm.TableName == "SocietyVipOrder")
                            {
                                var CurrentOrder = context.SocietyVipOrders.Where(
                                    order => order.ID == vm.OrderID
                                    &&
                                    (
                                      order.OrderStatus.ID == (int)Enums.OrderStatus.PreInvoice
                                      ||
                                      order.OrderStatus.ID == (int)Enums.OrderStatus.NotConfirmed
                                    )
                                    )
                                 .SingleOrDefault();
                                CurrentOrder.OrderStatus = context.OrderStatuses.Where(orderStatus => orderStatus.ID == (int)Enums.OrderStatus.Checking).SingleOrDefault();
                                Amount = context.SocietyVipOrders
                                          .Include(order => order.PackageName)
                                          .Where(order => order.ID == vm.OrderID)
                                          .Select(order => order.Count * order.PackageName.Price)
                                          .DefaultIfEmpty(0)
                                          .Sum();
                            }
                            var invoice = new Invoice()
                            {

                                Amount = Amount,
                                DocumentID = vm.OrderID,
                                TableName = vm.TableName,
                                BankName = vm.Cash2BankBankName,
                                Cash2BankPayFileUrl = Cash2BankPayFileUrl,
                                Cash2BankPayReceipt = vm.Cash2BankPayReceipt,
                                Cash2BankPayDate = CommonHelper.DateAndTimes.GetGregorianDate(vm.Cash2BankPayDate),
                                InvoiceType = context.InvoiceTypes.Where(invoiceType => invoiceType.ID == (int)Enums.InvoiceType.CashBankPay).SingleOrDefault(),
                                CreateDate = DateTime.Now,
                                CreatorUser = context.Users.Where(item => item.UserName == User.Identity.Name).SingleOrDefault()
                            };
                            context.Invoices.Add(invoice);
                            context.SaveChanges();
                            trans.Commit();
                            int cnt = 0;
                            if (vm.TableName == "Order")
                            {
                                cnt = context.Orders.Count(order => order.OrderStatus.ID == (int)Enums.OrderStatus.Checking);
                                EmailService.SendPayNotification("خانم", "قوامی", cnt.ToString(), "گزارش مالی همایش", "s.h.ghavami@gmail.com");
                            }
                            if (vm.TableName == "SocietyVipOrder")
                            {
                                cnt = context.SocietyVipOrders.Count(order => order.OrderStatus.ID == (int)Enums.OrderStatus.Checking);
                                EmailService.SendPayNotification("خانم", "قوامی", cnt.ToString(), "گزارش مالی انجمن", "s.h.ghavami@gmail.com");
                            }

                            return ControllerHelper.SuccessResult("پرداخت شما با موفقیت ثبت شد. برای ادامه مراحل لطفا منتظر تایید کارشناس باشید.");


                        }
                        else
                        {
                            string ReservationCode = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString();
                            decimal Amount = 0;
                            if (vm.TableName == "Order")
                            {
                                Amount = context.OrderItems
                                    .Include(orderItem => orderItem.ConferencePackage)
                                    .Where(orderItem => orderItem.Order.ID == vm.OrderID)
                                    .Select(orderItem => orderItem.Count * orderItem.ConferencePackage.Price)
                                    .DefaultIfEmpty(0)
                                    .Sum();
                            }
                            if (vm.TableName == "SocietyVipOrder")
                            {

                                Amount = context.SocietyVipOrders
                                          .Include(order => order.PackageName)
                                          .Where(order => order.ID == vm.OrderID)
                                          .Select(order => order.Count * order.PackageName.Price)
                                          .DefaultIfEmpty(0)
                                          .Sum();
                            }
                            var invoice = new Invoice()
                            {
                                Amount = Amount,
                                DocumentID = vm.OrderID,
                                ReservationCode = ReservationCode,
                                TableName = vm.TableName,
                                DigitalCode = vm.DigitalCode,
                                InvoiceType = context.InvoiceTypes.Where(invoiceType => invoiceType.ID == (int)Enums.InvoiceType.Online).SingleOrDefault(),
                                CreateDate = DateTime.Now,
                                CreatorUser = context.Users.Where(item => item.UserName == User.Identity.Name).SingleOrDefault()

                            };
                            context.Invoices.Add(invoice);
                            context.SaveChanges();
                            trans.Commit();

                            ir.shaparak.ikc.Token.Service1 service = new ir.shaparak.ikc.Token.Service1();
                            var tokenObj = service.MakeToken(((int)invoice.Amount).ToString(), "A3E3", invoice.ID.ToString(), ReservationCode, "", System.Configuration.ConfigurationManager.AppSettings["WebsiteURL"].ToString() + "/#/ResultBank?id=" + invoice.DocumentID.ToString() + "&tbl=Order", "");
                            vm.token = tokenObj.token;
                            vm.merchantId = "A3E3";
                            //vm.paymentId = ReservationCode;
                            //vm.revertURL = "http://www.ijcm.ir/Home/Index#/ReultBank?id=" + invoiceObj.ID.ToString();                            
                            return new JsonResult()
                            {
                                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                                Data = vm
                            };
                        }


                    }
                    else
                    {
                        return ControllerHelper.FormNotValid(ModelState);
                    }
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    return ControllerHelper.ErrorResult(ex.Message/*"بروز خطای سیستمی"*/);
                }

            }
            catch (Exception ex)
            {
                //return ControllerHelper.ErrorResult("بروز خطای سیستمی");
                return ControllerHelper.ErrorResult("بروز خطای سیستمی");
            }
        }
        const int OrderListPageSize = 20;
        OrderListViewModel OrderListFeedGenerator(int pageNumber)
        {
            string CurrentLang = Request.Cookies["Lang"].Value;
            var context = new ApplicationDbContext();
            int skipRows = (pageNumber - 1) * OrderListPageSize;
            var OrderViewModels = new List<OrderViewModel>();
            int CurrentConfID = 0;
            int totalRecords = 0;
            if (context.Conferences.Where(con => (bool)con.Enable && (bool)con.Visible).SingleOrDefault() != null)
            {
                CurrentConfID = context.Conferences.Where(con => (bool)con.Enable && (bool)con.Visible).SingleOrDefault().ID;
                totalRecords = context.Orders
                    .Include(order => order.CreatorUser)
                    .Where(order =>
                        order.CreatorUser.UserName == User.Identity.Name
                    )
                   .Count();
                //↑↑↑↑↑↑↑↑↑ ↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓    realated to each other

                var DbOrders = context.Orders
                    .Include(order => order.OrderItems)
                    .Include(order => order.OrderItems.Select(item => item.ConferencePackage))
                    //.Include(order => order.OrderItems.Select(item => item.ConferencePackage.PackageName.PackageNameTranslations.Select(trans => trans.Language)))
                    .Include(order => order.OrderStatus)
                    .Include(order => order.CreatorUser)
                    .Where(order =>
                        order.CreatorUser.UserName == User.Identity.Name
                     )
                    .Select(order => new
                    {
                        order.ID,
                        OrderItems = order.OrderItems
                                .Select(orderitem => new
                                {
                                    orderitem.Count,
                                    orderitem.ConferencePackage.PackageName.PackageNameTranslations.Where(trans => trans.Language.Value == CurrentLang).FirstOrDefault().Name,
                                    orderitem.ConferencePackage.Price
                                }),
                        OrderStatusName = order.OrderStatus.Name,
                        OrderStatusID = order.OrderStatus.ID,
                        order.CreateDate,
                        //PackagePrice = confCom.ConferencePackage.Price
                        // item.ConferencePackage.PackageName.PackageNameTranslations.Where(trans => trans.Language.Value == CurrentLang)
                    })
                    .OrderByDescending(order => order.CreateDate)
                    .Skip(skipRows).Take(OrderListPageSize)
                   .ToList();
                foreach (var order in DbOrders)
                {
                    List<OrderItemViewModel> OrderItems = new List<OrderItemViewModel>();
                    foreach (var item in order.OrderItems)
                    {
                        OrderItems.Add(new OrderItemViewModel()
                        {
                            Name = item.Name,
                            Price = (int)item.Price,
                            Count = (int)item.Count,
                        });
                    }
                    var SelectedInvoice = context.Invoices
                         .Include(invoice => invoice.InvoiceType)
                         .Where(invoice => invoice.TableName == "Order" && invoice.DocumentID == order.ID)
                         .Select(invoice => new
                         {
                             invoice.ID,
                             InvoiceTypeName = invoice.InvoiceType.Name,
                             InvoiceTypeID = invoice.InvoiceType.ID,
                             invoice.Cash2BankPayReceipt,
                             invoice.Cash2BankPayDate,
                             invoice.Cash2BankPayFileUrl,
                             invoice.DigitalCode,
                             invoice.ReservationCode,
                             invoice.Amount,
                         }).SingleOrDefault();

                    OrderViewModels.Add(new OrderViewModel()
                    {
                        ID = order.ID,
                        TableName = "Order",
                        Enable = (int)order.OrderStatusID != (int)Enums.OrderStatus.PreInvoice,
                        OrderItems = OrderItems,
                        Status = order.OrderStatusName,

                        Amount = order.OrderItems
                                    .Select(orderItem => orderItem.Count * (int)orderItem.Price)
                                    .DefaultIfEmpty(0)
                                    .Sum(),
                        CreateDateConverted = CommonHelper.DateAndTimes.GetPersianDate(order.CreateDate),
                        Invoice = SelectedInvoice != null ? new InvoiceViewModel()
                        {
                            ID = SelectedInvoice.ID,
                            InvoiceType = SelectedInvoice.InvoiceTypeName,
                            Cash2BankPayReceipt = SelectedInvoice.Cash2BankPayReceipt,
                            Cash2BankPayDate = CommonHelper.DateAndTimes.GetPersianDate(SelectedInvoice.Cash2BankPayDate),
                            Cash2BankPayFileUrl = SelectedInvoice.Cash2BankPayFileUrl,
                            DigitalCode = SelectedInvoice.DigitalCode,
                            ReservationCode = SelectedInvoice.ReservationCode,
                            Explain = SelectedInvoice.InvoiceTypeID == (int)Enums.InvoiceType.Online ?
                                                    (Resources.Resource.InvoiceViewModel_DigitalCode + ":" + SelectedInvoice.DigitalCode + "<br />" + Resources.Resource.InvoiceViewModel_ReservationCode + ":" + SelectedInvoice.ReservationCode)
                                                    :
                                                    (CommonHelper.DateAndTimes.GetPersianDate(SelectedInvoice.Cash2BankPayDate) + "<br />" + SelectedInvoice.Cash2BankPayReceipt),
                            amount = SelectedInvoice.Amount
                        } : new InvoiceViewModel()

                    });
                }
                var DbVipOrders = context.SocietyVipOrders
                .Include(order => order.PackageName.PackageNameTranslations)
                .Include(order => order.OrderStatus)
                .Include(order => order.CreatorUser)
                .Where(order =>
                    order.CreatorUser.UserName == User.Identity.Name
                 )
                .Select(order => new
                {
                    order.ID,
                    ItemName = order.PackageName.PackageNameTranslations
                            .Where(orderitem => orderitem.Language.Value == CurrentLang)
                            .Select(orderitem => new { ItemName = orderitem.Name }).FirstOrDefault().ItemName,
                    Count = order.Count,
                    Price = order.PackageName.Price,
                    OrderStatusName = order.OrderStatus.Name,
                    OrderStatusID = order.OrderStatus.ID,
                    order.CreateDate,
                })
                .OrderByDescending(order => order.CreateDate)
                .Skip(skipRows).Take(OrderListPageSize)
                .ToList();
                foreach (var order in DbVipOrders)
                {
                    List<OrderItemViewModel> OrderItems = new List<OrderItemViewModel>();
                    OrderItems.Add(new OrderItemViewModel()
                    {
                        Name = order.ItemName,
                        Price = (int)order.Price,
                        Count = (int)order.Count,
                    });
                    var SelectedInvoice = context.Invoices
                       .Include(invoice => invoice.InvoiceType)
                       .Where(invoice => invoice.TableName == "SocietyVipOrder" && invoice.DocumentID == order.ID)
                       .Select(invoice => new
                       {
                           invoice.ID,
                           InvoiceTypeName = invoice.InvoiceType.Name,
                           InvoiceTypeID = invoice.InvoiceType.ID,
                           invoice.Cash2BankPayReceipt,
                           invoice.Cash2BankPayDate,
                           invoice.Cash2BankPayFileUrl,
                           invoice.DigitalCode,
                           invoice.ReservationCode,
                           invoice.Amount,
                       }).SingleOrDefault();
                    OrderViewModels.Add(new OrderViewModel()
                    {
                        ID = order.ID,
                        TableName = "SocietyVipOrder",
                        Enable = (int)order.OrderStatusID != (int)Enums.OrderStatus.PreInvoice,
                        OrderItems = OrderItems,
                        Status = order.OrderStatusName,
                        Amount = (int)(order.Price * order.Count),
                        CreateDateConverted = CommonHelper.DateAndTimes.GetPersianDate(order.CreateDate),
                        Invoice = SelectedInvoice != null ? new InvoiceViewModel()
                        {
                            ID = SelectedInvoice.ID,
                            InvoiceType = SelectedInvoice.InvoiceTypeName,
                            Cash2BankPayReceipt = SelectedInvoice.Cash2BankPayReceipt,
                            Cash2BankPayDate = CommonHelper.DateAndTimes.GetPersianDate(SelectedInvoice.Cash2BankPayDate),
                            Cash2BankPayFileUrl = SelectedInvoice.Cash2BankPayFileUrl,
                            DigitalCode = SelectedInvoice.DigitalCode,
                            ReservationCode = SelectedInvoice.ReservationCode,
                            Explain = SelectedInvoice.InvoiceTypeID == (int)Enums.InvoiceType.Online ?
                                (Resources.Resource.InvoiceViewModel_DigitalCode + ":" + SelectedInvoice.DigitalCode + "<br />" + Resources.Resource.InvoiceViewModel_ReservationCode + ":" + SelectedInvoice.ReservationCode)
                                :
                                (CommonHelper.DateAndTimes.GetPersianDate(SelectedInvoice.Cash2BankPayDate) + "<br />" + SelectedInvoice.Cash2BankPayReceipt),
                            amount = SelectedInvoice.Amount
                        } : new InvoiceViewModel()

                    });
                }
                OrderViewModels = OrderViewModels.OrderByDescending(order => order.CreateDateConverted).ToList();
            }
            return new OrderListViewModel()
            {
                Orders = OrderViewModels,
                pageNumber = pageNumber,
                pageSize = OrderListPageSize,
                totalRecords = totalRecords
            };


        }
        [UIAuthorizeUser]
        public ActionResult OrderListFeed(int pageNumber)
        {
            return Json(OrderListFeedGenerator(pageNumber), JsonRequestBehavior.AllowGet);
        }
        [UIAuthorizeUser]
        public ActionResult OrderList(int pageNumber)
        {
            return PartialView("~/Views/Home/UserProfile/OrderList.cshtml", OrderListFeedGenerator(pageNumber));
        }
        [HttpPost]
        [AjaxValidateAntiForgeryTokenAttribute]
        [UIAuthorizeUser]
        public ActionResult DeleteOrder(string[] ids)
        {
            var context = new ApplicationDbContext();
            var dbContextTransaction = context.Database.BeginTransaction();
            try
            {
                foreach (var id in ids)
                {
                    var ID = int.Parse(id);
                    var UserOrder = context.Orders
                    .Include(order => order.CreatorUser)
                    .Include(order => order.OrderItems)
                    .Where(order =>
                        order.CreatorUser.UserName == User.Identity.Name
                        &&
                        order.OrderStatus.ID == (int)Enums.OrderStatus.PreInvoice
                        &&
                        order.ID == ID
                    ).SingleOrDefault();
                    if (UserOrder != null)
                    {
                        context.OrderItems.RemoveRange(UserOrder.OrderItems);
                        context.Orders.Remove(UserOrder);
                        var invoiceObj = context.Invoices.Where(invoice =>
                              invoice.TableName == "Order"
                              &&
                              invoice.DocumentID == UserOrder.ID
                           ).SingleOrDefault();
                        if (invoiceObj != null)
                            context.Invoices.Remove(invoiceObj);
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
        #endregion
        #endregion

    }
}