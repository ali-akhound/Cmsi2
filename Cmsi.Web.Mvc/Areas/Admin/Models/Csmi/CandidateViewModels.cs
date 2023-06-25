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

    #region Candidate
    public class CandidateViewModel
    {
        [Key]
        [ShowInGridview]
        [GridColumnWidth(70)]
        [Display(Name = "ID")]
        public int ID { get; set; }
        [Display(Name = "نام")]
        [ShowInGridview]
        [Required(ErrorMessage = "نام ضروری است")]
        [StringLength(150)]
        public string FirstName { get; set; }
        [Display(Name = "نام خانوادگی")]
        [ShowInGridview]
        [Required(ErrorMessage = "نام خانوادگی ضروری است")]
        [StringLength(150)]
        public string LastName { get; set; }

        [ShowInGridview]
        [Display(Name = "نوع")]
        public string SelectedCandidateTypeName { get; set; }
        [Display(Name = "عکس پرسنلی")]
        public HttpPostedFileBase PersonPic { get; set; }
        [Display(Name = "عکس پرسنلی")]
        [ShowInGridview]
        [ImageGridviewColumn]
        public string PersonPicUrl { get; set; }

        [Display(Name = "رزومه")]
        public HttpPostedFileBase Resume { get; set; }
        [Display(Name = "رزومه")]
        [ImageGridviewColumn]
        public string ResumeUrl { get; set; }

        [StringLength(250)]
        [Display(Name = "مرتبه علمی")]
        public string Degree { get; set; }
        [StringLength(250)]
        [Display(Name = "دانشگاه")]
        public string University { get; set; }
        [StringLength(250)]
        [Display(Name = "تخصص")]
        public string FieldOfStudy { get; set; }
        [StringLength(250)]
        [Display(Name = "توضیحات")]
        public string Explain { get; set; }
        [ShowInGridview]
        [Display(Name = "وضعیت")]
        public bool Enable { get; set; } = true;
        public int ElectionID { get; set; }
        public string ElectionName { get; set; }
        [ShowInGridview]
        [Display(Name = "تاریخ ثبت")]
        public string CreateDateConverted { get; set; }
        public ObjectState ObjectState { get; set; }
        [Display(Name = "نوع کاندید")]
        public List<DropDownVm> CandidateTypes { get; set; } = new List<DropDownVm>();
        public string SelectedCandidateTypeID { get; set; }
    }
    #endregion
}
