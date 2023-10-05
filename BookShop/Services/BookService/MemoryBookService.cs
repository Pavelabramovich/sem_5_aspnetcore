using BookShop.Domain.Entities;
using BookShop.Domain.Models;
using BookShop.Services.CategoryService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookShop.Services.BookService;

public class MemoryBookService : IBookService
{
    private List<Book> _books;
    private List<Category> _categories;

    private IConfiguration _config;

    public MemoryBookService([FromServices] IConfiguration config, ICategoryService categoryService)
    {
        _categories = categoryService.GetAllAsync().Result.Data.ToList();
        _config = config;

        SetupData();
    }


    private void SetupData()
    {
        _books = new List<Book>
        {
            new Book
            {
                Id = 1,
                Title = "Schildt",
                Category = _categories.Find(c => c.Name?.Equals("Programming") ?? false),
                Price = 3m,
                ImagePath = "Images/light.png",
            },
            new Book
            {
                Id = 2,
                Title = "GoF",
                Category = _categories.Find(c => c.Name?.Equals("Programming") ?? false),
                Price = 5m,
                ImagePath = "Images/light.png",
            },
            new Book
            {
                Id = 3,
                Title = "Schildt",
                Category = _categories.Find(c => c.Name?.Equals("Programming") ?? false),
                Price = 8m,
                ImagePath = "Images/light.png",
            },
            new Book
            {
                Id = 4,
                Title = "GoF",
                Category = _categories.Find(c => c.Name?.Equals("Programming") ?? false),
                Price = 6m,
                ImagePath = "Images/light.png",
            },
            new Book
            {
                Id = 5,
                Title = "ABC",
                Category = _categories.Find(c => c.Name?.Equals("Child") ?? false),
                Price = 2m,
                ImagePath = "Images/light.png",
            },
        };
    }


    public Task<ResponseData<IEnumerable<Book>>> GetAllAsync()
    {
        var response = new ResponseData<IEnumerable<Book>>(data: new List<Book>(_books));

        return Task.FromResult(response);
    }

    public Task<ResponseData<Book?>> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseData<IEnumerable<Book>>> GetWhereAsync(Func<Book, bool> predicate)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseData<IEnumerable<Book>>> GetWhereAsync(IEnumerable<Func<Book, bool>> predicates)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseData<Book?>> FirstOrNullAsync()
    {
        throw new NotImplementedException();
    }

    public Task<ResponseData<Book?>> FirstOrNullAsync(Func<Book, bool> predicate)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseData<Book?>> FirstOrNullAsync(IEnumerable<Func<Book, bool>> predicates)
    {
        throw new NotImplementedException();
    }

    public Task UpdateByIdAsync(int id, Book entity)
    {
        throw new NotImplementedException();
    }

    public Task UpdateByIdAsync(int id, Action<Book> replacement)
    {
        throw new NotImplementedException();
    }

    public Task UpdateByIdAsync(int id, IEnumerable<Action<Book>> replacements)
    {
        throw new NotImplementedException();
    }

    public Task DeleteByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task ClearAsync(int id)
    {
        throw new NotImplementedException();
    }
}
