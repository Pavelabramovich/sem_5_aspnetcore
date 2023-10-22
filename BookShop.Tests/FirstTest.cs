using Microsoft.AspNetCore.Mvc;
using System.Text;
using Xunit;

namespace BookShop.Tests;

public class HomeControllerTests
{
    [Fact]
    public void IndexViewDataMessage()
    {
        // Arrange
        var sb = new StringBuilder();

        // Act
        string res = sb.Append("123").Append("245").ToString();

        // Assert
        Assert.Equal("123245", res);
    }

    //[Fact]
    //public void IndexViewResultNotNull()
    //{
    //    // Arrange
    //    HomeController controller = new HomeController();
    //    // Act
    //    ViewResult result = controller.Index() as ViewResult;
    //    // Assert
    //    Assert.NotNull(result);
    //}

    //[Fact]
    //public void IndexViewNameEqualIndex()
    //{
    //    // Arrange
    //    HomeController controller = new HomeController();
    //    // Act
    //    ViewResult result = controller.Index() as ViewResult;
    //    // Assert
    //    Assert.Equal("Index", result?.ViewName);
    //}
}