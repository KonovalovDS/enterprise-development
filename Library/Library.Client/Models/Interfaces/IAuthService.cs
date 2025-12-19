using Library.Client.Models.AuthDtos;
using Library.Client.Models.CustomerDtos;

namespace Library.Client.Models.Interfaces;

/// <summary>
/// Interface defining authentication-related operations for users.
/// Includes registration, login, logout, token retrieval, profile access, and role checks.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Registers a new user along with associated customer data.
    /// </summary>
    /// <param name="dto">The registration data transfer object containing user credentials and customer information.</param>
    /// <returns>
    /// An <see cref="AuthResponseDto"/> containing the JWT token if registration succeeds, or null if registration fails.
    /// </returns>
    public Task<AuthResponseDto?> RegisterAsync(RegisterDto dto);

    /// <summary>
    /// Authenticates an existing user using email and password.
    /// </summary>
    /// <param name="dto">The login data transfer object containing email and password.</param>
    /// <returns>
    /// An <see cref="AuthResponseDto"/> containing the JWT token if login succeeds, or null if authentication fails.
    /// </returns>
    public Task<AuthResponseDto?> LoginAsync(LoginDto dto);

    /// <summary>
    /// Logs out the currently authenticated user.
    /// </summary>
    public Task LogoutAsync();

    /// <summary>
    /// Retrieves the JWT token of the currently authenticated user, if available.
    /// </summary>
    /// <returns>The JWT token as a string, or null if the user is not authenticated.</returns>
    public Task<string?> GetTokenAsync();

    /// <summary>
    /// Retrieves the profile information of the currently authenticated user.
    /// </summary>
    /// <returns>A <see cref="CustomerGetDto"/> containing the user's profile data, or null if not authenticated.</returns>
    public Task<CustomerGetDto?> GetMyProfileAsync();

    /// <summary>
    /// Retrieves the list of roles assigned to the currently authenticated user.
    /// </summary>
    /// <returns>A list of role names.</returns>
    public Task<List<string>> GetRolesAsync();

    /// <summary>
    /// Checks whether the currently authenticated user is in a specific role.
    /// </summary>
    /// <param name="role">The role name to check.</param>
    /// <returns>True if the user is in the role; otherwise, false.</returns>
    public Task<bool> IsInRoleAsync(string role);
}
