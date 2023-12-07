using System;
using System.Net;
using System.Threading.Tasks;
using smartcache.API.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError($"An unhandled exception occurred: {ex}");
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        string message = string.Empty;

        if (exception.GetType() == typeof(UnathorizedException))
        {
            message = "Unauthorized";
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }
        else if (exception.GetType() == typeof(InvalidCredentialsException))
        {
            message = "Forbidden"; 
            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
        }
        else
        {
            message = "An error occurred while processing your request.";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        }
          
        var errorMessage = new
        {
            Message = message,
            Detail = exception.Message
        };

        await context.Response.WriteAsJsonAsync(errorMessage);
    }


}
