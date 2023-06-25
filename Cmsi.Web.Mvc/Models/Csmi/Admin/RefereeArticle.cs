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

    public class RefereeArticleViewModel
    {
        [Key]
        [ShowInGridview]
        [Display(Name = "ID")]
        [GridColumnWidth(50)]
        public int ID { get; set; }
        [StringLength(500)]
        [Display(Name = "عنوان")]
        [GridColumnWidth(90)]
        [ShowInGridview]
        public string Title { get; set; }
        [StringLength(500)]
        [Display(Name = "عنوان انگلیسی")]
        [ShowInGridview]
        public string EnglishTitle { get; set; }
        [StringLength(1000)]
        [Display(Name = "نویسندگان")]
        public string Writers { get; set; }
        [Display(Name = "خلاصه")]
        public string Summary { get; set; }
        [StringLength(500)]
        [Display(Name = "کلمات کلیدی")]
        public string Keywords { get; set; }
        [Display(Name = "وضعیت مقاله")]
        [ShowInGridview]
        public string ArticleStatusName { get; set; }
        [Display(Name = "نام فرستنده")]
        [ShowInGridview]
        public string OwnerName { get; set; }
        [Display(Name = "همراه")]
        public string OwnerCellphone { get; set; }
        [Display(Name = "ایمیل")]
        public string OwnerEmail { get; set; }
        [StringLength(500)]
        [GridColumnWidth(80)]
        [Display(Name = "فایل مقاله")]
        [HyperLinkGridviewColumn]
        [ShowInGridview]
        [HyperLinkGridviewColumnText("FileUrlText")]
        public string FileUrl { get; set; }
        public string FileUrlText { get; set; }
        [ShowInGridview]
        [Display(Name = "زمینه ها")]
        public string Fields { get; set; }
        [Display(Name = "بازدید")]
        //[ShowInGridview]
        //[GridColumnWidth(50)]
        public int? Visit { get; set; }
        [Display(Name = "ارسال به داوری")]
        [ShowInGridview]
        [GridColumnWidth(150)]
        public int? RefereeSendTimes { get; set; } = 0;
        [ShowInGridview]
        [GridColumnWidth(50)]
        [Display(Name = "فعال")]
        public bool? Enable { get; set; }
        [ShowInGridview]
        [Display(Name = "تاریخ ثبت")]
        public string CreateDateConverted { get; set; }


        [Display(Name = "نظر نهایی")]
        public List<RefereeStatusViewModel> RefereeStatuses { get; set; }
        [Display(Name = "نحوه ارائه مقاله")]
        public List<ArticlePresentTypeViewModel> ArticlePresentTypes { get; set; }
        [Display(Name = "مقاله")]
        public int ArticleID { get; set; }
        public int RefereeID { get; set; }
        [Display(Name = "فرم داوری مقاله")]
        public List<RefereeQuestionAnswerViewModel> RefereeQuestionAnswers { get; set; }
        [Display(Name = "توضیحات")]
        public string Explain { get; set; }
        [Display(Name = "پیوست داوری")]
        public string AttachUrl { get; set; }
        [Display(Name = "فایل پیوست داوری")]
        public HttpPostedFileBase Attach { get; set; }
        public ObjectState ObjectState { get; set; }
        public bool IsRead { get; set; }
        public int SelectedPresentTypeID { get; set; }
        public int SelectedRefereeStatuseID { get; set; }

    }
    public class RefereeQuestionAnswerViewModel
    {
        public List<AnswerViewModel> Answers { get; set; }
        public string RefereeQuestionText { get; set; }
        public int RefereeQuestionID { get; set; }
        public int SelectedAnswerID { get; set; }
    }
    public class AnswerViewModel
    {
        public int AnswerID { get; set; }
        public string Answer { get; set; }
    }
}
