namespace Api.Middlewares;

public class CultureMiddleware
{
    private readonly RequestDelegate _next;

    public CultureMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var userLangs = context.Request.Headers["Accept-Language"].ToString();
        var lang = userLangs.Split(',').FirstOrDefault();

        //If no language header was provided, then default to english.
        if (string.IsNullOrEmpty(lang))
        {
            lang = "en";
        }

        //You could set the environment culture based on the language.
        Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(lang);
        Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

        //you could save the language preference for later use as well.
        context.Items["Culture"] = lang;
        context.Items["ClientCulture"] = Thread.CurrentThread.CurrentUICulture.Name;


        await _next(context);
    }
}
