using BookShop.Services;
using BookShop.Services.CategoryService;
using BookShop.Services.BookService;
using BookShop.Services.PaginationService;
using BookShop.Domain.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IdentityModel.Tokens.Jwt;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddScoped<ICategoryService, ApiCategoryService>();
builder.Services.AddScoped<IBookService, ApiBookService>();
builder.Services.AddScoped<IPaginationService<Book>, PaginationService<Book>>();

var apiUri = builder.Configuration["UriData:ApiUri"];

builder.Services
    .AddHttpClient<IBookService, ApiBookService>(opt => opt.BaseAddress = new Uri(apiUri));

builder.Services
    .AddHttpClient<ICategoryService, ApiCategoryService>(opt => opt.BaseAddress = new Uri(apiUri));

builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthorization();

//JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
//JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultScheme = "Cookies";
    opt.DefaultChallengeScheme = "oidc";
})
.AddCookie("Cookies")
.AddOpenIdConnect("oidc", options =>
{
    options.Authority = builder.Configuration["InteractiveServiceSettings:AuthorityUrl"];
    options.ClientId = builder.Configuration["InteractiveServiceSettings:ClientId"];
    options.ClientSecret = builder.Configuration["InteractiveServiceSettings:ClientSecret"];

    options.GetClaimsFromUserInfoEndpoint = true;
    options.ResponseType = "code";
    options.ResponseMode = "query";

    //options.Scope.Clear();
    //options.Scope.Add("openid");
    //options.Scope.Add("profile");

    options.SaveTokens = true;
});

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
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages().RequireAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
