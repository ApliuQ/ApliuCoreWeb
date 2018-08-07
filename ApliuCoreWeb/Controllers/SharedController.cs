using Microsoft.AspNetCore.Mvc;

namespace ApliuCoreWeb.Controllers
{
    public class SharedController : Controller
    {
        public IActionResult Error()
        {
            return View();
        }

        public IActionResult Templet()
        {
            return View();
        }
    }
}
