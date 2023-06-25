using AVA.UI.Helpers.Base;
using AVA.UI.Helpers.CustomAttribute;
using AVA.Web.Mvc.Areas.Admin.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace AVA.Web.Mvc.Models.Admin
{

    #region News
    public class NewsViewModel
    {
        [Key]
        [ShowInGridview]
        [GridColumnWidth(70)]
        [Display(Name = "ID")]
        public int ID { get; set; }
        [Display(Name = "عنوان")]
        [ShowInGridview]
        [Required(ErrorMessage = "عنوان ضروری است")]
        [StringLength(250)]
        public string Subject { get; set; }

        [Display(Name = "خلاصه خبر")]
        [Required(ErrorMessage = "خلاصه خبر ضروری است")]
        [StringLength(800)]
        public string Summery { get; set; }

        [Display(Name = "شرح خبر")]
        [Required(ErrorMessage = "شرح خبر ضروری است")]
        [AllowHtml]
        public string Explain { get; set; }

        [Display(Name = "عکس بزرگ")]
        public HttpPostedFileBase LongImage { get; set; }

        [Display(Name = "عکس بزرگ")]        
        public string LongImageUrl { get; set; }

        [Display(Name = "عکس کوچک")]
        public HttpPostedFileBase SmallImage { get; set; }

        [Display(Name = "عکس کوچک")]
        [ShowInGridview]
        [ImageGridviewColumn]
        public string SmallImageUrl { get; set; }

        [ShowInGridview]
        [Display(Name = "تاریخ ثبت")]
        public string CreateDateConverted { get; set; }
        [ShowInGridview]
        [Display(Name = "تاریخ")]
        [Required(ErrorMessage = "تاریخ ضروری است")]
        public string DateConverted { get; set; }
        public string MonthName { get; set; }
        public string Day { get; set; }
        public string Year { get; set; }
        [ShowInGridview]
        [Display(Name = "فعال")]
        public bool? Enable { get; set; }
        [ShowInGridview]
        [Display(Name = "بازدید")]
        public int? Visit { get; set; }
        public ObjectState ObjectState { get; set; }
        [StringLength(200)]
        public string Image { get; set; }
        [StringLength(500)]
        [Display(Name = "کلمات کلیدی")]
        public string Keywords { get; set; }
        [StringLength(500)]
        [Display(Name = "توضیحات")]
        public string Description { get; set; }
        public int? Position { get; set; }
        public bool? IsEspecial { get; set; }
        public byte? Type { get; set; }
        [ShowInGridview]
        [Display(Name = "زبان")]
        [GridColumnWidth(100)]
        public string LanguageName { get; set; }
        public string LanguageID { get; set; }
        public List<DropDownVm> Languages { get; set; }
    }
    public class NewsGroupListViewModel
    {
        public string Year { get; set; }
        public int Count { get; set; }
    }
    public class NewsListViewModel
    {
        public List<NewsViewModel> DataModel { get; set; }
        public List<NewsGroupListViewModel> GroupList { get; set; }
        public NewsGroupListViewModel SingleGroup { get; set; } = new NewsGroupListViewModel();
        public int pageSize { get; set; }
        public int totalRecords { get; set; }
        public int pageNumber { get; set; }

    }
    #endregion
}
