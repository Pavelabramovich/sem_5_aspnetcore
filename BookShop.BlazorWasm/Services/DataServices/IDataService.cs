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

    int PagesCount { get;  }
    int PageNum { get; }


    Task GetBookListAsync(string? categoryName, int pageNum = 0);

    Task<Book> GetBookByIdAsync(int id);

    Task GetCategoryListAsync();
}