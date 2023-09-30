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
        _categories = categoryService.GetCategoryListAsync().Result.Data;
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
                Id = 1,
                Title = "Schildt",
                Category = _categories.Find(c => c.Name?.Equals("Programming") ?? false),
                Price = 8m,
                ImagePath = "Images/light.png",
            },
            new Book
            {
                Id = 2,
                Title = "GoF",
                Category = _categories.Find(c => c.Name?.Equals("Programming") ?? false),
                Price = 6m,
                ImagePath = "Images/light.png",
            },
            new Book
            {
                Id = 3,
                Title = "ABC",
                Category = _categories.Find(c => c.Name?.Equals("Child") ?? false),
                Price = 2m,
                ImagePath = "Images/light.png",
            },
        };
    }


    public Task<ResponseData<ListModel<Book>>> GetBookListAsync()
    {
        var response = new ResponseData<ListModel<Book>>
        {
            Data = new() { Items = _books }
        };

        return Task.FromResult(response);
    }

    public Task<ResponseData<Book?>> FirstOrDefaultAsync()
    {
        var result = _books.FirstOrDefault();

        var response = new ResponseData<Book?>
        {
            Data = result,
        };

        if (result is null)
        {
            response.Success = false;
            response.ErrorMessage = "No books in collection";
        }


        return Task.FromResult(response);
    }

    public Task<ResponseData<ListModel<Book>>> GetBookListByIdAsync(int categoryId, int pageNo = 1)
    {
        var booksByCategory = _books.Where(book => book!.Category!.Id == categoryId);

        int booksPerPage = _config.GetValue<int>("ItemsPerPage");
        var pageCount = booksByCategory.Count() / booksPerPage + 1;

        var booksOnPage = booksByCategory
            .Skip(booksPerPage * (pageNo - 1))
            .Take(booksPerPage);

        var result = new ResponseData<ListModel<Book>>
        {
            Data = new() { Items = booksOnPage.ToList(), CurrentPage = pageNo, TotalPages = pageCount }
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
