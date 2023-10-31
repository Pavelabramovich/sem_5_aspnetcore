namespace BookShop.BlazorWasm.Services;

public interface IHttpFactory
{
    HttpClient Client { get; }
}
