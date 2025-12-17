using AutoMapper;
using Library.Application.Contracts.BorrowRecordDtos;
using Library.Domain.Entities;
using Library.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Library.Api.Controllers;

/// <summary>
/// Endpoints for managing borrow records.
/// </summary>
/// <param name="borrowRecordRepository">Repository for accessing borrow records.</param>
/// <param name="bookRepository">Repository for accessing books.</param>
/// <param name="customerRepository">Repository for accessing customers.</param>
/// <param name="mapper">Mapper for dtos and entities.</param>
/// <param name="userManager">User role manager that provides allowed methods.</param>
[ApiController]
[Authorize]
[Route("api/records")]
public class BorrowRecordController(
    IBorrowRecordRepository borrowRecordRepository,
    IBookRepository bookRepository,
    ICustomerRepository customerRepository,
    IMapper mapper,
    UserManager<ApplicationUser> userManager) : ControllerBase
{
    /// <summary>
    /// Returns all borrow records.
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<BorrowRecordGetDto>>> GetAllRecords()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var isAdmin = User.IsInRole("Admin");
        var user = await userManager.FindByIdAsync(userId!);

        if (!isAdmin && user?.CustomerId == null)
            return Forbid();

        var allRecords = await borrowRecordRepository.GetAllAsync();

        IEnumerable<BorrowRecord> records = isAdmin
            ? allRecords
            : allRecords.Where(r => r.CustomerId == user!.CustomerId);

        return Ok(mapper.Map<List<BorrowRecordGetDto>>(records));
    }

    /// <summary>
    /// Returns a borrow record by its unique ID.
    /// </summary>
    /// <param name="id">The ID of the borrow record to return.</param>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<BorrowRecordGetDto>> GetRecordById(int id)
    {
        var record = await borrowRecordRepository.GetByIdAsync(id);
        if (record == null)
            return NotFound();

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var isAdmin = User.IsInRole("Admin");
        var user = await userManager.FindByIdAsync(userId!);

        if (!isAdmin && user?.CustomerId != record.CustomerId)
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
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteRecordById(int id)
    {
        var record = await borrowRecordRepository.GetByIdAsync(id);
        if (record == null)
            return NoContent();

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var isAdmin = User.IsInRole("Admin");
        var user = await userManager.FindByIdAsync(userId!);

        if (!isAdmin && user?.CustomerId != record.CustomerId)
            return NoContent();

        await borrowRecordRepository.DeleteAsync(id);
        return NoContent();
    }

    /// <summary>
    /// Creates a new borrow record.
    /// </summary>
    /// <param name="newRecordDto">The data for the new borrow record.</param>
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<BorrowRecordGetDto>> CreateRecord([FromBody] BorrowRecordEditDto newRecordDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var isAdmin = User.IsInRole("Admin");
        var user = await userManager.FindByIdAsync(userId!);

        if (!isAdmin && user?.CustomerId != newRecordDto.CustomerId)
            return NotFound();

        if (!await bookRepository.ExistsById(newRecordDto.BookId) ||
            !await customerRepository.ExistsById(newRecordDto.CustomerId))
            return NotFound();

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
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize]
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateRecord(int id, [FromBody] BorrowRecordEditDto updatedRecordDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var record = await borrowRecordRepository.GetByIdAsync(id);
        if (record == null)
            return NotFound();

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var isAdmin = User.IsInRole("Admin");
        var user = await userManager.FindByIdAsync(userId!);

        if (!isAdmin && user?.CustomerId != record.CustomerId)
            return NotFound();

        if (!await bookRepository.ExistsById(updatedRecordDto.BookId) ||
            !await customerRepository.ExistsById(updatedRecordDto.CustomerId))
            return NotFound();

        var updatedRecord = mapper.Map<BorrowRecord>(updatedRecordDto);
        updatedRecord.Id = record.Id;
        await borrowRecordRepository.UpdateAsync(updatedRecord);

        return NoContent();
    }
}
