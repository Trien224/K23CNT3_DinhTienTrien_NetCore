using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Neme.Filters
{
    public class AuthorizeAdminAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session.GetString("AdminEmail");
            if (string.IsNullOrEmpty(session))
            {
                context.Result = new RedirectToActionResult("Login", "AdminAccount", null);
            }
            base.OnActionExecuting(context);
        }
    }
}
