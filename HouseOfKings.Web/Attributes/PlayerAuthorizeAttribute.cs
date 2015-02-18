using HouseOfKings.Web.Properties;
using System.Web.Mvc;
using System.Web.Routing;

namespace HouseOfKings.Web.Attributes
{
    public class PlayerAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            var cookie = filterContext.HttpContext.Request.Cookies[Resources.CookieName];

            if (cookie == null || string.IsNullOrEmpty(cookie[Resources.CookieUsername]))
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "Username", returnUrl = filterContext.HttpContext.Request.Url }));
            }
        }
    }
}