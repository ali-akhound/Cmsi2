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
    public class ConferenceDefinedCategoryController : BaseController
    {
        #region ConferenceDefinedCategory
        public ActionResult ConferenceDefinedCategoryManagement(string ConferenceID)
        {
            ViewBag.ConferenceID = ConferenceID;
            int conferenceID = int.Parse(ConferenceID);
            var context = new ApplicationDbContext();
            var selectedConference = context.Conferences
               .Where(item => item.ID.Equals(conferenceID))
               .Select(item => new { item.Title })
               .SingleOrDefault();
            ViewBag.GridID = "ConferenceDefinedCategoryManagementGrid";
            ViewBag.ConferenceName = selectedConference.Title;
            return PartialView("~/Areas/Admin/Views/Csmi/ConferenceDefinedCategoryManagement.cshtml");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Conference)]
        public ActionResult HtmlEditorPartial()
        {
            return new HtmlEditorPartialController().HtmlEditorView("HtmlEditor", "OnHtmlChangedChanged", "OnInit");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Conference)]
        public ActionResult GetConferenceDefinedCategoryPartial(string ConferenceID)
        {
            return new GridViewPartialController().GridViewPartial<ConferenceDefinedCategoryDetailViewModel>(Bind(ConferenceID), "ConferenceDefinedCategory", "GetConferenceDefinedCategoryPartial", "ConferenceDefinedCategoryManagementGrid", "ID");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Conference)]
        public ActionResult ExportTo(int ActionType, string ConferenceID)
        {
            return new GridViewPartialController().ExportTo<ConferenceDefinedCategoryDetailViewModel>(Bind(ConferenceID), ActionType, "ConferenceDefinedCategoryManagementGrid");
        }
        List<ConferenceDefinedCategoryDetailViewModel> Bind(string ConferenceID)
        {
            var ConferenceDefinedCategory = new List<ConferenceDefinedCategoryDetailViewModel>();
            var context = new ApplicationDbContext();
            var model = new object[0];
            int conferenceID = int.Parse(ConferenceID);
            var DbConferenceDefinedCategories =
                 context.ConferenceCategories
                .Include(confRef => confRef.Category)
                .Where(confRef => confRef.Conference.ID == conferenceID)
                .Select(confRef => new { confRef.ID, confRef.Category.Name, confRef.Enable, confRef.CreateDate })
                .ToList().OrderByDescending(confRef => confRef.CreateDate);
            foreach (var item in DbConferenceDefinedCategories)
            {
                ConferenceDefinedCategory.Add(new ConferenceDefinedCategoryDetailViewModel()
                {
                    ID = item.ID,
                    DefinedCategoryName = item.Name,
                    CreateDateConverted = CommonHelper.DateAndTimes.GetPersianDate(item.CreateDate),
                    Enable = item.Enable,

                });
            }
            if (ConferenceDefinedCategory.Count() == 0)
            {
                ConferenceDefinedCategory.Add(new ConferenceDefinedCategoryDetailViewModel());
            }
            return ConferenceDefinedCategory;
        }
        [HttpPost]
        [AjaxValidateAntiForgeryTokenAttribute]
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Conference)]
        public ActionResult ActiveConferenceDefinedCategory(string[] ids)
        {
            var context = new ApplicationDbContext();
            try
            {
                foreach (var id in ids)
                {
                    int CurrentID = int.Parse(id);
                    var selectedItem = context.ConferenceCategories
                        .Include(item => item.Conference)
                        .Include(item => item.Category)
                        .Include(item => item.CreatorUser)
                        .Where(item => item.ID == CurrentID)
                        .SingleOrDefault();
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
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Conference)]
        public ActionResult DeleteConferenceDefinedCategory(string[] ids)
        {
            var context = new ApplicationDbContext();
            try
            {
                foreach (var id in ids)
                {
                    int ID = int.Parse(id);
                    var selectedItem = context.ConferenceCategories
                        .Where(item => item.ID == ID).Single();
                    if (selectedItem != null)
                    {
                        context.ConferenceCategories.Remove(selectedItem);
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

        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Conference)]
        public ActionResult ConferenceDefinedCategory(string ConferenceID)
        {

            //var context = new ApplicationDbContext();
            //var selectedID = int.Parse(Request.QueryString["Conference"]);
            //var DbConferenceDefinedCategorys =
            //    context.ConferenceDefinedCategorys
            //    .Include(confRef => confRef.Referee)
            //    .Include(confRef => confRef.Referee.CreatorUser)
            //    .Where(confRef => confRef.Conference.ID == selectedID && confRef.Referee.Enable).ToList();
            //var Referees = new List<RefereeSelective>();
            return PartialView("~/Areas/Admin/Views/Csmi/ConferenceDefinedCategory.cshtml", new ConferenceDefinedCategoryViewModel()
            {
                ConferenceID = int.Parse(ConferenceID),
                DefinedCategories = new List<DefinedCategorySelective>(),
                DefinedCategoriesFeed = new List<DefinedCategorySelective>(),
                ObjectState = ObjectState.Insert,
                Enable = true
            });
        }
        [HttpPost]
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Conference)]
        //[AjaxValidateAntiForgeryTokenAttribute]
        public ActionResult ConferenceDefinedCategoryFeed(Select2ConferenceDefinedCategoryViewModel Input)
        {
            var context = new ApplicationDbContext();
            var ConferenceDefinedCategory = new List<DefinedCategorySelective>();
            int ConferenceID = int.Parse(Input.ConferenceID);
            var cats = context.Categories
                 .Where(item =>
                 (
                     item.TableName == "Conference")
                     &&
                     ((item.Name).Contains(Input.Name) || string.IsNullOrEmpty(Input.Name))
                     && context.ConferenceCategories.Count(ConfCat => ConfCat.Category.ID == item.ID && ConfCat.Conference.ID == ConferenceID) == 0
                     && (item.Enable == true)
                 )
                 .Select(item => new { item.ID, item.Name })
                 .Take(20)
                 .ToList();
            foreach (var item in cats)
            {
                ConferenceDefinedCategory.Add(new DefinedCategorySelective() { ID = item.ID.ToString(), Name = item.Name });
            }
            return Json(ConferenceDefinedCategory, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [AjaxValidateAntiForgeryTokenAttribute]
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Conference)]
        public ActionResult SubmitConferenceDefinedCategory(ConferenceDefinedCategoryViewModel viewmodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        if (viewmodel.ObjectState == ObjectState.Insert)//Update
                        {

                            var context = new ApplicationDbContext();
                            var selectedConference = context.Conferences.Where(item => item.ID.Equals(viewmodel.ConferenceID)).SingleOrDefault();
                            foreach (var cat in viewmodel.DefinedCategories)
                            {
                                var catID = int.Parse(cat.ID);
                                context.ConferenceCategories.Add(new Core.Entities.ConferenceCategory()
                                {
                                    Conference = selectedConference,
                                    Category = context.Categories.Where(item => item.ID == catID).SingleOrDefault(),
                                    Enable = true,
                                    CreateDate = DateTime.Now,
                                    CreatorUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault(),
                                });
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
    }
}