using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using BookShop.Domain.Entities;
using BookShop.Api.Services;
using BookShop.Domain.Models;
using System.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace BookShop.Api.Controllers;


[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;
    private readonly IEntityService<Category> _categoryService;

    private readonly EntityImageService<Book> _imageService;

    private readonly string _imagesPath;
    private readonly string _appUri;

    public BooksController(
        IBookService bookService, 
        IEntityService<Category> categoryService, 
        EntityImageService<Book> imageService,
        IConfiguration configuration
    )
    {
        _bookService = bookService;
        _categoryService = categoryService;
        _imageService = imageService;

        _appUri = configuration.GetSection("AppUrl")?.Value ?? throw new InvalidOperationException("AppUrl don't set.");
    }

    [AllowAnonymous]
    [HttpGet("{categoryId}/{pageNum}")]
    public async Task<ActionResult<ResponseData<List<Book>>>> GetBooksPage(int? categoryId = null, int pageNum = 0, int itemsPerPage = 3)
    {
        var categoryResponse = await _categoryService.GetAllAsync();

        if (!categoryResponse)
            return NotFound(categoryResponse.ErrorMessage);

        categoryId ??= categoryResponse.Data!.FirstOrDefault()!.Id;


        //var booksResponse = await _bookService.GetAllAsync();

        //if (!booksResponse)
        //    return NotFound(booksResponse.ErrorMessage);

        //var books = booksResponse.Data is IQueryable<Book> query
        //    ? query
        //          .Include(book => book.Category)
        //          .Where(book => book.Category != null && book.Category.Id == categoryId)
        //    : booksResponse.Data!
        //          .Where(book => book.Category?.Id == categoryId);

        var booksOnPageResponse = await _bookService.GetPageAsync(itemsPerPage, (int)categoryId, pageNum);

        if (!booksOnPageResponse)
            return NotFound(booksOnPageResponse.ErrorMessage);

        return Ok(booksOnPageResponse);
    }


    [AllowAnonymous]
    [HttpGet]
    [Route("pageNum{pageNum:int}")]
    public async Task<ActionResult<ResponseData<List<Book>>>> GetPage(int pageNum = 0, int itemsPerPage = 3)
    {
        var booksOnPageResponse = await _bookService.GetPageAsync(itemsPerPage, categoryId: null, pageNum);

        if (!booksOnPageResponse)
            return NotFound(booksOnPageResponse.ErrorMessage);

        return Ok(booksOnPageResponse);
    }


    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<Book>> GetBooks()
    {
        var booksResponse = await _bookService.GetAllAsync();

        if (!booksResponse)
        {
            return NotFound(booksResponse.ErrorMessage);
        }
        else
        {
            return Ok(booksResponse);
        }
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult<Book>> GetBook(int id)
    { 
        var response = await _bookService.GetByIdAsync(id);

        if (!response)
        {
            return NotFound(response.ErrorMessage);
        }
        else
        {
            return Ok(response);
        }
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutBook(int id, Book book)
    {
        if (id != book.Id)       
            return BadRequest();

        await _bookService.UpdateAsync(book);

        return NoContent();
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Book>> PostBook(Book book)
    {
        await _bookService.AddAsync(book);

        return CreatedAtAction("GetBook", new { id = book.Id }, book);
    }

    [Authorize]
    [HttpPost("{id}")]
    public async Task<ActionResult<ResponseData<string>>> PostImage(int id, IFormFile formFile)
    {
        var response = await _imageService.SaveImageAsync(id, formFile);

        if (response)
        {
            return Ok(response);
        }
        else
        {
            return NotFound(response);
        }       
    }

    [AllowAnonymous]
    [HttpGet("image/{id}")]
    public async Task<ActionResult<ResponseData<string>>> GetImage(int id)
    {
        var response = await _imageService.GetImageAsync(id);

        if (!response)
        {
            return NotFound(response.ErrorMessage);
        }
        else
        {
            return Ok(response);
        }
    }


    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        await _bookService.DeleteByIdAsync(id);

        return NoContent();
    }
}
