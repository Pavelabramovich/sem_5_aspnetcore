using BookShop.Api.Data;
using BookShop.Domain.Entities;
using BookShop.Domain.Models;

namespace BookShop.Api.Services.CategoryService;

public class CategoryService : ICategoryService
{
    private readonly BookShopContext _context;

    public CategoryService(BookShopContext context)
    {
        _context = context;
    }

    public Task<ResponseData<IEnumerable<Category>>> GetAllAsync()
    {
      //  var d = _context.Books.ToList();
        
        var response = new ResponseData<IEnumerable<Category>>(data: _context.Categories);

        return Task.FromResult(response);
    }

    public Task ClearAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task DeleteByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseData<Category?>> FirstOrNullAsync()
    {
        throw new NotImplementedException();
    }

    public Task<ResponseData<Category?>> FirstOrNullAsync(Func<Category, bool> predicate)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseData<Category?>> FirstOrNullAsync(IEnumerable<Func<Category, bool>> predicates)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseData<Category?>> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseData<IEnumerable<Category>>> GetWhereAsync(Func<Category, bool> predicate)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseData<IEnumerable<Category>>> GetWhereAsync(IEnumerable<Func<Category, bool>> predicates)
    {
        throw new NotImplementedException();
    }

    public Task UpdateByIdAsync(int id, Category entity)
    {
        throw new NotImplementedException();
    }

    public Task UpdateByIdAsync(int id, Action<Category> replacement)
    {
        throw new NotImplementedException();
    }

    public Task UpdateByIdAsync(int id, IEnumerable<Action<Category>> replacements)
    {
        throw new NotImplementedException();
    }
}