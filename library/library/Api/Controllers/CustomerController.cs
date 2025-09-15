using library.Api.DTOs;
using library.Domain.Entities;
using library.Domain.Interfaces;
using library.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace library.Api.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomerController : Controller
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerController(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAllCustomers()
    {
        var customers = await _customerRepository.GetAllAsync();
        return Ok(customers);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetCustomerById(int id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer == null) return NotFound();
        return Ok(customer);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCustomerById(int id)
    {
        var isExists = await _customerRepository.ExistsById(id);
        if (!isExists) return NotFound();
        return NoContent();
    }

    [HttpPost("")]
    public async Task<IActionResult> CreateCustomer([FromBody] CustomerDto dto)
    {
        var customer = new Customer(dto.Name, dto.Address, dto.PhoneNumber);
        await _customerRepository.AddAsync(customer);
        return CreatedAtAction(nameof(GetCustomerById), new { id = customer.Id }, customer);
    }

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
