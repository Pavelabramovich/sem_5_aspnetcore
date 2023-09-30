using Microsoft.AspNetCore.Mvc;

namespace BookShop.Controllers;

public class CartController : Controller
{
    [HttpPost]
    public IActionResult Add(int id, string returnUrl)
    {
        return View();
    }
}
