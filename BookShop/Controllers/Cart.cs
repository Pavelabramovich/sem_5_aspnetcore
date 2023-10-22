using BookShop.Extensions;
using BookShop.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BookShop.Services.BookService;
using BookShop.Services.CategoryService;

namespace BookShop.Controllers;

[Route("[controller]")]
public class CartController : Controller
{
    private IBookService _bookService;
    private ICategoryService _categoryService;
    private readonly Cart _cart;

    public const string CartSessionKey = "_Cart";

    public CartController(IBookService bookService, ICategoryService categoryService, Cart cart)
    {
        _bookService = bookService;
        _categoryService = categoryService;

        _cart = cart;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var categories = new Dictionary<int, Category>();
        _categoryService.GetAllAsync().Result.Data.ForEach(c => categories.Add(c.Id, c));

        foreach (var item in _cart)
        {
            if (item.Book.CategoryId is not null)
            {
                item.Book.Category = categories[(int)item.Book.CategoryId];
            }
        }

        return View(_cart);
    }


    [Authorize]
    [Route("add/{id:int}")]
    public async Task<ActionResult> Add(int id, string returnUrl)
    {
        var responce = await _bookService.GetByIdAsync(id);

        if (responce)
        {
            _cart.AddToCart(responce.Data!);
            HttpContext.Session.Set<Cart>(CartSessionKey, _cart);
        }

        return Redirect(returnUrl);
    }

    [Authorize]
    [Route("removeOne/{id:int}")]
    public async Task<ActionResult> RemoveOne(int id, string returnUrl)
    {

        var responce = await _bookService.GetByIdAsync(id);

        if (responce)
        {
            _cart.RemoveOne(responce.Data!.Id);
            HttpContext.Session.Set<Cart>(CartSessionKey, _cart);
        }

        return Redirect(returnUrl);
    }

    [Authorize]
    [Route("remove/{id:int}")]
    public async Task<ActionResult> Remove(int id, string returnUrl)
    {
        var responce = await _bookService.GetByIdAsync(id);

        if (responce)
        {
            _cart.RemoveItems(responce.Data!.Id);
            HttpContext.Session.Set<Cart>(CartSessionKey, _cart);
        }

        return Redirect(returnUrl);
    }


    [Authorize]
    [Route("clear")]
    public async Task<ActionResult> Clear(string returnUrl)
    {
        _cart.Clear();
        HttpContext.Session.Set<Cart>(CartSessionKey, _cart);

        return Redirect(returnUrl);
    }
}
