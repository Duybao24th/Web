using Microsoft.AspNetCore.Mvc;
using DrinkStore.Extensions;
using DrinkStore.Models;

namespace DrinkStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public abstract class BaseAdminController : Controller
    {
        protected SessionCustomer CurrentAdmin =>
            HttpContext.Session.GetObjectOrNull<SessionCustomer>(DrinkStore.Controllers.AccountController.SESSION_USER_KEY);

        public override void OnActionExecuting(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext context)
        {
            var user = CurrentAdmin;
            if (user == null || !user.IsAdmin)
            {
                context.Result = RedirectToAction("Login", "Account", new { area = "", returnUrl = context.HttpContext.Request.Path });
                return;
            }

            ViewBag.CurrentAdmin = user;
            base.OnActionExecuting(context);
        }
    }
}
