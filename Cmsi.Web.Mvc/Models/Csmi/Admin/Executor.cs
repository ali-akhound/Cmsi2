using AVA.UI.Helpers.Base;
using AVA.UI.Helpers.CustomAttribute;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace AVA.Web.Mvc.Models
{

    public class ExecutorViewModel
    {
        [Key]
        [ShowInGridview]
        [Display(Name = "ID")]
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
        //[ShowInGridview]
        //[Display(Name = "نام کاربری")]
        //[Required(ErrorMessage = "نام کاربری ضروری است")]
        //[StringLength(50)]
        //public string UserName { get; set; }
        [ShowInGridview]
        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "ایمیل ضروری است")]
        [StringLength(50)]
        public string Email { get; set; }
        [ShowInGridview]
        [Display(Name = "همراه")]
        public string PhoneNumber { get; set; }
        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "رمز عبور ضروری است")]
        public string Password { get; set; }
        [StringLength(150)]
        public string ImageUrl { get; set; }
        public ObjectState ObjectState { get; set; }
        [ShowInGridview]
        [Display(Name = "تاریخ ثبت")]
        public string CreateDateConverted { get; set; }
        [ShowInGridview]
        [Display(Name = "فعال")]
        public bool? Enable { get; set; }
        public string RefrenceUserID { get; set; }
    }
}
