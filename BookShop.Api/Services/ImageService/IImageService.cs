using BookShop.Domain.Models;


namespace BookShop.Api.Services;


public interface IImageService<T>
{
    public Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile);

    public Task<ResponseData<string>> GetImageAsync(int id);
}
