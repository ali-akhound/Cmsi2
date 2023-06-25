using AVA.UI.Helpers.Base;
using AVA.UI.Helpers.CustomAttribute;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace AVA.Web.Mvc.Models
{

    public class ConferenceViewModel
    {
        [Key]
        [ShowInGridview]
        [Display(Name = "ID")]
        public int ID { get; set; }
        [StringLength(500)]
        [ShowInGridview]
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "عنوان ضروری است")]
        public string Title { get; set; }
        [Display(Name = "عنوان انگلیسی")]
        [Required(ErrorMessage = "عنوان انگلیسی ضروری است")]
        public string EnglishTitle { get; set; }
        [ShowInGridview]
        [Display(Name = "تاریخ شروع ارسال")]
        [Required(ErrorMessage = "تاریخ شروع ضروری است")]
        public string SendStartDateConverted { get; set; }
        [ShowInGridview]
        [Display(Name = "تاریخ پایان ارسال")]
        [Required(ErrorMessage = "تاریخ پایان ضروری است")]
        public string SendEndDateConverted { get; set; }
        [ShowInGridview]
        [Display(Name = "تاریخ برگزاری")]
        [Required(ErrorMessage = "تاریخ برگزاری ضروری است")]
        public string EventDateConverted { get; set; }
        [Display(Name = "مبلغ شرکت در همایش")]
        [Required(ErrorMessage = "مبلغ شرکت در همایش ضروری است")]
        public decimal PublishPrice { get; set; }
        public ObjectState ObjectState { get; set; }
        [ShowInGridview]
        [Display(Name = "تعداد مقالات")]
        public int ArticleCount { get; set; }
        [ShowInGridview]
        [Display(Name = "زمینه ها")]
        public string Fields { get; set; }

        [Display(Name = "پوستر همایش")]
        public string PosterImageUrl { get; set; }

        //[Display(Name = "SocietyMemberViewModel_PersonalImage", ResourceType = typeof(AVA.Web.Resources.Resource))]
        //[Required(ErrorMessageResourceName = "SocietyMemberViewModel_PersonalImage_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        [Display(Name = "پوستر همایش")]
        public HttpPostedFileBase PosterImage { get; set; }

        [Display(Name = "فایل پیوست همایش")]
        public string AttachFileUrl { get; set; }

        //[Display(Name = "SocietyMemberViewModel_PersonalImage", ResourceType = typeof(AVA.Web.Resources.Resource))]
        //[Required(ErrorMessageResourceName = "SocietyMemberViewModel_PersonalImage_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        [Display(Name = "فایل پیوست همایش")]
        public HttpPostedFileBase AttachFile { get; set; }

        [Display(Name = "فایل پیوست آمایش مقاله")]
        public string AttachAmayeshFileUrl { get; set; }
        [Display(Name = "فایل پیوست آمایش مقاله")]
        public HttpPostedFileBase AttachAmayeshFile { get; set; }

        [Display(Name = "فایل پیوست کمیته علمی")]
        public string AttachScientificCommitteeFileUrl { get; set; }
        [Display(Name = "فایل پیوست کمیته علمی")]
        public HttpPostedFileBase AttachScientificCommitteeFile { get; set; }

        [Display(Name = "فایل پیوست کمیته اجرایی همایش")]
        public string AttachExecutiveCommitteeFileUrl { get; set; }
        [Display(Name = "فایل پیوست کمیته اجرایی همایش")]
        public HttpPostedFileBase AttachExecutiveCommitteeFile { get; set; }


        [Display(Name = "راهنمای ارائه سخنرانی")]
        public string AttachPresentationHelpFileUrl { get; set; }
        [Display(Name = "راهنمای ارائه سخنرانی")]
        public HttpPostedFileBase AttachPresentationHelpFile { get; set; }
        [Display(Name = "فرمت پاورپوینت")]
        public string AttachPresentationPowerpointFileUrl { get; set; }
        [Display(Name = "فرمت پاورپوینت")]
        public HttpPostedFileBase AttachPresentationPowerpointFile { get; set; }
        [Display(Name = "فرمت پوستر")]
        public string AttachPosterTemplateFileUrl { get; set; }
        [Display(Name = "فرمت پوستر")]
        public HttpPostedFileBase AttachPosterTemplateFile { get; set; }

        [Display(Name = "برنامه سخنرانی همایش گروه فیزیک")]
        public string AttachPhysicsPresentationProgramFileUrl { get; set; }
        [Display(Name = "برنامه سخنرانی همایش گروه فیزیک")]
        public HttpPostedFileBase AttachPhysicsPresentationProgramFile { get; set; }

        [Display(Name = "برنامه سخنرانی همایش گروه شیمی")]
        public string AttachChemistryPresentationProgramFileUrl { get; set; }
        [Display(Name = "برنامه سخنرانی همایش گروه شیمی")]
        public HttpPostedFileBase AttachChemistryPresentationProgramFile { get; set; }

        [Display(Name = "برنامه سخنرانی همایش گروه زمین شناسی")]
        public string AttachGeologyPresentationProgramFileUrl { get; set; }
        [Display(Name = "برنامه سخنرانی همایش گروه زمین شناسی")]
        public HttpPostedFileBase AttachGeologyPresentationProgramFile { get; set; }
        [Display(Name = "برنامه افتتاحیه")]
        public string AttachOpeningPlanFileUrl { get; set; }
        [Display(Name = "برنامه افتتاحیه")]
        public HttpPostedFileBase AttachOpeningPlanFile { get; set; }

        [Display(Name = "راهنمای شرکت در همایش")]
        public string AttachAttendingHelpFileUrl { get; set; }
        [Display(Name = "راهنمای شرکت در همایش")]
        public HttpPostedFileBase AttachAttendingHelpFile { get; set; }

        [Display(Name = "پیوست مقالات همایش")]
        public string AttachTotalArticlesFileUrl { get; set; }
        [Display(Name = "پیوست مقالات همایش")]
        public HttpPostedFileBase AttachTotalArticlesFile { get; set; }


        [Display(Name = "همراه")]
        public string MobileTel { get; set; }

        /***********************************************************************************/
        [Display(Name = "آدرس تلگرام")]
        public string TelegramUrl { get; set; }
        [Display(Name = "توضیحات")]
        [AllowHtml]
        public string Explain { get; set; }
        [Display(Name = "مکان")]
        public string Place { get; set; }
        [ShowInGridview]
        [Display(Name = "فعال")]
        public bool? Enable { get; set; }
        [ShowInGridview]
        [Display(Name = "نمایش")]
        public bool? Visible { get; set; }
        [ShowInGridview]
        [Display(Name = "تاریخ ثبت")]
        public string CreateDateConverted { get; set; }
        public List<AdminConferencePackageViewModel> AdminConferencePackage { get; set; } = new List<AdminConferencePackageViewModel>();
        public string EventDate { get; set; } 
        public string Year { get; set; }
    }
    public class ConferenceListViewModel
    {
        public List<ConferenceViewModel> DataModel { get; set; }
        public List<ConferenceViewModel> GroupList { get; set; }
        public ConferenceViewModel SingleGroup { get; set; } = new ConferenceViewModel();

    }
    public class CurrentFutureConferenceListViewModel
    {
        public List<ConferenceViewModel> FutureConferences { get; set; }
        public ConferenceViewModel CurrentConference { get; set; } = new ConferenceViewModel();
        public ConferenceViewModel SingleConference { get; set; } = new ConferenceViewModel();

    }

}
