using AVA.UI.Helpers.Base;
using AVA.UI.Helpers.CustomAttribute;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace AVA.Web.Mvc.Models
{
    #region SocietyMemberPeriod
    public class SocietyMemberPeriodViewModel
    {
        [Key]
        [ShowInGridview]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [ShowInGridview]
        [Display(Name = "SocietyMemberPeriodViewModel_StartDate", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "SocietyMemberPeriodViewModel_StartDate_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        public string StartDate { get; set; }
        [StringLength(100)]
        [ShowInGridview]
        [Display(Name = "SocietyMemberPeriodViewModel_EndDate", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "SocietyMemberPeriodViewModel_EndDate_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        public string EndDate { get; set; }

        [ShowInGridview]
        [Display(Name = "SocietyMemberPeriodViewModel_CreateDateConverted", ResourceType = typeof(AVA.Web.Resources.Resource))]

        public string CreateDateConverted { get; set; }
      
        public ObjectState ObjectState { get; set; }

    }
    #endregion
}
