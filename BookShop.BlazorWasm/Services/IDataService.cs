using BookShop.Domain.Entities;
using BookShop.Domain.Models;

namespace BookShop.BlazorWasm.Services;

public interface IDataService
{
    event Action DataLoaded;

    Task<List<Book>> GetBooksAsync();
    Task<List<Category>> GetCategoriesAsync();


    bool Success { get; }
    string ErrorMessage { get;  }

    int TotalPages { get;  }
    int CurrentPage { get; }


    Task<PageModel<Book>> GetProductListAsync(string? categoryName, int pageNum = 0);

    Task GetProductByIdAsync(int id);

    Task GetCategoryListAsync();
}