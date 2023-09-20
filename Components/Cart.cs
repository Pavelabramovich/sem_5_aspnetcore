using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;


namespace Web_153505_Abramovich.Components;


public class Cart : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return new HtmlContentViewComponentResult(
            new HtmlString("""00,0 руб <i class="fa-solid fa-cart-shopping"></i> (0)""")
        );
    }
}
