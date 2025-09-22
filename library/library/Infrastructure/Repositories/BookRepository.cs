using Microsoft.EntityFrameworkCore;

using library.Domain.Entities;
using library.Domain.Interfaces;
using library.Infrastructure.Persistence;

namespace library.Infrastructure.Repositories;

/// <summary>
/// Repository for managing Book entities.
/// Provides CRUD methods for books.
/// </summary>
public class BookRepository : IBookRepository 
{
    private readonly AppDbContext _context;

    /// <summary>
    /// Initializes a new instance of <see cref="BookRepository"/>.
    /// </summary>
    public BookRepository(AppDbContext context) 
    {
        _context = context;
    }

    /// <summary>Gets all books.</summary>
    public async Task<IEnumerable<Book>> GetAllAsync() => 
        await _context.Books.ToListAsync();

    /// <summary>Gets a book by its ID.</summary>
    /// <param name="id">Book ID.</param>
    /// <returns>The <see cref="Book"/> if found; otherwise, null.</returns>
    public async Task<Book?> GetByIdAsync(int id) => 
        await _context.Books.FirstOrDefaultAsync(b => b.Id == id);

    /// <summary>Checks if a book exists by ID.</summary>
    /// <param name="id">Book ID.</param>
    /// <returns>True if exists; otherwise, false.</returns>
    public async Task<bool> ExistsById(int id) => 
        await _context.Books.AnyAsync(b => b.Id == id);

    /// <summary>Adds a new book.</summary>
    /// <param name="book">Book to add.</param>
    public async Task AddAsync(Book book) 
    {
        if (book == null) throw new ArgumentNullException(nameof(book));
        await _context.Books.AddAsync(book);
        await _context.SaveChangesAsync();
    }

    /// <summary>Updates an existing book.</summary>
    /// <param name="book">Book with updated data.</param>
    public async Task UpdateAsync(Book book) 
    {
        if (book == null) throw new ArgumentNullException(nameof(book));
        var existingBook = await _context.Books.FindAsync(book.Id);
        if (existingBook == null)
            throw new KeyNotFoundException($"Book with Id {book.Id} not found.");

        existingBook.Title = book.Title;
        existingBook.Author = book.Author;
        existingBook.PublicationYear = book.PublicationYear;
        existingBook.Publisher = book.Publisher;
        existingBook.PublishingType = book.PublishingType;

        _context.Books.Update(existingBook);
        await _context.SaveChangesAsync();
    }

    /// <summary>Deletes a book by ID.</summary>
    /// <param name="id">Book ID.</param>
    public async Task DeleteAsync(int id) 
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null)
            throw new KeyNotFoundException($"Book with Id {id} not found.");

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();
    }
}
