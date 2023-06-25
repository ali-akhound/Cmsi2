using AVA.UI.Helpers.Base;
using AVA.UI.Helpers.CustomAttribute;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace AVA.Web.Mvc.Models
{

    public class PackageNameViewModel
    {
        [Key]
        [ShowInGridview]
        [Display(Name = "ID")]
        public int ID { get; set; }
        [Display(Name = "نام")]
        [ShowInGridview]
        public string Name { get; set; }
        [Display(Name = "قیمت")]
        [ShowInGridview]
        public int Price { get; set; }
        [Display(Name = "نوع تعرفه")]
        [ShowInGridview]
        public string PackageTypeName { get; set; }
        [ShowInGridview]
        [Display(Name = "تاریخ ثبت")]
        public string CreateDateConverted { get; set; }
        [ShowInGridview]
        [Display(Name = "فعال")]
        public bool? Enable { get; set; }
        public string RefrenceUserID { get; set; }
        public ObjectState ObjectState { get; set; }
        public List<PackageNameTranslationViewModel> PackageNameTranslations { get; set; }
    }
    public class PackageNameTranslationViewModel
    {
        public int ID { get; set; }
        [StringLength(200)]
        [Display(Name = "نام")]
        [Required(ErrorMessage = "نام ضروری است")]
        public string Name { get; set; }
        [Display(Name = "زبان")]
        public string LanguageName { get; set; }
        public int LanguageID { get; set; }
        
    }
}
