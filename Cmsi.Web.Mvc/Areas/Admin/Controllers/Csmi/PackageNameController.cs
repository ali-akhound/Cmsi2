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
using AVA.UI.Helpers;
namespace AVA.Web.Mvc.Controllers.Admin
{

    //Optimized
    public class PackageNameController : BaseController
    {
        // GET: Admin/PackageName
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.PackageName)]
        public ActionResult PackageName()
        {
            var context = new ApplicationDbContext();
            if (Request.QueryString["id"] == null || Request.QueryString["id"] == "undefined")
            {
                var dbLanguages = context.Languages
                    .Select(Lang => new { Lang.ID, Lang.Name })
                    .ToList();
                var Languages = new List<PackageNameTranslationViewModel>();
                foreach (var lang in dbLanguages)
                {
                    Languages.Add(new PackageNameTranslationViewModel()
                    {
                        LanguageID = lang.ID,
                        LanguageName = lang.Name
                    });
                }
                return PartialView("~/Areas/Admin/Views/Csmi/PackageName.cshtml", new PackageNameViewModel()
                {
                    ObjectState = ObjectState.Insert,
                    PackageNameTranslations = Languages

                });
            }
            else
            {
                var selectedID = int.Parse(Request.QueryString["id"]);
                var selectedPackageName = context.PackageNames
                    .Where(
                        item => item.ID.Equals(selectedID)
                    )
                    .Select(item => new
                    {
                        item.ID,
                        item.Price,
                        item.Enable,
                        item.CreateDate
                    }).SingleOrDefault();
                var dbLanguages = context.Languages
                .Select(Lang => new { Lang.ID, Lang.Name })
                .ToList();
                var Languages = new List<PackageNameTranslationViewModel>();
                foreach (var lang in dbLanguages)
                {
                    Languages.Add(new PackageNameTranslationViewModel()
                    {
                        LanguageID = lang.ID,
                        LanguageName = lang.Name,
                        Name =
                            context.PackageNameTranslations
                                .Where(trans =>
                                    trans.PackageName.ID == selectedPackageName.ID
                                    &&
                                    trans.Language.ID == lang.ID
                                    )
                                .Select(item => new { item.Name })
                                .SingleOrDefault().Name
                    });
                }
                return PartialView("~/Areas/Admin/Views/Csmi/PackageName.cshtml", new PackageNameViewModel()
                {
                    ID = selectedPackageName.ID,
                    Price=(int)selectedPackageName.Price,
                    CreateDateConverted = CommonHelper.DateAndTimes.GetPersianDate(selectedPackageName.CreateDate),
                    Enable = selectedPackageName.Enable,
                    PackageNameTranslations = Languages,
                    ObjectState = ObjectState.Update
                });
            }
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.PackageName)]
        public ActionResult PackageNameManagement()
        {
            ViewBag.GridID = "PackageNameManagementGrid";
            return PartialView("~/Areas/Admin/Views/Csmi/PackageNameManagement.cshtml");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.PackageName)]
        public ActionResult GetPackageNamePartial()
        {
            return new GridViewPartialController().GridViewPartial<PackageNameViewModel>(Bind(), "PackageName", "GetPackageNamePartial", "PackageNameManagementGrid", "ID");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.PackageName)]
        public ActionResult ExportTo(int ActionType)
        {
            return new GridViewPartialController().ExportTo<PackageNameViewModel>(Bind(), ActionType, "PackageNameManagementGrid");
        }
        List<PackageNameViewModel> Bind()
        {
            var PackageNames = new List<PackageNameViewModel>();
            var context = new ApplicationDbContext();
            var model = new object[0];
            var DbPackageNames = context.PackageNames
                .Include(packageName => packageName.PackageNameTranslations)
                .Include(packageName => packageName.PackageType)
                .Where(
                        packageName => packageName.PackageNameTranslations.Any(trans => trans.Language.Value == "fa")
                        &&
                        packageName.PackageType.ID == (int)Enums.PackageType.SocietyPackage
                )
                .Select(item => new
                {
                    item.ID,
                    item.PackageNameTranslations.FirstOrDefault().Name,
                    PackageTypeName = item.PackageType.Name,
                    PackageTypeID = item.PackageType.ID,
                    item.Price,
                    item.CreateDate,
                    item.Enable,
                })
                .ToList().OrderBy(item => item.PackageTypeID);
            foreach (var item in DbPackageNames)
            {
                PackageNames.Add(new PackageNameViewModel()
                {
                    ID = item.ID,
                    CreateDateConverted = CommonHelper.DateAndTimes.GetPersianDate(item.CreateDate),
                    Name = item.Name,
                    PackageTypeName = item.PackageTypeName,
                    Price = (int)item.Price,
                    Enable = item.Enable,
                });
            }
            if (PackageNames.Count() == 0)
            {
                PackageNames.Add(new PackageNameViewModel());
            }
            return PackageNames;
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.PackageName)]
        public ActionResult HtmlEditorPartial()
        {
            return new HtmlEditorPartialController().HtmlEditorView("HtmlEditor", "OnHtmlChangedChanged", "OnInit");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.PackageName)]
        [AjaxValidateAntiForgeryTokenAttribute]
        public ActionResult SubmitPackageName(PackageNameViewModel viewmodel)
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
                                var Pack = new PackageName()
                                {
                                    CreateDate = DateTime.Now,
                                    PackageType = context.PackageTypes.Where(packType => packType.ID == (int)Enums.PackageType.RegisterConferencePackage).SingleOrDefault(),
                                    Enable = true,
                                    Price = viewmodel.Price,
                                    CreatorUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault(),
                                };
                                context.PackageNames.Add(Pack);
                                foreach (var lang in viewmodel.PackageNameTranslations)
                                {
                                    context.PackageNameTranslations.Add(new PackageNameTranslation()
                                    {
                                        PackageName = Pack,
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
                            PackageName selectedModel = context.PackageNames
                                 .Include(PackageName => PackageName.PackageNameTranslations)
                                 .Where(PackageName => PackageName.ID == viewmodel.ID).Single();
                            selectedModel.Price = viewmodel.Price;
                            selectedModel.LastModifyDate = DateTime.Now;
                            selectedModel.LastModifyUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault();
                            var dbContextTransaction = context.Database.BeginTransaction();
                            try
                            {
                                context.PackageNameTranslations.RemoveRange(selectedModel.PackageNameTranslations);
                                foreach (var lang in viewmodel.PackageNameTranslations)
                                {
                                    context.PackageNameTranslations.Add(new PackageNameTranslation()
                                    {
                                        PackageName = selectedModel,
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
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.PackageName)]
        public ActionResult ActivePackageName(string[] ids)
        {
            var context = new ApplicationDbContext();
            try
            {
                foreach (var id in ids)
                {
                    int CurrentID = int.Parse(id);
                    var selectedItem = context.PackageNames
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
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.PackageName)]
        public ActionResult DeletePackageName(string[] ids)
        {
            var context = new ApplicationDbContext();
            var dbContextTransaction = context.Database.BeginTransaction();
            try
            {
                foreach (var id in ids)
                {
                    int ID = int.Parse(id);
                    var selectedItem = context.PackageNames
                        .Include(PackageName => PackageName.PackageNameTranslations)
                        .Include(PackageName => PackageName.PackageNameTranslations.Select(lang => new { lang.Language }))
                        .Where(item => item.ID == ID).Single();
                    if (selectedItem != null)
                    {
                        context.PackageNameTranslations.RemoveRange(selectedItem.PackageNameTranslations);
                        context.PackageNames.Remove(selectedItem);

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