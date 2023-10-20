using BookShop.Api.Data;
using BookShop.Api.Services;
using BookShop.Api.Services.CategoryService;
using BookShop.Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddRazorPages();

builder.Services.AddDbContext<BookShopContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<IEntityService<Book>, BookService>();
builder.Services.AddScoped<IEntityService<Category>, CategoryService>();

builder.Services.AddScoped<IPaginationService<Book>, PaginationService<Book>>();
builder.Services.AddScoped<EntityImageService<Book>, BookImageService>();

builder.Services
    .AddControllers()
    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);


builder.Services
.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(opt =>
{
    opt.Authority = builder
    .Configuration
    .GetSection("isUri").Value;
    opt.TokenValidationParameters.ValidateAudience = false;
    opt.TokenValidationParameters.ValidTypes =
    new[] { "at+jwt" };
});


builder.Services.AddHttpContextAccessor();

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

var defaultDateCulture = "en-US";
var ci = new CultureInfo(defaultDateCulture);
ci.NumberFormat.NumberDecimalSeparator = ".";
ci.NumberFormat.CurrencyDecimalSeparator = ".";

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(ci),
    SupportedCultures = new List<CultureInfo>
    {
        ci,
    },
    SupportedUICultures = new List<CultureInfo>
    {
        ci,
    }
});


app.Run();
