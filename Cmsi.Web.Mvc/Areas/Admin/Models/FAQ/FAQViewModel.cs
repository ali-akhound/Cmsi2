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
    #region FAQViewModel
    public class FAQViewModel
    {
        [Key]
        [ShowInGridview]
        [Display(Name = "ID")]
        [GridColumnWidth(35)]
        public int ID { get; set; }

        [Display(Name = "FAQViewModel_Question", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "FAQViewModel_Question_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        [ShowInGridview]
        public string Question { get; set; }
        [Display(Name = "FAQViewModel_Answer", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "FAQViewModel_Answer_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        [ShowInGridview]
        public string Answer { get; set; }
        [Display(Name = "FAQViewModel_Priority", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "FAQViewModel_Priority_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        [ShowInGridview]
        [GridColumnWidth(50)]
        public int Priority { get; set; }
        [ShowInGridview]
        [Display(Name = "FAQViewModel_CreateDateConverted", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [GridColumnWidth(100)]
        public string CreateDateConverted { get; set; }
        [ShowInGridview]
        [Display(Name = "FAQViewModel_LanguageName", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [GridColumnWidth(100)]
        public string LanguageName { get; set; }
        public string LanguageID { get; set; }
        public List<DropDownVm> Languages { get; set; }
        [Display(Name = "FAQViewModel_Enable", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [ShowInGridview]
        [GridColumnWidth(80)]
        public bool? Enable { get; set; }
        public ObjectState ObjectState { get; set; }

    }
    public class FAQListViewModel
    {
        public List<FAQViewModel> FAQs { get; set; }
        public FAQViewModel SingleFAQ { get; set; } = new FAQViewModel();
        public int pageSize { get; set; }
        public int totalRecords { get; set; }
        public int pageNumber { get; set; }
    }
    #endregion
}
