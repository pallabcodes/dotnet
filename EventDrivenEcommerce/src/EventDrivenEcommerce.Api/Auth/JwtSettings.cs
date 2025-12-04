namespace EventDrivenEcommerce.Api.Auth;

/// <summary>
/// Configuration settings for JWT authentication.
/// </summary>
public sealed class JwtSettings
{
    public string Issuer { get; set; } = "EventDrivenEcommerce";
    public string Audience { get; set; } = "EventDrivenEcommerce";
    public string Key { get; set; } = "replace-this-with-a-secure-256-bit-key-in-production";
}

