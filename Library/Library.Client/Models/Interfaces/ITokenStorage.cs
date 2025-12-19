namespace Library.Client.Models.Interfaces;

/// <summary>
/// Interface for managing storage of authentication tokens.
/// Provides methods to set, retrieve, and remove tokens.
/// </summary>
public interface ITokenStorage
{
    /// <summary>
    /// Stores the specified authentication token.
    /// </summary>
    /// <param name="token">The JWT or authentication token to store.</param>
    public Task SetTokenAsync(string token);

    /// <summary>
    /// Retrieves the currently stored authentication token.
    /// </summary>
    /// <returns>The stored token as a string, or null if no token is present.</returns>
    public Task<string?> GetTokenAsync();

    /// <summary>
    /// Removes the currently stored authentication token.
    /// </summary>
    public Task RemoveTokenAsync();
}
