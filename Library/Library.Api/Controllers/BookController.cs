using AutoMapper;
using Library.Application.Contracts.BookDtos;
using Library.Domain.Entities;
using Library.Domain.Enums;
using Library.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Controllers;

/// <summary>
/// Endpoints for managing books.
/// </summary>
/// <param name="bookRepository">Repository for accessing book data.</param>
/// <param name="mapper">Mapper for dtos and entities.</param>
[ApiController]
[Route("api/books")]
public class BookController(
    IBookRepository bookRepository, 
    IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Returns all books in the system.
    /// </summary>
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<List<BookGetDto>>> GetAllBooks()
    {
        var books = await bookRepository.GetAllAsync();
        var booksDto = mapper.Map<List<BookGetDto>>(books);
        return Ok(booksDto);
    }

    /// <summary>
    /// Returns a book by its unique ID.
    /// </summary>
    /// <param name="id">The ID of the book to return.</param>
    [AllowAnonymous]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<BookGetDto>> GetBookById(int id)
    {
        var book = await bookRepository.GetByIdAsync(id);
        if (book == null) return NotFound();

        var bookDto = mapper.Map<BookGetDto>(book);
        return Ok(bookDto);
    }

    /// <summary>
    /// Deletes a book by its unique ID.
    /// </summary>
    /// <param name="id">The ID of the book to delete.</param>
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteBookById(int id)
    {
        var isExists = await bookRepository.ExistsById(id);
        if (!isExists) return NotFound();

        await bookRepository.DeleteAsync(id);
        return NoContent();
    }

    /// <summary>
    /// Creates a new book.
    /// </summary>
    /// <param name="newBookDto">The data of the book to create.</param>
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<BookGetDto>> CreateBook([FromBody] BookEditDto newBookDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        if (!Enum.IsDefined(typeof(Publisher), newBookDto.Publisher))
            return BadRequest($"Invalid publishing type: {newBookDto.Publisher}");
        if (!Enum.IsDefined(typeof(PublishingType), newBookDto.PublishingType))
            return BadRequest($"Invalid publishing type: {newBookDto.PublishingType}");

        var newBook = mapper.Map<Book>(newBookDto);
        await bookRepository.AddAsync(newBook);

        var resultDto = mapper.Map<BookGetDto>(newBook);
        return CreatedAtAction(nameof(GetBookById), new { id = newBook.Id }, resultDto);
    }

    /// <summary>
    /// Updates an existing book by its unique ID.
    /// </summary>
    /// <param name="id">The ID of the book to update.</param>
    /// <param name="updatedBookDto">The updated book data.</param>
    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateBook(int id, [FromBody] BookEditDto updatedBookDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        if (!Enum.IsDefined(typeof(Publisher), updatedBookDto.Publisher))
            return BadRequest($"Invalid publishing type: {updatedBookDto.Publisher}");
        if (!Enum.IsDefined(typeof(PublishingType), updatedBookDto.PublishingType))
            return BadRequest($"Invalid publishing type: {updatedBookDto.PublishingType}");

        var book = await bookRepository.GetByIdAsync(id);
        if (book == null) return NotFound();

        var updatedBook = mapper.Map<Book>(updatedBookDto);
        updatedBook.Id = book.Id;
        await bookRepository.UpdateAsync(updatedBook);
        return NoContent();
    }
}
