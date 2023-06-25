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

namespace AVA.Web.Mvc.Controllers.Admin
{
    //Optimized
    public class BookController : BaseController
    {
        // GET: Admin/Book
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Book)]
        public ActionResult Book()
        {
            var context = new ApplicationDbContext();
            List<DropDownVm> Languages = new List<DropDownVm>();
            var Dblanguage = context.Languages
                .Select(lang => new
                {
                    lang.ID,
                    lang.Name
                })
                .ToList();
            foreach (var lang in Dblanguage)
            {
                Languages.Add(new DropDownVm()
                {
                    Text = lang.Name,
                    Value = lang.ID.ToString()
                });
            }

            if (Request.QueryString["id"] == null || Request.QueryString["id"] == "undefined")
            {
                return PartialView("~/Areas/Admin/Views/Csmi/Book.cshtml", new BookViewModel() { ObjectState = ObjectState.Insert ,Languages = Languages, LanguageID = Languages[0].Value });
            }
            else
            {
                var selectedID = int.Parse(Request.QueryString["id"]);
                var selectedBook = context.Books
                    .Include(Book => Book.Language)
                    .Where(item => item.ID.Equals(selectedID))
                    .Select(item => new
                    {
                        item.ID,
                        item.Name,
                        item.ImageUrl,
                        item.PrintPeriod,
                        item.Year,
                        item.Writer,
                        item.Language,
                        item.CreateDate,
                        item.Enable,
                    })
                    .SingleOrDefault();
                return PartialView("~/Areas/Admin/Views/Csmi/Book.cshtml", new BookViewModel()
                {
                    ID = selectedBook.ID,
                    CreateDateConverted = CommonHelper.DateAndTimes.GetPersianDate(selectedBook.CreateDate),
                    Name = selectedBook.Name,
                    Writer = selectedBook.Writer,
                    Year = selectedBook.Year,
                    PrintPeriod = selectedBook.PrintPeriod,
                    LanguageID = selectedBook.Language.ID.ToString(),
                    ImageUrl = string.IsNullOrEmpty(selectedBook.ImageUrl) ? "" : Url.Content(selectedBook.ImageUrl),
                    Languages= Languages,
                    ObjectState = ObjectState.Update
                });
            }
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Book)]
        public ActionResult BookManagement()
        {
            ViewBag.GridID = "BookManagementGrid";
            return PartialView("~/Areas/Admin/Views/Csmi/BookManagement.cshtml");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Book)]
        public ActionResult GetBookPartial()
        {
            return new GridViewPartialController().GridViewPartial<BookViewModel>(Bind(), "Book", "GetBookPartial", "BookManagementGrid", "ID");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Book)]
        public ActionResult ExportTo(int ActionType)
        {
            return new GridViewPartialController().ExportTo<BookViewModel>(Bind(), ActionType, "BookManagementGrid");
        }
        List<BookViewModel> Bind()
        {
            var Books = new List<BookViewModel>();
            var context = new ApplicationDbContext();
            var model = new object[0];
            var DbBooks = context.Books
                .Include(Book => Book.Language)
                .Select(item => new
                {
                    item.ID,
                    Name = item.Name,
                    Writer = item.Writer,
                    Year = item.Year,
                    PrintPeriod = item.PrintPeriod,
                    LanguageName = item.Language.Name,
                    item.CreateDate,
                    item.Enable,
                })
                .ToList().OrderByDescending(item => item.CreateDate);
            foreach (var item in DbBooks)
            {
                Books.Add(new BookViewModel()
                {
                    ID = item.ID,
                    CreateDateConverted = CommonHelper.DateAndTimes.GetPersianDate(item.CreateDate),
                    Name = item.Name,
                    Writer = item.Writer,
                    Year = item.Year,
                    LanguageName = item.LanguageName,
                    PrintPeriod = item.PrintPeriod,
                    Enable = item.Enable,
                });
            }
            if (Books.Count() == 0)
            {
                Books.Add(new BookViewModel());
            }
            return Books;
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Book)]
        public ActionResult HtmlEditorPartial()
        {
            return new HtmlEditorPartialController().HtmlEditorView("HtmlEditor", "OnHtmlChangedChanged", "OnInit");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Book)]
        [AjaxValidateAntiForgeryTokenAttribute]
        public ActionResult SubmitBook(string viewmodel)
        {
            try
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                BookViewModel vm = jss.Deserialize<BookViewModel>(viewmodel);
                if (ModelState.IsValid)
                {
                    try
                    {
                        string BookImage = "";
                        if (Request.Files.Count > 0)
                        {
                            var file = Request.Files[CommonHelper.ExposeProperty.GetPropertyInfo(vm, m => m.Image).Name];

                            if (file != null && file.ContentLength > 0)
                            {
                                BookImage = FileUploadManagment.UploadFile("~/assets/img/Attach/Book/", "", file, FileUploadManagment.AppFileType.Image);
                                if (BookImage == "")
                                {
                                    return ControllerHelper.ErrorResult("فرمت یا حجم فایل صحیح نمی باشد");
                                }
                            }
                        }
                        if (vm.ObjectState == ObjectState.Insert)
                        {
                            var file = Request.Files[CommonHelper.ExposeProperty.GetPropertyInfo(vm, m => m.Image).Name];
                            if (file == null && vm.ObjectState == ObjectState.Insert)
                            {
                                return ControllerHelper.ErrorResult("تصویر پرسنلی را وارد کنید");
                            }

                        }
                        int langID = int.Parse(vm.LanguageID);
                        if (vm.ObjectState == ObjectState.Insert) // insert
                        {
                            var context = new ApplicationDbContext();
                            try
                            {
                                context.Books.Add(new Core.Entities.Book()
                                {
                                    Name = vm.Name,
                                    Writer = vm.Writer,
                                    Year = vm.Year,
                                    PrintPeriod = vm.PrintPeriod,
                                    Language = context.Languages.Where(lang => lang.ID == langID).SingleOrDefault(),
                                    CreateDate = DateTime.Now,
                                    CreatorUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault()
                                });
                                context.SaveChanges();

                            }
                            catch (Exception ex)
                            {
                                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.Message);
                            }

                        }
                        else if (vm.ObjectState == ObjectState.Update)//Update
                        {
                            var context = new ApplicationDbContext();
                            var selectedModel = context.Books
                                .Where(item => item.ID == vm.ID).Single();
                            try
                            {
                                if (selectedModel != null)
                                {
                                    selectedModel.ID = vm.ID;
                                    selectedModel.Name = vm.Name;
                                    selectedModel.PrintPeriod = vm.PrintPeriod;
                                    selectedModel.Writer = vm.Writer;
                                    selectedModel.Year = vm.Year;
                                    if (BookImage != "")
                                    {
                                        selectedModel.ImageUrl = BookImage;
                                    }
                                    selectedModel.Language = context.Languages.Where(lang => lang.ID == langID).SingleOrDefault();
                                    selectedModel.LastModifyUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault();
                                    selectedModel.LastModifyDate = DateTime.Now;
                                }
                                context.SaveChanges();

                            }
                            catch (Exception ex)
                            {
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
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Book)]
        public ActionResult ActiveBook(string[] ids)
        {
            var context = new ApplicationDbContext();
            try
            {
                foreach (var id in ids)
                {
                    int CurrentID = int.Parse(id);
                    var selectedItem = context.Books
                        .Where(item => item.ID == CurrentID).Single();
                    if (selectedItem != null)
                    {
                        selectedItem.Enable = !selectedItem.Enable;
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
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Book)]
        public ActionResult DeleteBook(string[] ids)
        {
            var context = new ApplicationDbContext();
            var dbContextTransaction = context.Database.BeginTransaction();
            try
            {
                foreach (var id in ids)
                {
                    int ID = int.Parse(id);
                    var selectedItem = context.Books
                        .Where(item => item.ID == ID).Single();
                    if (selectedItem != null)
                    {
                        try
                        {
                            System.IO.File.Delete(selectedItem.ImageUrl);
                        }
                        catch (Exception)
                        {

                        }
                        context.Books.Remove(selectedItem);
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