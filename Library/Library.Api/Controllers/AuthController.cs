using Library.Application.Contracts.AuthDtos;
using Library.Domain.Entities;
using Library.Domain.Interfaces;
using Library.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    ICustomerRepository customerRepository) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existingUser = await userManager.FindByEmailAsync(dto.Email);
        if (existingUser != null)
            return BadRequest("Email уже зарегистрирован.");

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

        return Ok(new { Message = "Регистрация прошла успешно" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var user = await userManager.FindByEmailAsync(dto.Email);
        if (user == null)
            return Unauthorized("Неверный Email или пароль");

        var result = await signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
        if (!result.Succeeded)
            return Unauthorized("Неверный Email или пароль");

        return Ok(new { Message = "Успешный вход" });
    }
}
