using BookShop.Services.BookService;
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

    protected Uri GetApiControllerUri(Type controllerType, string actionName, object? actionArgs = null)
    {
        ArgumentNullException
            .ThrowIfNull(controllerType, nameof(controllerType));

        ArgumentNullException
            .ThrowIfNull(actionName, nameof(actionName));

        string controllerName = controllerType.Name.Replace("Controller", null);

        try
        {
            actionName = controllerType.GetMethod(actionName)?.Name!;

            if (actionName is null)
                throw new ArgumentException("Invalid controller action.");
        }
        catch (AmbiguousMatchException)
        { 
            // This exception is thrown if there are overloaded methods. No exception handling needed.
        }

        var actionPath = actionArgs is null
            ? _linkGenerator.GetPathByAction(actionName, controllerName)
            : _linkGenerator.GetPathByAction(actionName, controllerName, actionArgs);

        if (actionPath is null)
            throw new ArgumentException("Link generator cant create valid link to this args.");

        return new Uri(_httpClient.BaseAddress!, actionPath);
    }
}
