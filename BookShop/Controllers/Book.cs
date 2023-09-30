using Microsoft.AspNetCore.Mvc;
using BookShop.Services.BookService;
using BookShop.Services.CategoryService;
using BookShop.Domain.Entities;
using BookShop.Domain.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

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

    public async Task<IActionResult> Index(int? categoryId, int? pageNo)
    {
        var categoryResponse = await _categoryService.GetCategoryListAsync();

        if (!categoryResponse.Success)
            return NotFound(categoryResponse.ErrorMessage);

        if (categoryResponse.Data.Count == 0)
            return NotFound("No categories in collection");

        if (categoryId is null)
            categoryId = categoryResponse.Data!.FirstOrDefault()!.Id;

        if (pageNo is null)
            pageNo = 1;
        

        var productResponse = await _bookService.GetBookListByIdAsync((int)categoryId, (int)pageNo);

        if (!productResponse.Success)
            return NotFound(productResponse.ErrorMessage);

        ViewData["Categories"] = categoryResponse.Data;

        return View(productResponse.Data);
    }
}
