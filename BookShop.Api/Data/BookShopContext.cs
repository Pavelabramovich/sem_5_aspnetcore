using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Web;
using Microsoft.EntityFrameworkCore;
using BookShop.Domain.Entities;

namespace BookShop.Api.Data;


public class BookShopContext : DbContext
{
    public DbSet<Book> Books { get; private set; }
    public DbSet<Category> Categories { get; private set; }

    public BookShopContext(DbContextOptions<BookShopContext> options) 
        : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>()
            .HasMany(category => category.Books)
            .WithOne(book => book.Category)
            .HasForeignKey(book => book.CategoryId);
    }
} 