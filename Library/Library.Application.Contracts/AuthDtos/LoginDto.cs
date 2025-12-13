using System.ComponentModel.DataAnnotations;

namespace Library.Application.Contracts.AuthDtos;

/// <summary>
/// DTO for user login.
/// </summary>
public class LoginDto
{
    /// <summary>
    /// User email.
    /// </summary>
    public required string Email { get; set; }

    /// <summary>
    /// User password.
    /// </summary>
    public required string Password { get; set; }
}