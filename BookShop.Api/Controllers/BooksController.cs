using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookShop.Api.Data;
using BookShop.Domain.Entities;
using BookShop.Api.Services;
using BookShop.Domain.Models;
using NuGet.Protocol;

namespace BookShop.Api.Controllers;


[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly IEntityService<Book> _bookService;
    private readonly IEntityService<Category> _categoryService;

    private readonly IPaginationService<Book> _paginationService;


    public BooksController(IEntityService<Book> bookService, IEntityService<Category> categoryService, IPaginationService<Book> paginationService)
    {
        _bookService = bookService;
        _categoryService = categoryService;
        _paginationService = paginationService;
    }

    [HttpGet("{categoryId}/{pageNum}")]
    public async Task<ActionResult<ResponseData<List<Book>>>> GetBooks(int? categoryId = null, int pageNum = 0, int itemsPerPage = 3)
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
        var bookResponse = await _bookService.GetByIdAsync(id);

        if (!bookResponse)
        {
            return NotFound(bookResponse.ErrorMessage);
        }
        else
        {
            return Ok(bookResponse);
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

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        await _bookService.DeleteByIdAsync(id);

        return NoContent();
    }
}
