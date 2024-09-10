using Microsoft.AspNetCore.Mvc;

namespace TravelEasy.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
