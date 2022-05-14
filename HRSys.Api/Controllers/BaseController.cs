using System;
using System.Globalization;
using System.Security.Claims;
using System.Threading;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Filters;
using HRSys.Api.Helpers;
using HRSys.Enum;

namespace HRSys.Api.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class BaseController : Controller
    {

       public CurrentUserInfo CurrentUserInfo { get; private set; }
        public BaseController(IHttpContextAccessor contextAccessor)
        {
            if (CurrentUserInfo == null)
                CurrentUserInfo = JsonConvert.DeserializeObject<CurrentUserInfo>(contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.UserData));

            //CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.IetfLanguageTag);
            //System.Threading.Thread.CurrentThread.CurrentCulture = ci;
            //System.Threading.Thread.CurrentThread.CurrentUICulture = ci;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string area = context.RouteData.Values["Area"] == null ? "" : context.RouteData.Values["Area"].ToString();
            string controller = context.RouteData.Values["Controller"].ToString();
            string action = context.RouteData.Values["Action"].ToString();

            //How can I pass in additional parameters?
            //foreach (var parameter in context.ActionParameters)
            //{
            //    var parameterKey = parameter.Key;
            //    var parameterValue = parameter.Value;
            //}

            //How can I get the user? 
            var user = this.User; // IPrinciple instance, explore this object


            base.OnActionExecuting(context);
        }
        public ActionResult SetCulture(string culture)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(culture);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(culture);
            string cultureValue = Request.Cookies["_culture"];
            if (!string.IsNullOrWhiteSpace(cultureValue))
                RemoveCookies("_culture");
            SetCookiesValue("_culture", culture, DateTime.Now.AddYears(1));

            return Redirect(Request.Headers["Referer"].ToString());
        }
        public void RemoveCookies(string key)
        {
            Response.Cookies.Delete(key);
        }
        public void SetCookiesValue(string key, string value, DateTime? expireTime)
        {
            CookieOptions option = new CookieOptions();
            if (expireTime.HasValue && expireTime.Value != DateTime.MinValue)
                option.Expires = expireTime.Value;
            else
                option.Expires = DateTime.Now.AddMilliseconds(10);
            Response.Cookies.Append(key, value, option);
        }
        public Lang CurrentLang
        {
            get
            {
                string code = Thread.CurrentThread.CurrentCulture.Name;
                try
                {
                    Lang lang = (Lang)HRSys.Enum.Lang.Parse(typeof(Lang), code);
                    return lang;
                }
                catch
                {
                    return Lang.ar;
                }

            }
        }
      
    }
}