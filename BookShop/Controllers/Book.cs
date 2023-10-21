using Microsoft.AspNetCore.Mvc;
using BookShop.Services.BookService;
using BookShop.Services.CategoryService;
using BookShop.Services.PaginationService;
using BookShop.Domain.Entities;
using BookShop.Domain.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using BookShop.Extensions;

namespace BookShop.Controllers;

[Route("catalog")]
public class BookController : Controller
{
    private readonly IBookService _bookService;
    private readonly ICategoryService _categoryService;

    public BookController(IBookService bookService, ICategoryService categoryService)
    {
        _bookService = bookService;
        _categoryService = categoryService;
    }

    [HttpGet]
    [Route("")]
    [Route("{categoryName}")]
    public async Task<IActionResult> Index(string? categoryName, int pageNum = 0)
    {
        var categoryResponse = await _categoryService.GetAllAsync();
        ViewData["Categories"] = categoryResponse.Data;

        int? categoryId = categoryResponse.Data.Where(category => category.Name == categoryName).SingleOrDefault()?.Id;


        var booksOnPageResponse = await _bookService.GetProductListAsync(categoryId, pageNum);

        if (!booksOnPageResponse)
            return NotFound(booksOnPageResponse.ErrorMessage);

        if (Request.IsAjaxRequest())
        {
            throw new NotImplementedException();
            return PartialView("_ListPartial", booksOnPageResponse.Data);
        }
        
        return View(booksOnPageResponse.Data);
    }
}
