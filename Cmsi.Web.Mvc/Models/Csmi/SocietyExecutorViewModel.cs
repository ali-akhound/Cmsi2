using AVA.UI.Helpers.Base;
using AVA.UI.Helpers.CustomAttribute;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using static AVA.Web.Mvc.Models.Admin.BaseViewModel;
using System.Web.Mvc;

namespace AVA.Web.Mvc.Models
{
    #region SocietyExecutor
    public class SocietyExecutorViewModel
    {
        [Key]
        [ShowInGridview]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [ShowInGridview]
        [Display(Name = "SocietyExecutorViewModel_Name", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "SocietyExecutorViewModel_Name_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        public string Name { get; set; }
        [StringLength(100)]
        [ShowInGridview]
        [Display(Name = "SocietyExecutorViewModel_Family", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "SocietyExecutorViewModel_Family_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        public string Family { get; set; }
        [StringLength(50)]
        [ShowInGridview]
        [Display(Name = "SocietyExecutorViewModel_Email", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "SocietyExecutorViewModel_Email_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        public string Email { get; set; }
        [StringLength(50)]
        [ShowInGridview]
        [Display(Name = "SocietyExecutorViewModel_Position", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "SocietyExecutorViewModel_Position_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        public string Position { get; set; }
        [StringLength(100)]
        [ShowInGridview]
        [Display(Name = "SocietyExecutorViewModel_ResumeUrl", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "SocietyExecutorViewModel_ResumeUrl_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        public string ResumeUrl { get; set; }
        [Display(Name = "SocietyExecutorViewModel_Resume", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "SocietyExecutorViewModel_Resume_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        public HttpPostedFileBase Resume { get; set; }
        public string ResumeUrlText { get; set; }
        [StringLength(50)]
        [ShowInGridview]
        [Display(Name = "SocietyExecutorViewModel_DegreeLevel", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "SocietyExecutorViewModel_DegreeLevel_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        public string DegreeLevel { get; set; }
        [Display(Name = "SocietyExecutorViewModel_PersonalImageUrl", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "SocietyExecutorViewModel_PersonalImageUrl_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        public string PersonalImageUrl { get; set; }
        [Display(Name = "SocietyExecutorViewModel_PersonalImage", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "SocietyExecutorViewModel_PersonalImage_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        public HttpPostedFileBase PersonalImage { get; set; }
        [ShowInGridview]
        [Display(Name = "SocietyExecutorViewModel_LanguageName", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public string LanguageName { get; set; }


        //[Display(Name = "SocietyExecutorViewModel_StartDate", ResourceType = typeof(AVA.Web.Resources.Resource))]
        //[Required(ErrorMessageResourceName = "SocietyExecutorViewModel_StartDate_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        //public string StartDate { get; set; }

        //[Display(Name = "SocietyExecutorViewModel_EndDate", ResourceType = typeof(AVA.Web.Resources.Resource))]
        //[Required(ErrorMessageResourceName = "SocietyExecutorViewModel_EndDate_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        //public string EndDate { get; set; }

        public string LanguageID { get; set; }
        public List<DropDownVm> Languages { get; set; }
        [ShowInGridview]
        [GridColumnWidth(50)]
        [Display(Name = "SocietyExecutorViewModel_Enable", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public bool Enable { get; set; }
        [ShowInGridview]
        [Display(Name = "SocietyExecutorViewModel_CreateDateConverted", ResourceType = typeof(AVA.Web.Resources.Resource))]

        public string CreateDateConverted { get; set; }
        public ObjectState ObjectState { get; set; }
    }
    public class SocietyExecutorListViewModel
    {
        public List<SocietyExecutorViewModel> SocietyExecutors { get; set; }
        public SocietyExecutorViewModel SingleSocietyExecutor { get; set; } = new SocietyExecutorViewModel();
    }
    #endregion
}
