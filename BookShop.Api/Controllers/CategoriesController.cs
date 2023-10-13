using Microsoft.AspNetCore.Mvc;

using BookShop.Api.Services;
using BookShop.Domain.Models;
using BookShop.Domain.Entities;


namespace BookShop.Api.Controllers;


[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private IEntityService<Category> _categoryService;

    public CategoriesController(IEntityService<Category> categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<ActionResult<ResponseData<List<Category>>>> GetCategories()
    {
        var categoryResponse = await _categoryService.GetAllAsync();

        if (!categoryResponse)
            return NotFound(categoryResponse.ErrorMessage);

        return Ok(categoryResponse);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Category>> GetCategory(int id)
    {
        var response = await _categoryService.GetByIdAsync(id);

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
    public async Task<IActionResult> PutCategory(int id, Category category)
    {
        if (id != category.Id)
            return BadRequest();

        await _categoryService.UpdateAsync(category);

        return NoContent();
    }


    [HttpPost]
    public async Task<ActionResult<Category>> PostCategory(Category category)
    {
        await _categoryService.AddAsync(category);

        return CreatedAtAction("GetCategory", new { id = category.Id }, category);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        await _categoryService.DeleteByIdAsync(id);

        return NoContent();
    }
}
