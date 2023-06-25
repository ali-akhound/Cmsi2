using AVA.UI.Helpers.Base;
using AVA.UI.Helpers.CustomAttribute;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace AVA.Web.Mvc.Models
{

    public class RefereeQuestionViewModel
    {
        [Key]
        [ShowInGridview]
        [Display(Name = "ID")]
        public int ID { get; set; }
        [StringLength(1000)]
        [ShowInGridview]
        [Display(Name = "متن سوال")]
        [Required(ErrorMessage = "متن سوال ضروری است")]
        public string Question { get; set; }
        [Display(Name = "اولویت")]
        [ShowInGridview]
        [Required(ErrorMessage = "اولویت ضروری است")]
        public int Priority { get; set; }
        [ShowInGridview]
        [Display(Name = "فعال")]
        public bool? Enable { get; set; }
        [ShowInGridview]
        [Display(Name = "تاریخ ثبت")]
        public string CreateDateConverted { get; set; }
        public ObjectState ObjectState { get; set; }
        public List<RefereeAnswerViewModel> RefereeAnswers { get; set; }
        public RefereeAnswerViewModel RefereeAnswer { get; set; }
        public bool ActiveEditMode { get; set; }



    }
}
