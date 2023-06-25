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
    public class PaymentViewModel
    {
        [Key]
        [ShowInGridview]
        [GridColumnWidth(50)]
        [Display(Name = "ID")]
        public int ID { get; set; }
        [ShowInGridview]
        [Display(Name = "PaymentViewModel_UserNameFamily", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [StringLength(100)]
        public string UserNameFamily { get; set; }
        [ShowInGridview]
        [Display(Name = "PaymentViewModel_Amount", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [GridColumnWidth(100)]
        public string Amount { get; set; }
        [ShowInGridview]
        [Display(Name = "PaymentViewModel_InvoiceTypeName", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public string InvoiceTypeName { get; set; }
        [GridColumnWidth(100)]
        [ShowInGridview]
        [Display(Name = "PaymentViewModel_PaymentTypeName", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public string PaymentTypeName { get; set; }
        [ShowInGridview]
        [Display(Name = "PaymentViewModel_PayExplain", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [GridColumnEncodeHtml(false)]
        public string PayExplain { get; set; }
        [ShowInGridview]
        [Display(Name = "PaymentViewModel_OrderStatus", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [GridColumnWidth(100)]
        public string OrderStatus { get; set; }
        [ShowInGridview]
        [GridColumnWidth(100)]
        [Display(Name = "PaymentViewModel_CreateDateConverted", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public string CreateDateConverted { get; set; }
        [Display(Name = "PaymentViewModel_Explain", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [ShowInGridview]
        [GridColumnEncodeHtml(false)]
        public string Explain { get; set; }

    }
    #endregion
}
