using Library.Client.Models.ClaimDtos;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using System.Security.Claims;

namespace Library.Client.Services;

public class AuthService(
    HttpClient http,
    AuthenticationStateProvider authStateProvider)
{
    private readonly ApiAuthenticationStateProvider _authStateProvider = (ApiAuthenticationStateProvider)authStateProvider;

    public async Task<bool> LoginAsync(string email, string password)
    {
        var response = await http.PostAsJsonAsync(
            "api/auth/login",
            new { email, password });

        if (!response.IsSuccessStatusCode)
            return false;

        var claims = await response.Content
            .ReadFromJsonAsync<List<ClaimDto>>();

        var identity = new ClaimsIdentity(
            claims.Select(c => new Claim(c.Type, c.Value)),
            "apiauth");

        _authStateProvider.SetUser(
            new ClaimsPrincipal(identity));

        return true;
    }

    public void Logout()
    {
        _authStateProvider.SetAnonymous();
    }
}
