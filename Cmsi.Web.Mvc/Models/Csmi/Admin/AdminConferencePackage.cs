using AVA.UI.Helpers.Base;
using AVA.UI.Helpers.CustomAttribute;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace AVA.Web.Mvc.Models
{

    public class AdminConferencePackageViewModel
    {
        [Key]
        [ShowInGridview]
        [Display(Name = "ID")]
        public int ID { get; set; }
        [StringLength(500)]
        [ShowInGridview]
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "عنوان ضروری است")]
        public string Name { get; set; }
        [ShowInGridview]
        [Display(Name = "تعرفه")]
        [Required(ErrorMessage = "تعرفه ضروری است")]
        public decimal Price { get; set; }

    }
}
