using BookShop.Domain.Entities;
using BookShop.Domain.Models;
using Microsoft.Extensions.Primitives;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace BookShop.Services.BookService;

public class DbBookService : IBookService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<DbBookService> _logger;

    private readonly JsonSerializerOptions _serializerOptions;

    private string _pageSize;

    public DbBookService(HttpClient httpClient, IConfiguration configuration, ILogger<DbBookService> logger)
    {
        _httpClient = httpClient;

        _pageSize = configuration.GetSection("ItemsPerPage").Value;

         _serializerOptions = new JsonSerializerOptions()
         {
             PropertyNamingPolicy = JsonNamingPolicy.CamelCase
         };

        _logger = logger;
    }


    public async Task<ResponseData<PageModel<Book>>> GetProductListAsync(int? categoryId, int pageNo = 0)
    {
        var urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}Books/");

        if (categoryId != null)
        {
            urlString.Append($"{categoryId}/");
        }
        else
            urlString.Append("7/");


        urlString.Append($"{pageNo}");

        var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));

        if (response.IsSuccessStatusCode)
        {
            try
            {
                return await response.Content.ReadFromJsonAsync<ResponseData<PageModel<Book>>>(_serializerOptions);
            }
            catch (JsonException ex)
            {
                _logger.LogError($"-----> Ошибка: {ex.Message}");

                return new ResponseData<PageModel<Book>>(data: null, errorMessage : $"Ошибка: {ex.Message}");
            }
        }

        _logger.LogError($"-----> Данные не получены от сервера. Error:{response.StatusCode}");
         
        return new ResponseData<PageModel<Book>>(data: null, errorMessage: $"Данные не получены от сервера. Error: {response.StatusCode}");
    }

    public async Task<ResponseData<Book>> CreateProductAsync(Book product)
    {
        var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + "Dishes");

        var response = await _httpClient.PostAsJsonAsync(uri, product, _serializerOptions);

        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadFromJsonAsync<ResponseData<Book>>(_serializerOptions);

            return data; // dish;
        }

        _logger.LogError($"-----> object not created. Error: {response.StatusCode}");

        return new ResponseData<Book>(data: null, errorMessage: $"Объект не добавлен. Error: {response.StatusCode}");
    }



    public Task ClearAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task DeleteByIdAsync(int id)
    {
        throw new NotImplementedException();
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

    public Task<ResponseData<List<Book>>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<ResponseData<Book?>> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseData<IEnumerable<Book>>> GetWhereAsync(Func<Book, bool> predicate)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseData<IEnumerable<Book>>> GetWhereAsync(IEnumerable<Func<Book, bool>> predicates)
    {
        throw new NotImplementedException();
    }

    public Task UpdateByIdAsync(int id, Book entity)
    {
        throw new NotImplementedException();
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
