﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using BookShop.Domain.Entities;
using BookShop.Api.Services;
using BookShop.Domain.Models;
using System.Configuration;


namespace BookShop.Api.Controllers;


[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly IEntityService<Book> _bookService;
    private readonly IEntityService<Category> _categoryService;

    private readonly IPaginationService<Book> _paginationService;
    private readonly EntityImageService<Book> _imageService;

    private readonly string _imagesPath;
    private readonly string _appUri;

    public BooksController(
        IEntityService<Book> bookService, 
        IEntityService<Category> categoryService, 
        IPaginationService<Book> paginationService,
        EntityImageService<Book> imageService,
        IWebHostEnvironment env,
        IConfiguration configuration
    )
    {
        _bookService = bookService;
        _categoryService = categoryService;
        _paginationService = paginationService;
        _imageService = imageService;

        string relativeImagePath = configuration.GetSection("ImageDirectory")?.Value ?? "Images";

  //      _imagesPath = Path.Combine(env.ContentRootPath, relativeImagePath);
        _appUri = configuration.GetSection("AppUrl")?.Value ?? throw new InvalidOperationException("AppUrl don't set.");
    }

    [HttpGet("{categoryId}/{pageNum}")]
    public async Task<ActionResult<ResponseData<List<Book>>>> GetBooksPage(int? categoryId = null, int pageNum = 0, int itemsPerPage = 3)
    {
        var categoryResponse = await _categoryService.GetAllAsync();

        if (!categoryResponse)
            return NotFound(categoryResponse.ErrorMessage);

        if (categoryId is null)
            categoryId = categoryResponse.Data!.FirstOrDefault()!.Id;


        var booksResponse = await _bookService.GetAllAsync();

        if (!booksResponse)
            return NotFound(booksResponse.ErrorMessage);

        var books = booksResponse.Data is IQueryable<Book> query
            ? query
                  .Include(book => book.Category)
                  .Where(book => book.Category == null
                                        ? false
                                        : book.Category.Id == categoryId)
            : booksResponse.Data!
                .Where(book => book.Category?.Id == categoryId);

        var booksOnPageResponse = await _paginationService.GetPageAsync(itemsPerPage, books!, pageNum);

        if (!booksOnPageResponse)
            return NotFound(booksOnPageResponse.ErrorMessage);

        return Ok(booksOnPageResponse);
    }

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


    [HttpPut("{id}")]
    public async Task<IActionResult> PutBook(int id, Book book)
    {
        if (id != book.Id)       
            return BadRequest();

        await _bookService.UpdateAsync(book);

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<Book>> PostBook(Book book)
    {
        await _bookService.AddAsync(book);

        return CreatedAtAction("GetBook", new { id = book.Id }, book);
    }

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



    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        await _bookService.DeleteByIdAsync(id);

        return NoContent();
    }
}
