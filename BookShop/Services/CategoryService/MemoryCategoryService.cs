using BookShop.Domain.Entities;
using BookShop.Domain.Models;
using BookShop.Services.EntityService;

namespace BookShop.Services.CategoryService;

public class MemoryCategoryService : ICategoryService
{
    private List<Category> _categories;

    public MemoryCategoryService()
    {
        _categories = new List<Category>
        {
            new Category { Id = 1, Name = "Programming" },
            new Category { Id = 2, Name = "Child" },
        };
    }

    public Task AddAsync(Category entity)
    {
        throw new NotImplementedException();
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

    public Task<ResponseData<List<Category>>> GetAllAsync()
    {
        var response = new ResponseData<List<Category>>(data: _categories);

        return Task.FromResult(response);
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

    public Task UpdateByIdAsync(int id, Category entity, IFormFile? formFile)
    {
        throw new NotImplementedException();
    }

    Task<ResponseData<Category>> IEntityService<Category>.AddAsync(Category entity)
    {
        throw new NotImplementedException();
    }
}