using BookShop.Api.Data;
using BookShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace BookShop.Api.Services;


public class BookService : EntityService<Book>
{
    public BookService(BookShopContext context) 
        : base(context) 
    { }

    protected override DbSet<Book> DbSet => _context.Books;
}
