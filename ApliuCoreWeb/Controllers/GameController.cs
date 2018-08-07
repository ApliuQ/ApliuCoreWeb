using Microsoft.AspNetCore.Mvc;

namespace ApliuCoreWeb.Controllers
{
    public class GameController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
