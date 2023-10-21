using System;
using System.Collections.Generic;
using System.Linq;
using BookShop.Services.BookService;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using BookShop.Domain.Entities;
using BookShop.Services.CategoryService;
using BookShop.Api.Controllers;


namespace BookShop.Areas.Admin.Pages;


using BookPageViewModel = BookShop.Domain.Models.PageModel<Book>;


public class IndexModel : PageModel
{
    private readonly IBookService _bookService;
    private readonly ICategoryService _categoryService;

    public IndexModel(IBookService bookService, ICategoryService categoryService)
    {
        _bookService = bookService;
        _categoryService = categoryService;
    }

    public BookPageViewModel Book { get;set; } = default!;

    public async Task OnGetAsync(int pageNum = 0)
    {
        Book = _bookService.GetPageAsync(pageNum).Result.Data;

        var categories = new Dictionary<int, Category>();

        _categoryService.GetAllAsync().Result.Data.ForEach(c => categories.Add(c.Id, c));

        foreach (var item in Book.Items)
        {
            if (item.CategoryId is not null)
            {
                item.Category = categories[(int)item.CategoryId];
            }
        }
    }
}
