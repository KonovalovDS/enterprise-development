using Microsoft.AspNetCore.Mvc;

using library.Api.DTOs;
using library.Domain.Entities;
using library.Domain.Interfaces;

namespace library.Api.Controllers;

/// <summary>
/// Endpoints for managing books.
/// </summary>
[ApiController]
[Route("api/books")]
public class BookController : Controller
{
    private readonly IBookRepository _bookRepository;

    /// <summary>
    /// Initializes a new instance of <see cref="BookController"/>.
    /// </summary>
    public BookController(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    /// <summary>Get all books.</summary>
    [HttpGet("")]
    public async Task<IActionResult> GetAllBooks()
    {
        var books = await _bookRepository.GetAllAsync();
        return Ok(books);
    }

    /// <summary>Get a book by its ID.</summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetBookById(int id)
    {
        var book = await _bookRepository.GetByIdAsync(id);
        if (book == null) return NotFound();
        return Ok(book);
    }

    /// <summary>Delete a book by its ID.</summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteBookById(int id)
    {
        var isExists = await _bookRepository.ExistsById(id);
        if (!isExists) return NotFound();
        await _bookRepository.DeleteAsync(id);
        return NoContent();
    }

    /// <summary>Create a new book.</summary>
    [HttpPost("")]
    public async Task<IActionResult> CreateBook([FromBody] BookDto dto)
    {
        var book = new Book("", dto.Author, dto.Name, dto.PublicationYear, dto.Publisher, dto.PublishingType);
        await _bookRepository.AddAsync(book);
        return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, book);
    }

    /// <summary>Update an existing book by ID.</summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateBook(int id, [FromBody] BookDto updated)
    {
        var book = await _bookRepository.GetByIdAsync(id);
        if (book == null) return NotFound();
        book.Update(updated.Name, updated.Author, updated.PublicationYear, updated.Publisher, updated.PublishingType);
        await _bookRepository.UpdateAsync(book);
        return NoContent();
    }
}
