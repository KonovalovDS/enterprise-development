using AutoMapper;
using Library.Application.Contracts.CustomerDtos;
using Library.Domain.Entities;
using Library.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Library.Api.Controllers;

/// <summary>
/// Endpoints for managing customers.
/// </summary>
/// <param name="customerRepository">Repository for accessing customer data.</param>
/// <param name="mapper">Mapper for dtos and entities.</param>
/// <param name="userManager">User role manager that provides allowed methods.</param>
[ApiController]
[Authorize]
[Route("api/customers")]
public class CustomerController(
    ICustomerRepository customerRepository,
    IMapper mapper,
    UserManager<ApplicationUser> userManager) : ControllerBase
{
    /// <summary>
    /// Returns all customers.
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<ActionResult<List<CustomerGetDto>>> GetAllCustomers()
    {
        var customers = await customerRepository.GetAllAsync();
        var customersDto = mapper.Map<List<CustomerGetDto>>(customers);
        return Ok(customersDto);
    }

    /// <summary>
    /// Returns a customer by their unique ID.
    /// </summary>
    /// <param name="id">The ID of the customer to return.</param>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<CustomerGetDto>> GetCustomerById(int id)
    {
        var customer = await customerRepository.GetByIdAsync(id);
        if (customer == null) return NotFound();

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var isAdmin = User.IsInRole("Admin");
        var user = await userManager.FindByIdAsync(userId!);

        if (!isAdmin && user?.CustomerId != id)
            return NotFound();

        var customerDto = mapper.Map<CustomerGetDto>(customer);
        return Ok(customerDto);
    }

    /// <summary>
    /// Deletes a customer by their unique ID.
    /// </summary>
    /// <param name="id">The ID of the customer to delete.</param>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]  
    [Authorize]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteCustomerById(int id)
    {
        var isExists = await customerRepository.ExistsById(id);
        if (!isExists) 
            return NoContent();

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var isAdmin = User.IsInRole("Admin");
        var user = await userManager.FindByIdAsync(userId!);

        if (!isAdmin && user?.CustomerId != id)
            return NoContent();

        await customerRepository.DeleteAsync(id);
        return NoContent();
    }

    /// <summary>
    /// Creates a new customer.
    /// </summary>
    /// <param name="newCustomerDto">The data of the customer to create.</param>
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult<CustomerGetDto>> CreateCustomer([FromBody] CustomerEditDto newCustomerDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var newCustomer = mapper.Map<Customer>(newCustomerDto);
        await customerRepository.AddAsync(newCustomer);

        var resultDto = mapper.Map<CustomerGetDto>(newCustomer);
        return CreatedAtAction(nameof(GetCustomerById), new { id = newCustomer.Id }, resultDto);
    }

    /// <summary>
    /// Updates an existing customer by their unique ID.
    /// </summary>
    /// <param name="id">The ID of the customer to update.</param>
    /// <param name="updatedCustomerDto">The updated customer data.</param>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize]
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateCustomer(int id, [FromBody] CustomerEditDto updatedCustomerDto)
    {
        if (!ModelState.IsValid) 
            return BadRequest(ModelState);

        var customer = await customerRepository.GetByIdAsync(id);
        if (customer == null) 
            return NotFound();

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var isAdmin = User.IsInRole("Admin");
        var user = await userManager.FindByIdAsync(userId!);

        if (!isAdmin && user?.CustomerId != id) 
            return NotFound();

        var updatedCustomer = mapper.Map<Customer>(updatedCustomerDto);
        updatedCustomer.Id = customer.Id;
        await customerRepository.UpdateAsync(updatedCustomer);
        return NoContent();
    }

    /// <summary>
    /// Returns the profile of the currently authenticated user.
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize]
    [HttpGet("profile")]
    public async Task<ActionResult<CustomerGetDto>> GetMyProfile()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized();

        var user = await userManager.FindByIdAsync(userId);
        if (user?.CustomerId == null)
            return NotFound();

        var customer = await customerRepository.GetByIdAsync(user.CustomerId.Value);
        if (customer == null)
            return NotFound();

        var customerDto = mapper.Map<CustomerGetDto>(customer);
        return Ok(customerDto);
    }
}
