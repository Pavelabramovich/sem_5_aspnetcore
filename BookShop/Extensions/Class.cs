namespace BookShop.Extensions;

public static class RequestExtension
{
    public static bool IsAjaxRequest(this HttpRequest request)
    {
        ArgumentNullException
            .ThrowIfNull(request?.Headers);

        try
        {
            return request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }
        catch (KeyNotFoundException)
        {
            return false;
        }
    }
}


