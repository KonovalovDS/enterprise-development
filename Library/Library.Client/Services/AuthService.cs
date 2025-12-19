using Library.Client.Models.AuthDtos;
using Library.Client.Models.CustomerDtos;
using Library.Client.Models.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;

namespace Library.Client.Services;

/// <summary>
/// Client-side service responsible for authentication operations using HTTP API calls.
/// Implements <see cref="IAuthService"/>.
/// Stores and retrieves JWT tokens via <see cref="ITokenStorage"/>.
/// </summary>
/// <param name="httpFactory">The <see cref="IHttpClientFactory"/> used to create named HttpClient instances.</param>
/// <param name="tokenStorage">The <see cref="ITokenStorage"/> used to persist JWT tokens locally.</param>
public class AuthService(
    IHttpClientFactory httpFactory, 
    ITokenStorage tokenStorage) : IAuthService
{
    /// <summary>
    /// The HttpClient instance used to make API requests to the server.
    /// Created using the <see cref="IHttpClientFactory"/> with the named client "ApiClient".
    /// </summary>
    private readonly HttpClient _http = httpFactory.CreateClient("ApiClient");

    /// <summary>
    /// Authenticates a user by sending login credentials to the API.
    /// Stores the returned JWT token if authentication succeeds.
    /// </summary>
    /// <param name="dto">The login data transfer object containing email and password.</param>
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

    /// <summary>
    /// Registers a new user by sending registration data to the API.
    /// Stores the returned JWT token if registration succeeds.
    /// </summary>
    /// <param name="dto">The registration data transfer object containing credentials and customer information.</param>
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

    /// <summary>
    /// Logs out the current user by removing the stored JWT token.
    /// </summary>
    public Task LogoutAsync() => 
        tokenStorage.RemoveTokenAsync();

    /// <summary>
    /// Retrieves the currently stored JWT token, if any.
    /// </summary>
    public Task<string?> GetTokenAsync() => 
        tokenStorage.GetTokenAsync();

    /// <summary>
    /// Retrieves the profile information of the currently authenticated user from the API.
    /// </summary>
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

    /// <summary>
    /// Retrieves the list of roles from the stored JWT token of the currently authenticated user.
    /// </summary>
    public async Task<List<string>> GetRolesAsync()
    {
        var token = await GetTokenAsync();
        if (string.IsNullOrWhiteSpace(token))
            return [];

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        return [.. jwt.Claims
                  .Where(c => c.Type == ClaimTypes.Role)
                  .Select(c => c.Value)];
    }

    /// <summary>
    /// Checks whether the currently authenticated user has a specific role.
    /// </summary>
    /// <param name="role">The role name to check.</param>
    public async Task<bool> IsInRoleAsync(string role)
    {
        var roles = await GetRolesAsync();
        return roles.Contains(role);
    }
}
