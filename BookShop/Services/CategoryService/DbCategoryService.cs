using BookShop.Domain.Entities;
using BookShop.Domain.Models;
using BookShop.Services.BookService;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace BookShop.Services.CategoryService;

public class DbCategoryService : ICategoryService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<DbBookService> _logger;

    private readonly JsonSerializerOptions _serializerOptions;

    public DbCategoryService(HttpClient httpClient, IConfiguration configuration, ILogger<DbBookService> logger)
    {
        _httpClient = httpClient;

        _serializerOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        _logger = logger;
    }


    public Task ClearAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task DeleteByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseData<Category?>> FirstOrNullAsync()
    {
        throw new NotImplementedException();
    }

    public Task<ResponseData<Category?>> FirstOrNullAsync(Func<Category, bool> predicate)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseData<Category?>> FirstOrNullAsync(IEnumerable<Func<Category, bool>> predicates)
    {
        throw new NotImplementedException();
    }

    public async Task<ResponseData<List<Category>>> GetAllAsync()
    {
        var urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}categories/");


        var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));

        if (response.IsSuccessStatusCode)
        {
            try
            {
                return await response.Content.ReadFromJsonAsync<ResponseData<List<Category>>>(_serializerOptions);
            }
            catch (JsonException ex)
            {
                _logger.LogError($"-----> Ошибка: {ex.Message}");

                return new ResponseData<List<Category>>(data: null, errorMessage: $"Ошибка: {ex.Message}");
            }
        }

        _logger.LogError($"-----> Данные не получены от сервера. Error:{response.StatusCode}");

        return new ResponseData<List<Category>>(data: null, errorMessage: $"Данные не получены от сервера. Error: {response.StatusCode}");
    }

    public Task<ResponseData<Category?>> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseData<IEnumerable<Category>>> GetWhereAsync(Func<Category, bool> predicate)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseData<IEnumerable<Category>>> GetWhereAsync(IEnumerable<Func<Category, bool>> predicates)
    {
        throw new NotImplementedException();
    }

    public Task UpdateByIdAsync(int id, Category entity)
    {
        throw new NotImplementedException();
    }

    public Task UpdateByIdAsync(int id, Action<Category> replacement)
    {
        throw new NotImplementedException();
    }

    public Task UpdateByIdAsync(int id, IEnumerable<Action<Category>> replacements)
    {
        throw new NotImplementedException();
    }
}
