using BookShop.Api.Data;
using BookShop.Domain.Entities;
using BookShop.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Api.Services.CategoryService;

public class CategoryService : EntityService<Category>
{
    public CategoryService(BookShopContext context)
        : base(context)
    { }

    protected override DbSet<Category> DbSet => _context.Categories;
}