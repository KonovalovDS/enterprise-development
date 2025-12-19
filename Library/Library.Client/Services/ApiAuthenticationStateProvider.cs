using Library.Client.Models.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Library.Client.Services;

/// <summary>
/// Provides authentication state based on a stored JWT token.
/// Inherits from <see cref="AuthenticationStateProvider"/> to integrate with Blazor authentication system.
/// </summary>
/// <param name="tokenStorage">The <see cref="ITokenStorage"/> used to retrieve and manage JWT tokens.</param>
public class ApiAuthenticationStateProvider(ITokenStorage tokenStorage) : AuthenticationStateProvider
{
    /// <summary>
    /// Gets the current authentication state based on the stored JWT token.
    /// If no token is found or it is invalid, returns an unauthenticated state.
    /// </summary>
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await tokenStorage.GetTokenAsync();
        if (string.IsNullOrWhiteSpace(token))
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);
        var claims = jwt.Claims;
        var identity = new ClaimsIdentity(claims, "jwt");

        return new AuthenticationState(new ClaimsPrincipal(identity));
    }

    /// <summary>
    /// Notifies the system that a user has been authenticated.
    /// Updates the authentication state for all subscribers.
    /// </summary>
    public void NotifyUserAuthentication() => 
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

    /// <summary>
    /// Notifies the system that a user has logged out.
    /// Updates the authentication state for all subscribers.
    /// </summary>
    public void NotifyUserLogout() => 
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
}
