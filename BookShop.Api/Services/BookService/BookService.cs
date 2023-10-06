using BookShop.Api.Data;
using BookShop.Domain.Entities;
using BookShop.Domain.Models;
using BookShop.Api.Services.CategoryService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Api.Services;

public class BookService : IBookService
{
    private readonly BookShopContext _context;

    public BookService(BookShopContext context)
    {
        _context = context;
    }

    public Task<ResponseData<IEnumerable<Book>>> GetAllAsync()
    {
        var response = new ResponseData<IEnumerable<Book>>(data: _context.Books);

        return Task.FromResult(response);
    }


    public Task<ResponseData<Book?>> GetByIdAsync(int id)
    {
        return FirstOrNullAsync(book => book.Id == id);
    }

    public Task<ResponseData<IEnumerable<Book>>> GetWhereAsync(Func<Book, bool> predicate)
    {
        var response = new ResponseData<IEnumerable<Book>>(data: _context.Books.ToList().Where(predicate));

        return Task.FromResult(response);
    }

    public Task<ResponseData<IEnumerable<Book>>> GetWhereAsync(IEnumerable<Func<Book, bool>> predicates)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseData<Book?>> FirstOrNullAsync()
    {
        return FirstOrNullAsync(book => true);
    }

    public Task<ResponseData<Book?>> FirstOrNullAsync(Func<Book, bool> predicate)
    {
        var response = new ResponseData<Book?>(data: _context.Books.ToList().FirstOrDefault(predicate));

        return Task.FromResult(response);
    }

    public Task<ResponseData<Book?>> FirstOrNullAsync(IEnumerable<Func<Book, bool>> predicates)
    {
        throw new NotImplementedException();
    }

    public Task UpdateByIdAsync(int id, Book entity)
    {
        throw new NotImplementedException();
    }

    public Task UpdateByIdAsync(int id, Action<Book> replacement)
    {
        throw new NotImplementedException();
    }

    public Task UpdateByIdAsync(int id, IEnumerable<Action<Book>> replacements)
    {
        throw new NotImplementedException();
    }

    public Task DeleteByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task ClearAsync(int id)
    {
        throw new NotImplementedException();
    }
}
