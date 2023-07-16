using System.Net;
using System.Text.Json;
using UniApiProject.Exeptions;

namespace UniApiProject.Middlewares;

public sealed class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        this._next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleException(context, ex);
        }
    }

    private async Task HandleException(HttpContext context, Exception exception)
    {
        var statusCode = HttpStatusCode.InternalServerError;

        switch (exception)
        {
            case AuthException authException:
                statusCode = HttpStatusCode.Unauthorized; break;
            case NotFoundException notFoundException:
                statusCode = HttpStatusCode.NotFound; break;
            case RegisterException registerException:
                statusCode = HttpStatusCode.BadRequest; break;
            case TokenException tokenException:
                statusCode = HttpStatusCode.Unauthorized; break;
        }

        var response = JsonSerializer.Serialize(new
        {
            Success = false,
            StatusCode = statusCode,
            Errors = GetExceptions(exception).Select(e => e.Message)
        });

        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsync(response);
    }

    private IEnumerable<Exception> GetExceptions(Exception ex)
    {
        yield return ex;
        while (ex.InnerException is not null)
        {
            yield return ex.InnerException;
            ex = ex.InnerException;
        }
    }
}