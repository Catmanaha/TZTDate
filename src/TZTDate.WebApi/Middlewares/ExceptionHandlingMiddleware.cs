using System.Text;

namespace TZTDate.WebApi.Middlewares;

public class ExceptionHandlingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (BadHttpRequestException ex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync($"{ex.Message}\n{ex.InnerException?.Message}");
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(ex.Message));
        }
    }
}
