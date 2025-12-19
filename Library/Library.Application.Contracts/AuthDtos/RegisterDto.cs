using Library.Application.Contracts.CustomerDtos;
using System.ComponentModel.DataAnnotations;

namespace Library.Application.Contracts.AuthDtos;

/// <summary>
/// DTO for user registration.
/// </summary>
public class RegisterDto
{
    /// <summary>
    /// The user's email address.
    /// Must be a valid email format.
    /// </summary>
    [EmailAddress]
    public required string Email { get; set; } = null!;

    /// <summary>
    /// The user's password.
    /// </summary>
    public required string Password { get; set; } = null!;

    /// <summary>
    /// Customer-related data to be created along with the user.
    /// </summary>
    public required CustomerEditDto CustomerDto { get; set; }
}
