using AVA.UI.Helpers.Base;
using AVA.UI.Helpers.CustomAttribute;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace AVA.Web.Mvc.Models
{
    #region News
    public class NewsViewModel
    {
        [Key]
        [ShowInGridview]
        [Display(Name = "ID")]
        public int ID { get; set; }
        //[Display(Name = "NewsViewModel_Subject", ResourceType = typeof(AVA.Web.Resources.Resource))]
        //[Required(ErrorMessageResourceName = "NewsViewModel_Subject_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        [Display(Name = "عنوان")]
        [ShowInGridview]
        [Required(ErrorMessage = "عنوان ضروری است")]
        [StringLength(250)]
        public string Subject { get; set; }

        [Display(Name = "خلاصه خبر")]
        [Required(ErrorMessage = "خلاصه خبر ضروری است")]
        //[Display(Name = "NewsViewModel_Summery", ResourceType = typeof(AVA.Web.Resources.Resource))]
        //[Required(ErrorMessageResourceName = "NewsViewModel_Summery_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        [StringLength(800)]
        public string Summery { get; set; }

        [Display(Name = "شرح خبر")]
        [Required(ErrorMessage = "شرح خبر ضروری است")]
        //[Display(Name = "NewsViewModel_Explain", ResourceType = typeof(AVA.Web.Resources.Resource))]
        //[Required(ErrorMessageResourceName = "NewsViewModel_Explain_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        public string Explain { get; set; }

        [ShowInGridview]
        [Display(Name = "تاریخ ثبت")]
        //[Display(Name = "NewsViewModel_CreateDateConverted", ResourceType = typeof(AVA.Web.Resources.Resource))]
        //[Required(ErrorMessageResourceName = "NewsViewModel_CreateDateConverted_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        public string CreateDateConverted { get; set; }
        [ShowInGridview]
        [Display(Name = "تاریخ")]
        [Required(ErrorMessage = "تاریخ ضروری است")]
        //[Display(Name = "NewsViewModel_DateConverted", ResourceType = typeof(AVA.Web.Resources.Resource))]
        //[Required(ErrorMessageResourceName = "NewsViewModel_DateConverted_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        public string DateConverted { get; set; }
        [ShowInGridview]
        [Display(Name = "فعال")]
        //[Display(Name = "NewsViewModel_Enable", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public bool? Enable { get; set; }
        [ShowInGridview]
        [Display(Name = "بازدید")]
        //[Display(Name = "NewsViewModel_Visit", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public int? Visit { get; set; }
        public ObjectState ObjectState { get; set; }
        [StringLength(150)]
        public string Image { get; set; }
        [StringLength(500)]
        [Display(Name = "کلمات کلیدی")]
        //[Display(Name = "NewsViewModel_Keywords", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public string Keywords { get; set; }
        [StringLength(500)]
        [Display(Name = "توضیحات")]
        //[Display(Name = "NewsViewModel_Description", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public string Description { get; set; }
        public int? Position { get; set; }
        public bool? IsEspecial { get; set; }
        public byte? Type { get; set; }
    }
    public class NewsListViewModel
    {
        public List<NewsViewModel> News { get; set; }
        public int pageSize { get; set; }
        public int totalRecords { get; set; }
        public int pageNumber { get; set; }

    }

    #endregion
}
