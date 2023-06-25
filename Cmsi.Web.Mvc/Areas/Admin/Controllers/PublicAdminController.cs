using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AVA.Web.Mvc.Models.Admin;
using AVA.UI.Helpers.Base;
using Microsoft.AspNet.Identity.EntityFramework;
using AVA.Core.Entities;
using AVA.UI.Helpers.CustomAttribute;
using AVA.UI.Helpers.Controller;
using AVA.UI.Helpers.Common;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity;
using AVA.UI.Helpers.MailSmsService;

namespace AVA.Web.Mvc.Controllers.Admin
{
    public class PublicAdminController : BaseController
    {
        #region Role
        //Optimized
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult Role()
        {
            if (Request.QueryString["id"] == null || Request.QueryString["id"] == "undefined")
            {
                return PartialView("~/Areas/Admin/Views/PublicAdmin/Role.cshtml", new RoleViewModel() { Name = "", ObjectState = ObjectState.Insert });
            }
            else
            {
                var context = new ApplicationDbContext();
                var selectedID = Request.QueryString["id"];
                var selectedRole = context.Roles
                    .Where(item => item.Id.Equals(selectedID))
                    .Select(role => new { role.Id, role.Name })
                    .FirstOrDefault();
                return PartialView("~/Areas/Admin/Views/PublicAdmin/Role.cshtml", new RoleViewModel() { ID = Request.QueryString["id"], Name = selectedRole.Name, ObjectState = ObjectState.Update });
            }
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult RoleManagement()
        {
            ViewBag.GridID = "RoleManagementGrid";
            return PartialView("~/Areas/Admin/Views/PublicAdmin/RoleManagement.cshtml");
        }
        [HttpPost]
        [AjaxValidateAntiForgeryTokenAttribute]
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult SubmitRole(RoleViewModel viewmodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {

                        if (viewmodel.ObjectState == ObjectState.Insert) // insert
                        {
                            IdentityRole Role = new IdentityRole(viewmodel.Name);
                            var context = new ApplicationDbContext();
                            if (context.Roles.Where(item => item.Name == viewmodel.Name).Count() > 0)
                            {
                                return ControllerHelper.ErrorResult("نقشی با این نام قبلا ثبت شده است");
                            }
                            context.Roles.Add(new AspNetRole
                            {
                                Id = Role.Id,
                                Name = viewmodel.Name,
                                CreateDate = DateTime.Now,
                                CreatorUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault()
                            });
                            context.SaveChanges();

                        }
                        else if (viewmodel.ObjectState == ObjectState.Update)//Update
                        {
                            if (viewmodel.ID != null)
                            {
                                var context = new ApplicationDbContext();
                                if (context.Roles.Where(item => item.Name == viewmodel.Name && viewmodel.ID != item.Id).Count() > 0)
                                {
                                    return ControllerHelper.ErrorResult("نقشی با این نام قبلا ثبت شده است");
                                }
                                AspNetRole selectedModel = (AspNetRole)context.Roles.Where(item => item.Id == viewmodel.ID).Single();
                                selectedModel.Name = viewmodel.Name;
                                selectedModel.LastModifyUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault();
                                selectedModel.LastModifyDate = DateTime.Now;
                                context.SaveChanges();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return ControllerHelper.ErrorResult("بروز خطای سیستمی");
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
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult GetRolesPartial()
        {
            return new GridViewPartialController().GridViewPartial<RoleViewModel>(BindRoles(), "PublicAdmin", "GetRolesPartial", "RoleManagementGrid", "ID");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult ExportRolesTo(int ActionType)
        {
            return new GridViewPartialController().ExportTo<RoleViewModel>(BindRoles(), ActionType, "RoleManagementGrid");
        }
        List<RoleViewModel> BindRoles()
        {
            var DbRoles = new List<RoleViewModel>();
            var context = new ApplicationDbContext();
            var model = new object[0];
            var Roles = context.Roles
                .Select(role => new { role.Id, role.Name })
                .ToList();
            foreach (var item in Roles)
            {
                DbRoles.Add(new RoleViewModel() { ID = item.Id, Name = item.Name });
            }
            if (DbRoles.Count() == 0)
            {
                DbRoles.Add(new RoleViewModel());
            }
            return DbRoles;
        }
        [HttpPost]
        [AjaxValidateAntiForgeryTokenAttribute]
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult DeleteRoles(string[] ids)
        {
            var context = new ApplicationDbContext();
            try
            {
                foreach (var id in ids)
                {
                    var selectedItem = context.Roles.Where(item => item.Id == id).Single();
                    if (selectedItem != null)
                        context.Roles.Remove(selectedItem);
                }
                context.SaveChanges();
                return ControllerHelper.SuccessResult("با موفقیت ثبت شد");
            }
            catch (Exception ex)
            {
                return ControllerHelper.ErrorResult("بروز خطای سیستمی");
            }


        }
        #endregion
        #region ContactUs
        //Optimized
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult ContactUsManagement()
        {
            ViewBag.GridID = "ContactUsManagementGrid";
            return PartialView("~/Areas/Admin/Views/PublicAdmin/ContactUsManagement.cshtml");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult GetContactUsGridPartial()
        {
            return new GridViewPartialController().GridViewPartial<ContactUsViewModel>(BindContactUs(), "PublicAdmin", "GetContactUsGridPartial", "ContactUsManagementGrid", "ID");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult ExportContactUsTo(int ActionType)
        {
            return new GridViewPartialController().ExportTo<ContactUsViewModel>(BindContactUs(), ActionType, "ContactUsManagementGrid");
        }
        List<ContactUsViewModel> BindContactUs()
        {
            var DbContactUs = new List<ContactUsViewModel>();
            var context = new ApplicationDbContext();
            var model = new object[0];
            var contacts = context.ContactUses
                .Select(contact => new
                {
                    contact.ID,
                    contact.Name,
                    contact.Family,
                    contact.Email,
                    contact.TelNumber,
                    contact.Text,
                    contact.CreateDate
                })
                .ToList();
            foreach (var item in contacts)
            {
                DbContactUs.Add(new ContactUsViewModel()
                {
                    ID = item.ID,
                    Name = item.Name,
                    Family = item.Family,
                    Email = item.Email,
                    TelNumber = item.TelNumber,
                    Text = item.Text,
                    ContactDateConverted = CommonHelper.DateAndTimes.GetPersianDate(item.CreateDate)
                });
            }
            if (DbContactUs.Count() == 0)
            {
                DbContactUs.Add(new ContactUsViewModel());
            }
            return DbContactUs;
        }
        [HttpPost]
        [AjaxValidateAntiForgeryTokenAttribute]
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult DeleteContactUs(string[] ids)
        {
            var context = new ApplicationDbContext();
            try
            {
                foreach (var id in ids)
                {
                    var CurrentID = long.Parse(id);
                    var selectedItem = context.ContactUses.Where(item => item.ID == CurrentID).FirstOrDefault();
                    if (selectedItem != null)
                        context.ContactUses.Remove(selectedItem);
                }
                context.SaveChanges();
                return ControllerHelper.SuccessResult("با موفقیت ثبت شد");
            }
            catch (Exception ex)
            {
                return ControllerHelper.ErrorResult("بروز خطای سیستمی");
            }


        }
        #endregion
        #region DynamicPage
        //Optimized
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult HtmlEditorPartial()
        {
            return new HtmlEditorPartialController().HtmlEditorView("HtmlEditor", "OnHtmlChangedChanged", "OnInit");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult EnglishContextHtmlEditorPartial()
        {
            return new HtmlEditorPartialController().HtmlEditorView("EnglishContextHtmlEditor", "OnEnglishContextHtmlChangedChanged", "OnEnglishContextInit");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult DynamicPage()
        {
            if (Request.QueryString["id"] == null || Request.QueryString["id"] == "undefined")
            {
                return PartialView("~/Areas/Admin/Views/PublicAdmin/DynamicPage.cshtml", new DynamicPageViewModel() { ObjectState = ObjectState.Insert });
            }
            else
            {
                var context = new ApplicationDbContext();
                var selectedID = int.Parse(Request.QueryString["id"]);
                var selectedDynamicPage = context.DynamicPages
                    .Include(item => item.DynamicPageContents)
                    .Include(item => item.DynamicPageContents.Select(contentPage => contentPage.Language))
                    .Where(item => item.ID.Equals(selectedID))
                    .Select(page => new
                    {
                        page.ID,
                        page.Title,
                        page.Priority,
                        page.Enable,
                        page.CreateDate,
                        Context = page.DynamicPageContents.Where(content => content.Language.Value == "fa").FirstOrDefault().Context,
                        EnglishContext = page.DynamicPageContents.Where(content => content.Language.Value == "en").FirstOrDefault().Context,
                    }).FirstOrDefault();
                return PartialView("~/Areas/Admin/Views/PublicAdmin/DynamicPage.cshtml", new DynamicPageViewModel()
                {
                    ID = selectedDynamicPage.ID,
                    Title = selectedDynamicPage.Title,
                    Priority = selectedDynamicPage.Priority,
                    Context = selectedDynamicPage.Context,
                    EnglishContext = selectedDynamicPage.EnglishContext,
                    ObjectState = ObjectState.Update
                });
            }
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult DynamicPageManagement()
        {
            ViewBag.GridID = "DynamicPageManagementGrid";
            return PartialView("~/Areas/Admin/Views/PublicAdmin/DynamicPageManagement.cshtml");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult GetDynamicPageGridPartial()
        {
            return new GridViewPartialController().GridViewPartial<DynamicPageViewModel>(BindDynamicPage(), "PublicAdmin", "GetDynamicPageGridPartial", "DynamicPageManagementGrid", "ID");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult ExportDynamicPagetTo(int ActionType)
        {
            return new GridViewPartialController().ExportTo<DynamicPageViewModel>(BindDynamicPage(), ActionType, "DynamicPageManagementGrid");
        }
        List<DynamicPageViewModel> BindDynamicPage()
        {
            var DbDynamicPage = new List<DynamicPageViewModel>();
            var context = new ApplicationDbContext();
            var model = new object[0];
            var pages = context.DynamicPages
                .Select(page => new
                {
                    page.ID,
                    page.Title,
                    page.Priority,
                    page.Enable,
                    page.CreateDate
                }).ToList();
            foreach (var item in pages)
            {
                DbDynamicPage.Add(new DynamicPageViewModel()
                {
                    ID = item.ID,
                    Title = item.Title,
                    Enable = item.Enable,
                    //Keyword = item.Keyword,
                    Priority = item.Priority,
                    DynamicPageDateConverted = CommonHelper.DateAndTimes.GetPersianDate(item.CreateDate)
                });
            }
            if (DbDynamicPage.Count() == 0)
            {
                DbDynamicPage.Add(new DynamicPageViewModel());
            }
            return DbDynamicPage;
        }
        [HttpPost]
        [AjaxValidateAntiForgeryTokenAttribute]
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult DeleteDynamicPage(string[] ids)
        {
            var context = new ApplicationDbContext();
            try
            {
                foreach (var id in ids)
                {
                    var CurrentID = long.Parse(id);
                    var selectedItem = context.DynamicPages
                        .Include(item => item.DynamicPageContents)
                        .Where(item => item.ID == CurrentID && item.AllowDelete == true).FirstOrDefault();
                    if (selectedItem != null)
                    {
                        context.DynamicPageContents.RemoveRange(selectedItem.DynamicPageContents);
                        context.DynamicPages.Remove(selectedItem);
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
        [ValidateInput(false)]
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult SubmitDynamicPage(DynamicPageViewModel viewmodel)
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
                            var trans = context.Database.BeginTransaction();
                            try
                            {
                                var dynamicPage = new DynamicPage()
                                {
                                    ID = viewmodel.ID,
                                    Title = viewmodel.Title,
                                    AllowDelete = true,
                                    Enable = true,
                                    Priority = 0,//viewmodel.Priority,            
                                    CreateDate = DateTime.Now,
                                    CreatorUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault(),
                                };
                                dynamicPage.DynamicPageContents.Add(new Core.Entities.DynamicPageContent()
                                {
                                    AllowDelete = true,
                                    Context = viewmodel.Context,
                                    Description = viewmodel.Description,
                                    Keyword = viewmodel.Keyword,
                                    Title = viewmodel.Title,
                                    Priority = viewmodel.Priority,
                                    Enable = true,
                                    Language = context.Languages.Where(lang => lang.Value == "fa").SingleOrDefault(),
                                    //DynamicPage = dynamicPage,
                                    CreateDate = DateTime.Now,
                                    CreatorUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault(),
                                });
                                dynamicPage.DynamicPageContents.Add(new Core.Entities.DynamicPageContent()
                                {
                                    AllowDelete = true,
                                    Context = viewmodel.Context,
                                    Description = viewmodel.Description,
                                    Keyword = viewmodel.Keyword,
                                    Title = viewmodel.Title,
                                    Priority = viewmodel.Priority,
                                    Enable = true,
                                    Language = context.Languages.Where(lang => lang.Value == "en").SingleOrDefault(),
                                    //DynamicPage = dynamicPage,
                                    CreateDate = DateTime.Now,
                                    CreatorUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault(),
                                });

                                context.DynamicPages.Add(dynamicPage);
                                context.SaveChanges();

                                //context.SaveChanges();
                                trans.Commit();
                            }
                            catch (Exception ex)
                            {
                                trans.Rollback();
                                return ControllerHelper.ErrorResult("بروز خطای سیستمی");
                            }


                        }
                        else if (viewmodel.ObjectState == ObjectState.Update)//Update
                        {
                            var context = new ApplicationDbContext();
                            var trans = context.Database.BeginTransaction();
                            try
                            {

                                DynamicPage selectedModel = context.DynamicPages
                                     .Include(item => item.DynamicPageContents)
                                     .Include(item => item.DynamicPageContents.Select(contentPage => contentPage.Language))
                                     .Include(item => item.DynamicPageContents.Select(contentPage => contentPage.CreatorUser))
                                    .Where(item => item.ID == viewmodel.ID).Single();
                                if (selectedModel != null)
                                {
                                    selectedModel.Title = viewmodel.Title;
                                    selectedModel.Priority = viewmodel.Priority;
                                    selectedModel.LastModifyUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault();
                                    selectedModel.LastModifyDate = DateTime.Now;
                                    var EnglishContent = selectedModel.DynamicPageContents
                                        .Where(pageContent => pageContent.Language.Value == "en").SingleOrDefault();
                                    var Content = selectedModel.DynamicPageContents
                                        .Where(pageContent => pageContent.Language.Value == "fa").SingleOrDefault();

                                    EnglishContent.Context = viewmodel.EnglishContext;
                                    EnglishContent.LastModifyDate = DateTime.Now;
                                    EnglishContent.LastModifyUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault();
                                    //Description = viewmodel.Description,
                                    //Keyword = viewmodel.Keyword,
                                    //Title = viewmodel.Title,
                                    Content.Context = viewmodel.Context;
                                    Content.LastModifyDate = DateTime.Now;
                                    Content.LastModifyUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault();

                                }
                                context.SaveChanges();
                                trans.Commit();
                            }
                            catch (Exception ex)
                            {
                                trans.Rollback();
                                return ControllerHelper.ErrorResult("بروز خطای سیستمی");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return ControllerHelper.ErrorResult("بروز خطای سیستمی");
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
        #endregion
        #region DynamicPageContent
        //Optimized        
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult DynamicPageContent()
        {
            if (Request.QueryString["id"] == null || Request.QueryString["id"] == "undefined")
            {
                return PartialView("~/Areas/Admin/Views/PublicAdmin/DynamicPage.cshtml", new DynamicPageViewModel() { ObjectState = ObjectState.Insert });
            }
            else
            {
                var context = new ApplicationDbContext();
                var selectedID = int.Parse(Request.QueryString["id"]);
                var selectedDynamicPage = context.DynamicPageContents
                    .Where(item => item.ID.Equals(selectedID))
                    .Select(page => new
                    {
                        page.ID,
                        page.Title,
                        page.Priority,
                        page.Description,
                        page.Enable,
                        page.CreateDate,
                        page.Context,
                        page.Keyword
                    }).FirstOrDefault();
                return PartialView("~/Areas/Admin/Views/PublicAdmin/DynamicPage.cshtml", new DynamicPageViewModel()
                {
                    ID = selectedDynamicPage.ID,
                    Title = selectedDynamicPage.Title,
                    Context = selectedDynamicPage.Context,
                    Keyword = selectedDynamicPage.Keyword,
                    Description = selectedDynamicPage.Description,
                    Priority = selectedDynamicPage.Priority,
                    ObjectState = ObjectState.Update
                });
            }
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult DynamicPageContentManagement()
        {
            ViewBag.GridID = "DynamicPageManagementGrid";
            return PartialView("~/Areas/Admin/Views/PublicAdmin/DynamicPageManagement.cshtml");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult GetDynamicPageContentGridPartial()
        {
            return new GridViewPartialController().GridViewPartial<DynamicPageViewModel>(BindDynamicPageContent(), "PublicAdmin", "GetDynamicPageGridPartial", "DynamicPageManagementGrid", "ID");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult ExportDynamicPageContentTo(int ActionType)
        {
            return new GridViewPartialController().ExportTo<DynamicPageViewModel>(BindDynamicPageContent(), ActionType, "DynamicPageManagementGrid");
        }
        List<DynamicPageViewModel> BindDynamicPageContent()
        {
            var DbDynamicPage = new List<DynamicPageViewModel>();
            var context = new ApplicationDbContext();
            var model = new object[0];
            var pages = context.DynamicPageContents
                .Select(page => new
                {
                    page.ID,
                    page.Title,
                    page.Priority,
                    page.Description,
                    page.Enable,
                    page.CreateDate
                }).ToList();
            foreach (var item in pages)
            {
                DbDynamicPage.Add(new DynamicPageViewModel()
                {
                    ID = item.ID,
                    Title = item.Title,
                    //Context = item.Context,
                    //CreateDate = item.CreateDate,
                    //DefaultContext = item.DefaultContext,
                    Description = item.Description,
                    Enable = item.Enable,
                    //Keyword = item.Keyword,
                    Priority = item.Priority,
                    DynamicPageDateConverted = CommonHelper.DateAndTimes.GetPersianDate(item.CreateDate)
                });
            }
            if (DbDynamicPage.Count() == 0)
            {
                DbDynamicPage.Add(new DynamicPageViewModel());
            }
            return DbDynamicPage;
        }
        [HttpPost]
        [AjaxValidateAntiForgeryTokenAttribute]
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult DeleteDynamicPageContent(string[] ids)
        {
            var context = new ApplicationDbContext();
            try
            {
                foreach (var id in ids)
                {
                    var CurrentID = long.Parse(id);
                    var selectedItem = context.DynamicPages.Where(item => item.ID == CurrentID && item.AllowDelete == true).FirstOrDefault();
                    if (selectedItem != null)
                        context.DynamicPages.Remove(selectedItem);
                }
                context.SaveChanges();
                return ControllerHelper.SuccessResult("با موفقیت ثبت شد");
            }
            catch (Exception ex)
            {
                return ControllerHelper.ErrorResult("بروز خطای سیستمی");
            }


        }
        [ValidateInput(false)]
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult SubmitDynamicPageContent(DynamicPageViewModel viewmodel)
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
                            context.DynamicPageContents.Add(new DynamicPageContent()
                            {
                                ID = viewmodel.ID,
                                Title = viewmodel.Title,
                                Context = viewmodel.Context,
                                AllowDelete = true,
                                Enable = true,
                                Keyword = viewmodel.Keyword,
                                Description = viewmodel.Description,
                                Priority = viewmodel.Priority,
                                CreateDate = DateTime.Now,
                                CreatorUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault(),
                            });
                            context.SaveChanges();

                        }
                        else if (viewmodel.ObjectState == ObjectState.Update)//Update
                        {
                            var context = new ApplicationDbContext();
                            var selectedModel = context.DynamicPageContents.Where(item => item.ID == viewmodel.ID).Single();
                            if (selectedModel != null)
                            {
                                selectedModel.Title = viewmodel.Title;
                                selectedModel.Context = viewmodel.Context;
                                selectedModel.Keyword = viewmodel.Keyword;
                                selectedModel.Description = viewmodel.Description;
                                selectedModel.Priority = viewmodel.Priority;
                                selectedModel.LastModifyUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault();
                                selectedModel.LastModifyDate = DateTime.Now;
                            }
                            context.SaveChanges();
                        }
                    }
                    catch (Exception ex)
                    {
                        return ControllerHelper.ErrorResult("بروز خطای سیستمی");
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
        #endregion
        #region User
        //Optimized
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult UserView()
        {
            if (Request.QueryString["id"] == null || Request.QueryString["id"] == "undefined")
            {
                return PartialView("~/Areas/Admin/Views/PublicAdmin/UserView.cshtml", new AspUserViewModel() { ObjectState = ObjectState.Insert });
            }
            else
            {
                var context = new ApplicationDbContext();
                var selectedID = Request.QueryString["id"];
                var selectedUser = context.Users
                    .Include(item => item.Person)
                    .Where(item => item.Id.Equals(selectedID))
                    .Select(user => new
                    {
                        user.Id,
                        user.Person.FirstName,
                        user.Person.LastName,
                        user.UserName,
                        user.Enable,
                        user.Email,
                        user.Person.Cellphone,
                        user.CreateDate
                    })
                    .FirstOrDefault();
                return PartialView("~/Areas/Admin/Views/PublicAdmin/UserView.cshtml", new AspUserViewModel()
                {
                    ID = Request.QueryString["id"],
                    FirstName = selectedUser.FirstName,
                    LastName = selectedUser.LastName,
                    Email = selectedUser.Email,
                    UserName = selectedUser.UserName,
                    PhoneNumber = selectedUser.Cellphone,
                    ObjectState = ObjectState.Update
                });
            }
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult UserManagement()
        {
            ViewBag.GridID = "UserManagementGrid";
            return PartialView("~/Areas/Admin/Views/PublicAdmin/UserManagement.cshtml");
        }
        [HttpPost]
        [AjaxValidateAntiForgeryTokenAttribute]
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult SubmitUser(AspUserViewModel viewmodel)
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
                            if (context.Users.Where(item => item.UserName == viewmodel.UserName).Count() > 0)
                            {
                                return ControllerHelper.ErrorResult("نام کاربری قبلا ثبت شده است");
                            }
                            if (context.Users.Where(item => item.Email == viewmodel.Email).Count() > 0)
                            {
                                return ControllerHelper.ErrorResult("ایمیل قبلا ثبت شده است");
                            }
                            var trans = context.Database.BeginTransaction();
                            try
                            {

                                var person = context.Persons.Add(new Person()
                                {
                                    FirstName = viewmodel.FirstName,
                                    LastName = viewmodel.LastName,
                                    Email = viewmodel.Email,
                                    Cellphone = viewmodel.PhoneNumber,
                                    CreateDate = DateTime.Now,
                                    CreatorUser = context.Users.Where(users => users.UserName == User.Identity.Name).SingleOrDefault()
                                });
                                context.SaveChanges();
                                var user = new ApplicationUser
                                {
                                    Email = viewmodel.Email,
                                    PhoneNumber = viewmodel.PhoneNumber,
                                    UserName = viewmodel.UserName,
                                    CreateDate = DateTime.Now,
                                    Enable = true,
                                    EmailConfirmed = true,
                                    PasswordHash = new ApplicationUserManager(new UserStore<ApplicationUser>()).PasswordHasher.HashPassword(viewmodel.Password),
                                    SecurityStamp = Guid.NewGuid().ToString("D"),
                                    Person = person
                                    //CreatorUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault(),
                                };
                                context.Users.Add(user);
                                context.SaveChanges();
                                trans.Commit();
                                //var code = UserManager.GenerateEmailConfirmationToken(user.Id);
                                //var callbackUrl = "http://cmsi.ir/UI/ConfirmEmail?userId=" + user.Id + "&code=" + code;
                                //EmailService.EmailResponse MailResult = EmailService.SendWelcome(user.Id, callbackUrl);
                                //if (MailResult.Code == (int)EmailService.EmailEnum.Success)
                                //{
                                //    return ControllerHelper.SuccessResult("ثبت نام با موفقیت انجام شد. لطفا برای فعال سازی اکانت به ایمیل فعال سازی که برای شما ارسال شده است مراجعه فرمایید.در صورت نبودن ایمیل اسپم لیست خود را بررسی بفرمایید");
                                //}
                                //else
                                //    return ControllerHelper.ErrorResult("بروز خطای سیستمی در ارسال ایمیل فعال سازی");
                            }
                            catch (Exception ex)
                            {
                                trans.Rollback();
                                return ControllerHelper.ErrorResult("بروز خطای سیستمی" + ex.Message);
                            }

                        }
                        else if (viewmodel.ObjectState == ObjectState.Update)//Update
                        {
                            if (viewmodel.ID != null)
                            {
                                var context = new ApplicationDbContext();
                                if (context.Users.Where(item => item.UserName == viewmodel.UserName && viewmodel.ID != item.Id).Count() > 0)
                                {
                                    return ControllerHelper.ErrorResult("نام کاربری قبلا ثبت شده است");
                                }
                                if (context.Users.Where(item => item.UserName == viewmodel.Email && viewmodel.ID != item.Id).Count() > 0)
                                {
                                    return ControllerHelper.ErrorResult("ایمیل قبلا ثبت شده است");
                                }
                                ApplicationUser selectedModel = context.Users
                                    .Include(item => item.Person)
                                    .Where(item => item.Id == viewmodel.ID).Single();
                                selectedModel.Id = viewmodel.ID;
                                selectedModel.Person.FirstName = viewmodel.FirstName;
                                selectedModel.Person.LastName = viewmodel.LastName;
                                // if (!selectedModel.EmailConfirmed)
                                selectedModel.Email = viewmodel.Email;
                                selectedModel.PhoneNumber = viewmodel.PhoneNumber;
                                selectedModel.UserName = viewmodel.UserName;
                                if (viewmodel.Password != "")
                                {
                                    selectedModel.PasswordHash = new ApplicationUserManager(new UserStore<ApplicationUser>()).PasswordHasher.HashPassword(viewmodel.Password);
                                }
                                //selectedModel.LastModifyUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault();
                                //selectedModel.LastModifyDate = DateTime.Now;
                                context.SaveChanges();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return ControllerHelper.ErrorResult("بروز خطای سیستمی");
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
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult GetUsersPartial()
        {
            return new GridViewPartialController().GridViewPartial<AspUserViewModel>(BindUsers(), "PublicAdmin", "GetUsersPartial", "UserManagementGrid", "ID");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult ExportUsersTo(int ActionType)
        {
            return new GridViewPartialController().ExportTo<AspUserViewModel>(BindUsers(), ActionType, "UserManagementGrid");
        }
        List<AspUserViewModel> BindUsers()
        {
            var DbUsers = new List<AspUserViewModel>();
            var context = new ApplicationDbContext();
            var model = new object[0];
            var users = context.Users
                .Include(item => item.Person)
                .Include(item => item.Roles)
                .Select(user => new
                {
                    user.Id,
                    user.Person.FirstName,
                    user.Person.LastName,
                    user.UserName,
                    user.Enable,
                    user.Email,
                    user.CreateDate,
                    RoleIDs = user.Roles.Select(userRole => userRole.RoleId)
                })
                .ToList();
            var Roles = context.Roles.Select(role => new { role.Name, role.Id }).ToList();
            foreach (var item in users)
            {
                DbUsers.Add(new AspUserViewModel()
                {
                    ID = item.Id,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    UserName = item.UserName,
                    Email = item.Email,
                    Enable = item.Enable,
                    UserRoles = string.Join(",", Roles.Where(role => item.RoleIDs.Contains(role.Id)).Select(role => role.Name).ToList()),
                    CreateDateConverted = CommonHelper.DateAndTimes.GetPersianDate(item.CreateDate)
                });
            }
            if (DbUsers.Count() == 0)
            {
                DbUsers.Add(new AspUserViewModel());
            }
            return DbUsers;

        }
        [HttpPost]
        [AjaxValidateAntiForgeryTokenAttribute]
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult DeleteUsers(string[] ids)
        {
            var context = new ApplicationDbContext();
            try
            {
                foreach (var id in ids)
                {
                    var selectedItem = context.Users.Where(item => item.Id == id).Single();
                    if (selectedItem != null)
                        context.Users.Remove(selectedItem);
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
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult ActiveUsers(string[] ids)
        {
            var context = new ApplicationDbContext();
            try
            {
                foreach (var id in ids)
                {
                    var selectedItem = context.Users.Where(item => item.Id == id).Single();
                    if (selectedItem != null)
                        selectedItem.Enable = !selectedItem.Enable;
                }
                context.SaveChanges();
                return ControllerHelper.SuccessResult("با موفقیت ثبت شد");
            }
            catch (Exception ex)
            {
                return ControllerHelper.ErrorResult("بروز خطای سیستمی");
            }


        }
        #endregion
        #region User Role
        //Optimized
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult UserRole()
        {
            if (Request.QueryString["id"] == null || Request.QueryString["id"] == "undefined")
            {
                return PartialView("~/Areas/Admin/Views/PublicAdmin/UserRole.cshtml", new UserRoleViewModel() { ObjectState = ObjectState.Insert });
            }
            else
            {
                var context = new ApplicationDbContext();
                var selectedID = Request.QueryString["id"];
                ApplicationUser selectedUser = context.Users.Include(user => user.Roles).Where(item => item.Id.Equals(selectedID)).FirstOrDefault();
                var UserRoles = new List<RoleUserViewModel>();
                var roles = context.Roles
                     .Select(role => new { role.Id, role.Name });
                foreach (var item in roles)
                {
                    var selectedRole = selectedUser.Roles
                        .Where(role => role.RoleId == item.Id)
                        .SingleOrDefault();
                    if (selectedRole != null)
                        UserRoles.Add(new RoleUserViewModel() { ID = item.Id, Name = item.Name });
                    //else
                    //    UserRoles.Add(new RoleUserViewModel() { ID = item.Id, Name = item.Name, HasRole = false });
                }
                return PartialView("~/Areas/Admin/Views/PublicAdmin/UserRole.cshtml", new UserRoleViewModel()
                {
                    UserID = selectedUser.Id,
                    Roles = UserRoles,
                    RolesFeed = new List<RoleUserViewModel>(),
                    ObjectState = ObjectState.Update
                });
            }
        }
        [HttpPost]
        [AjaxValidateAntiForgeryTokenAttribute]
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult SubmitUserRole(UserRoleViewModel viewmodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        if (viewmodel.ObjectState == ObjectState.Update)//Update
                        {

                            var context = new ApplicationDbContext();
                            ApplicationUser selectedUser = context.Users
                                .Include(item => item.Roles)
                                .Where(item => item.Id.Equals(viewmodel.UserID)).FirstOrDefault();
                            selectedUser.Roles.Clear();
                            foreach (var role in viewmodel.Roles)
                            {
                                selectedUser.Roles.Add(new IdentityUserRole() { UserId = viewmodel.UserID, RoleId = role.ID });
                            }
                            context.SaveChanges();
                            //if (viewmodel.Roles.Count(item => item.ID == "0884e067-4f00-4d0e-93b6-a5bdc106b1d6") > 0)
                            //{
                            //    var callbackUrl = System.Configuration.ConfigurationManager.AppSettings["WebsiteURL"] + "/UI/Login";
                            //    EmailService.EmailResponse MailResult = EmailService.SendElectionInvitation(viewmodel.UserID, callbackUrl);
                            //    if (MailResult.Code != (int)EmailService.EmailEnum.Success)
                            //    {
                            //        return ControllerHelper.ErrorResult("بروز خطای سیستمی در ارسال ایمیل فعال سازی");

                            //    }
                            //}
                        }
                    }
                    catch (Exception ex)
                    {
                        return ControllerHelper.ErrorResult("بروز خطای سیستمی");
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
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        //[AjaxValidateAntiForgeryTokenAttribute]
        public ActionResult UserRoleFeed(string uid, Select2InputViewModel Input)
        {
            var context = new ApplicationDbContext();
            //ApplicationUser selectedUser = context.Users.Where(item => item.Id.Equals(uid)).FirstOrDefault();
            var UserRoles = new List<RoleUserViewModel>();
            var roles = context.Roles
                .Where(role => role.Name.Contains(Input.Name) || string.IsNullOrEmpty(Input.Name))
                .Select(role => new { role.Id, role.Name })
                .Take(20).ToList();
            foreach (var item in roles)
            {
                //if (Input.Roles == null)
                //if (Input.Roles.Where(selectedRoles => selectedRoles.ID == item.Id).ToList().Count == 0)
                UserRoles.Add(new RoleUserViewModel() { ID = item.Id, Name = item.Name });
            }
            return Json(UserRoles, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region MailTemplate
        //Optimized
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult MailTemplate()
        {
            var context = new ApplicationDbContext();
            List<SelectListItem> DprItems = new List<SelectListItem>();
            var MailTemplates = new List<MailTemplateVmItem>();
            var templates = context.MailTemplates
                .Select(temp => new { temp.ID, temp.Description, temp.SMS, temp.Subject, temp.Template, temp.Params })
                .ToList();
            foreach (var item in templates)
            {
                var ThemeParams = new List<object>();
                foreach (var param in item.Params.Split(','))
                {
                    if (param != "")
                        ThemeParams.Add(new { text = param });
                }
                MailTemplates.Add(new MailTemplateVmItem()
                {
                    ID = item.ID,
                    Description = item.Description,
                    Params = Json(ThemeParams, JsonRequestBehavior.AllowGet),
                    SMS = item.SMS,
                    Subject = item.Subject,
                    Template = item.Template,
                    ObjectState = ObjectState.Update
                });
                DprItems.Add(new SelectListItem() { Value = item.ID.ToString(), Text = item.Description });

            }
            if (Request.QueryString["id"] == null || Request.QueryString["id"] == "undefined")
            {

                return PartialView("~/Areas/Admin/Views/PublicAdmin/MailTemplate.cshtml", new MailTemplateViewModel()
                {
                    TotalMailTemplate = MailTemplates,
                    SingleMailTemplate = MailTemplates[0],
                    CurrentID = 0,
                    DprItems = DprItems.ToArray(),
                    SelectedID = DprItems[0].Value
                });
            }
            else
            {
                var selectedID = int.Parse(Request.QueryString["id"]);
                return PartialView("~/Areas/Admin/Views/PublicAdmin/MailTemplate.cshtml", new MailTemplateViewModel()
                {
                    TotalMailTemplate = MailTemplates,
                    SingleMailTemplate = MailTemplates[0],
                    CurrentID = selectedID,
                    DprItems = DprItems.ToArray(),
                    SelectedID = Request.QueryString["id"]
                });
            }
        }
        [HttpPost]
        [AjaxValidateAntiForgeryTokenAttribute]
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        [ValidateInput(false)]
        public ActionResult SubmitMailTemplate(MailTemplateViewModel viewmodel)
        {

            #region Authentication Guard
            base.CheckForAuthentication();
            #endregion Authentication Guard

            #region Authorization Guard
            base.CheckForAuthorization();
            #endregion Authorization Guard

            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var context = new ApplicationDbContext();
                        foreach (var theme in viewmodel.TotalMailTemplate)
                        {
                            MailTemplate selectedMailTemplate = context.MailTemplates.Where(item => item.ID.Equals(theme.ID)).FirstOrDefault();
                            selectedMailTemplate.SMS = theme.SMS;
                            selectedMailTemplate.Subject = theme.Subject;
                            selectedMailTemplate.Template = theme.Template;
                        }
                        context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        return ControllerHelper.ErrorResult("بروز خطای سیستمی");
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
        #endregion
        #region Membership
        //Optimized
        [HttpPost]
        [AllowAnonymous]
        [AjaxValidateAntiForgeryTokenAttribute]
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.PublicMethod)]
        public ActionResult SignIn(MembershipViewModel viewmodel)
        {
            if (User.Identity.IsAuthenticated)
            {
                return PartialView("~/Areas/Admin/Views/PublicAdmin/Home.cshtml");
            }
            else
            {

                if (!ModelState.IsValid)
                {
                    return PartialView(viewmodel);
                }
                var contex = new ApplicationDbContext();
                var CurrentUser = contex.Users
                    .Include(user => user.Roles)
                    .Where(user => user.UserName == viewmodel.Username).SingleOrDefault();
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, change to shouldLockout: true
                var result = SignInManager.PasswordSignIn(viewmodel.Username, viewmodel.Password, viewmodel.RememberMe, shouldLockout: false);
                switch (result)
                {
                    case SignInStatus.Success:
                        {
                            if (CurrentUser.Roles.Count == 0)
                            {
                                ModelState.AddModelError("", "کاربری شما اجازه ورود به این قسمت را ندارد");
                                return ControllerHelper.ModelStateInvalidResult(ModelState);
                            }
                            foreach (var role in CurrentUser.Roles)
                            {
                                string RoleName = contex.Roles.Where(mrole => mrole.Id == role.RoleId).SingleOrDefault().Name;
                                UserManager.AddToRole(CurrentUser.Id, RoleName);

                            }

                            if (CurrentUser.Enable)
                                return ControllerHelper.SuccessResult("1");
                            else
                            {
                                ModelState.AddModelError("", "کاربری شما غیر فعال گردیده است لطفا با مدیر تماس بگیرید");
                                return ControllerHelper.ModelStateInvalidResult(ModelState);
                            }
                        }
                    case SignInStatus.LockedOut:
                        return PartialView("Lockout");
                    case SignInStatus.RequiresVerification:
                        return RedirectToAction("SendCode", new { ReturnUrl = "", RememberMe = viewmodel.RememberMe });
                    case SignInStatus.Failure:
                    default:
                        ModelState.AddModelError("", "نام کاربری یا رمز عبور اشتباه است");
                        return ControllerHelper.ModelStateInvalidResult(ModelState);
                }


            }
        }
        [HttpPost]
        [AjaxValidateAntiForgeryTokenAttribute]
        public ActionResult SignOut()
        {
            base.AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return ControllerHelper.SuccessResult("1");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.PublicMethod)]
        public ActionResult ChangePass()
        {
            return PartialView("~/Areas/Admin/Views/PublicAdmin/ChangePass.cshtml", new PasswordChangeMembershipViewModel());
        }
        [Authorize]
        [AjaxValidateAntiForgeryTokenAttribute]
        public ActionResult UserChangePass(Models.Admin.PasswordChangeMembershipViewModel viewmodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {

                        var context = new ApplicationDbContext();
                        //User.Identity.Name
                        var selectedModel = context.Users
                            .Where(item => item.UserName == User.Identity.Name)
                            .Single();
                        selectedModel.PasswordHash = new ApplicationUserManager(new UserStore<ApplicationUser>()).PasswordHasher.HashPassword(viewmodel.Password);
                        context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        return ControllerHelper.ErrorResult("بروز خطای سیستمی");
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

        #endregion
        #region ConferenceCategories
        //Optimized
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult ConferenceCategory(string TableName, string ParentID)
        {
            ViewBag.TableName = TableName;
            ViewBag.ParentID = ParentID;
            if (Request.QueryString["id"] == null || Request.QueryString["id"] == "undefined")
            {
                return PartialView("~/Areas/Admin/Views/Csmi/ConferenceCategory.cshtml", new CategoryViewModel() { ObjectState = ObjectState.Insert });
            }
            else
            {
                ViewBag.TableName = "Conference";
                ViewBag.ParentID = "-1";
                var context = new ApplicationDbContext();
                var selectedID = int.Parse(Request.QueryString["id"]);
                var selectedModel =
                     context.Categories.Include(cat => cat.Parent)
                    .Where(item => item.ID.Equals(selectedID))
                    .Select(item => new
                    {
                        item.ID,
                        item.Name,
                        item.Enable,
                        item.priority,
                        item.Parent
                    })
                    .FirstOrDefault();
                return PartialView("~/Areas/Admin/Views/Csmi/ConferenceCategory.cshtml", new CategoryViewModel()
                {
                    ID = int.Parse(Request.QueryString["id"]),
                    Name = selectedModel.Name,
                    TableName = TableName,
                    Enable = selectedModel.Enable,
                    priority = selectedModel.priority,
                    ParentID = (selectedModel.Parent != null) ? selectedModel.Parent.ID : -1,
                    ObjectState = ObjectState.Update
                });
            }
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult ConferenceCategoryManagement(string TableName, string ParentID)
        {
            ViewBag.GridID = "CategoryManagementGrid";
            ViewBag.TableName = TableName;
            ViewBag.ParentID = ParentID;

            ViewBag.BreadCrump = RecersiveNodeGenerate(int.Parse(ParentID), "<li class='active'><a ui-sref=\"ConferenceCategoryManagement({TableName: '" + TableName + "', ParentID: '@CatID@'})\"><strong>@Name@</strong></a></li>").Trim();
            return PartialView("~/Areas/Admin/Views/Csmi/ConferenceCategoryManagement.cshtml");
        }
        [HttpPost]
        [AjaxValidateAntiForgeryTokenAttribute]
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult SubmitConferenceCategory(CategoryViewModel viewmodel)
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
                            context.Categories.Add(new Category
                            {
                                Name = viewmodel.Name,
                                Enable = true,
                                priority = 0,
                                Parent = viewmodel.ParentID != -1 ? context.Categories.Where(item => item.ID == viewmodel.ParentID).Single() : null,
                                TableName = viewmodel.TableName,
                                AllowDelete = true,
                                CreateDate = DateTime.Now
                            });
                            context.SaveChanges();

                        }
                        else if (viewmodel.ObjectState == ObjectState.Update)//Update
                        {
                            var context = new ApplicationDbContext();
                            Category selectedModel = context.Categories.Where(item => item.ID == viewmodel.ID).Single();
                            selectedModel.Name = viewmodel.Name;
                            selectedModel.LastModifyDate = DateTime.Now;
                            selectedModel.LastModifyUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault();
                            context.SaveChanges();
                        }
                    }
                    catch (Exception ex)
                    {
                        return ControllerHelper.ErrorResult("بروز خطای سیستمی");
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
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult GetCategoriesPartial(string TableName, string ParentID)
        {
            if (TableName != null)
            {
                return new GridViewPartialController().GridViewPartial<CategoryViewModel>(BindCategories(TableName, ParentID), "PublicAdmin", "GetCategoriesPartial", "CategoryManagementGrid", "ID", new { TableName = TableName, ParentID = ParentID });
            }
            return ControllerHelper.NotFoundResult();
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult ExportConferenceCategoryTo(int ActionType, string TableName, string ParentID)
        {
            return new GridViewPartialController().ExportTo<CategoryViewModel>(BindCategories(TableName, ParentID), ActionType, "CategoryManagementGrid");
        }
        List<CategoryViewModel> BindCategories(string TableName, string ParentID)
        {
            var DbCategories = new List<CategoryViewModel>();
            var context = new ApplicationDbContext();
            var model = new object[0];
            int parentID = int.Parse(ParentID);
            var cats = context.Categories
                .Include(Categories => Categories.Parent)
                .Where(cat => cat.TableName == TableName && (cat.Parent.ID == parentID || (parentID == -1 && cat.Parent == null)))
                .Select(item => new
                {
                    item.ID,
                    item.Name,
                    item.Enable,
                    item.Parent,
                    item.priority,
                    item.CreateDate
                })
                .OrderBy(cat => cat.priority)
                .ToList();
            foreach (var item in cats)
            {
                DbCategories.Add(new CategoryViewModel()
                {
                    ID = item.ID,
                    Name = item.Name,
                    ParentName = (item.Parent != null) ? item.Parent.Name : "",
                    Enable = item.Enable,
                    CreateDateConverted = CommonHelper.DateAndTimes.GetPersianDate(item.CreateDate)
                });
            }
            if (DbCategories.Count() == 0)
            {
                DbCategories.Add(new CategoryViewModel());
            }
            return DbCategories;
        }
        [HttpPost]
        [AjaxValidateAntiForgeryTokenAttribute]
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult DeleteCategory(string[] ids)
        {
            var context = new ApplicationDbContext();
            try
            {
                foreach (var id in ids)
                {
                    var ID = int.Parse(id);
                    var selectedItem = context.Categories.Where(item => item.ID == ID).Single();
                    if (selectedItem != null)
                        context.Categories.Remove(selectedItem);
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
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult ActiveCategory(string[] ids)
        {
            var context = new ApplicationDbContext();
            try
            {
                foreach (var id in ids)
                {
                    var ID = int.Parse(id);
                    var selectedItem = context.Categories.Where(item => item.ID == ID).Single();
                    if (selectedItem != null)
                        selectedItem.Enable = !selectedItem.Enable;
                }
                context.SaveChanges();
                return ControllerHelper.SuccessResult("با موفقیت ثبت شد");
            }
            catch (Exception ex)
            {
                return ControllerHelper.ErrorResult("بروز خطای سیستمی");
            }


        }

        string RecersiveNodeGenerate(int CatID, string Html)
        {
            string OutPut = "";
            var context = new ApplicationDbContext();
            var selectedCat = context.Categories.Include(cat => cat.Parent).Where(cat => cat.ID == CatID).SingleOrDefault<Category>();
            if (selectedCat != null)
            {
                int CurrentCatID = int.Parse(selectedCat.Parent == null ? "-1" : selectedCat.Parent.ID.ToString().Trim());
                OutPut = Html.Replace("@CatID@", (selectedCat.ID.ToString().Trim())).Replace("@Name@", selectedCat.Name);
                if (CurrentCatID != -1)
                    OutPut = RecersiveNodeGenerate(CurrentCatID, Html) + OutPut;
            }
            return OutPut;



        }
        private bool NodeHasChild(int CatID)
        {
            var context = new ApplicationDbContext();
            var selectedCat = context.Categories.Include(cat => cat.Parent).Where(cat => cat.ID == CatID).SingleOrDefault<Category>();
            if (selectedCat.Parent != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #region showing Menu & Submenu in Dropdownlist
        private List<Category> FindChildrenQuery(int id, List<Category> cats)
        {
            return cats.Where(cat => cat.Parent.ID == id).OrderBy(cat => cat.priority).ToList<Category>();
        }
        private bool HasChildren(int ID, List<Category> cats)
        {
            if (FindChildrenQuery(ID, cats).Count == 0)
                return (false);
            else
                return (true);
        }
        public void CreateMenuLevelDropDownList(List<SelectListItem> Catdrop, string TableName)
        {
            var context = new ApplicationDbContext();
            var cats = context.Categories.Include(cat => cat.Parent).Where(cat => cat.TableName == TableName).OrderBy(cat => cat.priority).ToList<Category>();
            if (cats != null)
            {
                foreach (var cat in cats.Where(catItem => catItem.Parent == null))
                {
                    if (HasChildren(cat.ID, cats))
                    {
                        SelectListItem item = new SelectListItem();
                        item.Text = cat.Name.ToString();
                        item.Value = cat.ID.ToString();
                        Catdrop.Add(item);
                        CreateSubmenuDropDownList(cat.ID, 1, cats, Catdrop);
                    }
                    else
                    {
                        SelectListItem item = new SelectListItem();
                        item.Text = cat.Name.ToString();
                        item.Value = cat.ID.ToString();
                        Catdrop.Add(item);
                    }
                }
            }
        }
        private void CreateSubmenuDropDownList(int ID, int submenu, List<Category> cats, List<SelectListItem> Catdrop)
        {
            if (HasChildren(ID, cats))
            {
                foreach (var cat in FindChildrenQuery(ID, cats))
                {
                    SelectListItem item = new SelectListItem();
                    string prefix = "";
                    for (int k = 1; k <= submenu; k++)
                        prefix += "◊";
                    item.Text = prefix + cat.Name.ToString();
                    item.Value = cat.ID.ToString();
                    Catdrop.Add(item);
                    if (HasChildren(ID, cats))
                    {
                        CreateSubmenuDropDownList(cat.ID, submenu + 1, cats, Catdrop);

                    }
                }
            }
        }
        #endregion
        #region showing Menu & Submenu
        private string CreateMenuLevel(string TableName)
        {
            string MenuItems = "";
            var context = new ApplicationDbContext();
            var cats = context.Categories.Include(cat => cat.Parent).Where(cat => cat.TableName == TableName).OrderBy(cat => cat.priority).ToList<Category>();
            foreach (var cat in cats.Where(catItem => catItem.Parent == null))
            {
                if (HasChildren(cat.ID, cats))
                {

                    MenuItems += "<li ><a href='" + Url.Action("", "", new { CatProduct = cat.ID.ToString() }) + "'>" + cat.Name + "</a><ul>";
                    MenuItems += CreateSubmenu(cat.ID, 1, cats);
                    MenuItems += "</ul>";
                }
                else
                {
                    MenuItems += "<li ><a href='" + Url.Action("", "", new { CatProduct = cat.ID.ToString() }) + "'>" + cat.Name + "</a></li>";
                }
            }
            return MenuItems;
        }
        private string CreateSubmenu(int ID, int submenu, List<Category> cats)
        {
            string MenuItems = "";
            if (HasChildren(ID, cats))
            {
                foreach (var cat in FindChildrenQuery(ID, cats))
                {
                    MenuItems += "<li><a href='" + Url.Action("", "", new { CatProduct = cat.ID.ToString() }) + "'>" + cat.Name + "</a>";
                    if (HasChildren(cat.ID, cats))
                    {
                        MenuItems += "<ul>" + CreateSubmenu(cat.ID, submenu + 1, cats) + "</ul></li>";

                    }
                    else
                        MenuItems += "</li>";
                }
            }
            return MenuItems;
        }
        #endregion
        #endregion
        #region SysRoleModule
        //Optimized
        // GET: Admin/SysRoleModule
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult SysRoleModule(string roleID)
        {
            if (roleID == null || roleID == "undefined")
            {
                return PartialView("~/Areas/Admin/Views/PublicAdmin/SysRoleModule.cshtml", new SysRoleModuleViewModel() { ObjectState = ObjectState.Insert });
            }
            else
            {
                var context = new ApplicationDbContext();
                var SelectedRoleName = context.Roles.Where(role => role.Id == roleID).Select(role => new { role.Name }).SingleOrDefault().Name;
                var SysModules = context.SysModules
                    .Select(item => new
                    {
                        ModuleID = item.ID,
                        ModuleName = item.PersianName
                    })
                    .ToList();
                var Modules = new List<Models.Admin.SysRoleModule>();
                foreach (var SysModule in SysModules)
                {

                    Modules.Add(new Models.Admin.SysRoleModule()
                    {
                        ID = SysModule.ModuleID.ToString(),
                        Name = SysModule.ModuleName,
                        IsChecked = context.SysRoleModules.Count(sysrole => sysrole.Role.Id == roleID && sysrole.Module.ID == SysModule.ModuleID) > 0
                    });
                }
                return PartialView("~/Areas/Admin/Views/PublicAdmin/SysRoleModule.cshtml", new SysRoleModuleViewModel()
                {
                    RoleID = roleID,
                    RoleName = SelectedRoleName,
                    Modules = Modules,
                    ObjectState = ObjectState.Update
                });
            }
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        [AjaxValidateAntiForgeryTokenAttribute]
        public ActionResult SubmitSysRoleModule(SysRoleModuleViewModel viewmodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var context = new ApplicationDbContext();
                    var trans = context.Database.BeginTransaction();
                    try
                    {
                        context.SysRoleModules.RemoveRange(context.SysRoleModules.Where(roleModule => roleModule.Role.Id == viewmodel.RoleID));
                        context.SaveChanges();
                        var CheckedModule = viewmodel.Modules.Where(module => module.IsChecked);
                        foreach (var Module in CheckedModule)
                        {
                            int ModuleID = int.Parse(Module.ID);
                            context.SysRoleModules
                            .Add(new Core.Entities.SysRoleModule()
                            {
                                Module = context.SysModules.Where(module => module.ID == ModuleID).SingleOrDefault(),
                                Role = (AspNetRole)context.Roles.Where(role => role.Id == viewmodel.RoleID).SingleOrDefault(),
                            });
                        }
                        context.SaveChanges();
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
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
        #endregion SysRoleModule
        #region Reset Password
        public ActionResult ForgotPassword()
        {
            return PartialView("~/Areas/Admin/Views/PublicAdmin/ForgotPassword.cshtml", new PasswordRecoveryMembershipViewModel());
        }
        public ActionResult ForgetPass(PasswordRecoveryMembershipViewModel viewmodel)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var context = new ApplicationDbContext();
                        var user = new ApplicationUser();
                        if (context.Users.Where(item => item.Email == viewmodel.Email || item.UserName == viewmodel.Email).Count() > 0)
                        {
                            user = context.Users.Where(item => item.Email == viewmodel.Email || item.UserName == viewmodel.Email).Single();
                            var code = UserManager.GeneratePasswordResetToken(user.Id);
                            //var result = UserManager.ConfirmEmail(user.Id, code);
                            //code = System.Web.HttpUtility.UrlEncode(code);
                            var callbackUrl = Url.Action("Index", "PublicAdmin", new { }, protocol: Request.Url.Scheme) + "#/ResetPassword?id=" + user.Id + "&code=" + code;

                            EmailService.EmailResponse MailResult = EmailService.SendForgetPassMail(user.Id, callbackUrl);
                            if (MailResult.Code == (int)EmailService.EmailEnum.Success)
                            {
                                return ControllerHelper.SuccessResult("لطفا برای ریست پسورد به ایمیلی که برای شما ارسال شده است مراجعه فرمایید.در صورت نبودن ایمیل اسپم لیست خود را بررسی بفرمایید");
                            }
                            else
                                return ControllerHelper.ErrorResult("بروز خطای سیستمی در ارسال ایمیل فعال سازی");
                        }
                        else
                        {
                            return ControllerHelper.ErrorResult("ایمیل وارد شده در سیستم ثبت نشده است");
                        }
                    }
                    catch (Exception ex)
                    {
                        return ControllerHelper.ErrorResult("بروز خطای سیستمی");
                    }
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
        public ActionResult ResetPassword(string id, string code)
        {
            try
            {
                // Verifies the parameters of the password reset request
                // True if the token is valid for the specified user, false if the token is invalid or has expired
                // By default, the generated tokens are single-use and expire in 1 day
                code = code.Replace(" ", "+");
                if (UserManager.VerifyUserToken(id, "ResetPassword", code))
                {
                    // If the password request is valid, displays the password reset form
                    var model = new ResetPasswordViewModel
                    {
                        UserId = id,
                        Code = code
                    };

                    return PartialView("~/Areas/Admin/Views/PublicAdmin/ResetPassword.cshtml", model);
                }

                // If the password request is invalid, returns a view informing the user
                return PartialView("~/Areas/Admin/Views/Shared/Error.cshtml");
            }
            catch (InvalidOperationException)
            {
                // An InvalidOperationException occurs if a user with the given ID is not found
                // Returns a view informing the user that the password reset request is not valid
                return PartialView("~/Areas/Admin/Views/Shared/Error.cshtml");
            }
            // return code == null ? PartialView("~/Views/Home/Error.cshtml") : PartialView("~/Views/Home/ResetPassword.cshtml",new ResetPasswordViewModel() {Code=code });
        }
        [HttpPost]
        [AllowAnonymous]
        [AjaxValidateAntiForgeryTokenAttribute]
        public ActionResult ResetPasswordAction(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("~/Areas/Admin/Views/PublicAdmin/ResetPassword.cshtml", model);
            }
            var context = new ApplicationDbContext();
            ApplicationUser user = context.Users.Where(item => item.Email == model.Email).Single();
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return PartialView("~/Areas/Admin/Views/PublicAdmin/Login.cshtml");
            }
            else
            {
                user.PasswordHash = new ApplicationUserManager(new UserStore<ApplicationUser>()).PasswordHasher.HashPassword(model.Password);
            }
            try
            {
                context.SaveChanges();
                return ControllerHelper.SuccessResult("رمز عبور با موفقیت تغییر یافت");
            }
            catch (Exception)
            {
                return ControllerHelper.ErrorResult("بروز خطای سیستمی");
            }

        }
        #endregion
        public ActionResult Index()
        {
            var model = new LayoutViewModel();
            if (User.Identity.IsAuthenticated)
            {
                var context = new ApplicationDbContext();
                var userid = User.Identity.GetUserId();
                var selectedUser = context.Users
                    .Include(item => item.Person)
                    .Where(user => user.Id == userid).SingleOrDefault();
                if (selectedUser != null)
                {
                    model.FirstName = selectedUser.Person.FirstName;
                    model.LastName = selectedUser.Person.LastName;
                    var roles = new List<RoleViewModel>();
                    foreach (var role in selectedUser.Roles)
                    {
                        roles.Add(
                        new RoleViewModel()
                        {
                            Name = context.Roles.Where(dbRole => dbRole.Id == role.RoleId).SingleOrDefault().Name,
                            ID = role.RoleId
                        });

                    }
                    model.Roles = roles;
                }

                ViewBag.SpecialClass = "";
            }
            else
            {
                ViewBag.SpecialClass = "gray-bg";
            }
            return View("~/Areas/Admin/Views/PublicAdmin/Index.cshtml", model);
        }
        public ActionResult Home(string Mode)
        {
            ViewBag.Mode = Mode;
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.SpecialClass = "";
                return PartialView("~/Areas/Admin/Views/PublicAdmin/Home.cshtml");
            }
            else
            {
                new ApplicationDbContext().Newses.Take(3).ToList();
                return PartialView("~/Areas/Admin/Views/PublicAdmin/Login.cshtml", new MembershipViewModel());
            }
        }
        public ActionResult DevScript()
        {

            return PartialView("~/Areas/Admin/Views/Shared/DevScript.cshtml");
        }

    }
}