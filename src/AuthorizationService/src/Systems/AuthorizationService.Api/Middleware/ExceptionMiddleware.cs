using AuthorizationService.Domain.Exceptions;

namespace AuthorizationService.Api.Middleware;

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
        catch (RefreshTokenException ex)
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsJsonAsync(new { Error = ex.Message });
        }
        catch (NotFoundException ex)
        {
            context.Response.StatusCode = 404;
            await context.Response.WriteAsJsonAsync(new { Error = ex.Message });
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(new { Error = ex.Message });
        }
    }
}