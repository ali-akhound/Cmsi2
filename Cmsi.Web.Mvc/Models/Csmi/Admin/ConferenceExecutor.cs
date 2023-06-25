using AVA.UI.Helpers.Base;
using AVA.UI.Helpers.CustomAttribute;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace AVA.Web.Mvc.Models
{
    public class ConferenceExecutorViewModel
    {
        [ShowInGridview]
        [Display(Name = "ID")]
        public string ID { get; set; }
        [ShowInGridview]
        [Display(Name = "ConferenceID")]
        public int ConferenceID { get; set; }
        [Display(Name = "فعال")]
        public bool Enable { get; set; }
        [Display(Name = "نام مدیر")]
        public ICollection<ExecutorSelective> Executors { get; set; }
        public ICollection<ExecutorSelective> ExecutorsFeed { get; set; }
        public ObjectState ObjectState { get; set; }
    }
    public class ExecutorSelective
    {
        public string ID { get; set; }
        public string Name { get; set; }
    }
    public class Select2ConferenceExecutorViewModel
    {
        public string Name { get; set; }
        public string ConferenceID { get; set; }
        public ICollection<ExecutorSelective> Executors { get; set; }

    }
    public class ConferenceExecutorDetailViewModel
    {
        [ShowInGridview]
        [Display(Name = "ID")]
        public int ID { get; set; }
        [Display(Name = "نام مدیر")]
        [ShowInGridview]
        public string ExecutorName { get; set; }
        [ShowInGridview]
        [Display(Name = "تاریخ ثبت")]
        public string CreateDateConverted { get; set; }
        [ShowInGridview]
        [Display(Name = "فعال")]
        public bool? Enable { get; set; }
    }

}
