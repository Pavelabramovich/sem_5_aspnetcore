using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BookShop.Services.BookService;
using BookShop.Api.Data;
using BookShop.Domain.Entities;

namespace BookShop.Areas.Admin.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly IBookService _bookService;

        public DetailsModel(IBookService bookService)
        {
            _bookService = bookService;
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
            return Page();
        }
    }
}
