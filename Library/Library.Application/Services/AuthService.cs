using Library.Application.Contracts.AuthDtos;
using Library.Domain.Entities;
using Library.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Library.Application.Services;

/// <summary>
/// Service responsible for user authentication and registration.
/// Handles user creation, login, role assignment, and JWT token generation.
/// </summary>
/// <param name="userManager">The <see cref="UserManager{TUser}"/> used to manage application users.</param>
/// <param name="signInManager">The <see cref="SignInManager{TUser}"/> used to handle user sign-in operations.</param>
/// <param name="customerRepository">The repository used to manage <see cref="Customer"/> entities.</param>
/// <param name="jwtTokenService">The service responsible for generating JWT tokens for authenticated users.</param>
public class AuthService(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    ICustomerRepository customerRepository,
    JwtTokenService jwtTokenService)
{
    /// <summary>
    /// Registers a new user along with an associated customer record.
    /// Creates the user, assigns the "User" role, and returns a JWT token.
    /// </summary>
    /// <param name="dto">The registration data transfer object containing user credentials and customer information.</param>
    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        var existingUser = await userManager.FindByEmailAsync(dto.Email);
        if (existingUser != null)
            throw new InvalidOperationException("Email already in use.");

        var customer = new Customer
        {
            Name = dto.CustomerDto.Name,
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

    /// <summary>
    /// Logins in an existing user using email and password credentials.
    /// Returns a JWT token if authentication succeeds.
    /// </summary>
    /// <param name="dto">The login data transfer object containing user email and password.</param>
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
