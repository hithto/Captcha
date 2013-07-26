using System;
using System.Text;
using System.Web.Mvc;

namespace ImageDraw
{
    public class MvcCaptchaAttribute : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var captchaActual = filterContext.HttpContext.Request.Form["CaptchaActual"];

            if (captchaActual == "")
            {
                filterContext.Controller.ViewData.ModelState.AddModelError("CaptchaActual", "您還沒輸入你的驗證碼！！");
                return;
            }

            var captchaExpected = Encoding.Unicode.GetString(Convert.FromBase64String(filterContext.HttpContext.Request.Form["CaptchaExpected"]));

            if (captchaActual != captchaExpected)
                filterContext.Controller.ViewData.ModelState.AddModelError("CaptchaActual", "驗證碼無效");
        }
    }
}