using Library.Application.Contracts.AuthDtos;
using Library.Application.Services;
using Library.Domain.Entities;
using Library.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    ICustomerRepository customerRepository,
    JwtTokenService jwtTokenService) : ControllerBase
{
    /// <summary>
    /// Registers a new user and creates related customer.
    /// </summary>
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existingUser = await userManager.FindByEmailAsync(dto.Email);
        if (existingUser != null)
            return BadRequest("Email already in use.");

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
            return BadRequest(result.Errors);

        await userManager.AddToRoleAsync(user, "User");
        var token = await jwtTokenService.GenerateTokenAsync(user);
        return Ok(new AuthResponseDto { Token = token });
    }

    /// <summary>
    /// Authenticates user and returns JWT token.
    /// </summary>
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto dto)
    {
        var user = await userManager.FindByEmailAsync(dto.Email);
        if (user == null)
            return Unauthorized("Invalid Email or Password");

        var result = await signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
        if (!result.Succeeded)
            return Unauthorized("Invalid Email or Password");

        var token = await jwtTokenService.GenerateTokenAsync(user);

        return Ok(new AuthResponseDto { Token = token });
    }
}
