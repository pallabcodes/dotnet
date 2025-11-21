using Microsoft.Extensions.Logging;
using Movies.Contracts.Responses;
using ValidationException = FluentValidation.ValidationException;

namespace Movies.Api.Mapping;

public class ValidationMappingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ValidationMappingMiddleware> _logger;

    public ValidationMappingMiddleware(RequestDelegate next, ILogger<ValidationMappingMiddleware> logger)
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
        catch (ValidationException ex)
        {
            _logger.LogWarning(
                "Validation failed for request: {Path}. Errors: {ErrorCount}",
                context.Request.Path,
                ex.Errors.Count);

            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            var validationFailureResponse = new ValidationFailureResponse
            {
                Errors = ex.Errors.Select(x => new ValidationResponse
                {
                    PropertyName = x.PropertyName,
                    Message = x.ErrorMessage
                })
            };

            await context.Response.WriteAsJsonAsync(validationFailureResponse);
        }
    }
}
