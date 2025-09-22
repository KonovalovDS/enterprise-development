using Microsoft.AspNetCore.Mvc;

using library.Domain.Entities;
using library.Domain.Interfaces;

namespace library.Api.Controllers;

/// <summary>
/// Endpoints for managing customers.
/// </summary>
/// <param name="customerRepository">Repository for accessing customer data.</param>
[ApiController]
[Route("api/customers")]
public class CustomerController(ICustomerRepository customerRepository) : Controller
{
    /// <summary>
    /// Repository for accessing customer data.
    /// </summary>
    private readonly ICustomerRepository _customerRepository = customerRepository;

    /// <summary>
    /// Get all customers.
    /// </summary>
    [HttpGet("")]
    public async Task<IActionResult> GetAllCustomers()
    {
        var customers = await _customerRepository.GetAllAsync();
        return Ok(customers);
    }

    /// <summary>
    /// Get a customer by ID.
    /// </summary>
    /// <param name="id">The ID of the customer to get.</param>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetCustomerById(int id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer == null) return NotFound();

        return Ok(customer);
    }

    /// <summary>
    /// Delete a customer by ID.
    /// </summary>
    /// <param name="id">The ID of the customer to delete.</param>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCustomerById(int id)
    {
        var isExists = await _customerRepository.ExistsById(id);
        if (!isExists) return NotFound();

        await _customerRepository.DeleteAsync(id);
        return NoContent();
    }

    /// <summary>
    /// Create a new customer.
    /// </summary>
    /// <param name="newCustomer">The customer object to create.</param>
    [HttpPost("")]
    public async Task<IActionResult> CreateCustomer([FromBody] Customer newCustomer)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        await _customerRepository.AddAsync(newCustomer);
        return CreatedAtAction(nameof(GetCustomerById), new { id = newCustomer.Id }, newCustomer);
    }

    /// <summary>
    /// Update an existing customer by ID.
    /// </summary>
    /// <param name="id">The ID of the customer to update.</param>
    /// <param name="updated">The updated customer object.</param>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateCustomer(int id, [FromBody] Customer updated)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer == null) return NotFound();

        updated.Id = customer.Id;
        await _customerRepository.UpdateAsync(updated);
        return NoContent();
    }
}
