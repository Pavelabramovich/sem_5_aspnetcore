using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BookShop.Services.BookService;

using BookShop.Domain.Entities;
using BookShop.Services.CategoryService;

namespace BookShop.Areas.Admin.Pages;

public class DetailsModel : PageModel
{
    private readonly IBookService _bookService;
    private readonly ICategoryService _categoryService;

    public DetailsModel(IBookService bookService, ICategoryService categoryService)
    {
        _bookService = bookService;
        _categoryService = categoryService;
    }

    public Book Book { get; set; } = default!; 

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var bookResponse = await _bookService.GetByIdAsync((int)id);

        if (!bookResponse)
        {
            return NotFound(bookResponse.ErrorMessage);
        }
        else 
        {
            Book = bookResponse.Data;
        }

        if (Book.CategoryId is not null)
        {
            var categoryResponse = await _categoryService.GetByIdAsync((int)Book.CategoryId);

            if (!categoryResponse)
            {
                return NotFound(categoryResponse.ErrorMessage);
            }
            else
            {
                Book.Category = categoryResponse.Data;
            }
        }
        
        return Page();
    }
}
