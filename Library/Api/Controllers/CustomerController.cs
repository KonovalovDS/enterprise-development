using Microsoft.AspNetCore.Mvc;

using Domain.Entities;
using Domain.Interfaces;

namespace Api.Controllers;

/// <summary>
/// Endpoints for managing customers.
/// </summary>
/// <param name="customerRepository">Repository for accessing customer data.</param>
[ApiController]
[Route("api/customers")]
public class CustomerController(ICustomerRepository customerRepository) : Controller
{
    /// <summary>
    /// Get all customers.
    /// </summary>
    [HttpGet("")]
    public async Task<IActionResult> GetAllCustomers()
    {
        var customers = await customerRepository.GetAllAsync();
        return Ok(customers);
    }

    /// <summary>
    /// Get a customer by ID.
    /// </summary>
    /// <param name="id">The ID of the customer to get.</param>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetCustomerById(int id)
    {
        var customer = await customerRepository.GetByIdAsync(id);
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
        var isExists = await customerRepository.ExistsById(id);
        if (!isExists) return NotFound();

        await customerRepository.DeleteAsync(id);
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

        await customerRepository.AddAsync(newCustomer);
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

        var customer = await customerRepository.GetByIdAsync(id);
        if (customer == null) return NotFound();

        updated.Id = customer.Id;
        await customerRepository.UpdateAsync(updated);
        return NoContent();
    }
}
