using Library.Client.Models.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Library.Client.Services;

public class TokenStorage(IJSRuntime js) : ITokenStorage
{
    private const string Key = "jwt_token";

    public async Task SetTokenAsync(string token) =>
        await js.InvokeVoidAsync("localStorage.setItem", Key, token);

    public async Task<string?> GetTokenAsync() =>
        await js.InvokeAsync<string?>("localStorage.getItem", Key);

    public async Task RemoveTokenAsync() =>
        await js.InvokeVoidAsync("localStorage.removeItem", Key);
}
