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
  
    public class RefereeStatusController : BaseController
    {
        // GET: Admin/RefereeStatus
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult RefereeStatus()
        {
            if (Request.QueryString["id"] == null || Request.QueryString["id"] == "undefined")
            {
                return PartialView("~/Areas/Admin/Views/Csmi/RefereeStatus.cshtml", new RefereeStatusViewModel() { ObjectState = ObjectState.Insert });
            }
            else
            {
                var context = new ApplicationDbContext();
                var selectedID = int.Parse(Request.QueryString["id"]);
                var selectedRefereeStatus = context.RefereeStatuses
                    .Where(item => item.ID.Equals(selectedID))
                    .Select(item => new { item.ID, item.Name, item.Enable, item.CreateDate })
                    .SingleOrDefault();
                return PartialView("~/Areas/Admin/Views/Csmi/RefereeStatus.cshtml", new RefereeStatusViewModel()
                {
                    ID = selectedRefereeStatus.ID,
                    Name = selectedRefereeStatus.Name,
                    CreateDateConverted = CommonHelper.DateAndTimes.GetPersianDate(selectedRefereeStatus.CreateDate),
                    Enable = selectedRefereeStatus.Enable,
                    ObjectState = ObjectState.Update
                });
            }
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult RefereeStatusManagement()
        {
            ViewBag.GridID = "RefereeStatusManagementGrid";
            return PartialView("~/Areas/Admin/Views/Csmi/RefereeStatusManagement.cshtml");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult GetRefereeStatusPartial()
        {
            return new GridViewPartialController().GridViewPartial<RefereeStatusViewModel>(Bind(), "RefereeStatus", "GetRefereeStatusPartial", "RefereeStatusManagementGrid", "ID");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult ExportTo(int ActionType)
        {
            return new GridViewPartialController().ExportTo<RefereeStatusViewModel>(Bind(), ActionType, "RefereeStatusManagementGrid");
        }
        List<RefereeStatusViewModel> Bind()
        {
            var RefereeStatuses = new List<RefereeStatusViewModel>();
            var context = new ApplicationDbContext();
            var model = new object[0];
            var DbRefereeStatuses = context.RefereeStatuses
                .Select(item => new { item.ID, item.Name, item.Enable, item.CreateDate })
                .ToList()
                .OrderByDescending(item => item.CreateDate);
            foreach (var item in DbRefereeStatuses)
            {
                RefereeStatuses.Add(new RefereeStatusViewModel()
                {
                    ID = item.ID,
                    Name = item.Name,
                    CreateDateConverted = CommonHelper.DateAndTimes.GetPersianDate(item.CreateDate),
                    Enable = item.Enable,

                });
            }
            if (RefereeStatuses.Count() == 0)
            {
                RefereeStatuses.Add(new RefereeStatusViewModel());
            }
            return RefereeStatuses;
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult HtmlEditorPartial()
        {
            return new HtmlEditorPartialController().HtmlEditorView("HtmlEditor", "OnHtmlChangedChanged", "OnInit");
        }
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        [AjaxValidateAntiForgeryTokenAttribute]
        public ActionResult SubmitRefereeStatus(RefereeStatusViewModel viewmodel)
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
                            context.RefereeStatuses
                            .Add(new RefereeStatus()
                            {
                                Name = viewmodel.Name,
                                Enable = true,
                                CreateDate = DateTime.Now,
                                CreatorUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault(),

                            });
                            context.SaveChanges();
                        }
                        else if (viewmodel.ObjectState == ObjectState.Update)//Update
                        {
                            var context = new ApplicationDbContext();
                            RefereeStatus selectedModel = context.RefereeStatuses.Where(item => item.ID == viewmodel.ID).Single();
                            if (selectedModel != null)
                            {
                                selectedModel.Name = viewmodel.Name;
                                selectedModel.LastModifyUser = context.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault();
                                selectedModel.LastModifyDate = DateTime.Now;
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
        [AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        public ActionResult ActiveRefereeStatus(string[] ids)
        {
            var context = new ApplicationDbContext();
            try
            {
                foreach (var id in ids)
                {
                    int CurrentID = int.Parse(id);
                    var selectedItem = context.RefereeStatuses
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
        //[HttpPost]
        //[AjaxValidateAntiForgeryTokenAttribute]
        //[AuthorizeUser(AccessLevelID = (int)ControllerHelper.SysModuleType.Managment)]
        //public ActionResult DeleteRefereeStatus(string[] ids)
        //{
        //    var context = new ApplicationDbContext();
        //    try
        //    {
        //        foreach (var id in ids)
        //        {
        //            int ID = int.Parse(id);
        //            var selectedItem = context.RefereeStatuses
        //                .Where(item => item.ID == ID).Single();
        //            if (selectedItem != null)
        //            {
        //                context.RefereeStatuses.Remove(selectedItem);
        //            }

        //        }
        //        context.SaveChanges();
        //        return ControllerHelper.SuccessResult("با موفقیت ثبت شد");
        //    }
        //    catch (Exception ex)
        //    {
        //        return ControllerHelper.ErrorResult("بروز خطای سیستمی");
        //    }


        //}
    }
}