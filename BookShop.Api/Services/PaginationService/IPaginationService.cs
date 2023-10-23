using BookShop.Domain.Entities;
using BookShop.Domain.Models;

namespace BookShop.Api.Services;

public interface IPaginationService<T> where T : Entity
{
    public Task<ResponseData<PageModel<T>>> GetPageAsync(int itemsPerPage, int pageNum = 0);
}
