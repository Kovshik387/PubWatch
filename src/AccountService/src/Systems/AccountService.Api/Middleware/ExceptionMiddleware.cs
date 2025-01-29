namespace AccountService.Api.Middleware;

public class ExceptionMiddleware 
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await context.Response.WriteAsJsonAsync(new { Error = ex.Message });
        }
    }
}