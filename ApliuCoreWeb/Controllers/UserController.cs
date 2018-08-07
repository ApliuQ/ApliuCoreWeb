using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApliuCoreWeb.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Password()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterUser(FormCollection form)
        {
            //string a = form["phone"];
            //string b = form["password"];
            return View();
        }
    }
}
