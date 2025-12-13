using Library.Application.Contracts.CustomerDtos;
using System.ComponentModel.DataAnnotations;

namespace Library.Application.Contracts.AuthDtos;

/// <summary>
/// DTO for user registration.
/// </summary>
public class RegisterDto
{
    [EmailAddress]
    public required string Email { get; set; } = null!;

    public required string Password { get; set; } = null!;

    public required string FullName { get; set; } = null!;

    public required CustomerEditDto CustomerDto { get; set; }
}
