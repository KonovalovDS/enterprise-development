using library.Domain.Entities;

namespace library.Domain.Interfaces;

/// <summary>
/// Provides methods for managing <see cref="Book"/> entities in the repository.
/// </summary>
public interface IBookRepository 
{
    /// <summary>
    /// Returns all books.
    /// </summary>
    public Task<IEnumerable<Book>> GetAllAsync();

    /// <summary>
    /// Returns a book by its unique identifier.
    /// Returns null if the book does not exist.
    /// </summary>
    public Task<Book?> GetByIdAsync(int id);

    /// <summary>
    /// Checks if a book exists by its unique identifier.
    /// </summary>
    public Task<bool> ExistsById(int id);

    /// <summary>
    /// Adds a new book to the repository.
    /// </summary>
    public Task AddAsync(Book book);

    /// <summary>
    /// Updates an existing book in the repository.
    /// </summary>
    public Task UpdateAsync(Book book);

    /// <summary>
    /// Deletes a book by its unique identifier.
    /// </summary>
    public Task DeleteAsync(int id);
}
