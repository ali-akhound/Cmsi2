using AVA.UI.Helpers.Base;
using AVA.UI.Helpers.CustomAttribute;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace AVA.Web.Mvc.Models
{

    public class RefereeAnswerViewModel
    {
        [Key]
        [ShowInGridview]
        [Display(Name = "ID")]
        public int ID { get; set; }
        public int QuestionID { get; set; }
        [StringLength(1000)]
        [Display(Name = "متن جواب")]
        public string Answer { get; set; }
        [Display(Name = "اولویت")]
        [ShowInGridview]
        public int Priority { get; set; }
        [ShowInGridview]
        [Display(Name = "فعال")]
        public bool? Enable { get; set; }
        [ShowInGridview]
        [Display(Name = "تاریخ ثبت")]
        public string CreateDateConverted { get; set; }
        public ObjectState ObjectState { get; set; }




    }
}
