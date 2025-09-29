using AutoMapper;
using Microsoft.AspNetCore.Mvc;

using Application.Dtos;
using Domain.Entities;
using Domain.Interfaces;

namespace Api.Controllers;

/// <summary>
/// Endpoints for managing customers.
/// </summary>
/// <param name="customerRepository">Repository for accessing customer data.</param>
/// <param name="mapper">Mapper for dtos and entities.</param>
[ApiController]
[Route("api/customers")]
public class CustomerController(
    ICustomerRepository customerRepository, 
    IMapper mapper
) : ControllerBase
{
    /// <summary>
    /// Returns all customers.
    /// </summary>
    [HttpGet("")]
    public async Task<IActionResult> GetAllCustomers()
    {
        var customers = await customerRepository.GetAllAsync();
        var customersDto = mapper.Map<List<CustomerDto>>(customers);
        return Ok(customersDto);
    }

    /// <summary>
    /// Returns a customer by their unique ID.
    /// </summary>
    /// <param name="id">The ID of the customer to return.</param>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetCustomerById(int id)
    {
        var customer = await customerRepository.GetByIdAsync(id);
        if (customer == null) return NotFound();

        var customerDto = mapper.Map<CustomerDto>(customer);
        return Ok(customerDto);
    }

    /// <summary>
    /// Deletes a customer by their unique ID.
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
    /// Creates a new customer.
    /// </summary>
    /// <param name="newCustomerDto">The data of the customer to create.</param>
    [HttpPost("")]
    public async Task<IActionResult> CreateCustomer([FromBody] CustomerDto newCustomerDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var newCustomer = mapper.Map<Customer>(newCustomerDto);
        await customerRepository.AddAsync(newCustomer);
        return CreatedAtAction(nameof(GetCustomerById), new { id = newCustomer.Id }, newCustomer);
    }

    /// <summary>
    /// Updates an existing customer by their unique ID.
    /// </summary>
    /// <param name="id">The ID of the customer to update.</param>
    /// <param name="updatedCustomerDto">The updated customer data.</param>
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
