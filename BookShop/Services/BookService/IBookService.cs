using BookShop.Domain.Entities;
using BookShop.Domain.Models;
using BookShop.Services.EntityService;

namespace BookShop.Services.BookService;


public interface IBookService : IEntityService<Book>
{ }