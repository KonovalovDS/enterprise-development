using Microsoft.AspNetCore.Mvc;

using Domain.Entities;
using Domain.Interfaces;

namespace Api.Controllers;

/// <summary>
/// Endpoints for managing borrow records.
/// </summary>
/// <param name="borrowRecordRepository">Repository for accessing borrow records.</param>
/// <param name="bookRepository">Repository for accessing books.</param>
/// <param name="customerRepository">Repository for accessing customers.</param>
[ApiController]
[Route("api/records")]
public class BorrowRecordController(
    IBorrowRecordRepository borrowRecordRepository,
    IBookRepository bookRepository,
    ICustomerRepository customerRepository) : Controller
{
    /// <summary>
    /// Get all borrow records.
    /// </summary>
    [HttpGet("")]
    public async Task<IActionResult> GetAllRecords()
    {
        var records = await borrowRecordRepository.GetAllAsync();
        return Ok(records);
    }

    /// <summary>
    /// Get a borrow record by ID.
    /// </summary>
    /// <param name="id">The ID of the record to get.</param>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetRecordById(int id)
    {
        var record = await borrowRecordRepository.GetByIdAsync(id);
        if (record == null)
            return NotFound();

        var book = await bookRepository.GetByIdAsync(record.BookId);
        var customer = await customerRepository.GetByIdAsync(record.CustomerId);
        if (book == null || customer == null)
            return Conflict("Book or Customer not found");

        return Ok(record);
    }

    /// <summary>
    /// Delete a borrow record by ID.
    /// summary>
    /// <param name="id">The ID of the record to delete.</param>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteRecordById(int id)
    {
        var isExists = await borrowRecordRepository.ExistsById(id);
        if (!isExists) return NotFound();

        await borrowRecordRepository.DeleteAsync(id);
        return NoContent();
    }

    /// <summary>
    /// Create a new borrow record.
    /// </summary>
    /// <param name="newRecord">The borrow record object to create.</param>
    [HttpPost("")]
    public async Task<IActionResult> CreateRecord([FromBody] BorrowRecord newRecord)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var isBookExists = await bookRepository.ExistsById(newRecord.BookId);
        var isCustomerExists = await customerRepository.ExistsById(newRecord.CustomerId);

        if (!isBookExists || !isCustomerExists) return NotFound();

        await borrowRecordRepository.AddAsync(newRecord);
        return CreatedAtAction(nameof(GetRecordById), new { id = newRecord.Id }, newRecord);
    }

    /// <summary>
    /// Create a new borrow record.
    /// </summary>
    /// <param name="id">The ID of the record to update.</param>
    /// <param name="updated">The updated record object.</param>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateRecord(int id, [FromBody] BorrowRecord updated)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var isBookExists = await bookRepository.ExistsById(updated.BookId);
        var isCustomerExists = await customerRepository.ExistsById(updated.CustomerId);

        if (!isBookExists || !isCustomerExists) return NotFound();

        var record = await borrowRecordRepository.GetByIdAsync(id);
        if (record == null) return NotFound();

        updated.Id = record.Id;
        await borrowRecordRepository.UpdateAsync(updated);
        return NoContent();
    }
}
