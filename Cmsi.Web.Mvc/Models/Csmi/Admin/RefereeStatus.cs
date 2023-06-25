using AVA.UI.Helpers.Base;
using AVA.UI.Helpers.CustomAttribute;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace AVA.Web.Mvc.Models
{

    public class RefereeStatusViewModel
    {
        [Key]
        [ShowInGridview]
        [Display(Name = "ID")]
        public int ID { get; set; }
        [StringLength(50)]
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "عنوان ضروری است")]
        [ShowInGridview]
        public string Name { get; set; }
        [ShowInGridview]
        [Display(Name = "فعال")]
        public bool? Enable { get; set; }
        [ShowInGridview]
        [Display(Name = "تاریخ ثبت")]
        public string CreateDateConverted { get; set; }
        public ObjectState ObjectState { get; set; }



    }
}
