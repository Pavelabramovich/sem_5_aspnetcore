using Microsoft.AspNetCore.Mvc;
using BookShop.Services.BookService;
using BookShop.Services.CategoryService;
using BookShop.Services.PaginationService;
using BookShop.Domain.Entities;
using BookShop.Domain.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookShop.Controllers;

public class BookController : Controller
{
    private readonly IBookService _bookService;
    private readonly ICategoryService _categoryService;
    private readonly IPaginationService<Book> _paginationService;

    public BookController(IBookService bookService, ICategoryService categoryService, IPaginationService<Book> paginationService)
    {
        _bookService = bookService;
        _categoryService = categoryService;
        _paginationService = paginationService;
    }

    public async Task<IActionResult> Index(int? categoryId, int pageNum = 0)
    {
        var categoryResponse = await _categoryService.GetAllAsync();
        ViewData["Categories"] = categoryResponse.Data;

        var productsOnPageResponse = await _bookService.GetProductListAsync(categoryId, pageNum);

        if (!productsOnPageResponse)
            return NotFound(productsOnPageResponse.ErrorMessage);

        return View(productsOnPageResponse.Data);
    }
}
