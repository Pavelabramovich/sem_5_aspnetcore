using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web_153505_Abramovich.Models;

namespace Web_153505_Abramovich.Controllers;

public class Home : Controller
{
    private IEnumerable<SelectListItem> listDemo;

    public IActionResult Index()
    {
        listDemo = new[]
        {
            new ListDemo { Id = 1, Name = "111" },
            new ListDemo { Id = 2, Name = "222" },
            new ListDemo { Id = 3, Name = "333" },
        }
        .Select(elem => new SelectListItem
        {
            Value = elem.Id.ToString(),
            Text = elem.Name
        });

        return View(model: listDemo);
    }
}
