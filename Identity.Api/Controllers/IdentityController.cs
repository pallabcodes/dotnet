using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Api.Controllers;

[ApiController]
public class IdentityController : ControllerBase
{
    // The secret key used to sign the JWT token (this should be stored securely in production)
    private const string TokenSecret = "ForTheLoveOfGodStoreAndLoadThisSecurely";

    // The lifetime of the token (8 hours in this case)
    private static readonly TimeSpan TokenLifetime = TimeSpan.FromHours(8);

    [HttpPost("token")]
    public IActionResult GenerateToken([FromBody] TokenGenerationRequest request)
    {
        // Create an instance of the JwtSecurityTokenHandler to create and write the token
        var tokenHandler = new JwtSecurityTokenHandler();

        // Convert the TokenSecret string to a byte array (needed for signing)
        var key = Encoding.UTF8.GetBytes(TokenSecret);

        // Initialize a list of claims to be included in the token
        var claims = new List<Claim>
        {
            // JTI (JWT ID) - A unique identifier for this token
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            // SUB (Subject) - Typically the username or email of the user
            new(JwtRegisteredClaimNames.Sub, request.Email),
            // EMAIL - The email of the user
            new(JwtRegisteredClaimNames.Email, request.Email),
            // Custom claim with the UserId provided in the request
            new("userid", request.UserId.ToString())
        };

        // Iterate over the custom claims provided in the request and add them to the token
        foreach (var claimPair in request.CustomClaims)
        {
            // Parse the claim value to determine the claim type
            var jsonElement = (JsonElement)claimPair.Value;

            var valueType = jsonElement.ValueKind switch
            {
                JsonValueKind.True => ClaimValueTypes.Boolean,
                JsonValueKind.False => ClaimValueTypes.Boolean,
                JsonValueKind.Number => ClaimValueTypes.Double,
                _ => ClaimValueTypes.String
            };

            // Create the claim and add it to the claims list
            var claim = new Claim(claimPair.Key, claimPair.Value.ToString()!, valueType);

            claims.Add(claim);
        }

        // Define the token's properties (expiration, issuer, audience, etc.)
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims), // Set the claims as the subject of the token
            Expires = DateTime.UtcNow.Add(TokenLifetime), // Set token expiration time
            Issuer = "https://id.nickchapsas.com", // The issuer of the token (usually the server or app name)
            Audience = "https://movies.nickchapsas.com", // The intended recipient(s) of the token
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature) // Set the signing credentials
        };

        // Create the token based on the defined descriptor
        var token = tokenHandler.CreateToken(tokenDescriptor);

        // Write the token to a string
        var jwt = tokenHandler.WriteToken(token);

        // Return the token as the response
        return Ok(jwt);
    }
}