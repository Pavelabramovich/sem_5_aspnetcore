using BookShop.Domain.Entities;
using BookShop.Domain.Models;

namespace BookShop.Services.PaginationService;

public class PaginationService<T> : IPaginationService<T> where T : Entity
{
    public Task<ResponseData<PageModel<T>>> GetPageAsync(int entitiesPerPage, IEnumerable<T> entities, int pageNo = 0)
    {
        var pageCount = entities.Count() / entitiesPerPage + 1;

        var entitiesOnPage = entities
            .Skip(entitiesPerPage * pageNo)
            .Take(entitiesPerPage);

        var result = new ResponseData<PageModel<T>>(data: new() { Items = entitiesOnPage.ToList(), PageNum = pageNo, PagesCount = pageCount });

        return Task.FromResult(result);
    }
}
