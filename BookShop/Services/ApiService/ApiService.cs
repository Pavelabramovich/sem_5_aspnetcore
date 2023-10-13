using BookShop.Services.BookService;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;


namespace BookShop.Services;


public abstract class ApiService
{
    protected readonly LinkGenerator _linkGenerator;
    protected readonly HttpClient _httpClient;

    public ApiService(HttpClient httpClient, LinkGenerator linkGenerator) 
    { 
        _httpClient = httpClient;
        _linkGenerator = linkGenerator;
    }

    protected Uri GetApiControllerUri(Type? controllerType, string actionName, IEnumerable<Type>? actionArgsTypes = null, object? actionArgs = null)
    {
        ArgumentNullException
            .ThrowIfNull(controllerType, nameof(controllerType));

        ArgumentNullException
            .ThrowIfNull(actionName, nameof(actionName));

        string controllerName = controllerType.Name.Replace("Controller", null);

        try
        {
            if (actionArgsTypes.IsNullOrEmpty())
            {
                actionName = controllerType.GetMethod(actionName)?.Name!;
            }
            else
            {
                actionName = controllerType.GetMethod(actionName, actionArgsTypes!.ToArray())?.Name!;
            }

            if (actionName is null)
                throw new ArgumentException("Invalid controller action.");
        }
        catch (AmbiguousMatchException e)
        {
            throw new ArgumentException("Invalid controller action.", e);
        }

        var actionPath = actionArgs is null
            ? _linkGenerator.GetPathByAction(actionName, controllerName)
            : _linkGenerator.GetPathByAction(actionName, controllerName, actionArgs);

        if (actionPath is null)
            throw new ArgumentException("Link generator cant create valid link to this args.");

        return new Uri(_httpClient.BaseAddress!, actionPath);
    }
}
