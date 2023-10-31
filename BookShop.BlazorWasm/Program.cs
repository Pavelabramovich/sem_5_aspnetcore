using BookShop.BlazorWasm;
using BookShop.BlazorWasm.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddSingleton<IDataService, DataService>();

var apiUri = builder.Configuration["UriData:ApiUri"];

builder.Services.AddSingleton<IHttpFactory, HttpFactory>();
builder.Services.Configure<HttpOptions>(options => options.BaseUrl = apiUri);

//builder.Services
//    .AddHttpClient<IDataService, DataService>(opt => opt.BaseAddress = new Uri(apiUri));



//builder.Services.AddRazorPages();
//builder.Services.AddHttpContextAccessor();


builder.Services.AddOidcAuthentication(options =>
{
    builder.Configuration.Bind("Local", options.ProviderOptions);
});

await builder.Build().RunAsync();
