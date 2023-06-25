using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace AVA.UI.Helpers.Controller
{
    public static class ControllerHelper
    {
        public const string MobileRegEx = @"^[0]\d{10}$";
        public const string EnglishONlyRegEx = @"^[a-zA-Z]+$";
        public class ResponseResult
        {
            public string ResultMessage { get; set; }
            public bool IsSuccess { get; set; }
        }

        public enum ExportGridviewActionType
        {
            PDF = 1,
            Excel = 2,
            Word = 3
        }
        public enum SysModuleType
        {
            News = 1,
            Slider = 2,
            Managment = 3,
            ContactUs = 4,
            DynamicPage = 5,
            PublicMethod = 8,
            FAQ = 14,
            Book = 15,
            GalleryAlbum = 18,
            GalleryImage = 19,
            Poll = 24,
            #region Pezeshkan
            Company = 6,
            // For Doctors to Edit their Profile کاربران پزشک
            Doctors = 7,
            #endregion
            #region Csmi
            Conference = 9,
            Referee = 10,
            Article = 11,
            Executor = 12,
            RefereeArticle = 13, //Referee Cartabl
            SocietyMember = 16,
            SocietyMemberPeriod = 17,
            ConferencePackageName = 20,
            ConferenceCompanion = 21,
            PackageName = 22,
            Payment = 23,
            SocietyPayment = 25,
            ScientificSecretary = 26,
            SocietyExecutor = 27,
            ScientificSecretaryCircle = 28,
            Election=29, 
            Candidate=30,
            Vote=31
            #endregion
        }
        public static string GetAntiForgeryToken()
        {
            string cookieToken, formToken;
            AntiForgery.GetTokens(null, out cookieToken, out formToken);
            return cookieToken + "," + formToken;
        }
        public static JsonResult ModelStateInvalidResult(ModelStateDictionary modelState)
        {
            var modelErrors =
                from x in modelState.Keys
                where modelState[x].Errors.Count > 0
                select new
                {
                    key = x,
                    errors = modelState[x]
                        .Errors
                        .Select(y => y.ErrorMessage)
                        .ToArray()
                };

            return new JsonResult()
            {
                Data = new { Type = "Error", Data = modelErrors },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public static JsonResult SuccessResult(string message = "Confirm")
        {
            return new JsonResult()
            {
                Data = new { Type = "Success", Data = message }
            };
        }
        public static JsonResult SuccessResult(object obj, string message = "Confirm")
        {
            return new JsonResult()
            {
                Data = new { Type = "Success", Message = message, Object = obj }
            };
        }
        public static JsonResult ErrorResult(string message)
        {

            HttpContext.Current.Response.StatusCode = 500;
            HttpContext.Current.Response.TrySkipIisCustomErrors = true;
            return new JsonResult
            {
                Data = new { error = message },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public static JsonResult ServerErrorResult(Exception ex)
        {
            HttpContext.Current.Response.StatusCode = 500;
            string message = "";
            for (var e = ex; e != null; e = e.InnerException) { message += e.Source + " " + e.Message; }
            return new JsonResult
            {
                Data = new { error = message },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public static JsonResult NotFoundResult(string message = "Item not found!")
        {
            return new JsonResult()
            {
                Data = new { Type = "NotFound", Data = message },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public static JsonResult FormNotValid(ModelStateDictionary modelState)
        {

            HttpContext.Current.Response.StatusCode = 500;
            return new JsonResult
            {
                Data = new { error = "فرم معتبر نیست " + "\n" + string.Join("\n", modelState.Values.SelectMany(v => v.Errors).ToList<ModelError>().Select(item => item.ErrorMessage).ToList()) },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public static string GetAbsoluteUrl()
        {
            string app_root = "";

            try
            {
                app_root = string.Format("{0}://{1}:{2}{3}",
                    HttpContext.Current.Request.Url.Scheme,
                    HttpContext.Current.Request.Url.Host,
                    HttpContext.Current.Request.Url.Port,
                    HttpContext.Current.Request.ApplicationPath);

                return app_root.EndsWith("/") ? app_root : string.Concat(app_root, "/");
            }
            catch (Exception ex) { throw ex; }
        }
        public static string GetAbsoluteUrlTo(string relativeUri)
        {
            try
            {
                relativeUri = relativeUri.TrimStart('~');
                relativeUri = relativeUri.TrimStart('/');

                return HttpUtility.UrlPathEncode(GetAbsoluteUrl() + relativeUri);
            }
            catch (Exception ex) { throw ex; }
        }

    }
}