using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Controllers;

public class IdentityController : Controller
{
    private readonly ISession _session;


    public IdentityController(IHttpContextAccessor httpContextAccessor)
    {
        _session = httpContextAccessor.HttpContext.Session;
    }

    public async Task Login()
    {
        await HttpContext.ChallengeAsync("oidc", new AuthenticationProperties
        {
            RedirectUri = Url.Action("Index", "Home")
        });
    }

    [HttpPost]
    public async Task Logout()
    {
        await HttpContext.SignOutAsync("cookie");
        await HttpContext.SignOutAsync("oidc", new AuthenticationProperties
        {
            RedirectUri = Url.Action("Index", "Home")
        });

        _session.Clear();
    }

}
