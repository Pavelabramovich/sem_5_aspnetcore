using BookShop.Domain.Entities;
using BookShop.Extensions;
using Microsoft.AspNetCore.Mvc;


namespace BookShop.Services;


public class SessionCart : Cart
{
    private const string CartSessionKey = "_Cart";
    private readonly ISession _session;
    private readonly Cart _cart;

    public SessionCart(IHttpContextAccessor httpContextAccessor)
    {
        _session = httpContextAccessor.HttpContext.Session;

        _cart = _session.Get<Cart>(CartSessionKey)!;

        if (_cart is null)
        {
            _cart = new();
            _session.Set<Cart>(CartSessionKey, _cart);
        }

        CartItems = _cart.CartItems;
    }


    public override void AddToCart(Book book)
    {
        _cart.AddToCart(book);
        _session.Set<Cart>(CartSessionKey, _cart);
    }

    public override void RemoveOne(int id)
    {
        _cart.RemoveOne(id);
        _session.Set<Cart>(CartSessionKey, _cart);
    }

    public override void RemoveItems(int id)
    {
        _cart.RemoveItems(id);
        _session.Set<Cart>(CartSessionKey, _cart);
    }

    public override void Clear()
    {
        _cart.Clear();
        _session.Set<Cart>(CartSessionKey, _cart);
    }
}
