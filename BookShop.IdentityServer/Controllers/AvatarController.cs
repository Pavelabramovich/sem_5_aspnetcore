using BookShop.IdentityServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace BookShop.IdentityServer.Controllers;


[Authorize]
[Route("[controller]")] 
[ApiController]
public class AvatarController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IWebHostEnvironment _environment;
    private readonly string _defaultAvatar;

    public AvatarController(UserManager<ApplicationUser> userManager,
                            IWebHostEnvironment environment)
    {
        _environment = environment;
        _userManager = userManager;
        _defaultAvatar = "avatar_default.jpg";
    }

    public async Task<ActionResult<IFormFile>> GetAvatar()
    {
        var userId = _userManager.GetUserId(User);

        if (userId is null)
            return BadRequest("No such user.");

        var avatarPath = Directory.GetFiles(Path.Combine(_environment.ContentRootPath, "Images"), $"{userId}.*") is ICollection<string> { Count: >= 1 } matches
            ? matches.SingleOrDefault()
            : Directory.GetFiles(Path.Combine(_environment.ContentRootPath, "Images"), _defaultAvatar).Single();


        FileStream fs = new(avatarPath, FileMode.Open);
        string extension = Path.GetExtension(avatarPath);

        var extProvider = new FileExtensionContentTypeProvider();

        return File(fs, extProvider.Mappings[extension]);
    }
}
