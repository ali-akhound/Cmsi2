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
using AVA.UI.Helpers.MailSmsService;

namespace AVA.Web.Mvc.Controllers.Admin
{
    //Optimized
    public class ExecutorController : BaseController
    {
        // GET: Admin/Executor
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Executor)]
        public ActionResult Executor()
        {
            if (Request.QueryString["id"] == null || Request.QueryString["id"] == "undefined")
            {
                return PartialView("~/Areas/Admin/Views/Csmi/Executor.cshtml", new ExecutorViewModel() { ObjectState = ObjectState.Insert });
            }
            else
            {
                var context = new ApplicationDbContext();
                var selectedID = int.Parse(Request.QueryString["id"]);
                var selectedExecutor = context.Executors
                    .Include(Executor => Executor.RefrenceUser)
                    .Include(Executor => Executor.RefrenceUser.Person)
                    .Where(item => item.ID.Equals(selectedID))
                    .Select(item => new
                    {
                        item.ID,
                        item.RefrenceUser.UserName,
                        item.RefrenceUser.Email,
                        item.RefrenceUser.Person.FirstName,
                        item.RefrenceUser.Person.LastName,
                        item.RefrenceUser.Person.Cellphone,
                        item.RefrenceUser.Person.PersonalImageUrl,
                        UserID = item.RefrenceUser.Id,
                        item.CreateDate,
                        item.Enable,
                    })
                    .SingleOrDefault();
                return PartialView("~/Areas/Admin/Views/Csmi/Executor.cshtml", new ExecutorViewModel()
                {
                    ID = selectedExecutor.ID,
                    CreateDateConverted = CommonHelper.DateAndTimes.GetPersianDate(selectedExecutor.CreateDate),
                    //UserName = selectedExecutor.UserName,
                    Password = "*",
                    Email = selectedExecutor.Email,
                    FirstName = selectedExecutor.FirstName,
                    LastName = selectedExecutor.LastName,
                    PhoneNumber = selectedExecutor.Cellphone,
                    ImageUrl = selectedExecutor.PersonalImageUrl,
                    RefrenceUserID = selectedExecutor.UserID,
                    Enable = selectedExecutor.Enable,
                    ObjectState = ObjectState.Update
                });
            }
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Executor)]
        public ActionResult ExecutorManagement()
        {
            ViewBag.GridID = "ExecutorManagementGrid";
            return PartialView("~/Areas/Admin/Views/Csmi/ExecutorManagement.cshtml");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Executor)]
        public ActionResult GetExecutorPartial()
        {
            return new GridViewPartialController().GridViewPartial<ExecutorViewModel>(Bind(), "Executor", "GetExecutorPartial", "ExecutorManagementGrid", "ID");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Executor)]
        public ActionResult ExportTo(int ActionType)
        {
            return new GridViewPartialController().ExportTo<ExecutorViewModel>(Bind(), ActionType, "PollManagementGrid");
        }
        List<ExecutorViewModel> Bind()
        {
            var Executors = new List<ExecutorViewModel>();
            var context = new ApplicationDbContext();
            var model = new object[0];
            var DbExecutors = context.Executors
                .Include(Executor => Executor.RefrenceUser)
                .Include(Executor => Executor.RefrenceUser.Person)
                .Select(item => new
                {
                    item.ID,
                    item.RefrenceUser.UserName,
                    item.RefrenceUser.Email,
                    item.RefrenceUser.Person.FirstName,
                    item.RefrenceUser.Person.LastName,
                    item.RefrenceUser.Person.Cellphone,
                    item.CreateDate,
                    item.Enable,
                })
                .ToList().OrderByDescending(item => item.CreateDate);
            foreach (var item in DbExecutors)
            {
                Executors.Add(new ExecutorViewModel()
                {
                    ID = item.ID,
                    CreateDateConverted = CommonHelper.DateAndTimes.GetPersianDate(item.CreateDate),
                    //UserName = item.UserName,
                    Email = item.Email,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    PhoneNumber = item.Cellphone,
                    Enable = item.Enable,
                });
            }
            if (Executors.Count() == 0)
            {
                Executors.Add(new ExecutorViewModel());
            }
            return Executors;
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Executor)]
        public ActionResult HtmlEditorPartial()
        {
            return new HtmlEditorPartialController().HtmlEditorView("HtmlEditor", "OnHtmlChangedChanged", "OnInit");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Executor)]
        [AjaxValidateAntiForgeryTokenAttribute]
        public ActionResult SubmitExecutor(ExecutorViewModel viewmodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        if (viewmodel.ObjectState == ObjectState.Insert) // insert
                        {
                            var context = new ApplicationDbContext();
                            var RefUser = context.Users
                                .Include(user => user.Roles)
                                .Include(user => user.Person)
                                .Where(item => item.Email == viewmodel.Email).SingleOrDefault();
                            var NewUser = new ApplicationUser();
                            //if (context.Users.Where(item => item.Email == viewmodel.Email).Count() > 0)
                            //{
                            //    return ControllerHelper.ErrorResult("ایمیل قبلا ثبت شده است");
                            //}
                            //if (context.Users.Where(item => item.UserName == viewmodel.Email).Count() > 0)
                            //{
                            //    return ControllerHelper.ErrorResult("نام کاربری قبلا ثبت شده است");
                            //}
                            var dbContextTransaction = context.Database.BeginTransaction();
                            try
                            {
                                if (RefUser == null)
                                {
                                    var person = context.Persons.Add(new Person()
                                    {
                                        FirstName = viewmodel.FirstName,
                                        LastName = viewmodel.LastName,
                                        Email = viewmodel.Email,
                                        Cellphone = viewmodel.PhoneNumber,
                                        CreateDate = DateTime.Now,
                                        CreatorUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault()
                                    });
                                    context.SaveChanges();
                                    NewUser = new ApplicationUser
                                    {
                                        Email = viewmodel.Email,
                                        PhoneNumber = viewmodel.PhoneNumber,
                                        UserName = viewmodel.Email,
                                        SecurityStamp= Guid.NewGuid().ToString(),
                                        CreateDate = DateTime.Now,
                                        Enable = true,
                                        EmailConfirmed = true,
                                        PasswordHash = new ApplicationUserManager(new UserStore<ApplicationUser>()).PasswordHasher.HashPassword(viewmodel.Password),
                                        Person = person
                                    };
                                    context.Users.Add(NewUser);
                                    context.SaveChanges();
                                    NewUser.Roles.Add(new IdentityUserRole() { UserId = NewUser.Id, RoleId = "a06674c1-6905-4364-85bf-9c3105b1ba49" });
                                }
                                else
                                {
                                    RefUser.Person.FirstName = viewmodel.FirstName;
                                    RefUser.Person.LastName = viewmodel.LastName;
                                    RefUser.Person.Email = viewmodel.Email;
                                    RefUser.Person.Cellphone = viewmodel.PhoneNumber;
                                    RefUser.Person.LastModifyDate = DateTime.Now;
                                    RefUser.Person.LastModifyUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault();
                                    NewUser = RefUser;
                                    NewUser.PhoneNumber = viewmodel.PhoneNumber;
                                    NewUser.CreateDate = DateTime.Now;
                                    NewUser.PasswordHash = new ApplicationUserManager(new UserStore<ApplicationUser>()).PasswordHasher.HashPassword(viewmodel.Password);
                                    NewUser.Person = RefUser.Person;
                                    RefUser.LastModifyDate = DateTime.Now;
                                    if (NewUser.Roles.Count(role => role.RoleId == "a06674c1-6905-4364-85bf-9c3105b1ba49") == 0)
                                        NewUser.Roles.Add(new IdentityUserRole() { UserId = NewUser.Id, RoleId = "a06674c1-6905-4364-85bf-9c3105b1ba49" });
                                }
                                if (context.Executors.Count(Exec => Exec.RefrenceUser.Id == NewUser.Id) == 0)
                                {
                                    context.Executors.Add(new Executor()
                                    {
                                        RefrenceUser = context.Users.Where(item => item.Id.Equals(NewUser.Id)).SingleOrDefault(),
                                        CreateDate = DateTime.Now,
                                        Enable = true,
                                        CreatorUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault(),
                                    });
                                }
                                context.SaveChanges();
                                dbContextTransaction.Commit();
                                EmailService.SendExecutorWelcome(NewUser.Id);
                            }
                            catch (Exception ex)
                            {
                                dbContextTransaction.Rollback();
                                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.Message);
                            }

                        }
                        else if (viewmodel.ObjectState == ObjectState.Update)//Update
                        {
                            var context = new ApplicationDbContext();
                            //if (context.Users.Where(item => item.UserName == viewmodel.UserName && viewmodel.RefrenceUserID != item.Id).Count() > 0)
                            //{
                            //    return ControllerHelper.ErrorResult("نام کاربری قبلا ثبت شده است");
                            //}
                            if (context.Users.Where(item => item.UserName == viewmodel.Email && viewmodel.RefrenceUserID != item.Id).Count() > 0)
                            {
                                return ControllerHelper.ErrorResult("ایمیل قبلا ثبت شده است");
                            }
                            Executor selectedModel = context.Executors
                                .Include(Executor => Executor.RefrenceUser)
                                .Include(Executor => Executor.RefrenceUser.Person)
                                .Where(item => item.ID == viewmodel.ID).Single();
                            var dbContextTransaction = context.Database.BeginTransaction();
                            try
                            {
                                if (selectedModel != null)
                                {
                                    selectedModel.ID = viewmodel.ID;
                                    selectedModel.RefrenceUser.UserName = viewmodel.Email;
                                    selectedModel.RefrenceUser.Email = viewmodel.Email;
                                    selectedModel.RefrenceUser.Person.FirstName = viewmodel.FirstName;
                                    selectedModel.RefrenceUser.Person.LastName = viewmodel.LastName;
                                    selectedModel.RefrenceUser.PhoneNumber = viewmodel.PhoneNumber;
                                    selectedModel.RefrenceUser.Person.Cellphone = viewmodel.PhoneNumber;
                                    selectedModel.LastModifyUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault();
                                    selectedModel.LastModifyDate = DateTime.Now;
                                }
                                context.SaveChanges();
                                dbContextTransaction.Commit();

                            }
                            catch (Exception ex)
                            {
                                dbContextTransaction.Rollback();
                                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.Message);
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.Message);
                    }
                    return ControllerHelper.SuccessResult("با موفقیت ثبت شد");
                }
                else
                {
                    return ControllerHelper.FormNotValid(ModelState);
                }

            }
            catch (Exception ex)
            {
                //return ControllerHelper.ErrorResult("بروز خطای سیستمی");
                return ControllerHelper.ErrorResult("بروز خطای سیستمی");
            }
        }
        [HttpPost]
        [AjaxValidateAntiForgeryTokenAttribute]
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Executor)]
        public ActionResult ActiveExecutor(string[] ids)
        {
            var context = new ApplicationDbContext();
            try
            {
                foreach (var id in ids)
                {
                    int CurrentID = int.Parse(id);
                    var selectedItem = context.Executors
                        .Include(Executor => Executor.RefrenceUser)
                        .Where(item => item.ID == CurrentID).Single();
                    if (selectedItem != null)
                    {
                        selectedItem.Enable = !selectedItem.Enable;
                        selectedItem.RefrenceUser.Enable = !selectedItem.Enable;
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
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Executor)]
        public ActionResult DeleteExecutor(string[] ids)
        {
            var context = new ApplicationDbContext();
            var dbContextTransaction = context.Database.BeginTransaction();
            try
            {
                foreach (var id in ids)
                {
                    int ID = int.Parse(id);
                    var selectedItem = context.Executors
                        .Include(Executor => Executor.RefrenceUser)
                        .Where(item => item.ID == ID).Single();
                    if (selectedItem != null)
                    {
                        ApplicationUser selectedUser = context.Users.Where(item => item.Id.Equals(selectedItem.RefrenceUser.Id)).SingleOrDefault();
                        foreach (var role in selectedUser.Roles.ToList<IdentityUserRole>())
                        {
                            selectedUser.Roles.Remove(role);
                        }
                        context.Executors.Remove(selectedItem);
                        context.Users.Remove(selectedUser);

                    }

                }
                context.SaveChanges();
                dbContextTransaction.Commit();
                return ControllerHelper.SuccessResult("با موفقیت ثبت شد");
            }
            catch (Exception ex)
            {
                dbContextTransaction.Rollback();
                return ControllerHelper.ErrorResult("بروز خطای سیستمی");
            }


        }
    }
}