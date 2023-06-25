using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AVA.Web.Mvc.Models.Admin;
using AVA.Core.Entities;
using AVA.UI.Helpers.Base;
using AVA.UI.Helpers.Common;
using System.Net;
using AVA.UI.Helpers.Controller;
using AVA.UI.Helpers.CustomAttribute;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using AVA.Web.Mvc.Models;
using AVA.UI.Helpers.FileUploadManagment;
using System.Web.Script.Serialization;
using static AVA.Web.Mvc.Models.Admin.BaseViewModel;
using AVA.UI.Helpers.MailSmsService;
using AVA.UI.Helpers;
namespace AVA.Web.Mvc.Controllers.Admin
{
    //Optimized
    public class PaymentController : BaseController
    {
        // GET: Admin/Payment
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Payment)]
        public ActionResult PaymentManagement()
        {
            ViewBag.GridID = "PaymentManagementGrid";
            return PartialView("~/Areas/Admin/Views/Csmi/PaymentManagement.cshtml");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Payment)]
        public ActionResult GetPaymentPartial()
        {
            return new GridViewPartialController().GridViewPartial<PaymentViewModel>(Bind(), "Payment", "GetPaymentPartial", "PaymentManagementGrid", "ID");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Payment)]
        public ActionResult ExportTo(int ActionType)
        {
            return new GridViewPartialController().ExportTo<PaymentViewModel>(Bind(), ActionType, "PaymentManagementGrid");
        }
        List<PaymentViewModel> Bind()
        {
            var Payments = new List<PaymentViewModel>();
            var context = new ApplicationDbContext();
            var model = new object[0];
            var DbPayments = context.Invoices
                .Include(Payment => Payment.InvoiceType)
                .Include(Payment => Payment.CreatorUser.Person)
                .Where(Payment => Payment.TableName == "Order")
                .Select(item => new
                {
                    item.ID,
                    item.TableName,
                    FirstName = item.CreatorUser.Person.FirstName,
                    LastName = item.CreatorUser.Person.LastName,
                    Amount = item.Amount,
                    InvoiceTypeName = item.InvoiceType.Name,
                    InvoiceTypeID = item.InvoiceType.ID,
                    ReservationCode = item.ReservationCode,
                    DigitalCode = item.DigitalCode,
                    Cash2BankPayDate = item.Cash2BankPayDate,
                    Cash2BankPayFileUrl = item.Cash2BankPayFileUrl,
                    Cash2BankPayReceipt = item.Cash2BankPayReceipt,
                    CreateDate = item.CreateDate,
                    DocumentID = item.DocumentID
                })
                .ToList().OrderByDescending(item => item.CreateDate);
            foreach (var item in DbPayments)
            {
                PaymentViewModel vm = new PaymentViewModel()
                {
                    ID = item.ID,
                    CreateDateConverted = CommonHelper.DateAndTimes.GetPersianDate(item.CreateDate),
                    UserNameFamily = item.FirstName + " " + item.LastName,
                    Amount = string.Format("{0:n0}", (int)item.Amount),
                    PaymentTypeName = item.InvoiceTypeName
                };
                if (item.TableName == "Order")
                {
                    vm.InvoiceTypeName = "همایش";
                    var Items = context.OrderItems
                        .Include(orderItem => orderItem.Order.OrderStatus)
                        .Where(orderItem => orderItem.Order.ID == item.DocumentID)
                        .GroupBy(orderItem => new { orderItem.ConferencePackage, orderItem.Order })
                        .Select(orderItem => new
                        {
                            ConferenceID = orderItem.Key.Order.Conference.ID,
                            UserID = orderItem.Key.Order.CreatorUser.Id,
                            OrderStatusName = orderItem.Key.Order.OrderStatus.Name,
                            Explain = orderItem.Key.ConferencePackage.PackageName.PackageNameTranslations.Where(lang => lang.Language.Value == "fa").FirstOrDefault().Name + " :" + orderItem.Sum(InvoiceItem => InvoiceItem.Count).ToString() + " عدد"
                        }).ToList();
                    if (Items.Count > 0)
                    {
                        vm.Explain = string.Join("<br />", Items.Select(i => i.Explain).ToArray<string>()) + "<br /><a class='btn btn-primary btn-sm' style='color:white;text-decoration:none' ui-sref=\"ConferenceCompanionManagement({ConferenceID:'" + Items[0].ConferenceID.ToString() + "',UserID:'" + Items[0].UserID + "'})\">شرکت کنندگان</a>";
                    }
                    vm.OrderStatus = context.Orders
                                                   .Include(Order => Order.OrderStatus)
                                                   .Where(order => order.ID == item.DocumentID)
                                                   .Select(order => order.OrderStatus.Name).FirstOrDefault();
                }
                if (item.InvoiceTypeID == (int)Enums.InvoiceType.Online)
                {
                    vm.PayExplain = "شماره فاکتور:" + item.ReservationCode + "<br /> رسید دیجیتال" + item.DigitalCode;
                }
                if (item.InvoiceTypeID == (int)Enums.InvoiceType.CashBankPay)
                {
                    vm.PayExplain = "شماره فیش:" + item.Cash2BankPayReceipt + "<br /> تاریخ پرداخت:" + CommonHelper.DateAndTimes.GetPersianDate(item.Cash2BankPayDate) + "<br /><a class='btn btn-primary btn-sm' style='color:white;text-decoration:none' target='_blank' href='" + Url.Content(item.Cash2BankPayFileUrl) + "'>تصویر فیش</a>";
                }

                Payments.Add(vm);
            }
            if (Payments.Count() == 0)
            {
                Payments.Add(new PaymentViewModel());
            }
            return Payments;
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Payment)]
        public ActionResult HtmlEditorPartial()
        {
            return new HtmlEditorPartialController().HtmlEditorView("HtmlEditor", "OnHtmlChangedChanged", "OnInit");
        }
        [HttpPost]
        [AjaxValidateAntiForgeryTokenAttribute]
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Payment)]
        public ActionResult ConfirmPayment(string[] ids)
        {
            var context = new ApplicationDbContext();
            try
            {
                foreach (var id in ids)
                {
                    int CurrentID = int.Parse(id);
                    var selectedInvoice =
                        context.Invoices
                        .Include(inv => inv.CreatorUser)
                        .Include(inv => inv.CreatorUser.Person)
                        .Where(item => item.ID == CurrentID).Single();
                    if (selectedInvoice != null)
                    {
                        if (selectedInvoice.TableName == "Order")
                        {
                            var CurrentOrder = context.Orders
                                .Include(order => order.CreatorUser)
                                .Include(order => order.Conference)
                                .Include(order => order.OrderStatus)
                                .Where(order => order.ID == selectedInvoice.DocumentID).SingleOrDefault();
                            if (CurrentOrder != null)
                                if (CurrentOrder.OrderStatus.ID != (int)Enums.OrderStatus.Confirmed)
                                {
                                    CurrentOrder.OrderStatus = context.OrderStatuses.Where(orderStatus => orderStatus.ID == (int)Enums.OrderStatus.Confirmed).SingleOrDefault();
                                    EmailService.SendConferencePaymentConfirmation(selectedInvoice.CreatorUser.Person.FirstName, CurrentOrder.CreatorUser.Person.LastName, CurrentOrder.CreatorUser.Email);
                                }
                        }
                        //if (selectedItem.TableName == "SocietyVipOrder")
                        //{

                        //    var CurrentOrder = context.SocietyVipOrders
                        //        .Include(order => order.CreatorUser)
                        //        .Include(order => order.CreatorUser.Person)
                        //        .Include(order => order.PackageName)
                        //        .Include(order => order.OrderStatus)
                        //        .Where(order => order.ID == selectedItem.DocumentID).SingleOrDefault();
                        //    if (CurrentOrder.OrderStatus.ID != (int)Enums.OrderStatus.Confirmed)
                        //    {
                        //        CurrentOrder.OrderStatus = context.OrderStatuses.Where(orderStatus => orderStatus.ID == (int)Enums.OrderStatus.Confirmed).SingleOrDefault();
                        //        CurrentOrder.CreatorUser.VipStartDate = DateTime.Now;
                        //        CurrentOrder.CreatorUser.VipEndDate = DateTime.Now.AddYears(CurrentOrder.Count);

                        //    }
                        //}
                    }
                }
                context.SaveChanges();
                return ControllerHelper.SuccessResult("با موفقیت ثبت شد");
            }
            catch (Exception ex)
            {
                return ControllerHelper.ErrorResult("بروز خطای سیستمی" + ex.Message);
            }


        }
        [HttpPost]
        [AjaxValidateAntiForgeryTokenAttribute]
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Payment)]
        public ActionResult NotConfirmPayment(string[] ids)
        {
            var context = new ApplicationDbContext();
            try
            {
                foreach (var id in ids)
                {
                    int CurrentID = int.Parse(id);
                    var selectedItem = context.Invoices
                        .Include(inv => inv.CreatorUser)
                        .Include(inv => inv.CreatorUser.Person)
                        .Where(item => item.ID == CurrentID).Single();
                    if (selectedItem != null)
                    {
                        if (selectedItem.TableName == "Order")
                        {
                            var CurrentOrder = context.Orders
                                .Include(order => order.CreatorUser)
                                .Include(order => order.Conference)
                                .Include(order => order.OrderStatus)
                                .Where(order => order.ID == selectedItem.DocumentID).SingleOrDefault();
                            CurrentOrder.OrderStatus = context.OrderStatuses.Where(orderStatus => orderStatus.ID == (int)Enums.OrderStatus.NotConfirmed).SingleOrDefault();
                            EmailService.SendConferencePaymentNotConfirmation(CurrentOrder.CreatorUser.Person.FirstName, CurrentOrder.CreatorUser.Person.LastName, CurrentOrder.CreatorUser.Email);
                        }
                        //if (selectedItem.TableName == "SocietyVipOrder")
                        //{

                        //    var CurrentOrder = context.SocietyVipOrders
                        //        .Include(order => order.CreatorUser)
                        //        .Include(order => order.PackageName)
                        //        .Include(order => order.OrderStatus)
                        //        .Where(order => order.ID == selectedItem.DocumentID).SingleOrDefault();
                        //    CurrentOrder.LastModifyDate = DateTime.Now;
                        //    CurrentOrder.LastModifyUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault();
                        //    CurrentOrder.OrderStatus = context.OrderStatuses.Where(orderStatus => orderStatus.ID == (int)Enums.OrderStatus.NotConfirmed).SingleOrDefault();
                        //}

                    }
                }
                context.SaveChanges();
                return ControllerHelper.SuccessResult("با موفقیت ثبت شد");
            }
            catch (Exception ex)
            {
                return ControllerHelper.ErrorResult("بروز خطای سیستمی" + ex.Message);
            }


        }
    }
}