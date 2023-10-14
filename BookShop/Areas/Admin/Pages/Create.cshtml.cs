using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

using BookShop.Services.BookService;
using BookShop.Services.CategoryService;
using BookShop.Services.PaginationService;
using BookShop.Domain.Entities;
using BookShop.Api.Services.CategoryService;

namespace BookShop.Areas.Admin.Pages;

public class CreateModel : PageModel
{
    private readonly IBookService _bookService;
    private readonly ICategoryService _categoryService;

    public CreateModel(IBookService bookService, ICategoryService categoryService)
    {
        _bookService = bookService;
        _categoryService = categoryService;
    }

    public IActionResult OnGet()
    {
        var categoryResponse = _categoryService.GetAllAsync().Result;

        if (!categoryResponse)
            return NotFound();

        CategoriesOptions = new SelectListItem[] { new(text: string.Empty, value: string.Empty) }
            .Concat(categoryResponse.Data!
                .Select(category => new SelectListItem
                {
                    Value = category.Id.ToString(),
                    Text = category.Name
                }))
                .ToList();

        return Page();
    }

    [BindProperty]
    public Book Book { get; set; } = default!;

    public List<SelectListItem> CategoriesOptions { get; set; }

    [BindProperty]
    public IFormFile? Image { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid || !_bookService.GetAllAsync().Result || Book == null)
        {
            return Page();
        }

        await _bookService.AddAsync(Book, Image);

        return RedirectToPage("./Index");
    }
}
