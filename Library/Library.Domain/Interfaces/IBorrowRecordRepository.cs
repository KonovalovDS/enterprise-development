using Library.Domain.Entities;
using System.Linq.Expressions;

namespace Library.Domain.Interfaces;

/// <summary>
/// Provides methods for managing <see cref="BorrowRecord"/> entities in the repository.
/// </summary>
public interface IBorrowRecordRepository 
{
    /// <summary>
    /// Returns all borrow records.
    /// </summary>
    public Task<IEnumerable<BorrowRecord>> GetAllAsync();

    public Task<IEnumerable<BorrowRecord>> GetAllAsync(Expression<Func<BorrowRecord, bool>>? predicate = null);

    /// <summary>
    /// Returns a borrow record by its unique identifier.
    /// Returns null if the record does not exist.
    /// </summary>
    /// <param name="id">Record ID.</param>
    public Task<BorrowRecord?> GetByIdAsync(int id);

    /// <summary>
    /// Checks if a borrow record exists by its unique identifier.
    /// </summary>
    /// <param name="id">Record ID.</param>
    public Task<bool> ExistsById(int id);

    /// <summary>
    /// Adds a new borrow record to the repository.
    /// </summary>
    /// <param name="id">Record to add.</param>
    public Task AddAsync(BorrowRecord record);

    /// <summary>
    /// Updates an existing borrow record in the repository.
    /// </summary>
    /// <param name="id">Record to update.</param>
    public Task UpdateAsync(BorrowRecord record);

    /// <summary>
    /// Deletes a borrow record by its unique identifier.
    /// </summary>
    /// <param name="id">Record ID.</param>
    public Task DeleteAsync(int id);
}
