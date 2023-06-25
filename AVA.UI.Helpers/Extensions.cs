using AVA.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Data.Entity;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Resources;
using System.Net;

namespace AVA.UI.Helpers.CustomAttribute
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AjaxValidateAntiForgeryTokenAttribute : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            try
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest()) // if it is ajax request.
                {
                    this.ValidateRequestHeader(filterContext.HttpContext.Request); // run the validation.
                }
                else
                {
                    AntiForgery.Validate();
                }
            }
            catch (HttpAntiForgeryException e)
            {
                throw new HttpAntiForgeryException("Anti forgery token not found");
            }
        }

        private void ValidateRequestHeader(HttpRequestBase request)
        {
            string cookieToken = string.Empty;
            string formToken = string.Empty;
            string tokenValue = request.Headers["VerificationToken"]; // read the header key and validate the tokens.
            if (!string.IsNullOrEmpty(tokenValue))
            {
                string[] tokens = tokenValue.Split(',');
                if (tokens.Length == 2)
                {
                    cookieToken = tokens[0].Trim();
                    formToken = tokens[1].Trim();
                }
            }

            AntiForgery.Validate(cookieToken, formToken); // this validates the request token.
        }
    }
    public class ShowInGridview : Attribute
    {
    }
    public class GridColumnWidth : Attribute
    {
        Unit _width;
        public GridColumnWidth(int Width)
        {
            _width = Unit.Pixel(Width);
        }
        public Unit getWidth()
        {
            return _width;
        }
    }
    public class GridColumnEncodeHtml : Attribute
    {
        bool _Enable;
        public GridColumnEncodeHtml(bool Enable)
        {
            _Enable = Enable;
        }
        public bool getEncodeHtml()
        {
            return _Enable;
        }
    }

    public class HyperLinkGridviewColumn : Attribute
    {
    }

    public class ImageGridviewColumn : Attribute
    {
    }
    public class HyperLinkGridviewColumnText : Attribute
    {
        string _ColumnText = "";
        public HyperLinkGridviewColumnText(string ColumnText)
        {
            _ColumnText = ColumnText;
        }
        public string getColumnText()
        {
            return _ColumnText;
        }
    }

    public class AuthorizeUserAttribute : AuthorizeAttribute
    {
        // Custom property
        public int AccessLevelID { get; set; }
        public int[] AccessLevelIDs { get; set; }
        public AuthorizeUserAttribute()
        {
            AccessLevelIDs = new int[0];
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var isAuthorized = base.AuthorizeCore(httpContext);
            if (!isAuthorized)
            {
                return false;
            }
            return HasAccess(httpContext);
            //if (GetUserRights(httpContext).Where(item => item.Module.ID == AccessLevelID).Count() > 0)
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {            
            var viewResult = new PartialViewResult
            {
                ViewName = "Unauthorized"
                //ViewData = new ViewDataDictionary(model)
            };
            filterContext.Result = viewResult;
        }
        public bool HasAccess(HttpContextBase httpContext)
        {
            if (AccessLevelIDs.Length > 0)
            {
                foreach(int levelID in AccessLevelIDs)
                {
                    if (GetUserRights(httpContext).Where(item => item.Module.ID == levelID).Count() > 0)
                    {
                        return true;
                    }
                }
                return false;
            }
            else
            {
                if (GetUserRights(httpContext).Where(item => item.Module.ID == AccessLevelID).Count() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public List<SysRoleModule> GetUserRights(HttpContextBase httpContext)
        {
            var context = new ApplicationDbContext();
            List<SysRoleModule> ModuleArray = new List<SysRoleModule>();
            var Roles = context.Users.Include(user => user.Roles).Where(user => httpContext.User.Identity.Name == user.UserName).SingleOrDefault()
                .Roles
                .Select(role => new { role.RoleId })
                .ToList();
            foreach (var role in Roles)
            {
                var RoleModules = context.SysRoleModules
                    .Include(x => x.Module)
                    .Where(item => item.Role.Id == role.RoleId)
                    .ToList();
                foreach (var roleModule in RoleModules)
                {
                    if (!ModuleArray.Contains(roleModule))
                        ModuleArray.Add(roleModule);
                }
            }
            return ModuleArray;
        }
    }
    public class UIAuthorizeUserAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            HttpContext.Current.Response.StatusCode = 403;
            HttpContext.Current.Response.TrySkipIisCustomErrors = true;
            filterContext.Result = new JsonResult
            {
                Data = new { error = "Forbidden" },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
    public class LocalizedDisplayNameAttribute : DisplayNameAttribute
    {
        private readonly string _FiledName;
        private readonly Type _EnglishResourceType;
        private readonly Type _PersianResourceType;
        public LocalizedDisplayNameAttribute(string FiledName, Type EnglishResourceType, Type PersianResourceType)
            : base()
        {
            this._FiledName = FiledName;
            this._EnglishResourceType = EnglishResourceType;
            this._PersianResourceType = PersianResourceType;
        }

        public override string DisplayName
        {
            get
            {

                if (HttpContext.Current.Request.Cookies["Lang"].Value == "fa")
                    return new ResourceManager(_PersianResourceType).GetString(_FiledName);
                else if (HttpContext.Current.Request.Cookies["Lang"].Value == "en")

                    return new ResourceManager(_EnglishResourceType).GetString(_FiledName);
                else
                    return "";
            }
        }
    }
}