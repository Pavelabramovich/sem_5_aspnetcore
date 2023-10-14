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
using BookShop.Services.CategoryService;

namespace BookShop.Areas.Admin.Pages
{
    public class EditModel : PageModel
    {
        private readonly IBookService _bookService;
        private readonly ICategoryService _categoryService;

        public EditModel(IBookService bookService, ICategoryService categoryService)
        {
            _bookService = bookService;
            _categoryService = categoryService;
        }

        [BindProperty]
        public Book Book { get; set; } = default!;


        [BindProperty]
        public IFormFile? Image { get; set; }

        public List<SelectListItem> CategoriesOptions { get; set; }


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

            var categoryResponse = await _categoryService.GetAllAsync();

            if (!categoryResponse)
                return NotFound();

            CategoriesOptions = categoryResponse.Data!
                .Select(category => new SelectListItem
                {
                    Value = category.Id.ToString(),
                    Text = category.Name
                })
                .ToList();

            if (Book.CategoryId is null)
                CategoriesOptions.Insert(0, new SelectListItem(text: string.Empty, value: string.Empty));

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
          //  var categoryResponse = await _categoryService.GetByIdAsync(CategoryId);

         //   if (!categoryResponse)
         //       return NotFound();
         //
          //  Book.CategoryId = CategoryId;


            if (!ModelState.IsValid)
                return Page();

            await _bookService.UpdateByIdAsync(Book.Id, Book, Image);

            return RedirectToPage("./Index");
        }
    }
}
