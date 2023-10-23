using BookShop.Controllers;
using BookShop.Domain.Entities;
using BookShop.Domain.Models;
using BookShop.IdentityServer.Data.Migrations;
using BookShop.Services.BookService;
using BookShop.Services.CategoryService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Moq;
using NuGet.Protocol.Core.Types;
using System.Net.Http;
using System.Text;
using Xunit;


namespace BookShop.Tests.BookControllerTests;


public class BookControllerTests
{
    [Fact]
    public async void Test_Invalid_Category_Response()
    {
        // Arrange
        var categoryServiceMock = new Mock<ICategoryService>();

        var invalidCategoriesResponse = Task.FromResult(new ResponseData<List<Category>>(data: null, "Error happend"));
        categoryServiceMock.Setup(s => s.GetAllAsync()).Returns(invalidCategoriesResponse);

        var categoryService = categoryServiceMock.Object;

        var controller = new BookController(bookService: null!, categoryService);


        // Act
        IActionResult res = await controller.Index("Programming");


        // Assert
        Assert.IsType<NotFoundObjectResult>(res);
    }

    [Fact]
    public async void Test_Invalid_Book_Response()
    {
        // Arrange
        var categories = new List<Category> { new() { Id = 1, Name = "First" }, new() { Id = 2, Name = "Second" }  };

        
        var categoryServiceMock = new Mock<ICategoryService>();

        var categoriesResponse = Task.FromResult(new ResponseData<List<Category>>(categories));
        categoryServiceMock.Setup(repo => repo.GetAllAsync()).Returns(categoriesResponse);

        var categoryService = categoryServiceMock.Object;


        var bookServiceMock = new Mock<IBookService>();

        var invalidBooksPageResponse = Task.FromResult(new ResponseData<PageModel<Book>>(data: null, "Error happend"));
        bookServiceMock.Setup(s => s.GetProductListAsync(1, 2)).Returns(invalidBooksPageResponse);

        var bookService = bookServiceMock.Object;


        var controller = new BookController(bookService, categoryService);


        // Act
        IActionResult res = await controller.Index("First", 2);


        // Assert
        Assert.IsType<NotFoundObjectResult>(res);
    }
}



public class TestBookControllerViewData
{
    private readonly BookController _controller;

    private readonly List<Category> _categories;


    //SetUp
    public TestBookControllerViewData()
    {
        // Arrange
        _categories = new List<Category> { new() { Id = 1, Name = "First" }, new() { Id = 2, Name = "Second" } };

        var books = new List<Book>
        {
            new() { Id = 1, Title = "Title1", ImagePath = "Path1", Price = 1, CategoryId = 1 },
            new() { Id = 2, Title = "Title2", ImagePath = "Path2", Price = 2, CategoryId = 1 },
            new() { Id = 3, Title = "Title3", ImagePath = "Path3", Price = 3, CategoryId = 2 },
            new() { Id = 4, Title = "Title4", ImagePath = "Path4", Price = 4, CategoryId = 2 },
        };

        var bookPageModel = new PageModel<Book>(books.Take(2), 1, 0);


        var categoryServiceMock = new Mock<ICategoryService>();

        var categoriesResponse = Task.FromResult(new ResponseData<List<Category>>(_categories));
        categoryServiceMock.Setup(repo => repo.GetAllAsync()).Returns(categoriesResponse);

        var categoryService = categoryServiceMock.Object;


        var bookServiceMock = new Mock<IBookService>();

        var booksPageResponse = Task.FromResult(new ResponseData<PageModel<Book>>(bookPageModel));
        bookServiceMock.Setup(s => s.GetProductListAsync(1, 0)).Returns(booksPageResponse);
        var bookService = bookServiceMock.Object;


        var modelState = new ModelStateDictionary();
        var modelMetadataProvider = new EmptyModelMetadataProvider();

        var viewData = new ViewDataDictionary(modelMetadataProvider, modelState);

        var controllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext()
        };

        _controller = new BookController(bookService, categoryService) { ViewData = viewData, ControllerContext = controllerContext };
    }



    [Fact]
    public async void Test_Category_List_Passed_To_ViewData()
    {
        // Act
        await _controller.Index("First", 0);


        // Assert
        Assert.Equal(_categories, _controller.ViewData["Categories"]);
    }

    [Fact]
    public async void Test_CategoryName_Passed_To_ViewData()
    {
        // Act
        IActionResult res = await _controller.Index("First", 0);


        // Assert
        Assert.Equal("First", _controller.ViewData["CategoryName"]);
    }

    [Fact]
    public async void Test_Model_Is_PageModel()
    {
        // Act
        IActionResult res = await _controller.Index("First", 0);


        // Assert
        var viewResult = Assert.IsType<ViewResult>(res);
        Assert.IsAssignableFrom<PageModel<Book>>(viewResult.Model);
    }
}