using Microsoft.Extensions.Options;

namespace Movies.Api.Middleware;

public class SecurityHeadersMiddleware
{
    private readonly RequestDelegate _next;
    private readonly SecurityHeadersOptions _options;
    private readonly ILogger<SecurityHeadersMiddleware> _logger;

    public SecurityHeadersMiddleware(
        RequestDelegate next,
        IOptions<SecurityHeadersOptions> options,
        ILogger<SecurityHeadersMiddleware> logger)
    {
        _next = next;
        _options = options.Value;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var response = context.Response;

        if (_options.EnableStrictTransportSecurity)
        {
            response.Headers.Append("Strict-Transport-Security", 
                $"max-age={_options.StrictTransportSecurityMaxAge}; includeSubDomains");
        }

        if (_options.EnableContentSecurityPolicy)
        {
            response.Headers.Append("Content-Security-Policy", _options.ContentSecurityPolicy);
        }

        if (_options.EnableXContentTypeOptions)
        {
            response.Headers.Append("X-Content-Type-Options", "nosniff");
        }

        if (_options.EnableXFrameOptions)
        {
            response.Headers.Append("X-Frame-Options", _options.XFrameOptions);
        }

        if (_options.EnableXssProtection)
        {
            response.Headers.Append("X-XSS-Protection", "1; mode=block");
        }

        if (_options.EnableReferrerPolicy)
        {
            response.Headers.Append("Referrer-Policy", _options.ReferrerPolicy);
        }

        if (_options.EnablePermissionsPolicy)
        {
            response.Headers.Append("Permissions-Policy", _options.PermissionsPolicy);
        }

        response.Headers.Remove("X-Powered-By");
        response.Headers.Remove("Server");

        await _next(context);
    }
}

public class SecurityHeadersOptions
{
    public bool EnableStrictTransportSecurity { get; set; } = true;
    public int StrictTransportSecurityMaxAge { get; set; } = 31536000;

    public bool EnableContentSecurityPolicy { get; set; } = true;
    public string ContentSecurityPolicy { get; set; } = 
        "default-src 'self'; script-src 'self' 'unsafe-inline' 'unsafe-eval'; style-src 'self' 'unsafe-inline'; img-src 'self' data: https:; font-src 'self' data:; connect-src 'self'; frame-ancestors 'none';";

    public bool EnableXContentTypeOptions { get; set; } = true;

    public bool EnableXFrameOptions { get; set; } = true;
    public string XFrameOptions { get; set; } = "DENY";

    public bool EnableXssProtection { get; set; } = true;

    public bool EnableReferrerPolicy { get; set; } = true;
    public string ReferrerPolicy { get; set; } = "strict-origin-when-cross-origin";

    public bool EnablePermissionsPolicy { get; set; } = true;
    public string PermissionsPolicy { get; set; } = 
        "geolocation=(), microphone=(), camera=(), payment=(), usb=(), magnetometer=(), gyroscope=(), speaker=()";
}

