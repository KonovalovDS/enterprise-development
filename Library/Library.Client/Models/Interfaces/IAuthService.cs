using Library.Client.Models.AuthDtos;
using Library.Client.Models.CustomerDtos;

namespace Library.Client.Models.Interfaces;

public interface IAuthService
{
    public Task<AuthResponseDto?> RegisterAsync(RegisterDto dto);
    public Task<AuthResponseDto?> LoginAsync(LoginDto dto);
    public Task LogoutAsync();
    public Task<string?> GetTokenAsync();
    public Task<CustomerGetDto?> GetMyProfileAsync();

    public Task<List<string>> GetRolesAsync();
    public Task<bool> IsInRoleAsync(string role);
}
