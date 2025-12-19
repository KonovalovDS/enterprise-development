using Library.Application.Contracts.AuthDtos;
using Library.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Controllers;

/// <summary>
/// Controller responsible for authentication endpoints.
/// Provides user registration and login actions.
/// </summary>
/// <param name="authService">The <see cref="AuthService"/> used to handle authentication logic.</param>
[ApiController]
[Route("api/auth")]
public class AuthController(AuthService authService) : ControllerBase
{
    /// <summary>
    /// Registers a new user and returns a JWT token upon successful registration.
    /// </summary>
    /// <param name="dto">The registration data transfer object containing user credentials and customer information.</param>
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register(RegisterDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            return Ok(await authService.RegisterAsync(dto));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Authenticates an existing user and returns a JWT token upon successful login.
    /// </summary>
    /// <param name="dto">The login data transfer object containing email and password.</param>
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginDto dto)
    {
        try
        {
            return Ok(await authService.LoginAsync(dto));
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized("Invalid Email or Password");
        }
    }
}
