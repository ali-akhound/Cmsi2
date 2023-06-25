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
    #region Order
    public class OrderItemViewModel
    {
        [Key]
        [ShowInGridview]
        [Display(Name = "ID")]
        public int ID { get; set; }
        [ShowInGridview]

        [Display(Name = "OrderItemViewModel_Name", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [StringLength(250)]
        public string Name { get; set; }
        [ShowInGridview]
        [Display(Name = "OrderItemViewModel_Count", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [StringLength(100)]
        public int Count { get; set; }
        [ShowInGridview]
        [Display(Name = "OrderItemViewModel_Price", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public int Price { get; set; }


        [ShowInGridview]
        [Display(Name = "OrderItemViewModel_CreateDateConverted", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public string CreateDateConverted { get; set; }
        public ObjectState ObjectState { get; set; }

    }
    public class OrderViewModel
    {
        [Key]
        [ShowInGridview]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [ShowInGridview]
        [Display(Name = "OrderViewModel_Status", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public string Status { get; set; }

        [ShowInGridview]
        [Display(Name = "OrderViewModel_CreateDateConverted", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public string CreateDateConverted { get; set; }

        [ShowInGridview]
        [Display(Name = "OrderViewModel_Invoice", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public InvoiceViewModel Invoice { get; set; }

        [Display(Name = "OrderViewModel_OrderItems", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public List<OrderItemViewModel> OrderItems { get; set; }
        [Display(Name = "OrderViewModel_Amount", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public int Amount { get; set; }
        public bool Enable { get; set; }
        public string TableName { get; set; }
        public ObjectState ObjectState { get; set; }

    }



    public class OrderListViewModel
    {
        public List<OrderViewModel> Orders { get; set; }
        public OrderViewModel SingleOrder { get; set; } = new OrderViewModel() { OrderItems=new List<OrderItemViewModel>(),TableName= "Order" };
        public OrderItemViewModel SingleOrderItem { get; set; } = new OrderItemViewModel();
        public int pageSize { get; set; }
        public int totalRecords { get; set; }
        public int pageNumber { get; set; }
    }
    #endregion
}
