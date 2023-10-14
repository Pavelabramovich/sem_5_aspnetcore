using BookShop.Api.Controllers;
using BookShop.Domain.Entities;
using BookShop.Domain.Models;
using BookShop.Services.BookService;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace BookShop.Services.CategoryService;

public class ApiCategoryService : ApiService, ICategoryService
{
    private readonly ILogger<ApiBookService> _logger;

    private readonly JsonSerializerOptions _serializerOptions;

    public ApiCategoryService(HttpClient httpClient, IConfiguration configuration, ILogger<ApiBookService> logger, LinkGenerator linkGenerator) :
        base(httpClient, linkGenerator)
    {
        _serializerOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        _logger = logger;
    }

    public Task<ResponseData<Category>> AddAsync(Category entity)
    {
        throw new NotImplementedException();
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

    public async Task<ResponseData<Category>> GetByIdAsync(int id)
    {
        Type controllerType = typeof(CategoriesController);

        string actionName = "GetCategory";
        var actionArgsTypes = new Type[] { typeof(int) };
        object actionArgs = new { id = id };

        var uri = GetApiControllerUri(controllerType, actionName, actionArgsTypes, actionArgs);


        var response = await _httpClient.GetAsync(uri);

        if (response.IsSuccessStatusCode)
        {
            try
            {
                return await response.Content.ReadFromJsonAsync<ResponseData<Category>>(_serializerOptions);
            }
            catch (JsonException ex)
            {
                _logger.LogError($"-----> Ошибка: {ex.Message}");

                return new(errorMessage: $"Ошибка: {ex.Message}");
            }
        }

        _logger.LogError($"-----> Данные не получены от сервера. Error:{response.StatusCode}");

        return new(errorMessage: $"Данные не получены от сервера. Error: {response.StatusCode}");
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

    public Task UpdateByIdAsync(int id, Category entity, IFormFile? formFile)
    {
        throw new NotImplementedException();
    }
}
