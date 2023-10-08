using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BookShop.Api.Data;
using BookShop.Services.BookService;
using BookShop.Domain.Entities;

namespace BookShop.Areas.Admin.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly IBookService _bookService;

        public DeleteModel(IBookService bookService)
        {
            _bookService = bookService;
        }

        [BindProperty]
        public Book Book { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)           
                return NotFound();

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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)        
                return NotFound();

            var bookResponse = await _bookService.GetByIdAsync((int)id);

            if (bookResponse)
            {
                Book = bookResponse.Data;

                await _bookService.DeleteByIdAsync(Book.Id);
                //_context.Books.Remove(Book); 
                //await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
