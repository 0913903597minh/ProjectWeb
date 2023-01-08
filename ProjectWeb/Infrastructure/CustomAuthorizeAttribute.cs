using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ProjectWeb.Infrastructure
{

    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly string[] allowedroles;
        public CustomAuthorizeAttribute(params string[] roles)
        {
            this.allowedroles = roles;
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;

            if (httpContext.Session["User"] != null)
            {
                //var userRole = LibData.Configuration.UserConfig.RoleToDictionaryHTML[(httpContext.Session["User"] as LibData.User).Role.Value];
                //if (!string.IsNullOrEmpty(userRole))
                //{
                //    foreach (var role in allowedroles)
                //    {
                //        if (role.Equals(userRole)) return true;
                //    }
                //}
                return true;
            }
            else
            {
                return false;
            }
            return authorize;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectToRouteResult(
                 new RouteValueDictionary
                 {
                        { "controller", "Login" },
                        { "action", "Index" }
                 });
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary
                 {
                        { "controller", "User" },
                        { "action", "UnAuthorized" }
                 });
            }
          
        }
    }
}