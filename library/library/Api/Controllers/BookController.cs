using Microsoft.AspNetCore.Mvc;

using library.Domain.Entities;
using library.Domain.Interfaces;

namespace library.Api.Controllers;

/// <summary>
/// Endpoints for managing books.
/// </summary>
/// <param name="bookRepository">Repository for accessing book data.</param>
[ApiController]
[Route("api/books")]
public class BookController(IBookRepository bookRepository) : Controller
{
    /// <summary>
    /// Repository for accessing book data.
    /// </summary>
    private readonly IBookRepository _bookRepository = bookRepository;

    /// <summary>
    /// Get all books.
    /// </summary>
    [HttpGet("")]
    public async Task<IActionResult> GetAllBooks()
    {
        var books = await _bookRepository.GetAllAsync();
        return Ok(books);
    }

    /// <summary>
    /// Get a book by its ID.
    /// </summary>
    /// <param name="id">The ID of the book to get.</param>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetBookById(int id)
    {
        var book = await _bookRepository.GetByIdAsync(id);
        if (book == null) return NotFound();

        return Ok(book);
    }

    /// <summary>
    /// Delete a book by its ID.
    /// </summary>
    /// <param name="id">The ID of the book to delete.</param>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteBookById(int id)
    {
        var isExists = await _bookRepository.ExistsById(id);
        if (!isExists) return NotFound();

        await _bookRepository.DeleteAsync(id);
        return NoContent();
    }

    /// <summary>
    /// Create a new book.
    /// </summary>
    /// <param name="newBook">The book object to create.</param>
    [HttpPost("")]
    public async Task<IActionResult> CreateBook([FromBody] Book newBook)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        await _bookRepository.AddAsync(newBook);
        return CreatedAtAction(nameof(GetBookById), new { id = newBook.Id }, newBook);
    }

    /// <summary>
    /// Update an existing book by ID.
    /// </summary>
    /// <param name="id">The ID of the book to update.</param>
    /// <param name="updated">The updated book object.</param>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateBook(int id, [FromBody] Book updated)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var book = await _bookRepository.GetByIdAsync(id);
        if (book == null) return NotFound();

        updated.Id = book.Id;
        await _bookRepository.UpdateAsync(updated);
        return NoContent();
    }
}
