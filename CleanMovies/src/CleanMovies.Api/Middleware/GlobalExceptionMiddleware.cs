using CleanMovies.Api.Contracts.Responses;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CleanMovies.Api.Middleware;

public sealed class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;
    private readonly IHostEnvironment _environment;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger, IHostEnvironment environment)
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
        catch (ValidationException ex)
        {
            await HandleValidationExceptionAsync(context, ex);
        }
        catch (DbUpdateException ex)
        {
            await HandleDbUpdateExceptionAsync(context, ex);
        }
        catch (KeyNotFoundException ex)
        {
            await HandleNotFoundExceptionAsync(context, ex);
        }
        catch (ArgumentException ex)
        {
            await HandleBadRequestExceptionAsync(context, ex);
        }
        catch (UnauthorizedAccessException ex)
        {
            await HandleUnauthorizedExceptionAsync(context, ex);
        }
        catch (Exception ex)
        {
            await HandleUnexpectedExceptionAsync(context, ex);
        }
    }

    private async Task HandleValidationExceptionAsync(HttpContext context, ValidationException ex)
    {
        _logger.LogWarning(ex, "Validation failed for request {Path}", context.Request.Path);

        var errors = ex.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

        var errorResponse = ErrorResponse.Create(
            type: "https://tools.ietf.org/html/rfc9110#section-15.5.1",
            title: "One or more validation errors occurred",
            status: StatusCodes.Status400BadRequest,
            detail: "Please refer to the errors property for additional details",
            instance: context.Request.Path,
            errors: errors);

        await WriteErrorResponseAsync(context, errorResponse);
    }

    private async Task HandleDbUpdateExceptionAsync(HttpContext context, DbUpdateException ex)
    {
        _logger.LogWarning(ex, "Database update conflict for request {Path}", context.Request.Path);

        var errorResponse = ErrorResponse.Create(
            type: "https://tools.ietf.org/html/rfc9110#section-15.5.10",
            title: "Database conflict",
            status: StatusCodes.Status409Conflict,
            detail: _environment.IsDevelopment() ? ex.InnerException?.Message ?? ex.Message : "A database conflict occurred",
            instance: context.Request.Path);

        await WriteErrorResponseAsync(context, errorResponse);
    }

    private async Task HandleNotFoundExceptionAsync(HttpContext context, KeyNotFoundException ex)
    {
        _logger.LogWarning(ex, "Resource not found for request {Path}", context.Request.Path);

        var errorResponse = ErrorResponse.Create(
            type: "https://tools.ietf.org/html/rfc9110#section-15.5.5",
            title: "Resource not found",
            status: StatusCodes.Status404NotFound,
            detail: ex.Message,
            instance: context.Request.Path);

        await WriteErrorResponseAsync(context, errorResponse);
    }

    private async Task HandleBadRequestExceptionAsync(HttpContext context, ArgumentException ex)
    {
        _logger.LogWarning(ex, "Bad request for {Path}", context.Request.Path);

        var errorResponse = ErrorResponse.Create(
            type: "https://tools.ietf.org/html/rfc9110#section-15.5.1",
            title: "Bad request",
            status: StatusCodes.Status400BadRequest,
            detail: ex.Message,
            instance: context.Request.Path);

        await WriteErrorResponseAsync(context, errorResponse);
    }

    private async Task HandleUnauthorizedExceptionAsync(HttpContext context, UnauthorizedAccessException ex)
    {
        _logger.LogWarning(ex, "Unauthorized access attempt for {Path}", context.Request.Path);

        var errorResponse = ErrorResponse.Create(
            type: "https://tools.ietf.org/html/rfc9110#section-15.5.2",
            title: "Unauthorized",
            status: StatusCodes.Status401Unauthorized,
            detail: "Authentication is required to access this resource",
            instance: context.Request.Path);

        await WriteErrorResponseAsync(context, errorResponse);
    }

    private async Task HandleUnexpectedExceptionAsync(HttpContext context, Exception ex)
    {
        _logger.LogError(ex, "An unexpected error occurred while processing request {Path}", context.Request.Path);

        var errorResponse = ErrorResponse.Create(
            type: "https://tools.ietf.org/html/rfc9110#section-15.6.1",
            title: "An unexpected error occurred",
            status: StatusCodes.Status500InternalServerError,
            detail: _environment.IsDevelopment() ? ex.Message : "An unexpected error occurred. Please try again later.",
            instance: context.Request.Path);

        await WriteErrorResponseAsync(context, errorResponse);
    }

    private async Task WriteErrorResponseAsync(HttpContext context, ErrorResponse errorResponse)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = errorResponse.Status;
        await context.Response.WriteAsJsonAsync(errorResponse);
    }
}
