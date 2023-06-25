using AVA.UI.Helpers.Base;
using AVA.UI.Helpers.CustomAttribute;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using static AVA.Web.Mvc.Models.BaseViewModel;

namespace AVA.Web.Mvc.Models
{
    #region Invoice
    public class InvoiceViewModel
    {
        [Key]
        [ShowInGridview]
        [Display(Name = "ID")]
        public int ID { get; set; }
        [ShowInGridview]
        [Display(Name = "InvoiceViewModel_InvoiceType", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public string InvoiceType { get; set; }
        [ShowInGridview]
        [Display(Name = "InvoiceViewModel_DigitalCode", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [StringLength(250)]
        public string DigitalCode { get; set; }
        [Display(Name = "InvoiceViewModel_ReservationCode", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public string ReservationCode { get; set; }

        [Display(Name = "InvoiceViewModel_Cash2BankBankName", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "InvoiceViewModel_Cash2BankBankName_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        [ShowInGridview]
        public string Cash2BankBankName { get; set; }
        [ShowInGridview]
        [Display(Name = "InvoiceViewModel_Cash2BankPayReceipt", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "InvoiceViewModel_Cash2BankPayReceipt_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        [StringLength(200)]
        public string Cash2BankPayReceipt { get; set; }
        [Display(Name = "InvoiceViewModel_Cash2BankPayDate", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "InvoiceViewModel_Cash2BankPayDate_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        [ShowInGridview]
        public string Cash2BankPayDate { get; set; }
        [Display(Name = "InvoiceViewModel_Cash2BankPayFileUrl", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "InvoiceViewModel_Cash2BankPayFileUrl_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        [ShowInGridview]
        [HyperLinkGridviewColumn]
        public string Cash2BankPayFileUrl { get; set; }
        [ShowInGridview]
        [Display(Name = "InvoiceViewModel_CreateDateConverted", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public string CreateDateConverted { get; set; }
        [Display(Name = "InvoiceViewModel_Amount", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public decimal amount { get; set; }

        [Display(Name = "InvoiceViewModel_Cash2BankPayFile", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "InvoiceViewModel_Cash2BankPayFile_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        public HttpPostedFileBase Cash2BankPayFile { get; set; }
        public bool? Enable { get; set; }
        public int OrderID { get; set; }
        public ObjectState ObjectState { get; set; }
        public List<CheckBoxVm> PayTypes { get; set; }
        public string SelectedPayType { get; set; }
        public string merchantId { get; set; }
        public string token { get; set; }
        public string Explain { get; set; }
        public string TableName { get; set; }
    }
    public class InvoiceListViewModel
    {
        public List<InvoiceViewModel> Invoices { get; set; }
        public InvoiceViewModel SingleBooks { get; set; } = new InvoiceViewModel();
        public int pageSize { get; set; }
        public int totalRecords { get; set; }
        public int pageNumber { get; set; }
    }
    public class InvoiceOnlineResultViewModel
    {
        public string paymentId { get; set; }
        public string referenceId { get; set; }
        public string StatusMsg { get; set; }
    }

    #endregion
}
