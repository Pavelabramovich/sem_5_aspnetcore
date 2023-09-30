using BookShop.Domain.Entities;
using BookShop.Domain.Models;

namespace BookShop.Services.PaginationService;

public interface IPaginationService<T> where T : Entity
{
    public Task<ResponseData<PageModel<T>>> GetPageAsync(int itemsPerPage, IEnumerable<T> entities, int pageNo = 0);
}
