using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Movies.Api.Auth;

public class AdminAuthRequirement : IAuthorizationHandler, IAuthorizationRequirement
{
    private readonly string _apiKey;

    public AdminAuthRequirement(string apiKey)
    {
        _apiKey = apiKey;
    }

    public Task HandleAsync(AuthorizationHandlerContext context)
    {
        if (context.User.HasClaim(AuthConstants.AdminUserClaimName, "true"))
        {
            context.Succeed(this);
            return Task.CompletedTask;
        }

        var httpContext = context.Resource as HttpContext;
        if (httpContext is null) return Task.CompletedTask;

        if (!httpContext.Request.Headers.TryGetValue(AuthConstants.ApiKeyHeaderName, out var extractedApiKey))
        {
            context.Fail();
            return Task.CompletedTask;
        }

        if (_apiKey != extractedApiKey)
        {
            context.Fail();
            return Task.CompletedTask;
        }

        var identity = (ClaimsIdentity)httpContext.User.Identity!;
        if (identity is not null)
        {
            var userIdClaim = httpContext.User.Claims
                .FirstOrDefault(c => string.Equals(c.Type, "userid", StringComparison.OrdinalIgnoreCase));
            
            if (userIdClaim is null)
            {
                identity.AddClaim(new Claim("userid", Guid.NewGuid().ToString()));
            }
        }
        
        context.Succeed(this);
        return Task.CompletedTask;
    }
}