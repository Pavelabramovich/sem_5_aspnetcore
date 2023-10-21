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
using Azure.Core;
using System.Net.Http;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;

namespace BookShop.Services.BookService;


public class ApiBookService : ApiService, IBookService
{
    private readonly ICategoryService _categoryService;

    private readonly ILogger<ApiBookService> _logger;

    private readonly JsonSerializerOptions _serializerOptions;

    private readonly HttpContext _httpContext;

    public ApiBookService(HttpClient httpClient, ILogger<ApiBookService> logger, LinkGenerator linkGenerator, ICategoryService categoryService, IHttpContextAccessor httpContextAccessor) :
        base(httpClient, linkGenerator)
    {
        _serializerOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        _logger = logger;

        _categoryService = categoryService;
        _httpContext = httpContextAccessor.HttpContext;
    }


    public async Task<ResponseData<PageModel<Book>>> GetProductListAsync(int? categoryId, int? pageNum = null)
    {
        var token = await _httpContext.GetTokenAsync("access_token");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

        var categoryResponse = await _categoryService.GetAllAsync();

        if (!categoryResponse)
        {
            _logger.LogError($"-----> Ошибка: {categoryResponse.ErrorMessage}");

            return new(errorMessage: $"Ошибка: {categoryResponse.ErrorMessage}");
        }

        if (categoryId is null)
            categoryId = categoryResponse.Data!.FirstOrDefault()!.Id;

        Type controllerType = typeof(BooksController);

        string actionName = "GetBooksPage";
        var actionArgsTypes = new Type[] { typeof(int?), typeof(int), typeof(int) };
        object actionArgs = new { categoryId = categoryId, pageNum = pageNum ?? 0 };

        var uri = GetApiControllerUri(controllerType, actionName, actionArgsTypes, actionArgs);


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



    public async Task<ResponseData<PageModel<Book>>> GetPageAsync(int? pageNum)
    {
        var token = await _httpContext.GetTokenAsync("access_token");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);


        Type controllerType = typeof(BooksController);

        string actionName = "GetPage";
        var actionArgsTypes = new Type[] { typeof(int), typeof(int) };
        object actionArgs = new {  pageNum = pageNum ?? 0, itemsPerPage = 5 };

        var uri = GetApiControllerUri(controllerType, actionName, actionArgsTypes, actionArgs);


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

                return new(errorMessage: $"Ошибка: {ex.Message}");
            }
        }

        _logger.LogError($"-----> Данные не получены от сервера. Error:{response.StatusCode}");

        return new(errorMessage: $"Данные не получены от сервера. Error: {response.StatusCode}");
    }




    public async Task<ResponseData<Book>> AddAsync(Book book)
    {
        var token = await _httpContext.GetTokenAsync("access_token");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

        Type controllerType = typeof(BooksController);

        string actionName = "PostBook";
        var actionArgsTypes = new Type[] { book.GetType() };
        object actionArgs = new { book = book };

        var uri = GetApiControllerUri(controllerType, actionName, actionArgsTypes, actionArgs);


        var response = await _httpClient.PostAsJsonAsync(uri, book, _serializerOptions);

        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadFromJsonAsync<Book>(_serializerOptions);

            return new(data);
        }

        _logger.LogError($"-----> object not created. Error: {response.StatusCode}");

        return new ResponseData<Book>
        {
            ErrorMessage = $"Объект не добавлен. Error: {response.StatusCode}"
        };

    }


    public Task ClearAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteByIdAsync(int id)
    {
        var token = await _httpContext.GetTokenAsync("access_token");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

        Type controllerType = typeof(BooksController);

        string actionName = "DeleteBook";
        var actionArgsTypes = new Type[] { id.GetType() };
        object actionArgs = new { id = id };

        var uri = GetApiControllerUri(controllerType, actionName, actionArgsTypes, actionArgs);

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
        var token = await _httpContext.GetTokenAsync("access_token");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

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
        var token = await _httpContext.GetTokenAsync("access_token");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

        Type controllerType = typeof(BooksController);

        string actionName = "GetBook";
        var actionArgsTypes = new Type[] { typeof(int) };
        object actionArgs = new { id = id };

        var uri = GetApiControllerUri(controllerType, actionName, actionArgsTypes, actionArgs);


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
        var token = await _httpContext.GetTokenAsync("access_token");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

        Type controllerType = typeof(BooksController);

        string actionName = "PutBook";
        var actionArgsTypes = new Type[] { id.GetType(), typeof(Book) };
        object actionArgs = new { id = id, book = book };

        var uri = GetApiControllerUri(controllerType, actionName, actionArgsTypes, actionArgs);


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

    private async Task SaveImageAsync(int id, IFormFile image)
    {
        var token = await _httpContext.GetTokenAsync("access_token");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

        Type controllerType = typeof(BooksController);

        string actionName = "PostImage";
        var actionArgsTypes = new Type[] { typeof(int), typeof(IFormFile) };
        object actionArgs = new { id = id, formFile = image };

        var uri = GetApiControllerUri(controllerType, actionName, actionArgsTypes, actionArgs);

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = uri    
        };

        var content = new MultipartFormDataContent();
        var streamContent = new StreamContent(image.OpenReadStream());
        content.Add(streamContent, "formFile", image.FileName);

        request.Content = content;
        await _httpClient.SendAsync(request);
    }

    public async Task UpdateByIdAsync(int id, Book entity, IFormFile? image)
    {
        var token = await _httpContext.GetTokenAsync("access_token");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

        await UpdateByIdAsync(id, entity);

        if (image is not null)
            await SaveImageAsync(id, image);
    }

    public async Task<ResponseData<IFormFile>> GetImageAsync(int id)
    {
        var token = await _httpContext.GetTokenAsync("access_token");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

        Type controllerType = typeof(BooksController);

        string actionName = "GetImage";
        var actionArgsTypes = new Type[] { typeof(int) };
        object actionArgs = new { id = id };

        var uri = GetApiControllerUri(controllerType, actionName, actionArgsTypes, actionArgs);


        var response = await _httpClient.GetAsync(uri);

        if (response.IsSuccessStatusCode)
        {
            try
            {
                var imagepathResponce = await response.Content.ReadFromJsonAsync<ResponseData<string>>(_serializerOptions);

                if (imagepathResponce)
                {
                    using (var stream = File.OpenRead(imagepathResponce.Data))
                    {
                        return new(new FormFile(stream, 0, stream.Length, "lololol", Path.GetFileName(stream.Name)) { Headers = new HeaderDictionary() });
                    }
                }
                else
                {
                    return new(errorMessage: "No image in entity.");
                }
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

    public async Task<ResponseData<Book>> AddAsync(Book entity, IFormFile? formFile)
    {
        var token = await _httpContext.GetTokenAsync("access_token");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

        var bookResponse = await AddAsync(entity);

        if (!bookResponse)
        {
            _logger.LogError($"-----> Ошибка: {bookResponse.ErrorMessage}");

            return new(errorMessage: $"Ошибка: {bookResponse.ErrorMessage}");
        }

        var book = bookResponse.Data!;

        if (formFile is not null)
            await SaveImageAsync(book.Id, formFile);

        return bookResponse;
    }
}
