using BookShop.Api.Data;
using BookShop.Domain.Entities;
using BookShop.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BookShop.Api.Services;


public class BookService : EntityService<Book> , IBookService
{
    public BookService(BookShopContext context) 
        : base(context) 
    { }

    protected override DbSet<Book> DbSet => _context.Books;


    private const int MAX_PAGE_SIZE = 20;

    public Task<ResponseData<PageModel<Book>>> GetPageAsync(int booksPerPage, int? categoryId, int pageNum = 0)
    {
        if (booksPerPage > MAX_PAGE_SIZE)
            throw new ArgumentException($"Count of books on page can't be more than {MAX_PAGE_SIZE}.");

        var booksOnPages = categoryId is null
            ? DbSet
            : DbSet.Where(book => book.CategoryId == categoryId);

        var pagesCount = (int)Math.Ceiling(booksOnPages.Count() / (double)booksPerPage);

        var booksOnPage = booksOnPages
            .Skip(booksPerPage * pageNum)
            .Take(booksPerPage);

        if (booksOnPage.IsNullOrEmpty())
            throw new ArgumentException($"Invalid page number.");

        var response = new ResponseData<PageModel<Book>>(new PageModel<Book>(booksOnPage, pagesCount, pageNum));

        return Task.FromResult(response);
    }
}
