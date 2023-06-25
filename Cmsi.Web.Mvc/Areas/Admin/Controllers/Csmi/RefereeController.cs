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
    public class RefereeController : BaseController
    {
        // GET: Admin/Referee
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Referee)]
        public ActionResult Referee()
        {
            if (Request.QueryString["id"] == null || Request.QueryString["id"] == "undefined")
            {
                return PartialView("~/Areas/Admin/Views/Csmi/Referee.cshtml", new RefereeViewModel() { ObjectState = ObjectState.Insert });
            }
            else
            {
                var context = new ApplicationDbContext();
                var selectedID = int.Parse(Request.QueryString["id"]);
                var selectedReferee = context.Referees
                    .Include(Referee => Referee.RefrenceUser)
                    .Include(Referee => Referee.RefrenceUser.Person)
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
                       item.University,
                       item.Degree,
                       item.Enable,
                   })
                    .SingleOrDefault();
                return PartialView("~/Areas/Admin/Views/Csmi/Referee.cshtml", new RefereeViewModel()
                {
                    ID = selectedReferee.ID,
                    CreateDateConverted = CommonHelper.DateAndTimes.GetPersianDate(selectedReferee.CreateDate),
                    Password = "*",
                    Email = selectedReferee.Email,
                    FirstName = selectedReferee.FirstName,
                    LastName = selectedReferee.LastName,
                    PhoneNumber = selectedReferee.Cellphone,
                    University = selectedReferee.University,
                    Degree = selectedReferee.Degree,
                    ImageUrl = selectedReferee.PersonalImageUrl,
                    RefrenceUserID = selectedReferee.UserID,
                    Enable = selectedReferee.Enable,
                    ObjectState = ObjectState.Update
                });
            }
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Referee)]
        public ActionResult RefereeManagement()
        {
            ViewBag.GridID = "RefereeManagementGrid";
            return PartialView("~/Areas/Admin/Views/Csmi/RefereeManagement.cshtml");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Referee)]
        public ActionResult GetRefereePartial()
        {
            return new GridViewPartialController().GridViewPartial<RefereeViewModel>(Bind(), "Referee", "GetRefereePartial", "RefereeManagementGrid", "ID");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Referee)]
        public ActionResult ExportTo(int ActionType)
        {
            return new GridViewPartialController().ExportTo<RefereeViewModel>(Bind(), ActionType, "PollManagementGrid");
        }
        List<RefereeViewModel> Bind()
        {
            var Referees = new List<RefereeViewModel>();
            var context = new ApplicationDbContext();
            var model = new object[0];
            var DbReferees = context.Referees
                .Include(Referee => Referee.RefrenceUser)
                .Include(Referee => Referee.RefrenceUser.Person)
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
                    item.University,
                    item.Degree,
                    item.Enable,
                })
                .ToList()
                .OrderByDescending(item => item.CreateDate);
            foreach (var item in DbReferees)
            {
                Referees.Add(new RefereeViewModel()
                {
                    ID = item.ID,
                    CreateDateConverted = CommonHelper.DateAndTimes.GetPersianDate(item.CreateDate),
                    Email = item.Email,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    PhoneNumber = item.Cellphone,
                    University = item.University,
                    Enable = item.Enable,
                    Degree = item.Degree,
                });
            }
            if (Referees.Count() == 0)
            {
                Referees.Add(new RefereeViewModel());
            }
            return Referees;
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Referee)]
        public ActionResult HtmlEditorPartial()
        {
            return new HtmlEditorPartialController().HtmlEditorView("HtmlEditor", "OnHtmlChangedChanged", "OnInit");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Referee)]
        [AjaxValidateAntiForgeryTokenAttribute]
        public ActionResult SubmitReferee(RefereeViewModel viewmodel)
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
                                .Include(user=>user.Roles)
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
                                        SecurityStamp = Guid.NewGuid().ToString(),
                                        UserName = viewmodel.Email,
                                        CreateDate = DateTime.Now,
                                        Enable = true,
                                        EmailConfirmed = true,
                                        PasswordHash = new ApplicationUserManager(new UserStore<ApplicationUser>()).PasswordHasher.HashPassword(viewmodel.Password),
                                        Person = person
                                    };
                                    context.Users.Add(NewUser);
                                    context.SaveChanges();
                                    NewUser.Roles.Add(new IdentityUserRole() { UserId = NewUser.Id, RoleId = "bd9edbb2-a2a5-4cc0-b7b9-927cb524983c" });
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
                                    if (NewUser.Roles.Count(role => role.RoleId == "bd9edbb2-a2a5-4cc0-b7b9-927cb524983c") == 0)
                                        NewUser.Roles.Add(new IdentityUserRole() { UserId = NewUser.Id, RoleId = "bd9edbb2-a2a5-4cc0-b7b9-927cb524983c" });
                                }
                                if (context.Referees.Count(Ref => Ref.RefrenceUser.Id == NewUser.Id) == 0)
                                {
                                    context.Referees.Add(new Referee()
                                    {
                                        RefrenceUser = context.Users.Where(item => item.Id.Equals(NewUser.Id)).SingleOrDefault(),
                                        CreateDate = DateTime.Now,
                                        Degree = viewmodel.Degree,
                                        University = viewmodel.University,
                                        Enable = true,
                                        CreatorUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault(),
                                    });
                                }
                                context.SaveChanges();
                                dbContextTransaction.Commit();
                                //حذف ایمیل به در خواست کاربر در تاریخ 1397/10/02
                                //EmailService.SendRefereeWelcome(NewUser.Id);
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
                            Referee selectedModel = context.Referees
                                .Include(Referee => Referee.RefrenceUser)
                                .Include(Referee => Referee.RefrenceUser.Person)
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
                                    selectedModel.University = viewmodel.University;
                                    selectedModel.Degree = viewmodel.Degree;
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
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Referee)]
        public ActionResult ActiveReferee(string[] ids)
        {
            var context = new ApplicationDbContext();
            try
            {
                foreach (var id in ids)
                {
                    int CurrentID = int.Parse(id);
                    var selectedItem = context.Referees
                        .Include(Referee => Referee.RefrenceUser)
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
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Referee)]
        public ActionResult DeleteReferee(string[] ids)
        {
            var context = new ApplicationDbContext();
            var dbContextTransaction = context.Database.BeginTransaction();
            try
            {
                foreach (var id in ids)
                {
                    int ID = int.Parse(id);
                    var selectedItem = context.Referees
                        .Include(Referee => Referee.RefrenceUser)
                        .Where(item => item.ID == ID).Single();
                    if (selectedItem != null)
                    {
                        ApplicationUser selectedUser = context.Users
                            .Include(item => item.Person)
                            .Where(item => item.Id.Equals(selectedItem.RefrenceUser.Id)).SingleOrDefault();
                        foreach (var role in selectedUser.Roles.ToList<IdentityUserRole>())
                        {
                            selectedUser.Roles.Remove(role);
                        }
                        context.Referees.Remove(selectedItem);
                        context.Persons.Remove(selectedUser.Person);
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