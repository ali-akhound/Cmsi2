using AVA.UI.Helpers.Base;
using AVA.UI.Helpers.CustomAttribute;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace AVA.Web.Mvc.Models.Admin
{
    #region BaseViewModel
    public class BaseViewModel
    {
        #region Public ViewModels
        public class DropDownVm
        {
            public string Value { get; set; }
            public string Text { get; set; }

        }
        public class CheckBoxVm
        {
            public string Value { get; set; }
            public string Text { get; set; }
            public bool IsChecked { get; set; }

        }

        public class PersonViewModel
        {
            [Key]
            [ShowInGridview]
            [Display(Name = "ID")]
            [GridColumnWidth(50)]
            public int ID { get; set; }
            [ShowInGridview]
            [Display(Name = "نام")]
            [Required(ErrorMessage = "نام ضروری است")]
            [StringLength(50)]
            public string FirstName { get; set; }
            [ShowInGridview]
            [Display(Name = "نام خانوادگی")]
            [Required(ErrorMessage = "نام خانوادگی ضروری است")]
            [StringLength(50)]
            public string LastName { get; set; }
            [ShowInGridview]
            [Display(Name = "ایمیل")]
            [Required(ErrorMessage = "ایمیل ضروری است")]
            [EmailAddress(ErrorMessage = "فرمت ایمیل صحیح نیست")]
            [StringLength(50)]
            public string Email { get; set; }
            [ShowInGridview]
            [Display(Name = "موبایل")]
            [Required(ErrorMessage = "موبایل ضروری است")]
            [StringLength(11)]
            public string Cellphone { get; set; }



            [Display(Name = "تاریخ تولد")]
            [Required(ErrorMessage = "تاریخ تولد ضروری است")]
            public string BornDate { get; set; }
            [Required(ErrorMessage = "کد ملی ضروری است")]
            [Display(Name = "کدملی")]
            public string Melicode { get; set; }
            [Required(ErrorMessage = "نام دانشگاه ضروری است")]
            [Display(Name = "نام دانشگاه")]
            public string University { get; set; }
            [Required(ErrorMessage = "رشته تحصیلی ضروری است")]
            [Display(Name = "رشته تحصیلی")]
            public string FieldOfStudy { get; set; }
            [Required(ErrorMessage = "مدرک تحصیلی ضروری است")]
            [Display(Name = "مدرک تحصیلی")]
            public string Degree { get; set; }


            [Display(Name = "پیوست کارت دانشجویی")]
            public string UniversityCardFileImageUrl { get; set; }
            [Display(Name = "پیوست رسید پرداخت")]
            public string PayReceiptFileImageUrl { get; set; }
            [Display(Name = "پیوست تصویر پرسنلی")]
            public string PersonalFileImageUrl { get; set; }


            [Display(Name = "پیوست کارت دانشجویی")]
            public HttpPostedFileBase UniversityCardFileImage { get; set; }
            [Display(Name = "پیوست رسید پرداخت")]
            public HttpPostedFileBase PayReceiptFileImage { get; set; }
            [Display(Name = "پیوست تصویر پرسنلی")]
            public HttpPostedFileBase PersonalFileImage { get; set; }
            public bool AllowEdit { get; set; }
        }
        #endregion
    }
    #endregion
}
