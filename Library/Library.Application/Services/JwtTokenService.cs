using Library.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Library.Application.Services;

/// <summary>
/// Service responsible for generating JWT tokens for authenticated users.
/// </summary>
/// <param name="userManager">The <see cref="UserManager{TUser}"/> used to retrieve user roles and information.</param>
public class JwtTokenService(UserManager<ApplicationUser> userManager)
{
    /// <summary>
    /// Generates a JWT token for the specified user, including claims for user ID, email, and roles.
    /// </summary>
    /// <param name="user">The <see cref="ApplicationUser"/> for whom to generate the token.</param>
    /// <returns>A JWT token as a string that can be used for authentication.</returns>
    public async Task<string> GenerateTokenAsync(ApplicationUser user)
    {
        var roles = await userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email ?? "")
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var keyBase64 = Environment.GetEnvironmentVariable("JwtSettingsSecretKey")!;
        var keyBytes = Convert.FromBase64String(keyBase64);
        var key = new SymmetricSecurityKey(keyBytes);
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: Environment.GetEnvironmentVariable("JwtSettingsIssuer"),
            audience: Environment.GetEnvironmentVariable("JwtSettingsAudience"),
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
