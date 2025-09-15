using library.Api.DTOs;
using library.Domain.Entities;
using library.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace library.Api.Controllers;

[ApiController]
[Route("api/books")]
public class BookController : Controller
{
    private readonly IBookRepository _bookRepository;

    public BookController(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAllBooks()
    {
        var books = await _bookRepository.GetAllAsync();
        return Ok(books);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetBookById(int id)
    {
        var book = await _bookRepository.GetByIdAsync(id);
        if (book == null) return NotFound();
        return Ok(book);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteBookById(int id)
    {
        var isExists = await _bookRepository.ExistsById(id);
        if (!isExists) return NotFound();
        await _bookRepository.DeleteAsync(id);
        return NoContent();
    }

    [HttpPost("")]
    public async Task<IActionResult> CreateBook([FromBody] BookDto dto)
    {
        var book = new Book("", dto.Author, dto.Name, dto.PublicationYear, dto.Publisher, dto.PublishingType);
        await _bookRepository.AddAsync(book);
        return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, book);
    }

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
