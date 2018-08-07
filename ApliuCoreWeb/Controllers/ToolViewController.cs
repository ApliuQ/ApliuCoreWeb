using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApliuCoreWeb.Controllers
{
    public class ToolViewController : Controller
    {
        public IActionResult QRCode()
        {
            return View();
        }

        public IActionResult SqlServer()
        {
            return View();
        }

        public IActionResult SMSSend()
        {
            return View();
        }

        public IActionResult Drawing()
        {
            return View();
        }

        public IActionResult HttpSend()
        {
            return View();
        }

        public IActionResult Security()
        {
            return View();
        }

        public IActionResult BaseConver()
        {
            return View();
        }
    }
}
