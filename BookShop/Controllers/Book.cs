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

    public async Task<IActionResult> Index(int? categoryId, int? pageNo)
    {
        var categoryResponse = await _categoryService.GetAllAsync();

        if (!categoryResponse)
            return NotFound(categoryResponse.ErrorMessage);

        if (categoryResponse.Data.Count() == 0)
            return NotFound("No categories in collection");

        if (categoryId is null)
            categoryId = categoryResponse.Data!.FirstOrDefault()!.Id;

        if (pageNo is null)
            pageNo = 0;
        

        var productResponse = await _bookService.GetAllAsync();

        if (!productResponse)
            return NotFound(productResponse.ErrorMessage);

        ViewData["Categories"] = categoryResponse.Data;

        var productsOnPageResponse = await _paginationService.GetPageAsync(3, productResponse.Data.Where(product => product.Category.Id == categoryId), (int)pageNo);

        if (!productsOnPageResponse)
            return NotFound(productsOnPageResponse.ErrorMessage);

        return View(productsOnPageResponse.Data);
    }
}
