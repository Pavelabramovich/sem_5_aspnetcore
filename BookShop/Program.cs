using BookShop.Services;
using BookShop.Services.CategoryService;
using BookShop.Services.BookService;
using BookShop.Services.PaginationService;
using BookShop.Domain.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ICategoryService, DbCategoryService>();
builder.Services.AddScoped<IBookService, DbBookService>();
builder.Services.AddScoped<IPaginationService<Book>, PaginationService<Book>>();

var apiUri = builder.Configuration["UriData:ApiUri"];

builder.Services
    .AddHttpClient<IBookService, DbBookService>(opt => opt.BaseAddress = new Uri(apiUri));

builder.Services
    .AddHttpClient<ICategoryService, DbCategoryService>(opt => opt.BaseAddress = new Uri(apiUri));



var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
