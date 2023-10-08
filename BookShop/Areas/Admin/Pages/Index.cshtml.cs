using System;
using System.Collections.Generic;
using System.Linq;
using BookShop.Services.BookService;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BookShop.Api.Data;
using BookShop.Domain.Entities;

namespace BookShop.Areas.Admin.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IBookService _bookService;

        public IndexModel(IBookService bookService)
        {
            _bookService = bookService;
        }

        public IList<Book> Book { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Book = _bookService.GetAllAsync().Result.Data.ToList();
        }
    }
}
