using BookShop.Api.Services;
using BookShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using BookShop.Api.Controllers;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using BookShop.Api.Data;
using static Duende.IdentityServer.Models.IdentityResources;

namespace BookShop.Tests.BookServiceTests;


public class BookServiceTests : IDisposable
{
    private SqliteConnection _connection;


    //SetUp
    public BookServiceTests()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();
    }


    //TearDown
    public void Dispose() => _connection?.Dispose();


    [Fact]
    public async void Test_Correct_PagesCount_And_Count_Of_Books()
    {
        var _contextOptions = new DbContextOptionsBuilder<BookShopContext>()
            .UseSqlite(_connection)
            .Options;

        using var context = new BookShopContext(_contextOptions);

        context.Categories.AddRange(
            new() { Id = 1, Name = "First" }, 
            new() { Id = 2, Name = "Second" }
        );

        var books = new[] {
            new Book() { Title = "Title1", ImagePath = "Path1", Price = 1, CategoryId = 1 },
            new Book() { Title = "Title2", ImagePath = "Path2", Price = 2, CategoryId = 1 },
            new Book() { Title = "Title3", ImagePath = "Path3", Price = 3, CategoryId = 1 },
            new Book() { Title = "Title4", ImagePath = "Path4", Price = 4, CategoryId = 1 },

            new Book() { Title = "Title5", ImagePath = "Path5", Price = 5, CategoryId = 2 },
            new Book() { Title = "Title6", ImagePath = "Path6", Price = 6, CategoryId = 2 },
            new Book() { Title = "Title7", ImagePath = "Path7", Price = 7, CategoryId = 2 },
            new Book() { Title = "Title8", ImagePath = "Path8", Price = 8, CategoryId = 2 }
        };

        context.AddRange(books);


        context.SaveChanges();

        var bookService = new BookService(context);


        //Act
        var response = await bookService.GetPageAsync(3, 1, 0);


        //Asert
        Assert.NotNull(response);
        var res = response.Data;

        Assert.Equal(3, res!.Items.Count());
        Assert.Equal(2, res!.PagesCount);
    }

    [Fact]
    public async void Test_Correct_Books_On_Page()
    {
        var _contextOptions = new DbContextOptionsBuilder<BookShopContext>()
            .UseSqlite(_connection)
            .Options;

        using var context = new BookShopContext(_contextOptions);

        context.Categories.AddRange(
            new() { Id = 1, Name = "First" },
            new() { Id = 2, Name = "Second" }
        );

        var books = new[] {
            new Book() { Title = "Title1", ImagePath = "Path1", Price = 1, CategoryId = 1 },
            new Book() { Title = "Title2", ImagePath = "Path2", Price = 2, CategoryId = 1 },
            new Book() { Title = "Title3", ImagePath = "Path3", Price = 3, CategoryId = 1 },
            new Book() { Title = "Title4", ImagePath = "Path4", Price = 4, CategoryId = 1 },

            new Book() { Title = "Title5", ImagePath = "Path5", Price = 5, CategoryId = 2 },
            new Book() { Title = "Title6", ImagePath = "Path6", Price = 6, CategoryId = 2 },
            new Book() { Title = "Title7", ImagePath = "Path7", Price = 7, CategoryId = 2 },
            new Book() { Title = "Title8", ImagePath = "Path8", Price = 8, CategoryId = 2 }
        };

        context.AddRange(books);


        context.SaveChanges();

        var bookService = new BookService(context);


        //Act
        var response = await bookService.GetPageAsync(3, 1, 0);


        //Asert
        Assert.NotNull(response);
        var res = response.Data;

        Assert.Equal(books.Where(book => book.CategoryId == 1).Take(3), res!.Items);
    }

    [Fact]
    public async void Test_Thrown_On_BooksPerPage_Greather_Than_MaxCount()
    {
        var _contextOptions = new DbContextOptionsBuilder<BookShopContext>()
            .UseSqlite(_connection)
            .Options;

        using var context = new BookShopContext(_contextOptions);

        context.Categories.AddRange(
            new() { Id = 1, Name = "First" },
            new() { Id = 2, Name = "Second" }
        );

        var books = new[] {
            new Book() { Title = "Title1", ImagePath = "Path1", Price = 1, CategoryId = 1 },
            new Book() { Title = "Title2", ImagePath = "Path2", Price = 2, CategoryId = 1 },
            new Book() { Title = "Title3", ImagePath = "Path3", Price = 3, CategoryId = 1 },
            new Book() { Title = "Title4", ImagePath = "Path4", Price = 4, CategoryId = 1 },

            new Book() { Title = "Title5", ImagePath = "Path5", Price = 5, CategoryId = 2 },
            new Book() { Title = "Title6", ImagePath = "Path6", Price = 6, CategoryId = 2 },
            new Book() { Title = "Title7", ImagePath = "Path7", Price = 7, CategoryId = 2 },
            new Book() { Title = "Title8", ImagePath = "Path8", Price = 8, CategoryId = 2 }
        };

        context.AddRange(books);


        context.SaveChanges();

        var bookService = new BookService(context);


        //Act
        var act = () => bookService.GetPageAsync(30, 1, 0).Result;


        //Assert
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public async void Test_Thrown_On_PageNum_Greather_Than_PagesCount()
    {
        var _contextOptions = new DbContextOptionsBuilder<BookShopContext>()
            .UseSqlite(_connection)
            .Options;

        using var context = new BookShopContext(_contextOptions);

        context.Categories.AddRange(
            new() { Id = 1, Name = "First" },
            new() { Id = 2, Name = "Second" }
        );

        var books = new[] {
            new Book() { Title = "Title1", ImagePath = "Path1", Price = 1, CategoryId = 1 },
            new Book() { Title = "Title2", ImagePath = "Path2", Price = 2, CategoryId = 1 },
            new Book() { Title = "Title3", ImagePath = "Path3", Price = 3, CategoryId = 1 },
            new Book() { Title = "Title4", ImagePath = "Path4", Price = 4, CategoryId = 1 },

            new Book() { Title = "Title5", ImagePath = "Path5", Price = 5, CategoryId = 2 },
            new Book() { Title = "Title6", ImagePath = "Path6", Price = 6, CategoryId = 2 },
            new Book() { Title = "Title7", ImagePath = "Path7", Price = 7, CategoryId = 2 },
            new Book() { Title = "Title8", ImagePath = "Path8", Price = 8, CategoryId = 2 }
        };

        context.AddRange(books);


        context.SaveChanges();

        var bookService = new BookService(context);


        //Act
        var act = () => bookService.GetPageAsync(3, 1, 10).Result;


        //Assert
        Assert.Throws<ArgumentException>(act);
    }
}
