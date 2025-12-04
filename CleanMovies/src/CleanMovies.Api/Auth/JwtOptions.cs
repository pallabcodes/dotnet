namespace CleanMovies.Api.Auth;

public sealed record JwtOptions
{
    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
    public string Key { get; init; } = string.Empty;

    public bool IsValid(bool isProduction)
    {
        if (string.IsNullOrWhiteSpace(Issuer) || string.IsNullOrWhiteSpace(Audience) || string.IsNullOrWhiteSpace(Key))
        {
            return false;
        }

        if (isProduction && Key == "replace-this-with-a-secure-long-secret-key")
        {
            return false;
        }

        return Key.Length >= 32; // 256-bit minimum for HS256
    }
}
