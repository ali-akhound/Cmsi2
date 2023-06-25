using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using AVA.Web.Mvc.Models.Admin;
using AVA.Core.Entities;
using AVA.UI.Helpers.Base;
using AVA.UI.Helpers.Common;
using System.Net;
using AVA.UI.Helpers.Controller;
using AVA.UI.Helpers.CustomAttribute;
using AVA.Web.Mvc.Areas.Admin.Models.Base;
using System.Web.Script.Serialization;
using AVA.UI.Helpers.FileUploadManagment;

namespace AVA.Web.Mvc.Controllers.Admin
{
    //Optimized
    public class NewsController : BaseController
    {
        // GET: Admin/News
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.News)]
        public ActionResult News()
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
                return PartialView("~/Areas/Admin/Views/News/News.cshtml", new NewsViewModel() { ObjectState = ObjectState.Insert, LanguageID = Languages[0].Value, Languages = Languages });
            }
            else
            {
                var selectedID = int.Parse(Request.QueryString["id"]);
                var selectedNews = context.Newses.Where(item => item.ID.Equals(selectedID))
                    .Select(item => new
                    {
                        item.ID,
                        item.Subject,
                        item.Summery,
                        item.CreateDate,
                        item.Explain,
                        item.Keywords,
                        item.Description,
                        item.LongImageUrl,
                        item.ShortImageUrl,
                        item.Date,
                        LanguageID = item.Language.ID
                    })
                    .SingleOrDefault();
                return PartialView("~/Areas/Admin/Views/News/News.cshtml", new NewsViewModel()
                {
                    ID = selectedNews.ID,
                    CreateDateConverted = CommonHelper.DateAndTimes.GetPersianDate(selectedNews.CreateDate),
                    DateConverted = CommonHelper.DateAndTimes.GetPersianDate(selectedNews.Date),
                    Subject = selectedNews.Subject,
                    Summery = selectedNews.Summery,
                    Explain = selectedNews.Explain,
                    Keywords = selectedNews.Keywords,
                    Description = selectedNews.Description,
                    LongImageUrl = string.IsNullOrEmpty(selectedNews.LongImageUrl) ? "" : Url.Content(selectedNews.LongImageUrl),
                    SmallImageUrl = string.IsNullOrEmpty(selectedNews.ShortImageUrl) ? "" : Url.Content(selectedNews.ShortImageUrl),
                    LanguageID = selectedNews.LanguageID.ToString(),
                    Languages = Languages,
                    ObjectState = ObjectState.Update
                });
            }
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.News)]
        public ActionResult NewsManagement()
        {
            ViewBag.GridID = "NewsManagementGrid";
            return PartialView("~/Areas/Admin/Views/News/NewsManagement.cshtml");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.News)]
        public ActionResult GetNewsPartial()
        {
            return new GridViewPartialController().GridViewPartial<NewsViewModel>(Bind(), "News", "GetNewsPartial", "NewsManagementGrid", "ID");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.News)]
        public ActionResult ExportTo(int ActionType)
        {
            return new GridViewPartialController().ExportTo<NewsViewModel>(Bind(), ActionType, "NewsManagementGrid");
        }
        List<NewsViewModel> Bind()
        {
            var NewsList = new List<NewsViewModel>();
            var context = new ApplicationDbContext();
            var model = new object[0];
            var DbNews = context.Newses
                .Include(item => item.Language)
                .Select(item => new
                {
                    item.ID,
                    item.Subject,
                    item.Summery,
                    item.CreateDate,
                    item.Explain,
                    item.Keywords,
                    item.Description,
                    item.ShortImageUrl,
                    item.Date,
                    item.Enable,
                    LanguageName = item.Language.Name
                })
            .ToList()
            .OrderByDescending(item => item.CreateDate);
            foreach (var item in DbNews)
            {
                NewsList.Add(new NewsViewModel()
                {
                    ID = item.ID,
                    DateConverted = CommonHelper.DateAndTimes.GetPersianDate(item.Date),
                    Subject = item.Subject,
                    SmallImageUrl = item.ShortImageUrl,
                    Description = item.Description,
                    LanguageName = item.LanguageName,
                    Enable = item.Enable,
                    CreateDateConverted = CommonHelper.DateAndTimes.GetPersianDate(item.CreateDate)
                });
            }
            if (NewsList.Count() == 0)
            {
                NewsList.Add(new NewsViewModel());
            }
            return NewsList;
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.News)]
        public ActionResult HtmlEditorPartial()
        {
            return new HtmlEditorPartialController().HtmlEditorView("HtmlEditor", "OnHtmlChangedChanged", "OnInit");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.News)]
        [HttpPost]
        [AjaxValidateAntiForgeryTokenAttribute]
        [ValidateInput(false)]
        public ActionResult SubmitNews(string viewmodel)
        {
            try
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                NewsViewModel vm = jss.Deserialize<NewsViewModel>(viewmodel);
                if (ModelState.IsValid)
                {
                    try
                    {
                        string SmallImage = "";
                        string LongImage = "";
                        if (Request.Files.Count > 0)
                        {
                            var file = Request.Files[CommonHelper.ExposeProperty.GetPropertyInfo(vm, m => m.SmallImage).Name];

                            if (file != null && file.ContentLength > 0)
                            {
                                SmallImage = FileUploadManagment.UploadFile("~/assets/img/Attach/News/", "", file, FileUploadManagment.AppFileType.Image);
                                if (SmallImage == "")
                                {
                                    return ControllerHelper.ErrorResult("فرمت یا حجم فایل صحیح نمی باشد");
                                }
                            }
                            file = Request.Files[CommonHelper.ExposeProperty.GetPropertyInfo(vm, m => m.LongImage).Name];

                            if (file != null && file.ContentLength > 0)
                            {
                                LongImage = FileUploadManagment.UploadFile("~/assets/img/Attach/News/", "", file, FileUploadManagment.AppFileType.Image);
                                if (LongImage == "")
                                {
                                    return ControllerHelper.ErrorResult("فرمت یا حجم فایل صحیح نمی باشد");
                                }
                            }
                        }
                        else
                        {
                            return ControllerHelper.ErrorResult("لطفا عکس بزرگ و کوچک را انتخاب نمایید");
                        }

                        int langID = int.Parse(vm.LanguageID);
                        DateTime? date = CommonHelper.DateAndTimes.GetGregorianDate(vm.DateConverted);
                        if (vm.ObjectState == ObjectState.Insert) // insert
                        {
                            var context = new ApplicationDbContext();
                            context.Newses.Add(new News()
                            {
                                Subject = vm.Subject,
                                Date = date != null ? (DateTime)date : DateTime.Now,
                                Summery = vm.Summery,
                                Explain = vm.Explain,
                                Keywords = vm.Keywords,
                                Description = vm.Description,
                                ShortImageUrl = SmallImage,
                                LongImageUrl = LongImage,
                                Language = context.Languages.Where(lang => lang.ID == langID).SingleOrDefault(),
                                Enable = true,
                                CreateDate = DateTime.Now,
                                CreatorUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault(),
                                Visit = 0,
                            });
                            context.SaveChanges();

                        }
                        else if (vm.ObjectState == ObjectState.Update)//Update
                        {
                            var context = new ApplicationDbContext();
                            News selectedModel = context.Newses.Where(item => item.ID == vm.ID).Single();
                            if (selectedModel != null)
                            {
                                selectedModel.Subject = vm.Subject;
                                selectedModel.Date = date != null ? (DateTime)date : DateTime.Now;
                                selectedModel.Summery = vm.Summery;
                                selectedModel.Explain = vm.Explain;
                                selectedModel.Keywords = vm.Keywords;
                                selectedModel.Description = vm.Description;
                                selectedModel.Language = context.Languages.Where(lang => lang.ID == langID).SingleOrDefault();
                                selectedModel.LastModifyUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault();
                                selectedModel.LastModifyDate = DateTime.Now;
                                if (SmallImage != "")
                                {
                                    selectedModel.ShortImageUrl = SmallImage;
                                }
                                if (LongImage != "")
                                {
                                    selectedModel.LongImageUrl = LongImage;
                                }
                            }
                            context.SaveChanges();
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
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.News)]
        public ActionResult ActiveNews(string[] ids)
        {
            var context = new ApplicationDbContext();
            try
            {
                foreach (var id in ids)
                {
                    int CurrentID = int.Parse(id);
                    var selectedItem = context.Newses.Where(item => item.ID == CurrentID).Single();
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
        [HttpPost]
        [AjaxValidateAntiForgeryTokenAttribute]
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.News)]
        public ActionResult DeleteNews(string[] ids)
        {
            var context = new ApplicationDbContext();
            var dbContextTransaction = context.Database.BeginTransaction();
            try
            {
                foreach (var id in ids)
                {
                    int ID = int.Parse(id);
                    var selectedItem = context.Newses
                        .Where(item => item.ID == ID).Single();
                    if (selectedItem != null)
                    {
                        try
                        {
                            System.IO.File.Delete(selectedItem.LongImageUrl);
                        }
                        catch (Exception)
                        {

                        }
                        try
                        {
                            System.IO.File.Delete(selectedItem.ShortImageUrl);
                        }
                        catch (Exception)
                        {

                        }

                        context.Newses.Remove(selectedItem);
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