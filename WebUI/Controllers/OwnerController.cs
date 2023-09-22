using Microsoft.AspNetCore.Mvc;

namespace NLayer.WebUI.Controllers
{
    public class OwnerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
