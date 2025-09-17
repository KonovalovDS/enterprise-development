using Microsoft.AspNetCore.Mvc;

using library.Api.DTOs;
using library.Domain.Entities;
using library.Domain.Interfaces;

namespace library.Api.Controllers;

/// <summary>
/// Endpoints for managing customers.
/// </summary>
[ApiController]
[Route("api/customers")]
public class CustomerController : Controller
{
    private readonly ICustomerRepository _customerRepository;

    /// <summary>
    /// Initializes a new instance of <see cref="CustomerController"/>.
    /// </summary>
    public CustomerController(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    /// <summary>Get all customers.</summary>
    [HttpGet("")]
    public async Task<IActionResult> GetAllCustomers()
    {
        var customers = await _customerRepository.GetAllAsync();
        return Ok(customers);
    }

    /// <summary>Get a customer by ID.</summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetCustomerById(int id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer == null) return NotFound();
        return Ok(customer);
    }

    /// <summary>Delete a customer by ID.</summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCustomerById(int id)
    {
        var isExists = await _customerRepository.ExistsById(id);
        if (!isExists) return NotFound();
        return NoContent();
    }

    /// <summary>Create a new customer.</summary>
    [HttpPost("")]
    public async Task<IActionResult> CreateCustomer([FromBody] CustomerDto dto)
    {
        var customer = new Customer(dto.Name, dto.Address, dto.PhoneNumber);
        await _customerRepository.AddAsync(customer);
        return CreatedAtAction(nameof(GetCustomerById), new { id = customer.Id }, customer);
    }

    /// <summary>Update an existing customer by ID.</summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateCustomer(int id, [FromBody] CustomerDto updated)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer == null) return NotFound();
        customer.Update(updated.Name, updated.Address, updated.PhoneNumber);
        await _customerRepository.UpdateAsync(customer);
        return NoContent();
    }
}
