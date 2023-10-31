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
    private readonly JsonSerializerOptions _serializerOptions;

    public event Action? DataLoaded;

    public DataService(IHttpFactory httpFactory)
    {
        Console.WriteLine("ctor");

        _serializerOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        _httpClient = httpFactory.Client;
    }

    public bool Success { get; private set; }
    public string ErrorMessage { get; private set; }

    public int TotalPages { get; private set; }
    public int CurrentPage { get; private set; }

    public IEnumerable<Book>? Books { get; private set; }
    public IEnumerable<Category>? Categories { get; private set; }


    public async Task GetCategoryListAsync()
    {
        var urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}categories/");

        var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));

        if (response.IsSuccessStatusCode)
        {
            try
            {
                var result = await response.Content.ReadFromJsonAsync<ResponseData<List<Category>>>(_serializerOptions);

                if (result?.Data is not null)
                {
                    Categories = result!.Data!;
                    DataLoaded?.Invoke();
                }
                else
                {
                    ErrorMessage = result!.ErrorMessage!;
                }
            }
            catch (JsonException ex)
            {
                ErrorMessage = ex.Message;
            }
        }
        else
        {
            ErrorMessage = "Unsecces response status code";
        }
    }

    public Task GetBookByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task GetBookListAsync(string? categoryName, int pageNum = 0)
    {
        Console.WriteLine("service");

        var urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}Books/");

       
        await GetCategoryListAsync();

        Category? category;

        if (categoryName is not null && Categories!.SingleOrDefault(c => c.Name == categoryName) is Category single)
        {
            category = single;
        }
        else
        {
            category = Categories!.FirstOrDefault();
        }
            
        
        if (category is null)
            throw new ArgumentException("CategoryName is invalid category name", nameof(categoryName));


        urlString.Append($"{category.Id}/{pageNum}");

        var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));

        if (response.IsSuccessStatusCode)
        {
            try
            {
                var result = await response.Content.ReadFromJsonAsync<ResponseData<PageModel<Book>>>(_serializerOptions);

                if (result?.Data is not null)
                {
                    var pageModel = result!.Data!;
                    (Books, TotalPages, CurrentPage) = pageModel;

                    DataLoaded?.Invoke();
                }
                else
                {
                    ErrorMessage = result!.ErrorMessage!;
                }
            }
            catch (JsonException ex)
            {
                ErrorMessage = ex.Message;
            }
        }
        else
        {
            ErrorMessage = "Unsecces response status code";
        }
    }
}
