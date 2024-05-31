using Microsoft.AspNetCore.Mvc;

namespace BabyMonitorApi.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return Redirect("/Main");
            }
            return View();
        }

        public IActionResult SignUp()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return Redirect("/Main");
            }
            return View();
        }

        public IActionResult SignIn()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return Redirect("/Main");
            }
            return View();
        }
    }
}