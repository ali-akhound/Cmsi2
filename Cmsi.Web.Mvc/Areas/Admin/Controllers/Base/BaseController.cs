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
using System.Web;
using Microsoft.Owin.Security;

namespace AVA.Web.Mvc.Controllers.Admin
{
    public class BaseController : Controller
    {

        #region Ctors
        public BaseController()
        {

        }
        #endregion Ctors

        #region Methods
        public void CheckForAuthentication()
        {
            //if (!_membershipHelper.IsAuthenticated())
            //    throw new HttpException((int)HttpStatusCode.Forbidden, "");
        }
        public void CheckForAuthorization()
        {
            //if (!_membershipHelper.IsAuthorized(LoggedInUser.Username, Request.Url.AbsoluteUri))
            //    throw new HttpException((int)HttpStatusCode.Unauthorized, "");
        }
        #endregion Methods

        #region Public Properties
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }
        public IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        public ApplicationUserManager UserManager
        {

            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        #endregion Public Properties

    }
}
