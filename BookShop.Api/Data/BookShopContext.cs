using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Web;
using Microsoft.EntityFrameworkCore;
using BookShop.Domain.Entities;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Extensions.Logging;

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

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    optionsBuilder
    //        .UseLazyLoadingProxies(); // <-- enable Lazy Loading

    //        //.UseLoggerFactory(LoggerFactory.Create(b => b
    //        //    .AddConsole()
    //        //    .AddFilter(level => level >= LogLevel.Information)))
    //        //.EnableSensitiveDataLogging()
    //        //.EnableDetailedErrors();
    //}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.Category)
                .WithMany(e => e.Books);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasMany(e => e.Books)
                .WithOne(e => e.Category);
        });
    }

    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    modelBuilder.Entity<Category>()
    //        .HasMany(category => category.Books)
    //        .WithOne(book => book.Category)
    //        .HasForeignKey(book => book.CategoryId);
    //}
} 