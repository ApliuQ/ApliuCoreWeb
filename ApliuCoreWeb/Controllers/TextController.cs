using Microsoft.AspNetCore.Mvc;

namespace ApliuCoreWeb.Controllers
{
    [Route("text/{id?}")]
    public class TextController : Controller
    {
        public IActionResult Index(string id)
        {
            return View((object)id);
        }
    }
}
