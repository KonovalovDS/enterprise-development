using Microsoft.AspNetCore.Mvc;

using library.Api.DTOs;
using library.Domain.Entities;
using library.Domain.Interfaces;

namespace library.Api.Controllers;

/// <summary>
/// Endpoints for managing borrow records.
/// </summary>
[ApiController]
[Route("records")]
public class BorrowRecordController : Controller
{
    private readonly IBorrowRecordRepository _borrowRecordRepository;
    private readonly IBookRepository _bookRepository;
    private readonly ICustomerRepository _customerRepository;

    /// <summary>
    /// Initializes a new instance of <see cref="BorrowRecordController"/>.
    /// </summary>
    public BorrowRecordController(
        IBorrowRecordRepository borrowRecordRepository, 
        IBookRepository bookRepository, 
        ICustomerRepository customerRepository)
    {
        _borrowRecordRepository = borrowRecordRepository;
        _bookRepository = bookRepository;
        _customerRepository = customerRepository;
    }

    /// <summary>Get all borrow records.</summary>
    [HttpGet("")]
    public async Task<IActionResult> GetAllRecords()
    {
        var records = await _borrowRecordRepository.GetAllAsync();
        return Ok(records);
    }

    /// <summary>Get a borrow record by ID.</summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetRecordById(int id)
    {
        var record = await _borrowRecordRepository.GetByIdAsync(id);
        if (record == null)
            return NotFound();

        var book = await _bookRepository.GetByIdAsync(record.BookId);
        var customer = await _customerRepository.GetByIdAsync(record.CustomerId);
        if (book == null || customer == null)
            return Conflict("Book or Customer not found");

        var recordDto = new BorrowRecordDto { 
            BookId = record.BookId, 
            CustomerId = record.CustomerId,
            BorrowDate = record.BorrowDate,
            BorrowDuration = record.BorrowDuration,
            BookName = book.Name!, 
            CustomerName = customer.Name!
        };

        return Ok(record);
    }

    /// <summary>Delete a borrow record by ID.</summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteRecordById(int id)
    {
        var isExists = await _borrowRecordRepository.ExistsById(id);
        if (!isExists)
            return NotFound();
        await _borrowRecordRepository.DeleteAsync(id);
        return NoContent();
    }

    /// <summary>Create a new borrow record.</summary>
    [HttpPost("")]
    public async Task<IActionResult> CreateRecord([FromBody] BorrowRecordDto dto)
    {
        var isBookExists = await _bookRepository.ExistsById(dto.BookId);
        var isCustomerExists = await _customerRepository.ExistsById(dto.CustomerId);

        if (!isBookExists || !isCustomerExists)
            return NotFound();

        var record = new BorrowRecord(dto.BookId, dto.CustomerId, dto.BorrowDuration);
        await _borrowRecordRepository.AddAsync(record);

        return CreatedAtAction(nameof(GetRecordById), new { id = record.Id }, record);
    }

    /// <summary>Create a new borrow record.</summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateRecord(int id, [FromBody] BorrowRecordDto updated)
    {
        var isBookExists = await _bookRepository.ExistsById(updated.BookId);
        var isCustomerExists = await _customerRepository.ExistsById(updated.CustomerId);

        if (!isBookExists || !isCustomerExists)
            return NotFound();

        var record = await _borrowRecordRepository.GetByIdAsync(id);
        if (record == null)
            return NotFound();

        record.Update(updated.BookId, updated.CustomerId, updated.BorrowDuration, updated.BorrowDate);
        await _borrowRecordRepository.UpdateAsync(record);

        return NoContent();
    }
}
