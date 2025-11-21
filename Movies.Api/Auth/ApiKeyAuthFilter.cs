using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Movies.Api.Auth;

public class ApiKeyAuthFilter : IAuthorizationFilter
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<ApiKeyAuthFilter> _logger;

    public ApiKeyAuthFilter(IConfiguration configuration, ILogger<ApiKeyAuthFilter> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context is null)
        {
            return;
        }

        if (!context.HttpContext.Request.Headers.TryGetValue(AuthConstants.ApiKeyHeaderName, out var extractedApiKey))
        {
            _logger.LogWarning("API Key missing in request");
            context.Result = new UnauthorizedObjectResult("API Key is missing");
            return;
        }

        var apiKey = _configuration["ApiKey"];
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            _logger.LogError("API Key not configured in application settings");
            context.Result = new UnauthorizedObjectResult("API Key configuration error");
            return;
        }

        if (!string.Equals(apiKey, extractedApiKey, StringComparison.Ordinal))
        {
            _logger.LogWarning("Invalid API Key provided");
            context.Result = new UnauthorizedObjectResult("Invalid API Key");
        }
    }
}
