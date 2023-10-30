using BookShop.Domain.Entities;
using BookShop.Domain.Models;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Net.Http.Json;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json;

namespace BookShop.BlazorWasm.Services;

public class DataService : IDataService
{
    private readonly HttpClient _httpClient;
    private readonly IAccessTokenProvider _tokenProvider;
    private readonly JsonSerializerOptions _serializerOptions;

    public event Action? DataLoaded;

    public DataService(HttpClient httpClient, ILogger<DataService> logger, IAccessTokenProvider tokenProvider)
    {
        _serializerOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        _httpClient = httpClient;
        _tokenProvider = tokenProvider;

        DataLoaded = () => { };
    }


    public async Task<List<Book>> GetBooksAsync()
    {
        var tokenRequest = await _tokenProvider.RequestAccessToken();

        if (tokenRequest.TryGetToken(out var token))
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}Books/");
            var uri = new Uri(urlString.ToString());

            var response = await _httpClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var res = response.Content.ReadFromJsonAsync<ResponseData<List<Book>>>(_serializerOptions).Result;
                     
                    if (res is null)
                    {
                        throw new InvalidOperationException("Unsecces response data");
                    }
                    else
                    {
                        if (res.ErrorMessage is not null)
                            DataLoaded?.Invoke();

                        return res.Data;
                    }
                }
                catch (JsonException ex)
                {
                    throw new JsonException("Incorrect format in api/Books", ex);
                }
   
            }
            else
            {
                throw new InvalidOperationException("Unsecces response status code");
            }
        }
        else
        {
            throw new InvalidOperationException("Can't get token from token request.");
        }
    }

    public bool Success { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public string ErrorMessage { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public int TotalPages { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public int CurrentPage { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }


    public Task GetCategoryListAsync()
    {
        throw new NotImplementedException();
    }

    public Task GetProductByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<PageModel<Book>> GetProductListAsync(string? categoryName, int pageNum = 0)
    {
        var urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}Books/");

        if (categoryName != null)
        {
            urlString.Append($"{categoryName}/");
        }
        else
            urlString.Append("7/");


        urlString.Append($"{pageNum}");

        await Console.Out.WriteLineAsync(urlString);


        var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));

        if (response.IsSuccessStatusCode)
        {
            try
            {

                var r = response.Content.ReadFromJsonAsync<ResponseData<PageModel<Book>>>(_serializerOptions).Result.Data;
                DataLoaded?.Invoke();
                return r;
            }
            catch (JsonException ex)
            {
                throw null;
            }
        }
        else
            throw null;
    }

    public async Task<List<Category>> GetCategoriesAsync()
    {
        var urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}categories/");


        var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));

        if (response.IsSuccessStatusCode)
        {
            try
            {
                return response.Content.ReadFromJsonAsync<ResponseData<List<Category>>>(_serializerOptions).Result.Data ?? throw new NullReferenceException(); ;
            }
            catch (JsonException ex)
            {
                throw new JsonException("Incorrect format in api/Categories", ex);
            }
            catch (NullReferenceException ex)
            {
                throw new InvalidOperationException("Unsecces response data", ex);
            }
        }
        else
        {
            throw new InvalidOperationException("Unsecces response status code");
        }
    }
}
