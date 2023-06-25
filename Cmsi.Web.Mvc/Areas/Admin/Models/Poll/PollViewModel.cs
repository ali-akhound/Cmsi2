using AVA.UI.Helpers.Base;
using AVA.UI.Helpers.CustomAttribute;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using static AVA.Web.Mvc.Models.Admin.BaseViewModel;

namespace AVA.Web.Mvc.Models.Admin
{
    #region PollViewModel
    public class PollViewModel
    {
        [Key]
        [ShowInGridview]
        [Display(Name = "ID")]
        [GridColumnWidth(100)]
        public int ID { get; set; }
        public List<PollQuestionViewModel> Questions { get; set; }
        public PollQuestionViewModel SingleQuestion { get; set; } = new PollQuestionViewModel();
        public string LanguageName { get; set; }
        public string LanguageID { get; set; }
        public List<DropDownVm> Languages { get; set; }
        public bool? Enable { get; set; }
    }
    public class PollQuestionViewModel
    {
        [Key]
        [ShowInGridview]
        [Display(Name = "ID")]
        [GridColumnWidth(100)]
        public int ID { get; set; }
        [StringLength(1000)]
        [ShowInGridview]
        [Display(Name = "سوال")]
        public string Question { get; set; }
        public List<PollAnswerViewModel> Answers { get; set; }
        public PollAnswerViewModel SingleAnswer { get; set; } = new PollAnswerViewModel();
        public int SelectedAnswerID { get; set; }
        [ShowInGridview]
        [Display(Name = "نتیجه")]
        [GridColumnEncodeHtml(false)]
        public string Result { get; set; }
        [ShowInGridview]
        [Display(Name ="تاریخ")]
        [GridColumnWidth(200)]
        public string CreateDateConverted { get; set; }
    }
    public class PollAnswerViewModel
    {
        [Key]
        [ShowInGridview]
        [Display(Name = "ID")]
        [GridColumnWidth(100)]
        public int ID { get; set; }
        [StringLength(1000)]
        public string Answer { get; set; }
    }
    #endregion
}
