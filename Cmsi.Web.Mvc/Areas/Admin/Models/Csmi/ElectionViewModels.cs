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

    #region Election
    public class ElectionViewModel
    {
        [Key]
        [ShowInGridview]
        [GridColumnWidth(70)]
        [Display(Name = "ID")]
        public int ID { get; set; }
        [Display(Name = "عنوان")]
        [ShowInGridview]
        [Required(ErrorMessage = "عنوان ضروری است")]
        [StringLength(150)]
        public string Name { get; set; }
        [ShowInGridview]
        [Display(Name = "تاریخ شروع")]
        [Required(ErrorMessage = "تاریخ شروع ضروری است")]
        public string StartDateConverted { get; set; }
        [ShowInGridview]
        [Display(Name = "تاریخ پایان")]
        [Required(ErrorMessage = "تاریخ پایان ضروری است")]
        public string EndDateConverted { get; set; }
        [Display(Name = "عکس پوستر")]
        public HttpPostedFileBase ElectionPoster { get; set; }
        [Display(Name = "عکس پوستر")]
        public string ElectionPosterUrl { get; set; }
        [Display(Name = "پیوست")]
        public HttpPostedFileBase ElectionAttach { get; set; }
        [Display(Name = "پیوست")]
        public string ElectionAttachUrl { get; set; }
        [ShowInGridview]
        [Display(Name = "وضعیت")]
        public bool Enable { get; set; } = true;
        [ShowInGridview]
        [Display(Name = "تاریخ ثبت")]
        public string CreateDateConverted { get; set; }
        public ObjectState ObjectState { get; set; }
    }
    public class ElectionResultViewModel
    {
        [Key]
        [ShowInGridview]
        [GridColumnWidth(70)]
        [Display(Name = "ID")]
        public int ID { get; set; }
        public string ElectionName { get; set; }
        public string CandidCategories { get; set; }
        public string InspectorCategories { get; set; }
        public string CandidVoteCount { get; set; }
        public string InspectorVoteCount { get; set; }

    }
    public class ElectionVoterDiagramViewModel
    {
        [Key]
        [ShowInGridview]
        [GridColumnWidth(70)]
        [Display(Name = "ID")]
        public int ID { get; set; }
        public string ElectionName { get; set; }
        public string DateCategories { get; set; }
        public string VoteCount { get; set; }

    }
    public class ElectionVoterViewModel
    {
        [ShowInGridview]
        [Display(Name = "نام کاربری")]
        public string UserName { get; set; }
        [ShowInGridview]
        [Display(Name = "نام")]
        public string FirstName { get; set; }
        [ShowInGridview]
        [Display(Name = "نام خانوادگی")]
        public string LastName { get; set; }
        [ShowInGridview]
        [Display(Name = "ایمیل")]
        public string Email { get; set; }
        //[ShowInGridview]
        [Display(Name = "تاریخ ثبت")]
        public string CreateDateConverted { get; set; }

    }
    #endregion
}
