namespace Library.Application.Contracts.AuthDtos;

/// <summary>
/// Represents an authentication response containing the JWT token.
/// </summary>
public class AuthResponseDto
{
    /// <summary>
    /// The JWT token issued after successful authentication.
    /// </summary>
    public required string Token { get; set; }
}
