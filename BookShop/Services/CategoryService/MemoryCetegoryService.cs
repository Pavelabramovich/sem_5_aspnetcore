using BookShop.Domain.Entities;
using BookShop.Domain.Models;

namespace BookShop.Services.CategoryService;

public class MemoryCategoryService : ICategoryService
{
    private List<Category> _categories;

    public MemoryCategoryService()
    {
        _categories = new List<Category>
        {
            new Category { Id=1, Name="Programming" },
            new Category {Id=2, Name="Child" },
        };
    }

    public Task<ResponseData<List<Category>>> GetCategoryListAsync()
    {
        var response = new ResponseData<List<Category>>
        {
            Data = _categories
        };

        return Task.FromResult(response);
    }

    public Task<ResponseData<Category?>> FirstOrDefaultAsync()
    {
        var result = _categories.FirstOrDefault();

        var response = new ResponseData<Category?>
        {
            Data = result,
        };

        if (result is null)
        {
            response.Success = false;
            response.ErrorMessage = "No categories in collection";
        }

        return Task.FromResult(response);
    }
}