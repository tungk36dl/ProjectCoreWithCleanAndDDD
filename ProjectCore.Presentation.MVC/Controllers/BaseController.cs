using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace ProjectCore.Presentation.MVC.Controllers
{

    public abstract class BaseController : Controller
    {
        protected Guid? CurrentUserId
        {
            get
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                return userId == null ? null : Guid.Parse(userId);
            }
        }

        protected bool IsAuthenticated => User.Identity?.IsAuthenticated ?? false;

        protected bool IsInRole(string role)
            => User.IsInRole(role);

        protected IActionResult RedirectToLogin()
            => RedirectToAction("Login", "Auth");

        protected IActionResult Forbidden()
            => View("Forbidden");
    }

}
