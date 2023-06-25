using AVA.UI.Helpers.Base;
using AVA.UI.Helpers.CustomAttribute;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using static AVA.Web.Mvc.Models.Admin.BaseViewModel;

namespace AVA.Web.Mvc.Models
{
    #region SocietyMember
    public class SocietyMemberViewModel
    {
        [Key]
        [ShowInGridview]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [ShowInGridview]
        [Display(Name = "SocietyMemberViewModel_Name", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "SocietyMemberViewModel_Name_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        public string Name { get; set; }
        [StringLength(100)]
        [ShowInGridview]
        [Display(Name = "SocietyMemberViewModel_Family", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "SocietyMemberViewModel_Family_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        public string Family { get; set; }
        [ShowInGridview]
        [StringLength(50)]
        [Display(Name = "SocietyMemberViewModel_Email", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "SocietyMemberViewModel_Email_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        public string Email { get; set; }
        [StringLength(50)]
        [ShowInGridview]
        [Display(Name = "SocietyMemberViewModel_Position", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "SocietyMemberViewModel_Position_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        public string Position { get; set; }
        [StringLength(100)]
        [ShowInGridview]
        [HyperLinkGridviewColumn]
        [HyperLinkGridviewColumnText("ResumeUrlText")]
        [Display(Name = "SocietyMemberViewModel_ResumeUrl", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "SocietyMemberViewModel_ResumeUrl_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        public string ResumeUrl { get; set; }
        public string ResumeUrlText { get; set; }

        [StringLength(50)]
        [ShowInGridview]
        [Display(Name = "SocietyMemberViewModel_DegreeLevel", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "SocietyMemberViewModel_DegreeLevel_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        public string DegreeLevel { get; set; }
        [Display(Name = "SocietyMemberViewModel_PersonalImageUrl", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public string PersonalImageUrl { get; set; }
        public int PeriodID { get; set; }

        //[Display(Name = "SocietyMemberViewModel_StartDate", ResourceType = typeof(AVA.Web.Resources.Resource))]
        //[Required(ErrorMessageResourceName = "SocietyMemberViewModel_StartDate_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        //public string StartDate { get; set; }

        //[Display(Name = "SocietyMemberViewModel_EndDate", ResourceType = typeof(AVA.Web.Resources.Resource))]
        //[Required(ErrorMessageResourceName = "SocietyMemberViewModel_EndDate_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        //public string EndDate { get; set; }

        [ShowInGridview]
        [Display(Name = "SocietyMemberViewModel_CreateDateConverted", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public string CreateDateConverted { get; set; }
        [Display(Name = "SocietyMemberViewModel_PersonalImage", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "SocietyMemberViewModel_PersonalImage_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        public HttpPostedFileBase PersonalImage { get; set; }
        [Display(Name = "SocietyMemberViewModel_Resume", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "SocietyMemberViewModel_Resume_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        public HttpPostedFileBase Resume { get; set; }
        [ShowInGridview]
        [GridColumnWidth(50)]
        [Display(Name = "SocietyMemberViewModel_Enable", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public bool Enable { get; set; }

        [ShowInGridview]
        [Display(Name = "SocietyMemberViewModel_LanguageName", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public string LanguageName { get; set; }
        public string LanguageID { get; set; }
        public List<DropDownVm> Languages { get; set; }
        public ObjectState ObjectState { get; set; }

    }
    public class SocietyMemberListViewModel
    {
        public List<SocietyMemberViewModel> SocietyMembers { get; set; }
        public List<SocietyMemberGroupViewModel> SocietyMemberGroupList { get; set; }
        public SocietyMemberViewModel SingleSocietyMember { get; set; } = new SocietyMemberViewModel();
    }
    public class SocietyMemberGroupViewModel
    {
        public int PeriodID { get; set; }
        public string PeriodName { get; set; }
    }
    #endregion
}
