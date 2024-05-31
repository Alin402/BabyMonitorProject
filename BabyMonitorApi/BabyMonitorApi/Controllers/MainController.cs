using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BabyMonitorApi.Controllers
{
    [Authorize]
    public class MainController : Controller
    {
        public IActionResult Index()
        {
            string indexPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "build", "index.html");

            if (System.IO.File.Exists(indexPath))
            {
                return PhysicalFile(indexPath, "text/html");
            }
            else
            {
                return NotFound();
            }
        }
    }
}
