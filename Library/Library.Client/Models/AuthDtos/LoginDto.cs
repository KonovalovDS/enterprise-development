namespace Library.Client.Models.AuthDtos;

/// <summary>
/// Data Transfer Object used for user login.
/// Contains the user's email and password credentials.
/// </summary>
public class LoginDto
{
    /// <summary>
    /// The email address of the user attempting to log in.
    /// </summary>
    public required string Email { get; set; }

    /// <summary>
    /// The password of the user attempting to log in.
    /// </summary>
    public required string Password { get; set; }
}
