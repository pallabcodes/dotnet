using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace Movies.Api.Middleware;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;
    private readonly IWebHostEnvironment _environment;

    public GlobalExceptionHandlerMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionHandlerMiddleware> logger,
        IWebHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred. RequestId: {RequestId}", 
                context.TraceIdentifier);
            
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var response = new
        {
            error = "An error occurred while processing your request",
            requestId = context.TraceIdentifier,
            timestamp = DateTime.UtcNow
        };

        if (_environment.IsDevelopment())
        {
            var detailedResponse = new
            {
                error = "An error occurred while processing your request",
                message = exception.Message,
                stackTrace = exception.StackTrace,
                innerException = exception.InnerException?.Message,
                requestId = context.TraceIdentifier,
                timestamp = DateTime.UtcNow
            };

            await context.Response.WriteAsJsonAsync(detailedResponse);
        }
        else
        {
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}

