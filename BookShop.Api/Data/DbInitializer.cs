using BookShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Api.Data;

public class DbInitializer
{
    public static async Task SeedData(IServiceCollection services)
    {
        using var provider = services.BuildServiceProvider();

        var context = provider.GetService<BookShopContext>();



        var _categories = new List<Category>
        {
            new Category { Name = "Programming" },
            new Category { Name = "Child" },
        };


        var _books = new List<Book>
        { 
            new Book
            {
                Title = "Schildt",
                Category = _categories.Find(c => c.Name?.Equals("Programming") ?? false),
                Price = 3m,
                ImagePath = "Images/light.png",
            },
            new Book
            {
                Title = "GoF",
                Category = _categories.Find(c => c.Name?.Equals("Programming") ?? false),
                Price = 5m,
                ImagePath = "Images/light.png",
            },
            new Book
            {
                Title = "Schildt",
                Category = _categories.Find(c => c.Name?.Equals("Programming") ?? false),
                Price = 8m,
                ImagePath = "Images/light.png",
            },
            new Book
            {
                Title = "GoF",
                Category = _categories.Find(c => c.Name?.Equals("Programming") ?? false),
                Price = 6m,
                ImagePath = "Images/light.png",
            },
            new Book
            {
                Title = "ABC",
                Category = _categories.Find(c => c.Name?.Equals("Child") ?? false),
                Price = 2m,
                ImagePath = "Images/light.png",
            },
        };

     //   await context.Database.MigrateAsync();
        

        foreach (var category in _categories)   
            await context.Categories.AddAsync(category);



        foreach (var book in _books)
            await context.Books.AddAsync(book);

        await context.SaveChangesAsync();
    }
}
