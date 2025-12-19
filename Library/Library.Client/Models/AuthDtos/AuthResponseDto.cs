namespace Library.Client.Models.AuthDtos;

/// <summary>
/// Data Transfer Object representing an authentication response.
/// Contains the JWT token issued after successful login or registration.
/// </summary>
public class AuthResponseDto
{
    /// <summary>
    /// The JWT token issued for the authenticated user.
    /// </summary>
    public required string Token { get; set; }
}
