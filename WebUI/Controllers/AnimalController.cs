using Microsoft.AspNetCore.Mvc;

namespace NLayer.WebUI.Controllers
{
    public class AnimalController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
