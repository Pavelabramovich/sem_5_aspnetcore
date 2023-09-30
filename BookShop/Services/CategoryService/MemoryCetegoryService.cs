using BookShop.Domain.Entities;
using BookShop.Domain.Models;

namespace BookShop.Services.CategoryService;

public class MemoryCategoryService : ICategoryService
{
    public Task<ResponseData<List<Category>>> GetCategoryListAsync()
    {
        var categories = new List<Category>
        {
            new Category { Id=1, Name="Programming" },
            new Category {Id=2, Name="Child" },
        };

        var result = new ResponseData<List<Category>>();
        result.Data = categories;

        return Task.FromResult(result);
    }
}