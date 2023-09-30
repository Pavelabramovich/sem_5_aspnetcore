using BookShop.Domain.Entities;
using BookShop.Domain.Models;

namespace BookShop.Services.BookService;


public interface IBookService //Task<ResponseData<List<Book>>>
{
    /// <include file='IBookService.cs.xml' path='doc/class[@name="IBookService"]/method[@name="GetBookListAsync"]' />
    public Task<ResponseData<ListModel<Book>>> GetBookListAsync(int categoryId, int pageNo = 1);

    /// <include file='IBookService.cs.xml' path='doc/class[@name="IBookService"]/method[@name="GetBookByIdAsync"]' />
    public Task<ResponseData<Book>> GetBookByIdAsync(int id);

    /// <include file='IBookService.cs.xml' path='doc/class[@name="IBookService"]/method[@name="UpdateBookAsync"]' />
    public Task UpdateBookAsync(int id, Book book, IFormFile? formFile);

    /// <include file='IBookService.cs.xml' path='doc/class[@name="IBookService"]/method[@name="DeleteBookAsync"]' />
    public Task DeleteBookAsync(int id);

    /// <include file='IBookService.cs.xml' path='doc/class[@name="IBookService"]/method[@name="CreateBookAsync"]' />
    public Task<ResponseData<Book>> CreateBookAsync(Book book, IFormFile? formFile);
}