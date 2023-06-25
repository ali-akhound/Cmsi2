using AVA.UI.Helpers.Base;
using AVA.UI.Helpers.CustomAttribute;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace AVA.Web.Mvc.Models
{
    public class ConferenceRefereeViewModel
    {
        [ShowInGridview]
        [Display(Name = "ID")]
        public string ID { get; set; }
        [ShowInGridview]
        [Display(Name = "ConferenceID")]
        public int ConferenceID { get; set; }
        [Display(Name = "فعال")]
        public bool Enable { get; set; }
        [Display(Name = "نام داور")]
        public ICollection<RefereeSelective> Referees { get; set; }
        public ICollection<RefereeSelective> RefereesFeed { get; set; }
        public ObjectState ObjectState { get; set; }
    }
    public class RefereeSelective
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
    public class Select2ConferenceRefereeViewModel
    {
        public string Name { get; set; }
        public string ConferenceID { get; set; }
        public ICollection<RefereeSelective> Referees { get; set; }

    }
    public class ConferenceRefereeDetailViewModel
    {
        [ShowInGridview]
        [Display(Name = "ID")]
        public int ID { get; set; }
        [Display(Name = "نام داور")]
        [ShowInGridview]
        public string RefereeName { get; set; }
        [ShowInGridview]
        [Display(Name = "تاریخ ثبت")]
        public string CreateDateConverted { get; set; }
        [ShowInGridview]
        [Display(Name = "فعال")]
        public bool? Enable { get; set; }
    }

}
