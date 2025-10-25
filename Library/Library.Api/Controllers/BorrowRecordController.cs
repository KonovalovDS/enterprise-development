using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Library.Domain.Entities;
using Library.Domain.Interfaces;
using Library.Application.Contracts.BorrowRecordDtos;

namespace Library.Api.Controllers;

/// <summary>
/// Endpoints for managing borrow records.
/// </summary>
/// <param name="borrowRecordRepository">Repository for accessing borrow records.</param>
/// <param name="bookRepository">Repository for accessing books.</param>
/// <param name="customerRepository">Repository for accessing customers.</param>
/// <param name="mapper">Mapper for dtos and entities.</param>
[ApiController]
[Route("api/records")]
public class BorrowRecordController(
    IBorrowRecordRepository borrowRecordRepository,
    IBookRepository bookRepository,
    ICustomerRepository customerRepository,
    IMapper mapper
) : ControllerBase
{
    /// <summary>
    /// Returns all borrow records.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<BorrowRecordGetDto>>> GetAllRecords()
    {
        var records = await borrowRecordRepository.GetAllAsync();
        var recordsDto = mapper.Map<List<BorrowRecordGetDto>>(records);
        return Ok(recordsDto);
    }

    /// <summary>
    /// Returns a borrow record by its unique ID.
    /// </summary>
    /// <param name="id">The ID of the borrow record to return.</param>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<BorrowRecordGetDto>> GetRecordById(int id)
    {
        var record = await borrowRecordRepository.GetByIdAsync(id);
        if (record == null)
            return NotFound();

        var book = await bookRepository.GetByIdAsync(record.BookId);
        var customer = await customerRepository.GetByIdAsync(record.CustomerId);
        if (book == null || customer == null)
            return NotFound("Book or Customer not found");

        var recordDto = mapper.Map<BorrowRecordGetDto>(record);
        return Ok(recordDto);
    }

    /// <summary>
    /// Deletes a borrow record by its unique ID.
    /// </summary>
    /// <param name="id">The ID of the borrow record to delete.</param>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteRecordById(int id)
    {
        var isExists = await borrowRecordRepository.ExistsById(id);
        if (!isExists) return NotFound();

        await borrowRecordRepository.DeleteAsync(id);
        return NoContent();
    }

    /// <summary>
    /// Creates a new borrow record.
    /// </summary>
    /// <param name="newRecordDto">The data for the new borrow record.</param>
    [HttpPost]
    public async Task<ActionResult<BorrowRecordGetDto>> CreateRecord([FromBody] BorrowRecordEditDto newRecordDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var isBookExists = await bookRepository.ExistsById(newRecordDto.BookId);
        var isCustomerExists = await customerRepository.ExistsById(newRecordDto.CustomerId);

        if (!isBookExists || !isCustomerExists) return NotFound();

        var newRecord = mapper.Map<BorrowRecord>(newRecordDto);
        await borrowRecordRepository.AddAsync(newRecord);

        var resultDto = mapper.Map<BorrowRecordGetDto>(newRecord);
        return CreatedAtAction(nameof(GetRecordById), new { id = newRecord.Id }, resultDto);
    }

    /// <summary>
    /// Updates an existing borrow record by its unique ID.
    /// </summary>
    /// <param name="id">The ID of the borrow record to update.</param>
    /// <param name="updatedRecordDto">The updated borrow record data.</param>
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateRecord(int id, [FromBody] BorrowRecordEditDto updatedRecordDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var isBookExists = await bookRepository.ExistsById(updatedRecordDto.BookId);
        var isCustomerExists = await customerRepository.ExistsById(updatedRecordDto.CustomerId);

        if (!isBookExists || !isCustomerExists) return NotFound();

        var record = await borrowRecordRepository.GetByIdAsync(id);
        if (record == null) return NotFound();

        var updatedRecord = mapper.Map<BorrowRecord>(updatedRecordDto);
        updatedRecord.Id = record.Id;
        await borrowRecordRepository.UpdateAsync(updatedRecord);
        return NoContent();
    }
}
