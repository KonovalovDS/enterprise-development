using Library.Domain.Entities;
using Library.Domain.Interfaces;
using Library.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Repositories;

/// <summary>
/// Repository for managing Book entities.
/// Provides CRUD methods for books.
/// </summary>
/// <param name="context">The application's database context used for data access.</param>
public class BookRepository(AppDbContext context) : IBookRepository 
{
    /// <summary>
    /// Gets all books.
    /// </summary>
    public async Task<IEnumerable<Book>> GetAllAsync() => 
        await context.Books.ToListAsync();

    /// <summary>
    /// Gets a book by its ID.
    /// </summary>
    /// <param name="id">Book ID.</param>
    /// <returns>The <see cref="Book"/> if found; otherwise, null.</returns>
    public async Task<Book?> GetByIdAsync(int id) => 
        await context.Books.FirstOrDefaultAsync(b => b.Id == id);

    /// <summary>
    /// Checks if a book exists by ID.
    /// </summary>
    /// <param name="id">Book ID.</param>
    /// <returns>True if exists; otherwise, false.</returns>
    public async Task<bool> ExistsById(int id) => 
        await context.Books.AnyAsync(b => b.Id == id);

    /// <summary>
    /// Adds a new book.
    /// </summary>
    /// <param name="book">Book to add.</param>
    public async Task AddAsync(Book book) 
    {
        await context.Books.AddAsync(book);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Updates an existing book.
    /// </summary>
    /// <param name="book">Book with updated data.</param>
    public async Task UpdateAsync(Book book) 
    {
        var existingBook = await context.Books.FindAsync(book.Id) ?? 
            throw new KeyNotFoundException($"Book with Id {book.Id} not found.");

        existingBook.Title = book.Title;
        existingBook.Code = book.Code;
        existingBook.Author = book.Author;
        existingBook.PublicationYear = book.PublicationYear;
        existingBook.Publisher = book.Publisher;
        existingBook.PublishingType = book.PublishingType;

        context.Books.Update(existingBook);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes a book by ID.
    /// </summary>
    /// <param name="id">Book ID.</param>
    public async Task DeleteAsync(int id) 
    {
        var book = await context.Books.FindAsync(id) ??
            throw new KeyNotFoundException($"Book with Id {id} not found.");

        context.Books.Remove(book);
        await context.SaveChangesAsync();
    }
}
