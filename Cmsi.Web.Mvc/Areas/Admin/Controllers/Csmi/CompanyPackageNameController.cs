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

namespace AVA.Web.Mvc.Controllers.Admin
{
    //Optimized
    public class CompanyPackageNameController : BaseController
    {
        // GET: Admin/CompanyPackageName
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.CompanyPackageName)]
        public ActionResult CompanyPackageName()
        {
            var context = new ApplicationDbContext();
            if (Request.QueryString["id"] == null || Request.QueryString["id"] == "undefined")
            {
                var dbLanguages = context.Languages
                    .Select(Lang => new { Lang.ID, Lang.Name })
                    .ToList();
                var Languages = new List<CompanyPackageNameTranslationViewModel>();
                foreach (var lang in dbLanguages)
                {
                    Languages.Add(new CompanyPackageNameTranslationViewModel()
                    {
                        LanguageID = lang.ID,
                        LanguageName = lang.Name
                    });
                }
                return PartialView("~/Areas/Admin/Views/Csmi/CompanyPackageName.cshtml", new CompanyPackageNameViewModel()
                {
                    ObjectState = ObjectState.Insert,
                    CompanyPackageNameTranslation = Languages

                });
            }
            else
            {
                var selectedID = int.Parse(Request.QueryString["id"]);
                var selectedCompanyPackageName = context.CompanyPackageNames
                    .Where(item => item.ID.Equals(selectedID))
                    .Select(item => new
                    {
                        item.ID,
                        item.IsRegisterPackage,
                        item.Enable,
                        item.Price,
                        item.CreateDate
                    }).SingleOrDefault();
                var dbLanguages = context.Languages
                .Select(Lang => new { Lang.ID, Lang.Name })
                .ToList();
                var Languages = new List<CompanyPackageNameTranslationViewModel>();
                foreach (var lang in dbLanguages)
                {
                    Languages.Add(new CompanyPackageNameTranslationViewModel()
                    {
                        LanguageID = lang.ID,
                        LanguageName = lang.Name,
                        Name =
                            context.CompanyPackageNameTranslations
                                .Where(trans => 
                                    trans.CompanyPackageName.ID == selectedCompanyPackageName.ID
                                    &&
                                    trans.Language.ID== lang.ID
                                    )
                                .Select(item => new { item.Name })
                                .SingleOrDefault().Name
                    });
                }
                return PartialView("~/Areas/Admin/Views/Csmi/CompanyPackageName.cshtml", new CompanyPackageNameViewModel()
                {
                    ID = selectedCompanyPackageName.ID,
                    CreateDateConverted = CommonHelper.DateAndTimes.GetPersianDate(selectedCompanyPackageName.CreateDate),
                    IsRegisterPackage = selectedCompanyPackageName.IsRegisterPackage,
                    Enable = selectedCompanyPackageName.Enable,
                    Price = string.Format("{0:n0}", selectedCompanyPackageName.Price),
                    CompanyPackageNameTranslation = Languages,
                    ObjectState = ObjectState.Update
                });
            }
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.CompanyPackageName)]
        public ActionResult CompanyPackageNameManagement()
        {
            ViewBag.GridID = "CompanyPackageNameManagementGrid";
            return PartialView("~/Areas/Admin/Views/Csmi/CompanyPackageNameManagement.cshtml");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.CompanyPackageName)]
        public ActionResult GetCompanyPackageNamePartial()
        {
            var CompanyPackageNames = new List<CompanyPackageNameViewModel>();
            var context = new ApplicationDbContext();
            var model = new object[0];
            var DbCompanyPackageNames = context.CompanyPackageNames
                .Include(CompanyPackageName => CompanyPackageName.CompanyPackageNameTranslations)
                .Where(CompanyPackageName => CompanyPackageName.CompanyPackageNameTranslations.Any(trans => trans.Language.Value == "fa"))
                .Select(item => new
                {
                    item.ID,
                    item.CompanyPackageNameTranslations.FirstOrDefault().Name,
                    item.IsRegisterPackage,
                    item.Price,
                    item.CreateDate,
                    item.Enable,
                })
                .ToList().OrderByDescending(item => item.CreateDate);
            foreach (var item in DbCompanyPackageNames)
            {
                CompanyPackageNames.Add(new CompanyPackageNameViewModel()
                {
                    ID = item.ID,
                    CreateDateConverted = CommonHelper.DateAndTimes.GetPersianDate(item.CreateDate),
                    Name = item.Name,
                    IsRegisterPackage = item.IsRegisterPackage,
                    Price = string.Format("{0:n0}", item.Price),
                    Enable = item.Enable,
                });
            }
            if (CompanyPackageNames.Count() == 0)
            {
                CompanyPackageNames.Add(new CompanyPackageNameViewModel());
            }
            return new GridViewPartialController().GridViewPartial<CompanyPackageNameViewModel>(CompanyPackageNames, "CompanyPackageName", "GetCompanyPackageNamePartial", "CompanyPackageNameManagementGrid", "ID");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.CompanyPackageName)]
        public ActionResult HtmlEditorPartial()
        {
            return new HtmlEditorPartialController().HtmlEditorView("HtmlEditor", "OnHtmlChangedChanged", "OnInit");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.CompanyPackageName)]
        [AjaxValidateAntiForgeryTokenAttribute]
        public ActionResult SubmitCompanyPackageName(CompanyPackageNameViewModel viewmodel)
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
                            var dbContextTransaction = context.Database.BeginTransaction();
                            try
                            {
                                var Pack = new CompanyPackageName()
                                {
                                    IsRegisterPackage = (bool)viewmodel.IsRegisterPackage,
                                    CreateDate = DateTime.Now,
                                    Enable = true,
                                    CreatorUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault(),
                                };
                                context.CompanyPackageNames.Add(Pack);
                                foreach (var lang in viewmodel.CompanyPackageNameTranslation)
                                {
                                    context.CompanyPackageNameTranslations.Add(new CompanyPackageNameTranslation()
                                    {
                                        CompanyPackageName = Pack,
                                        Language = context.Languages.Where(lng => lng.ID == lang.LanguageID).SingleOrDefault(),                                        
                                        Name = lang.Name,
                                        CreateDate = DateTime.Now,
                                        CreatorUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault(),
                                    });
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
                        else if (viewmodel.ObjectState == ObjectState.Update)//Update
                        {
                            var context = new ApplicationDbContext();
                            CompanyPackageName selectedModel = context.CompanyPackageNames
                                 .Include(CompanyPackageName => CompanyPackageName.CompanyPackageNameTranslations)
                                 .Where(CompanyPackageName => CompanyPackageName.ID == viewmodel.ID).Single();
                            var dbContextTransaction = context.Database.BeginTransaction();
                            try
                            {
                                selectedModel.Price = decimal.Parse(viewmodel.Price);
                                selectedModel.IsRegisterPackage = viewmodel.IsRegisterPackage;
                                context.CompanyPackageNameTranslations.RemoveRange(selectedModel.CompanyPackageNameTranslations);
                                foreach (var lang in viewmodel.CompanyPackageNameTranslation)
                                {
                                    context.CompanyPackageNameTranslations.Add(new CompanyPackageNameTranslation()
                                    {
                                        CompanyPackageName = selectedModel,
                                        Language = context.Languages.Where(lng => lng.ID == lang.LanguageID).SingleOrDefault(),
                                        Name = lang.Name,
                                        CreateDate = DateTime.Now,
                                        CreatorUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault(),
                                    });
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
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.CompanyPackageName)]
        public ActionResult ActiveCompanyPackageName(string[] ids)
        {
            var context = new ApplicationDbContext();
            try
            {
                foreach (var id in ids)
                {
                    int CurrentID = int.Parse(id);
                    var selectedItem = context.CompanyPackageNames
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
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.CompanyPackageName)]
        public ActionResult DeleteCompanyPackageName(string[] ids)
        {
            var context = new ApplicationDbContext();
            var dbContextTransaction = context.Database.BeginTransaction();
            try
            {
                foreach (var id in ids)
                {
                    int ID = int.Parse(id);
                    var selectedItem = context.CompanyPackageNames
                        .Include(CompanyPackageName => CompanyPackageName.CompanyPackageNameTranslations)
                        .Include(CompanyPackageName => CompanyPackageName.CompanyPackageNameTranslations.Select(lang => new { lang.Language }))
                        .Where(item => item.ID == ID).Single();
                    if (selectedItem != null)
                    {
                        context.CompanyPackageNameTranslations.RemoveRange(selectedItem.CompanyPackageNameTranslations);
                        context.CompanyPackageNames.Remove(selectedItem);

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