using Serilog;

using BookShop.Services;
using BookShop.Services.CategoryService;
using BookShop.Services.BookService;
using BookShop.Services.PaginationService;
using BookShop.Domain.Entities;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddScoped<ICategoryService, ApiCategoryService>();
builder.Services.AddScoped<IBookService, ApiBookService>();
builder.Services.AddScoped<IPaginationService<Book>, PaginationService<Book>>();

builder.Services.AddScoped<Cart, SessionCart>();

var apiUri = builder.Configuration["UriData:ApiUri"];

builder.Services
    .AddHttpClient<IBookService, ApiBookService>(opt => opt.BaseAddress = new Uri(apiUri));

builder.Services
    .AddHttpClient<ICategoryService, ApiCategoryService>(opt => opt.BaseAddress = new Uri(apiUri));

builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthorization();


builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();


builder.Services.AddAuthentication(opt =>
{
    opt.DefaultScheme = "cookie";
    opt.DefaultChallengeScheme = "oidc";
})
.AddCookie("cookie")
.AddOpenIdConnect("oidc", options =>
{
    options.Authority = builder.Configuration["InteractiveServiceSettings:AuthorityUrl"];
    options.ClientId = builder.Configuration["InteractiveServiceSettings:ClientId"];
    options.ClientSecret = builder.Configuration["InteractiveServiceSettings:ClientSecret"];

    options.GetClaimsFromUserInfoEndpoint = true;
    options.ResponseType = "code";
    options.ResponseMode = "query";

    options.SaveTokens = true;
});


const string OUTPUT_TEMPALTE = "{Timestamp:HH:mm:ss} [{Level}] ---> {Message}{NewLine}{Exception}";


var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console(outputTemplate: OUTPUT_TEMPALTE)               
    .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day, outputTemplate: OUTPUT_TEMPALTE)
    .MinimumLevel.Information()
    .Filter.ByExcluding(logEvent => logEvent.Properties.GetValueOrDefault("StatusCode")?.ToString() is ['2',_,_])
    .CreateLogger();


builder.Host.UseSerilog(logger);

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

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



app.UseSession();

app.UseSerilogRequestLogging();

app.Run();
