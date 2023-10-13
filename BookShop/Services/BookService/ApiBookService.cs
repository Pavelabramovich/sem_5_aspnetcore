using BookShop.Domain.Entities;
using BookShop.Domain.Models;
using BookShop.Controllers;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using BookShop.Api.Controllers;
using BookShop.Services.CategoryService;
using System.Reflection;

namespace BookShop.Services.BookService;


public class ApiBookService : ApiService, IBookService
{
    private readonly ICategoryService _categoryService;

    private readonly ILogger<ApiBookService> _logger;

    private readonly JsonSerializerOptions _serializerOptions;

    public ApiBookService(HttpClient httpClient, ILogger<ApiBookService> logger, LinkGenerator linkGenerator, ICategoryService categoryService) :
        base(httpClient, linkGenerator)
    {
         _serializerOptions = new JsonSerializerOptions()
         {
             PropertyNamingPolicy = JsonNamingPolicy.CamelCase
         };

        _logger = logger;

        _categoryService = categoryService;
    }


    public async Task<ResponseData<PageModel<Book>>> GetProductListAsync(int? categoryId, int? pageNum = null)
    {
        var categoryResponse = await _categoryService.GetAllAsync();

        if (!categoryResponse)
        {
            _logger.LogError($"-----> Ошибка: {categoryResponse.ErrorMessage}");

            return new(errorMessage: $"Ошибка: {categoryResponse.ErrorMessage}");
        }

        if (categoryId is null)
            categoryId = categoryResponse.Data!.FirstOrDefault()!.Id;

        Type controllerType = typeof(BooksController);

        string actionName = "GetBooks";
        object actionArgs = new { categoryId = categoryId, pageNum = pageNum ?? 0 };

        var uri = GetApiControllerUri(controllerType, actionName, actionArgs);


        var response = await _httpClient.GetAsync(uri);

        if (response.IsSuccessStatusCode)
        {
            try
            {
                return await response.Content.ReadFromJsonAsync<ResponseData<PageModel<Book>>>(_serializerOptions);
            }
            catch (JsonException ex)
            {
                _logger.LogError($"-----> Ошибка: {ex.Message}");

                return new(errorMessage : $"Ошибка: {ex.Message}");
            }
        }

        _logger.LogError($"-----> Данные не получены от сервера. Error:{response.StatusCode}");
         
        return new(errorMessage: $"Данные не получены от сервера. Error: {response.StatusCode}");
    }

    public async Task AddAsync(Book book)
    {
        Type controllerType = typeof(BooksController);

        string actionName = "PostBook";
        object actionArgs = new { book = book };

        var uri = GetApiControllerUri(controllerType, actionName, actionArgs);


        var response = await _httpClient.PostAsJsonAsync(uri, book, _serializerOptions);

        if (response.IsSuccessStatusCode)
        {
            return;
        }

        _logger.LogError($"-----> object not created. Error: {response.StatusCode}");

        return;
    }


    public Task ClearAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteByIdAsync(int id)
    {
        Type controllerType = typeof(BooksController);

        string actionName = "DeleteBook";
        object actionArgs = new { id = id };

        var uri = GetApiControllerUri(controllerType, actionName, actionArgs);

        var response = await _httpClient.DeleteAsync(uri);

        if (response.IsSuccessStatusCode)
        {
            return;
        }

        _logger.LogError($"-----> object not created. Error: {response.StatusCode}");

        return;
    }

    public Task<ResponseData<Book?>> FirstOrNullAsync()
    {
        throw new NotImplementedException();
    }

    public Task<ResponseData<Book?>> FirstOrNullAsync(Func<Book, bool> predicate)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseData<Book?>> FirstOrNullAsync(IEnumerable<Func<Book, bool>> predicates)
    {
        throw new NotImplementedException();
    }

    public async Task<ResponseData<List<Book>>> GetAllAsync()
    {
        Type controllerType = typeof(BooksController);

        string actionName = "GetBooks";

        var uri = GetApiControllerUri(controllerType, actionName);


        var response = await _httpClient.GetAsync(uri);

        if (response.IsSuccessStatusCode)
        {
            try
            {
                return await response.Content.ReadFromJsonAsync<ResponseData<List<Book>>>(_serializerOptions);
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

    public async Task<ResponseData<Book>> GetByIdAsync(int id)
    {
        Type controllerType = typeof(BooksController);

        string actionName = "GetBook";
        object actionArgs = new { id = id };

        var uri = GetApiControllerUri(controllerType, actionName, actionArgs);


        var response = await _httpClient.GetAsync(uri);

        if (response.IsSuccessStatusCode)
        {
            try
            {
                return await response.Content.ReadFromJsonAsync<ResponseData<Book>>(_serializerOptions);
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

    public Task<ResponseData<IEnumerable<Book>>> GetWhereAsync(Func<Book, bool> predicate)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseData<IEnumerable<Book>>> GetWhereAsync(IEnumerable<Func<Book, bool>> predicates)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateByIdAsync(int id, Book book)
    {
        Type controllerType = typeof(BooksController);

        string actionName = "PutBook";
        object actionArgs = new { id = id, book = book };

        var uri = GetApiControllerUri(controllerType, actionName, actionArgs);


        var response = await _httpClient.PutAsJsonAsync(uri, book, _serializerOptions);

        if (response.IsSuccessStatusCode)
        {
            return;
        }

        _logger.LogError($"-----> object not created. Error: {response.StatusCode}");

        return;
    }

    public Task UpdateByIdAsync(int id, Action<Book> replacement)
    {
        throw new NotImplementedException();
    }

    public Task UpdateByIdAsync(int id, IEnumerable<Action<Book>> replacements)
    {
        throw new NotImplementedException();
    }    
}
