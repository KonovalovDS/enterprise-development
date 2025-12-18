using Library.Client.Models.AuthDtos;
using Library.Client.Models.CustomerDtos;
using Library.Client.Models.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;

namespace Library.Client.Services;

public class AuthService(IHttpClientFactory httpFactory, ITokenStorage tokenStorage) : IAuthService
{
    private readonly HttpClient _http = httpFactory.CreateClient("ApiClient");

    public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
    {
        var resp = await _http.PostAsJsonAsync("api/auth/login", dto);
        if (!resp.IsSuccessStatusCode)
            return null;

        var result = await resp.Content.ReadFromJsonAsync<AuthResponseDto>();
        if (result != null)
            await tokenStorage.SetTokenAsync(result.Token);

        return result;
    }

    public async Task<AuthResponseDto?> RegisterAsync(RegisterDto dto)
    {
        var resp = await _http.PostAsJsonAsync("api/auth/register", dto);
        if (!resp.IsSuccessStatusCode)
            return null;

        var result = await resp.Content.ReadFromJsonAsync<AuthResponseDto>();
        if (result != null)
            await tokenStorage.SetTokenAsync(result.Token);

        return result;
    }

    public Task LogoutAsync() => tokenStorage.RemoveTokenAsync();
    public Task<string?> GetTokenAsync() => tokenStorage.GetTokenAsync();

    public async Task<CustomerGetDto?> GetMyProfileAsync()
    {
        try
        {
            var profile = await _http.GetFromJsonAsync<CustomerGetDto>("api/profile");
            return profile;
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }

    public async Task<List<string>> GetRolesAsync()
    {
        var token = await GetTokenAsync();
        if (string.IsNullOrWhiteSpace(token))
            return [];

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        return jwt.Claims
                  .Where(c => c.Type == ClaimTypes.Role)
                  .Select(c => c.Value)
                  .ToList();
    }

    public async Task<bool> IsInRoleAsync(string role)
    {
        var roles = await GetRolesAsync();
        return roles.Contains(role);
    }
}
