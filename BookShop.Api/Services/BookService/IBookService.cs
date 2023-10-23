using BookShop.Domain.Entities;
using BookShop.Domain.Models;

namespace BookShop.Api.Services;

public interface IBookService : IEntityService<Book>
{
    public Task<ResponseData<PageModel<Book>>> GetPageAsync(int booksPerPage, int? categoryId, int pageNum = 0);
}
