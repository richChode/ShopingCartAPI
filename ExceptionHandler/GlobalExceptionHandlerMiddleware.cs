using System.Net;
using System.Text.Json;
using shoppingcart.Exceptions;

namespace shoppingcart.ExceptionHandler;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {

            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
    {
        HttpStatusCode status;
        var stackTrace = ex.StackTrace;
        string message = ex.Message;
        var exType = ex.GetType();

        if (exType == typeof(AlreadyExistError))
        {
            status = HttpStatusCode.Conflict;
        }
        else if (exType == typeof(QuantityError))
        {
            status = HttpStatusCode.NotFound;
        }
        else
        {
            status = HttpStatusCode.InternalServerError;
        }

        var exResult = JsonSerializer.Serialize(new { error = message, status });
        httpContext.Response.StatusCode = (int)status;
        httpContext.Response.ContentType = "application/json";

        return httpContext.Response.WriteAsync(exResult);
    }

}