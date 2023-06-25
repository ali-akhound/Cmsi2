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
    public class SocietyPaymentController : BaseController
    {
        // GET: Admin/Payment
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.SocietyPayment)]
        public ActionResult SocietyPaymentManagement()
        {
            ViewBag.GridID = "SocietyPaymentManagementGrid";
            return PartialView("~/Areas/Admin/Views/Csmi/SocietyPaymentManagement.cshtml");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.SocietyPayment)]
        public ActionResult GetSocietyPaymentPartial()
        {
            return new GridViewPartialController().GridViewPartial<PaymentViewModel>(Bind(), "SocietyPayment", "GetSocietyPaymentPartial", "SocietyPaymentManagementGrid", "ID");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.SocietyPayment)]
        public ActionResult ExportTo(int ActionType)
        {
            return new GridViewPartialController().ExportTo<PaymentViewModel>(Bind(), ActionType, "SocietyPaymentManagementGrid");
        }
        List<PaymentViewModel> Bind()
        {
            var Payments = new List<PaymentViewModel>();
            var context = new ApplicationDbContext();
            var model = new object[0];
            var DbPayments = context.Invoices
                .Include(Payment => Payment.InvoiceType)
                .Include(Payment => Payment.CreatorUser.Person)
                .Where(Payment => Payment.TableName == "SocietyVipOrder")
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
                if (item.TableName == "SocietyVipOrder")
                {
                    vm.InvoiceTypeName = "انجمن";
                    var Item = context.SocietyVipOrders
                        .Include(order => order.PackageName.PackageNameTranslations)
                        .Where(order => order.ID == item.DocumentID)
                        .Select(order => new
                        {
                            UserID = order.CreatorUser.Id,
                            UniversityCardUrl = order.UniversityCardUrl,
                            OrderStatusName = order.OrderStatus.Name,
                            Explain = order.PackageName.PackageNameTranslations.Where(lang => lang.Language.Value == "fa").FirstOrDefault().Name + " :" + order.Count.ToString() + " عدد"
                        }).SingleOrDefault();
                    if (Item != null)
                    {
                        vm.Explain = Item.Explain + (Item.UniversityCardUrl != "" ? "<br /><a class='btn btn-primary btn-sm' style='color:white;text-decoration:none' href=\"" + Url.Content(Item.UniversityCardUrl) + "\">کارت دانشجویی</a>" : "");
                        vm.OrderStatus = Item.OrderStatusName;
                    }
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
        [HttpPost]
        [AjaxValidateAntiForgeryTokenAttribute]
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.SocietyPayment)]
        public ActionResult ConfirmPayment(string[] ids)
        {
            var context = new ApplicationDbContext();
            try
            {
                foreach (var id in ids)
                {
                    int CurrentID = int.Parse(id);
                    var selectedItem = context.Invoices
                        .Where(item => item.ID == CurrentID).Single();
                    if (selectedItem != null)
                    {
                        //if (selectedItem.TableName == "Order")
                        //{
                        //    var CurrentOrder = context.Orders
                        //        .Include(order => order.CreatorUser)
                        //        .Include(order => order.Conference)
                        //        .Include(order => order.OrderStatus)
                        //        .Where(order => order.ID == selectedItem.DocumentID).SingleOrDefault();
                        //    if (CurrentOrder.OrderStatus.ID != (int)Enums.OrderStatus.Confirmed)
                        //    {
                        //        CurrentOrder.OrderStatus = context.OrderStatuses.Where(orderStatus => orderStatus.ID == (int)Enums.OrderStatus.Confirmed).SingleOrDefault();
                        //    }
                        //}
                        if (selectedItem.TableName == "SocietyVipOrder")
                        {

                            var CurrentOrder = context.SocietyVipOrders
                                .Include(order => order.CreatorUser)
                                .Include(order => order.CreatorUser.Person)
                                .Include(order => order.PackageName)
                                .Include(order => order.OrderStatus)
                                .Where(order => order.ID == selectedItem.DocumentID).SingleOrDefault();
                            if (CurrentOrder.OrderStatus.ID != (int)Enums.OrderStatus.Confirmed)
                            {
                                CurrentOrder.OrderStatus = context.OrderStatuses.Where(orderStatus => orderStatus.ID == (int)Enums.OrderStatus.Confirmed).SingleOrDefault();
                                CurrentOrder.CreatorUser.VipStartDate = DateTime.Now;
                                CurrentOrder.CreatorUser.VipEndDate = DateTime.Now.AddYears(CurrentOrder.Count);
                                if (string.IsNullOrEmpty(CurrentOrder.CreatorUser.Person.CompanyID))
                                {
                                    CurrentOrder.CreatorUser.Person.CompanyID = CurrentOrder.CreatorUser.Person.EnglishFirstName.Substring(0, 1) + CurrentOrder.CreatorUser.Person.EnglishLastName.Substring(0, 1) + "-" + CommonHelper.DateAndTimes.GetSortableDate(DateTime.Now) + "-" + (context.Persons.Count() + 1).ToString();
                                    EmailService.SendSocietyPaymentConfirmation(CurrentOrder.CreatorUser.Person.FirstName, CurrentOrder.CreatorUser.Person.LastName, CurrentOrder.CreatorUser.Person.CompanyID, CurrentOrder.CreatorUser.Email);
                                }
                            }
                        }
                    }
                }
                context.SaveChanges();
                return ControllerHelper.SuccessResult("با موفقیت ثبت شد");
            }
            catch (Exception ex)
            {
                return ControllerHelper.ErrorResult("بروز خطای سیستمی");
            }


        }
        [HttpPost]
        [AjaxValidateAntiForgeryTokenAttribute]
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.SocietyPayment)]
        public ActionResult NotConfirmPayment(string[] ids)
        {
            var context = new ApplicationDbContext();
            try
            {
                foreach (var id in ids)
                {
                    int CurrentID = int.Parse(id);
                    var selectedItem = context.Invoices
                        .Where(item => item.ID == CurrentID).Single();
                    if (selectedItem != null)
                    {
                        //if (selectedItem.TableName == "Order")
                        //{
                        //    var CurrentOrder = context.Orders
                        //        .Include(order => order.CreatorUser)
                        //        .Include(order => order.Conference)
                        //        .Include(order => order.OrderStatus)
                        //        .Where(order => order.ID == selectedItem.DocumentID).SingleOrDefault();
                        //    CurrentOrder.OrderStatus = context.OrderStatuses.Where(orderStatus => orderStatus.ID == (int)Enums.OrderStatus.NotConfirmed).SingleOrDefault();
                        //}
                        if (selectedItem.TableName == "SocietyVipOrder")
                        {

                            var CurrentOrder = context.SocietyVipOrders
                                .Include(order => order.CreatorUser)
                                .Include(order => order.PackageName)
                                .Include(order => order.OrderStatus)
                                .Include(order => order.CreatorUser.Person)
                                .Where(order => order.ID == selectedItem.DocumentID).SingleOrDefault();
                            CurrentOrder.LastModifyDate = DateTime.Now;
                            CurrentOrder.LastModifyUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault();
                            CurrentOrder.OrderStatus = context.OrderStatuses.Where(orderStatus => orderStatus.ID == (int)Enums.OrderStatus.NotConfirmed).SingleOrDefault();
                            EmailService.SendSocietyPaymentNotConfirmation(CurrentOrder.CreatorUser.Person.FirstName, CurrentOrder.CreatorUser.Person.LastName, CurrentOrder.CreatorUser.Person.CompanyID, CurrentOrder.CreatorUser.Email);
                        }
                    }
                }
                context.SaveChanges();
                return ControllerHelper.SuccessResult("با موفقیت ثبت شد");
            }
            catch (Exception ex)
            {
                return ControllerHelper.ErrorResult("بروز خطای سیستمی");
            }


        }
    }
}