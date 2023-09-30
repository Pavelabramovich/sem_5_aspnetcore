using Microsoft.AspNetCore.Mvc;
using BookShop.Services.BookService;
using BookShop.Services.CategoryService;
using BookShop.Domain.Entities;


namespace BookShop.Controllers;

public class BookController : Controller
{
    private readonly IBookService _bookService;
    private readonly ICategoryService _categoryService;


    public BookController(IBookService bookService, ICategoryService categoryService)
    {
        _bookService = bookService;
        _categoryService = categoryService;
    }

    public async Task<IActionResult> Index()
    {
        var productResponse = await _bookService.GetBookListAsync(categoryId: 1);

        if (!productResponse.Success)
            return NotFound(productResponse.ErrorMessage);

        return View(productResponse.Data.Items);
    }
}
