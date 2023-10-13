using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BookShop.Services.BookService;
using Microsoft.EntityFrameworkCore;

using BookShop.Domain.Entities;

namespace BookShop.Areas.Admin.Pages
{
    public class EditModel : PageModel
    {
        private readonly IBookService _bookService;

        public EditModel(IBookService bookService)
        {
            _bookService = bookService;
        }

        [BindProperty]
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
                return NotFound();
            }
            Book = bookResponse.Data;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _bookService.UpdateByIdAsync(Book.Id, Book);

            //_context.Attach(Book).State = EntityState.Modified;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!BookExists(Book.Id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            return RedirectToPage("./Index");
        }

        private bool BookExists(int id)
        {
            return _bookService.GetByIdAsync(id) != null;
        }
    }
}
