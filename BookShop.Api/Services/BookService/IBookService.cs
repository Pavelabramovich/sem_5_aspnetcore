using BookShop.Domain.Entities;
using BookShop.Domain.Models;
using BookShop.Api.Services;

namespace BookShop.Api.Services;


public interface IBookService : IEntityService<Book>
{ }