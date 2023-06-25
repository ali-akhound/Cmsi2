using AVA.UI.Helpers.Base;
using AVA.UI.Helpers.CustomAttribute;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using static AVA.Web.Mvc.Models.BaseViewModel;

namespace AVA.Web.Mvc.Models
{

    public class ArticleViewModel
    {
        [Key]
        [ShowInGridview]
        [Display(Name = "ID")]
        [GridColumnWidth(50)]
        public int ID { get; set; }
        [StringLength(500)]
        [Display(Name = "ArticleViewModel_Title", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "ArticleViewModel_Title_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        [GridColumnWidth(350)]
        [ShowInGridview]
        [GridColumnEncodeHtml(false)]
        public string Title { get; set; }

        [Display(Name = "ArticleViewModel_RegisterationStatus", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [GridColumnWidth(150)]
        [ShowInGridview]
        public string RegisterationStatus { get; set; }
        [StringLength(500)]
        //[ShowInGridview]
        [Display(Name = "ArticleViewModel_EnglishTitle", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "ArticleViewModel_EnglishTitle_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        public string EnglishTitle { get; set; }
        [Display(Name = "ArticleViewModel_Summary", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "ArticleViewModel_Summary_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        public string Summary { get; set; }
        [StringLength(500)]
        [Display(Name = "ArticleViewModel_Keywords", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "ArticleViewModel_Keywords_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        public string Keywords { get; set; }
        [Display(Name = "ArticleViewModel_ArticleStatusName", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [ShowInGridview]
        [GridColumnWidth(130)]
        public string ArticleStatusName { get; set; }
        [Display(Name = "ArticleViewModel_ArticlePresentTypeName", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [ShowInGridview]
        [GridColumnWidth(130)]
        public string ArticlePresentTypeName { get; set; }
        public int ArticlePresentTypeID { get; set; }
        [ShowInGridview]
        [GridColumnWidth(130)]
        [Display(Name = "ArticleViewModel_FieldNames", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public string FieldNames { get; set; }

        [Display(Name = "ArticleViewModel_OwnerName", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public string OwnerName { get; set; }
        [Display(Name = "ArticleViewModel_OwnerCellphone", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public string OwnerCellphone { get; set; }
        [Display(Name = "ArticleViewModel_OwnerEmail", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [EmailAddress(ErrorMessageResourceName = "ArticleViewModel_OwnerEmail_EmailValidation", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        public string OwnerEmail { get; set; }


        [Display(Name = "ArticleViewModel_PosterUrl", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public string PosterUrl { get; set; }

        [Display(Name = "ArticleViewModel_PosterFile", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public HttpPostedFileBase PosterFile { get; set; }


        [Display(Name = "ArticleViewModel_PresentTime", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public string PresentTime { get; set; }
        public string PresentDate { get; set; }
        [Display(Name = "ArticleViewModel_PresentLocation", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public string PresentLocation { get; set; }


        [StringLength(500)]
        [GridColumnWidth(80)]
        [Display(Name = "ArticleViewModel_FileUrl", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "ArticleViewModel_FileUrl_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        [HyperLinkGridviewColumn]
        [ShowInGridview]
        [HyperLinkGridviewColumnText("FileUrlText")]
        public string FileUrl { get; set; }
        public string FileUrlText { get; set; }
        [StringLength(500)]
        [GridColumnWidth(80)]
        [Display(Name = "ArticleViewModel_FileWordUrl", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "ArticleViewModel_FileWordUrl_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        [HyperLinkGridviewColumn]
        [ShowInGridview]
        [HyperLinkGridviewColumnText("FileWordUrlText")]
        public string FileWordUrl { get; set; }
        public string FileWordUrlText { get; set; }
        [ShowInGridview]
        [Display(Name = "ArticleViewModel_RefereeState", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [GridColumnWidth(300)]
        [GridColumnEncodeHtml(false)]
        public string RefereeState { get; set; }
        [Display(Name = "ArticleViewModel_Visit", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [ShowInGridview]
        [GridColumnWidth(50)]
        public int? Visit { get; set; }
        [ShowInGridview]
        [GridColumnWidth(50)]
        [Display(Name = "ArticleViewModel_Enable", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public bool? Enable { get; set; }
        [ShowInGridview]
        [GridColumnWidth(100)]
        [Display(Name = "ArticleViewModel_Published", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public bool? Published { get; set; }
        [GridColumnWidth(150)]
        [ShowInGridview]
        [Display(Name = "ArticleViewModel_CreateDateConverted", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public string CreateDateConverted { get; set; }
        [StringLength(500)]
        public string ShortImageUrl { get; set; }
        [StringLength(500)]
        public string LongImageUrl { get; set; }
        public int ArticleStatusID { get; set; }
        public ObjectState ObjectState { get; set; }
        [Display(Name = "ArticleViewModel_Writers", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "ArticleViewModel_Writers_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]

        public List<WriterViewModel> Writers { get; set; }

        [Display(Name = "ArticleViewModel_Fields", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public List<CheckBoxVm> Fields { get; set; }
        public CheckBoxVm Field { get; set; }

        public WriterViewModel Writer { get; set; } = new WriterViewModel();
        [Display(Name = "ArticleViewModel_ArticleFileUrl", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public string ArticleFileUrl { get; set; }
        [Display(Name = "ArticleViewModel_ArticleFile", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "ArticleViewModel_ArticleFile_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        public HttpPostedFileBase ArticleFile { get; set; }
        [Display(Name = "ArticleViewModel_ArticleWordFile", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "ArticleViewModel_ArticleWordFile_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        public HttpPostedFileBase ArticleWordFile { get; set; }
        public List<DropDownVm> Languages { get; set; }
        public DropDownVm ArticleLanguage { get; set; }
        [Display(Name = "ArticleViewModel_ArticleLanguageSelectedID", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public string ArticleLanguageSelectedID { get; set; }
        [Display(Name = "ArticleViewModel_ArticleFields", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public DropDownVm ArticleFields { get; set; }
        public string ArticleFieldSelectedID { get; set; }
        [Display(Name = "ArticleViewModel_ConferenceName", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public string ConferenceName { get; set; }
        [Display(Name = "ArticleViewModel_ConferencePlace", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public string ConferencePlace { get; set; }
        public int ConferenceID { get; set; }
        [Display(Name = "ArticleViewModel_WriterNames", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [ShowInGridview]
        public string WriterNames { get; set; }

        public bool AllowEdit { get; set; } = true;
        public bool AllowLike { get; set; } = true;
        public int LikeCnt { get; set; } = 0;
        [Display(Name = "ArticleViewModel_RefereeArticleResults", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public List<RefereeArticleResultViewModel> RefereeArticleResults { get; set; }
        public RefereeArticleResultViewModel SingleRefereeArticleResultViewModel { get; set; }
    }
    public class RefereeArticleResultViewModel
    {
        public int RefereeArticleID { get; set; }
        [Display(Name = "نام داور")]
        public string RefereeName { get; set; }
        [Display(Name = "نام خانوادگی داور")]
        public string RefereeFamily { get; set; }
        [Display(Name = "نظر نهایی")]
        public string RefereeStatuse { get; set; }
        [Display(Name = "نحوه ارائه مقاله")]
        public string ArticlePresentType { get; set; }
        [Display(Name = "مقاله")]
        public int ArticleID { get; set; }
        public int RefereeID { get; set; }
        [Display(Name = "فرم داوری مقاله")]
        public List<RefereeQuestionAnswerViewModel> RefereeQuestionAnswers { get; set; }
        [Display(Name = "توضیحات")]
        public string Explain { get; set; }
        [Display(Name = "پیوست داوری")]
        public string AttachUrl { get; set; }
    }
    public class WriterViewModel
    {
        [Key]
        [ShowInGridview]
        [Display(Name = "ID")]
        [GridColumnWidth(50)]
        public int ID { get; set; }
        [ShowInGridview]
        [Display(Name = "WriterViewModel_FirstName", ResourceType = typeof(AVA.Web.Resources.Resource))]
        //[Required(ErrorMessageResourceName = "WriterViewModel_FirstName_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        [StringLength(50)]
        public string FirstName { get; set; }
        [ShowInGridview]
        [Display(Name = "WriterViewModel_LastName", ResourceType = typeof(AVA.Web.Resources.Resource))]
        //[Required(ErrorMessageResourceName = "WriterViewModel_LastName_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        [StringLength(50)]
        public string LastName { get; set; }
        [ShowInGridview]
        [Display(Name = "WriterViewModel_Email", ResourceType = typeof(AVA.Web.Resources.Resource))]
        //[Required(ErrorMessageResourceName = "WriterViewModel_Email_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        //[EmailAddress(ErrorMessageResourceName = "WriterViewModel_Email_EmailValidation", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        [StringLength(50)]
        public string Email { get; set; }
        [ShowInGridview]
        [StringLength(11)]
        [Display(Name = "WriterViewModel_Cellphone", ResourceType = typeof(AVA.Web.Resources.Resource))]
        //[Required(ErrorMessageResourceName = "WriterViewModel_Cellphone_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        public string Cellphone { get; set; }

        [Display(Name = "WriterViewModel_IsMainWriter", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public bool IsMainWriter { get; set; } = false;


    }

    //public class UIArticleViewModel
    //{
    //    [Key]
    //    [ShowInGridview]
    //    [Display(Name = "ID")]
    //    [GridColumnWidth(50)]
    //    public int ID { get; set; }
    //    [StringLength(500)]
    //    [Display(Name = "عنوان")]
    //    [Required(ErrorMessage = "عنوان ضروری است")]
    //    [GridColumnWidth(90)]
    //    [ShowInGridview]
    //    public string Title { get; set; }
    //    [StringLength(500)]
    //    [Display(Name = "عنوان انگلیسی")]
    //    [ShowInGridview]
    //    [Required(ErrorMessage = "عنوان انگلیسی ضروری است")]
    //    public string EnglishTitle { get; set; }
    //    [Display(Name = "خلاصه")]
    //    [Required(ErrorMessage = "خلاصه ضروری است")]
    //    public string Summary { get; set; }
    //    [StringLength(500)]
    //    [Display(Name = "کلمات کلیدی")]
    //    [Required(ErrorMessage = "کلمات کلیدی ضروری است")]
    //    public string Keywords { get; set; }

    //    [Display(Name = "نام فرستنده")]
    //    [ShowInGridview]
    //    public string OwnerName { get; set; }
    //    [Display(Name = "همراه")]
    //    public string OwnerCellphone { get; set; }
    //    [Display(Name = "ایمیل")]
    //    [EmailAddress(ErrorMessage = "فرمت ایمیل صحیح نیست")]
    //    public string OwnerEmail { get; set; }



    //    [Display(Name = "نویسندگان")]
    //    public List<UIWriterViewModel> Writers { get; set; }
    //    public UIWriterViewModel Writer { get; set; }
    //    [Display(Name = "آدرس فایل مقاله")]
    //    public string ArticleFileUrl { get; set; }
    //    [Display(Name = "پیوست فایل مقاله")]
    //    [Required(ErrorMessage = "پیوست فایل مقاله")]
    //    public HttpPostedFileBase ArticleFile { get; set; }

    //    public bool AllowEdit { get; set; } = true;
    //    [ShowInGridview]
    //    [Display(Name = "زمینه ها")]
    //    public List<CheckBoxVm> Fields { get; set; }
    //    public CheckBoxVm Field { get; set; }

    //    [ShowInGridview]
    //    public List<DropDownVm> Languages { get; set; }
    //    public DropDownVm ArticleLanguage { get; set; }
    //    [Display(Name = "زبان مقاله")]
    //    public string ArticleLanguageSelectedID { get; set; }

    //    [Display(Name = "بازدید")]
    //    [ShowInGridview]
    //    [GridColumnWidth(50)]
    //    public int? Visit { get; set; }
    //    [ShowInGridview]
    //    [GridColumnWidth(50)]
    //    [Display(Name = "فعال")]
    //    public bool? Enable { get; set; }
    //    [ShowInGridview]
    //    [Display(Name = "تاریخ ثبت")]
    //    public string CreateDateConverted { get; set; }

    //    [Display(Name = "زمینه")]
    //    public DropDownVm ArticleFields { get; set; }
    //    public string ArticleFieldSelectedID { get; set; }


    //    [Display(Name = "همایش")]
    //    public string ConferenceName { get; set; }
    //    [Display(Name = "نویسندگان")]
    //    public string WriterNames { get; set; }
    //    public string FieldNames { get; set; }
    //    public ObjectState ObjectState { get; set; }

    //}
    public class ArticleListViewModel
    {
        public List<ArticleListItemViewModel> DataModel { get; set; }
        public ArticleListItemViewModel SingleDataModel { get; set; }
        public int pageSize { get; set; }
        public int totalRecords { get; set; }
        public int pageNumber { get; set; }

    }
    public class ArticleFilterViewModel
    {
        [StringLength(500)]
        [Display(Name = "ArticleFilterViewModel_Title", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public string Title { get; set; }
        [StringLength(500)]
        [Display(Name = "ArticleFilterViewModel_EnglishTitle", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public string EnglishTitle { get; set; }
        [StringLength(1000)]
        [Display(Name = "ArticleFilterViewModel_Writers", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public string Writers { get; set; }
        [Display(Name = "ArticleFilterViewModel_Summary", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public string Summary { get; set; }
        [StringLength(500)]
        [Display(Name = "ArticleFilterViewModel_Keywords", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public string Keywords { get; set; }
        [Display(Name = "ArticleFilterViewModel_ArticleStatusName", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public string ArticleStatusName { get; set; }
        [Display(Name = "ArticleFilterViewModel_OwnerName", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public string OwnerName { get; set; }
        [Display(Name = "ArticleFilterViewModel_OwnerCellphone", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public string OwnerCellphone { get; set; }
        [Display(Name = "ArticleFilterViewModel_OwnerEmail", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public string OwnerEmail { get; set; }
        [Display(Name = "ArticleFilterViewModel_Enable", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public bool? Enable { get; set; }
        [Display(Name = "ArticleFilterViewModel_CreateDateFrom", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public string CreateDateFrom { get; set; }
        [Display(Name = "ArticleFilterViewModel_CreateDateTo", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public string CreateDateTo { get; set; }
        public int ConferenceID { get; set; }
        [Display(Name = "ArticleFilterViewModel_ArticleSelectedStatus", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public List<ArticleStatusDprViewModel> ArticleSelectedStatus { get; set; }
        public int ArticleSelectedStatusID { get; set; }
        [Display(Name = "ArticleFilterViewModel_ArticleCategories", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public List<ArticleCategoryDprViewModel> ArticleCategories { get; set; }
        [Display(Name = "ArticleFilterViewModel_ArticleSelectedCategory", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public int ArticleSelectedCategory { get; set; }

        public int IsReceive { get; set; }
        public ArticleSendArticleViewModel SendArticleViewModel { get; set; }
        public ArticleStatusTypeViewModel ArticleStatusTypeViewModel { get; set; }
    }
    public class ArticleStatusTypeViewModel
    {
        public int ConferenceID { get; set; }
        public int[] selectedArticleIDs { get; set; }
        [Display(Name = "نحوه ارائه مقاله")]
        public List<DropDownVm> ArticlePresentTypes { get; set; }
        public string SelectedPresentTypeID { get; set; }
        [Display(Name = "وضعیت داوری")]
        public List<DropDownVm> ArticleStatuses { get; set; }
        [Display(Name = "توضیحات")]
        public string Explain { get; set; }
        [Display(Name = "فایل پوستر")]
        public string PosterFileUrl { get; set; }
        [Display(Name = "زمان ارئه")]
        public string PresentTime { get; set; }
        [Display(Name = "تاریخ ارئه")]
        public string PresentDate { get; set; }
        [Display(Name = "مکان ارئه")]
        public string PresentLocation { get; set; }
        public HttpPostedFileBase PosterFile { get; set; }
        public string SelectedStatusID { get; set; }
    }
    public class ArticleSendArticleViewModel
    {
        public int ConferenceID { get; set; }
        public int[] selectedArticleIDs { get; set; }
        [Display(Name = "ArticleFilterViewModel_Referees", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public ICollection<RefereeSelective> Referees { get; set; }
        public ICollection<RefereeSelective> RefereesFeed { get; set; }
    }
    public class ArticleCategoryDprViewModel
    {
        [Key]
        public int ID { get; set; }
        [StringLength(50)]
        public string Name { get; set; }

    }

    #region Articles
    public class ArticleListItemViewModel
    {
        [Key]
        [ShowInGridview]
        [Display(Name = "ID")]
        [GridColumnWidth(50)]
        public int ID { get; set; }
        [Display(Name = "ArticleListItemViewModel_ArticleStatusName", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [ShowInGridview]
        public string ArticleStatusName { get; set; }
        [Display(Name = "ArticleListItemViewModel_ArticlePresentTypeName", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [ShowInGridview]
        public string ArticlePresentTypeName { get; set; }
        [Display(Name = "ArticleListItemViewModel_ArticlePresentTypeNameExpain", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [ShowInGridview]
        public string ArticlePresentTypeNameExpain { get; set; }
        [StringLength(500)]
        [Display(Name = "ArticleListItemViewModel_Title", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "ArticleListItemViewModel_Title_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        [GridColumnWidth(90)]
        [ShowInGridview]
        public string Title { get; set; }
        [ShowInGridview]
        [Display(Name = "ArticleListItemViewModel_CreateDateConverted", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public string CreateDateConverted { get; set; }
        [ShowInGridview]
        [Display(Name = "ArticleListItemViewModel_RefereeState", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [GridColumnWidth(300)]
        [GridColumnEncodeHtml(false)]
        public string RefereeState { get; set; }
        [ShowInGridview]
        [Display(Name = "ArticleListItemViewModel_Fields", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public string Fields { get; set; }

    }
    public class ArticleClientQuickFilterViewModel
    {
        [Display(Name = "ArticleClientQuickFilterViewModel_Title", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public string Title { get; set; } = "";
        [Display(Name = "ArticleClientQuickFilterViewModel_Writer", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public string Writer { get; set; } = "";
        [Display(Name = "ArticleClientQuickFilterViewModel_Keyword", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public string Keyword { get; set; } = "";
        [Display(Name = "ArticleClientQuickFilterViewModel_ConferenceName", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public string ConferenceName { get; set; } = "";
        [Display(Name = "ArticleClientQuickFilterViewModel_Fields", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public List<CheckBoxVm> Fields { get; set; }
        public List<ConferenceViewModel> Conferences { get; set; }
        public int ConferenceID { get; set; }
        public bool ShowPosterArticle { get; set; } = false;
    }
    public class ArticleClientListViewModel
    {
        public List<ArticleViewModel> Articles { get; set; }
        public ArticleViewModel SingleArticle { get; set; } = new ArticleViewModel();
        public ArticleClientQuickFilterViewModel FilterForm { get; set; } = new ArticleClientQuickFilterViewModel();
        public int pageSize { get; set; }
        public int totalRecords { get; set; }
        public int pageNumber { get; set; }
        public string ConferenceDate { get; set; }
        public string ConferenceName { get; set; }
    }
    #endregion
}
