using Library.Application.Contracts.AuthDtos;
using Library.Domain.Entities;
using Library.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Library.Application.Services;

public class AuthService(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    ICustomerRepository customerRepository,
    JwtTokenService jwtTokenService)
{
    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        var existingUser = await userManager.FindByEmailAsync(dto.Email);
        if (existingUser != null)
            throw new InvalidOperationException("Email already in use.");

        var customer = new Customer
        {
            Name = dto.FullName,
            Address = dto.CustomerDto.Address,
            PhoneNumber = dto.CustomerDto.PhoneNumber,
            RegisterDate = DateOnly.FromDateTime(DateTime.Today)
        };

        await customerRepository.AddAsync(customer);

        var user = new ApplicationUser
        {
            UserName = dto.Email,
            Email = dto.Email,
            CustomerId = customer.Id
        };

        var result = await userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
            throw new InvalidOperationException(
                string.Join("; ", result.Errors.Select(e => e.Description)));

        await userManager.AddToRoleAsync(user, "User");

        var token = await jwtTokenService.GenerateTokenAsync(user);
        return new AuthResponseDto { Token = token };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        var user = await userManager.FindByEmailAsync(dto.Email);
        if (user == null)
            throw new UnauthorizedAccessException();

        var result = await signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
        if (!result.Succeeded)
            throw new UnauthorizedAccessException();

        var token = await jwtTokenService.GenerateTokenAsync(user);
        return new AuthResponseDto { Token = token };
    }
}
