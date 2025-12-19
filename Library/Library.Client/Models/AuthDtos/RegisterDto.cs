using Library.Client.Models.CustomerDtos;

namespace Library.Client.Models.AuthDtos;

/// <summary>
/// Data Transfer Object used for registering a new user.
/// Contains user credentials and associated customer information.
/// </summary>
public class RegisterDto
{
    /// <summary>
    /// The email address of the user to register.
    /// </summary>
    public required string Email { get; set; }

    /// <summary>
    /// The password for the new user.
    /// </summary>
    public required string Password { get; set; }

    /// <summary>
    /// Customer-related data to be created along with the user.
    /// </summary>
    public required CustomerEditDto CustomerDto { get; set; }
}
