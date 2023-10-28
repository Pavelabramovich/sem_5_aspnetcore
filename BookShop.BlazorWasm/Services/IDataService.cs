using BookShop.Domain.Entities;

namespace BookShop.BlazorWasm.Services;

public interface IDataService
{
    Task<List<Book>> GetBooksAsync();
    Task<List<Category>> GetCategoriesAsync();


    bool Success { get; }
    string ErrorMessage { get;  }

    int TotalPages { get;  }
    int CurrentPage { get; }


    Task GetProductListAsync(string? categoryName, int pageNum = 1);

    Task GetProductByIdAsync(int id);

    Task GetCategoryListAsync();
}