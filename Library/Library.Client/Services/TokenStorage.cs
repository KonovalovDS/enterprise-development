using Library.Client.Models.Interfaces;
using Microsoft.JSInterop;

namespace Library.Client.Services;

/// <summary>
/// Implementation of <see cref="ITokenStorage"/> using browser localStorage via <see cref="IJSRuntime"/>.
/// Stores, retrieves, and removes JWT tokens in the client's browser.
/// </summary>
/// <param name="js">The <see cref="IJSRuntime"/> used to interact with browser localStorage.</param>
public class TokenStorage(IJSRuntime js) : ITokenStorage
{
    /// <summary>
    /// The key used to store the JWT token in localStorage.
    /// </summary>
    private const string Key = "jwt_token";

    /// <summary>
    /// Stores the specified JWT token in localStorage.
    /// </summary>
    /// <param name="token">The JWT token to store.</param>
    public async Task SetTokenAsync(string token) =>
        await js.InvokeVoidAsync("localStorage.setItem", Key, token);

    /// <summary>
    /// Retrieves the JWT token from localStorage.
    /// </summary>
    /// <returns>The stored JWT token, or null if no token is present.</returns>
    public async Task<string?> GetTokenAsync() =>
        await js.InvokeAsync<string?>("localStorage.getItem", Key);

    /// <summary>
    /// Removes the JWT token from localStorage.
    /// </summary>
    public async Task RemoveTokenAsync() =>
        await js.InvokeVoidAsync("localStorage.removeItem", Key);
}
