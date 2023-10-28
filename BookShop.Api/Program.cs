using BookShop.Api.Data;
using BookShop.Api.Services;
using BookShop.Api.Services.CategoryService;
using BookShop.Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();


builder.Services.AddDbContext<BookShopContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IEntityService<Category>, CategoryService>();

builder.Services.AddScoped<EntityImageService<Book>, BookImageService>();

builder.Services
    .AddMvc()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.WriteIndented = true;
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });


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


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

//builder.Services.AddHttpContextAccessor();

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

//app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.UseCors("AllowAnyOrigin");

app.Run();
