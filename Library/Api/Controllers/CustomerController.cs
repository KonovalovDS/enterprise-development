using Microsoft.AspNetCore.Mvc;

using Domain.Entities;
using Domain.Interfaces;
using AutoMapper;
using Application.Dtos;
using static System.Reflection.Metadata.BlobBuilder;

namespace Api.Controllers;

/// <summary>
/// Endpoints for managing customers.
/// </summary>
/// <param name="customerRepository">Repository for accessing customer data.</param>
[ApiController]
[Route("api/customers")]
public class CustomerController(
    ICustomerRepository customerRepository, 
    IMapper mapper) : Controller
{
    /// <summary>
    /// Get all customers.
    /// </summary>
    [HttpGet("")]
    public async Task<IActionResult> GetAllCustomers()
    {
        var customers = await customerRepository.GetAllAsync();
        var customersDto = mapper.Map<List<CustomerDto>>(customers);
        return Ok(customersDto);
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

        var customerDto = mapper.Map<CustomerDto>(customer);
        return Ok(customerDto);
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
    public async Task<IActionResult> CreateCustomer([FromBody] CustomerDto newCustomerDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var newCustomer = mapper.Map<Customer>(newCustomerDto);
        await customerRepository.AddAsync(newCustomer);
        return CreatedAtAction(nameof(GetCustomerById), new { id = newCustomer.Id }, newCustomer);
    }

    /// <summary>
    /// Update an existing customer by ID.
    /// </summary>
    /// <param name="id">The ID of the customer to update.</param>
    /// <param name="updated">The updated customer object.</param>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateCustomer(int id, [FromBody] CustomerDto updatedCustomerDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var customer = await customerRepository.GetByIdAsync(id);
        if (customer == null) return NotFound();

        var updatedCustomer = mapper.Map<Customer>(updatedCustomerDto);
        updatedCustomer.Id = customer.Id;
        await customerRepository.UpdateAsync(updatedCustomer);
        return NoContent();
    }
}
