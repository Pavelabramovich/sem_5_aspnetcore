using BookShop.Domain.Entities;
using BookShop.Domain.Models;

namespace BookShop.Services.CategoryService;

public interface ICategoryService
{
    /// <include file='ICategoryService.cs.xml' path='doc/class[@name="ICategoryService"]/method[@name="GetCategoryListAsync"]' />
    public Task<ResponseData<List<Category>>> GetCategoryListAsync();

    /// <include file='ICategoryService.cs.xml' path='doc/class[@name="ICategoryService"]/method[@name="GetCategoryListAsync"]' />
    public Task<ResponseData<Category?>> FirstOrDefaultAsync();
}

