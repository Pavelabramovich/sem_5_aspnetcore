using BookShop.Domain.Entities;
using BookShop.Domain.Models;

namespace BookShop.BlazorWasm.Services;

public interface IDataService
{
    event Action DataLoaded;

    IEnumerable<Book>? Books { get; }
    IEnumerable<Category>? Categories { get; }

    bool Success { get; }
    string? ErrorMessage { get;  }

    int TotalPages { get;  }
    int CurrentPage { get; }


    Task GetBookListAsync(string? categoryName, int pageNum = 0);

    Task GetBookByIdAsync(int id);

    Task GetCategoryListAsync();
}