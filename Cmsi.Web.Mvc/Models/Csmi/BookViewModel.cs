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
    #region Book
    public class BookViewModel
    {
        [Key]
        [ShowInGridview]
        [Display(Name = "ID")]
        public int ID { get; set; }
        [ShowInGridview]

        [Display(Name = "BookViewModel_Name", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "BookViewModel_Name_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        [StringLength(250)]
        public string Name { get; set; }
        [ShowInGridview]
        [Display(Name = "BookViewModel_Writer", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "BookViewModel_Writer_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        [StringLength(100)]
        public string Writer { get; set; }
        [StringLength(4)]
        [ShowInGridview]
        [Display(Name = "BookViewModel_Year", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "BookViewModel_Year_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        public string Year { get; set; }
        [StringLength(10)]
        [ShowInGridview]
        [Display(Name = "BookViewModel_PrintPeriod", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "BookViewModel_PrintPeriod_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        public string PrintPeriod { get; set; }
        [StringLength(200)]
        [Display(Name = "BookViewModel_ImageUrl", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public string ImageUrl { get; set; }
        [ShowInGridview]
        [Display(Name = "BookViewModel_LanguageName", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public string LanguageName { get; set; }
        [ShowInGridview]
        [Display(Name = "BookViewModel_CreateDateConverted", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public string CreateDateConverted { get; set; }
        public HttpPostedFileBase Image { get; set; }
        public bool? Enable { get; set; }
        public string LanguageID { get; set; }
        public List<DropDownVm> Languages { get; set; }
        public ObjectState ObjectState { get; set; }

    }
    public class BookListViewModel
    {
        public List<BookViewModel> Books { get; set; }
        public BookViewModel SingleBooks { get; set; } = new BookViewModel();
        public int pageSize { get; set; }
        public int totalRecords { get; set; }
        public int pageNumber { get; set; }
        [Display(Name = "عنوان کتاب یا نویسنده")]
        public string Writer { get; set; }
        public string OrderType { get; set; }
    }
    #endregion
}
