using Microsoft.Extensions.Options;


namespace BookShop.BlazorWasm.Services;


public class HttpFactory : IHttpFactory
{
    private readonly HttpOptions _options;
    private readonly HttpClient _client;

    public HttpFactory(IOptions<HttpOptions> options)
    {
        _options = options.Value;

        _client = new HttpClient
        {
            BaseAddress = new Uri(_options.BaseUrl)
        };
    }

    public HttpClient Client => _client;
}

