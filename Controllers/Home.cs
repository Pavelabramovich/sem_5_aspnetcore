using Microsoft.AspNetCore.Mvc;

namespace Web_153505_Abramovich.Controllers
{
    public class Home : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
