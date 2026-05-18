using Microsoft.AspNetCore.Mvc;

namespace tms_acl_api.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
    }
}
