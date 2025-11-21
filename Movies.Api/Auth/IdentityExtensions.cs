namespace Movies.Api.Auth;

public static class IdentityExtensions
{
    private const string UserIdClaimType = "userid";

    public static Guid? GetUserId(this HttpContext context)
    {
        if (context is null)
        {
            return null;
        }

        var userIdClaim = context.User.Claims
            .FirstOrDefault(x => string.Equals(x.Type, UserIdClaimType, StringComparison.OrdinalIgnoreCase));

        if (userIdClaim is null || string.IsNullOrWhiteSpace(userIdClaim.Value))
        {
            return null;
        }

        return Guid.TryParse(userIdClaim.Value, out var parsedId) ? parsedId : null;
    }
}