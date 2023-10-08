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
    private readonly IBookService _bookService;
    private readonly ICategoryService _categoryService;
    private readonly IPaginationService<Book> _paginationService;

    public BooksController(IBookService bookService, ICategoryService categoryService, IPaginationService<Book> paginationService)
    {
        _bookService = bookService;
        _categoryService = categoryService;
        _paginationService = paginationService;
    }


    // GET: api/Books
    [HttpGet("{categoryId}/{pageNum}")]
    public async Task<ActionResult<ResponseData<List<Book>>>> GetBooks(int? categoryId = null, int pageNum = 0, int itemsPerPage = 3)
    {
        //var categoryResponse = await _categoryService.GetAllAsync();

        //if (!categoryResponse)
        //    return NotFound(categoryResponse.ErrorMessage);

        //if (categoryResponse.Data.Count() == 0)
        //    return NotFound("No categories in collection");

        //if (categoryId is null)
        //    categoryId = categoryResponse.Data!.FirstOrDefault()!.Id;

        if (categoryId is null)
            categoryId = 7;

        var productResponse = await _bookService.GetAllAsync();

        if (!productResponse)
            return NotFound(productResponse.ErrorMessage);

        var productsOnPageResponse = await _paginationService.GetPageAsync(itemsPerPage, ((IQueryable<Book>)productResponse.Data).Include(book => book.Category).Where(product => product.Category.Id == categoryId), (int)pageNum);

        if (!productsOnPageResponse)
            return NotFound(productsOnPageResponse.ErrorMessage);

        return Ok(productsOnPageResponse);
    }


    // GET: api/Books/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Book>> GetBook(int id)
    {
        var productResponse = await _bookService.GetByIdAsync(id);

        if (!productResponse)
        {
            return NotFound(productResponse.ErrorMessage);
        }
        else
        {
            return Ok(productResponse);
        }
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


    // PUT: api/Books/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutBook(int id, Book book)
    {
        if (id != book.Id)
        {
            return BadRequest();
        }

        _bookService.UpdateByIdAsync(id, book);

        return NoContent();
    }

    // POST: api/Books
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Book>> PostBook(Book book)
    {
        await _bookService.AddAsync(book);

        return CreatedAtAction("GetBook", new { id = book.Id }, book);
    }

    // DELETE: api/Books/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        //if (_context.Books == null)
        //{
        //    return NotFound();
        //}
        //var book = await _context.Books.FindAsync(id);
        //if (book == null)
        //{
        //    return NotFound();
        //}

        //_context.Books.Remove(book);
        //await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool BookExists(int id)
    {
        return true;
        //return (_context.Books?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
