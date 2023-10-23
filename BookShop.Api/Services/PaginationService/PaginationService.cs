using BookShop.Domain.Entities;
using BookShop.Domain.Models;

namespace BookShop.Api.Services;

//public class PaginationService<T> : IPaginationService<T> where T : Entity
//{
//    public Task<ResponseData<PageModel<T>>> GetPageAsync(int entitiesPerPage, IEnumerable<T> entities, int pageNum = 0)
//    {
//        var pagesCount = (int)Math.Ceiling(entities.Count() / (double)entitiesPerPage);

//        IEnumerable<T> entitiesOnPage;

//        entitiesOnPage = entities
//            .Skip(entitiesPerPage * pageNum)
//            .Take(entitiesPerPage)
//            .ToArray();

//        var response = new ResponseData<PageModel<T>>(new PageModel<T>(entitiesOnPage, pagesCount, pageNum));

//        return Task.FromResult(response);
//    }
//}
