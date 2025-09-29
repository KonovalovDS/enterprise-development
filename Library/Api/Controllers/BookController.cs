using Microsoft.AspNetCore.Mvc;

using Domain.Entities;
using Domain.Interfaces;
using AutoMapper;
using Application.Dtos;
using static System.Reflection.Metadata.BlobBuilder;

namespace Api.Controllers;

/// <summary>
/// Endpoints for managing books.
/// </summary>
/// <param name="bookRepository">Repository for accessing book data.</param>
[ApiController]
[Route("api/books")]
public class BookController(
    IBookRepository bookRepository, 
    IMapper mapper) : Controller
{
    /// <summary>
    /// Get all books.
    /// </summary>
    [HttpGet("")]
    public async Task<IActionResult> GetAllBooks()
    {
        var books = await bookRepository.GetAllAsync();
        var booksDto = mapper.Map<List<BookDto>>(books);
        return Ok(booksDto);
    }

    /// <summary>
    /// Get a book by its ID.
    /// </summary>
    /// <param name="id">The ID of the book to get.</param>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetBookById(int id)
    {
        var book = await bookRepository.GetByIdAsync(id);
        if (book == null) return NotFound();

        var bookDto = mapper.Map<BookDto>(book);
        return Ok(bookDto);
    }

    /// <summary>
    /// Delete a book by its ID.
    /// </summary>
    /// <param name="id">The ID of the book to delete.</param>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteBookById(int id)
    {
        var isExists = await bookRepository.ExistsById(id);
        if (!isExists) return NotFound();

        await bookRepository.DeleteAsync(id);
        return NoContent();
    }

    /// <summary>
    /// Create a new book.
    /// </summary>
    /// <param name="newBook">The book object to create.</param>
    [HttpPost("")]
    public async Task<IActionResult> CreateBook([FromBody] BookDto newBookDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var newBook = mapper.Map<Book>(newBookDto);
        await bookRepository.AddAsync(newBook);
        return CreatedAtAction(nameof(GetBookById), new { id = newBook.Id }, newBook);
    }

    /// <summary>
    /// Update an existing book by ID.
    /// </summary>
    /// <param name="id">The ID of the book to update.</param>
    /// <param name="updated">The updated book object.</param>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateBook(int id, [FromBody] BookDto updatedBookDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var book = await bookRepository.GetByIdAsync(id);
        if (book == null) return NotFound();

        var updatedBook = mapper.Map<Book>(updatedBookDto);
        updatedBook.Id = book.Id;
        await bookRepository.UpdateAsync(updatedBook);
        return NoContent();
    }
}
