using BookShop.Api.Data;
using BookShop.Domain.Entities;
using BookShop.Domain.Models;
using Microsoft.EntityFrameworkCore;


namespace BookShop.Api.Services;


public class BookService : EntityService<Book>
{
    private IWebHostEnvironment _environment;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public BookService(BookShopContext context, IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor) 
        : base(context) 
    { 
        _environment = env;
        _httpContextAccessor = httpContextAccessor;
    }

    protected override DbSet<Book> DbSet => _context.Books;
}
