using Library.Client.Models.CustomerDtos;

namespace Library.Client.Models.AuthDtos;

public class RegisterDto
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required CustomerEditDto CustomerDto { get; set; }
}
