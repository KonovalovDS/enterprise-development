using Library.Client.Models.AuthDtos;
using Library.Client.Models.CustomerDtos;
using Library.Client.Models.Interfaces;
using System.Net.Http.Json;

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
}
