using DevExpress.Web.Mvc;
using AVA.Web.Mvc.Areas.Admin.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Reflection;
using AVA.Web.Mvc.Models.Admin;

namespace AVA.Web.Mvc.Controllers.Admin
{
    public class HtmlEditorPartialController : Controller
    {
        #region HtmlEditor
        public ActionResult HtmlEditorView(string HtmlEditorID, string HtmlChangedEventName, string InitEventName)
        {
            return PartialView("~/Areas/Admin/Views/PublicAdmin/HtmlEditorPartial.cshtml", new HtmlEditorViewModel() { HtmlEditorID = HtmlEditorID, HtmlChangedEventName = HtmlChangedEventName, InitEventName = InitEventName });
        }
        public ActionResult HtmlEditorPartialImageSelectorUpload()
        {
            HtmlEditorExtension.SaveUploadedImage("HtmlEditor", HtmlEditorSettings.ImageSelectorSettings);
            return null;
        }
        [ValidateInput(false)]
        public ActionResult HtmlEditorPartialImageUpload(string HtmlEditorID)
        {
            HtmlEditorExtension.SaveUploadedFile(HtmlEditorID, HtmlEditorSettings.ImageUploadValidationSettings, HtmlEditorSettings.ImageUploadDirectory);
            return null;
        }
        #endregion
    }
    public class HtmlEditorSettings
    {
        public const string ImageUploadDirectory = "~/assets/img/Attach/ModuleImages/";
        public const string ImageSelectorThumbnailDirectory = "~/assets/img/Attach/ModuleImages/Thumb/";

        public static DevExpress.Web.UploadControlValidationSettings ImageUploadValidationSettings = new DevExpress.Web.UploadControlValidationSettings()
        {
            AllowedFileExtensions = new string[] { ".jpg", ".jpeg", ".jpe", ".gif", ".png" },
            MaxFileSize = 4000000,
            DisableHttpHandlerValidation=false
            
        };

        static DevExpress.Web.Mvc.MVCxHtmlEditorImageSelectorSettings imageSelectorSettings;
        public static DevExpress.Web.Mvc.MVCxHtmlEditorImageSelectorSettings ImageSelectorSettings
        {
            get
            {
                if (imageSelectorSettings == null)
                {
                    imageSelectorSettings = new DevExpress.Web.Mvc.MVCxHtmlEditorImageSelectorSettings(null);
                    imageSelectorSettings.Enabled = true;
                    imageSelectorSettings.UploadCallbackRouteValues = new { Controller = "HtmlEditorPartial", Action = "HtmlEditorPartialImageSelectorUpload" };
                    imageSelectorSettings.CommonSettings.RootFolder = ImageUploadDirectory;
                    imageSelectorSettings.CommonSettings.ThumbnailFolder = ImageSelectorThumbnailDirectory;
                    imageSelectorSettings.CommonSettings.AllowedFileExtensions = new string[] { ".jpg", ".jpeg", ".jpe", ".gif" };
                    imageSelectorSettings.UploadSettings.Enabled = true;
                }
                return imageSelectorSettings;
            }
        }
    }
}