using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;


using CartModel = BookShop.Domain.Entities.Cart;


namespace BookShop.Components;





public class Cart : ViewComponent
{
    private readonly CartModel _cart;

    public Cart(CartModel cart) 
    {
        _cart = cart;
    }


    public IViewComponentResult Invoke()
    {
        int price = _cart.TotalPrice;
        int count = _cart.Count;

        return new HtmlContentViewComponentResult(
            new HtmlString($"""{price} $ <i class="fa-solid fa-cart-shopping"></i> ({count})""")
        );
    }
}
