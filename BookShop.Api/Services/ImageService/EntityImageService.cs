using Azure;
using BookShop.Api.Data;
using BookShop.Domain.Entities;
using BookShop.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


using BookShop.Api.Services;

using System.Security.Policy;

namespace BookShop.Api.Services;


public abstract class EntityImageService<T> : IImageService<T> where T : Entity
{
    private readonly IEntityService<T> _entityService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly string _directoryImagesPath;
    private readonly string _hostImagesPath;

    public EntityImageService(
        IEntityService<T> entityService, 
        IHttpContextAccessor httpContextAccessor, 
        IConfiguration config,
        IWebHostEnvironment env
    )
    {
        _entityService = entityService;
        _httpContextAccessor = httpContextAccessor;

        string relativeImagePath = config.GetSection("ImageDirectory")?.Value ?? "Images";

        _directoryImagesPath = Path.Combine(env.WebRootPath, relativeImagePath);
        _hostImagesPath = $"https://{_httpContextAccessor.HttpContext!.Request.Host}/{relativeImagePath}";
    }

    protected abstract string? GetImageField(T entity);
    protected abstract void SetImageField(T entity, string value);

    public async Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile)
    {
        var entityResponse = await _entityService.GetByIdAsync(id);

        if (entityResponse is null)        
            return new(errorMessage: "No item found.");

        var entity = entityResponse.Data!;


        if (_httpContextAccessor.HttpContext is null)
            return new(errorMessage: "HttpContext is null.");

        if (formFile is not null)
        {
            if (GetImageField(entity) is string { Length: > 0 } prevFileName)
            {
                var separators = @"/\".ToArray();

                prevFileName = prevFileName.Split(separators)[^1];
                string oldImagePath = Path.Combine(_directoryImagesPath, prevFileName);

                File.Delete(oldImagePath);
            }

            var ext = Path.GetExtension(formFile.FileName);
            var fileName = Path.ChangeExtension(Path.GetRandomFileName(), ext);

            SetImageField(entity, Path.Combine(_hostImagesPath, fileName));
            await _entityService.UpdateAsync(entity);

            using (Stream fileStream = new FileStream(Path.Combine(_directoryImagesPath, fileName), FileMode.Create))
            {
                await formFile.CopyToAsync(fileStream);
            }

            return new(_hostImagesPath);
        }
        else
        {
            return new(null);
        }  
    }
}


//string fileName = response.Data!;

//string filePath = Path.Combine(_imagesPath, fileName);

//using (Stream fileStream = new FileStream(filePath, FileMode.Create))
//{
//    await formFile.CopyToAsync(fileStream);
//}