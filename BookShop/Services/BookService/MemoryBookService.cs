using BookShop.Domain.Entities;
using BookShop.Domain.Models;
using BookShop.Services.CategoryService;

namespace BookShop.Services.BookService;

public class MemoryBookService : IBookService
{
    private List<Book> _books;
    private List<Category> _categories;

    public MemoryBookService(ICategoryService categoryService)
    {
        _categories = categoryService.GetCategoryListAsync().Result.Data;

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
                Category = _categories.Find(c=>c.Name?.Equals("Programming") ?? false),
                Price = 5m,
                ImagePath = "Images/light.png",
            },
            new Book
            {
                Id = 1,
                Title = "GoF",
                Category = _categories.Find(c=>c.Name?.Equals("Programming") ?? false),
                Price = 5m,
                ImagePath = "Images/light.png",
            },
            new Book
            {
                Id = 2,
                Title = "ABC",
                Category = _categories.Find(c=>c.Name?.Equals("Child") ?? false),
                Price = 2m,
                ImagePath = "Images/light.png",
            },
        };
    }

    public Task<ResponseData<ListModel<Book>>> GetBookListAsync(int categoryId, int pageNo = 1)
    {
        var result = new ResponseData<ListModel<Book>>
        {
            Data = new() { Items = _books.Where(book => book!.Category!.Id == categoryId).ToList() }
        };

        return Task.FromResult(result);
    }


    public Task<ResponseData<Book>> GetBookByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateBookAsync(int id, Book book, IFormFile? formFile)
    {
        throw new NotImplementedException();
    }

    public Task DeleteBookAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseData<Book>> CreateBookAsync(Book book, IFormFile? formFile)
    {
        throw new NotImplementedException();
    }
}
