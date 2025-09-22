using Domain.Entities;

namespace Domain.Interfaces;

/// <summary>
/// Provides methods for managing <see cref="BorrowRecord"/> entities in the repository.
/// </summary>
public interface IBorrowRecordRepository 
{
    /// <summary>
    /// Returns all borrow records.
    /// </summary>
    public Task<IEnumerable<BorrowRecord>> GetAllAsync();

    /// <summary>
    /// Returns a borrow record by its unique identifier.
    /// Returns null if the record does not exist.
    /// </summary>
    public Task<BorrowRecord?> GetByIdAsync(int id);

    /// <summary>
    /// Checks if a borrow record exists by its unique identifier.
    /// </summary>
    public Task<bool> ExistsById(int id);

    /// <summary>
    /// Adds a new borrow record to the repository.
    /// </summary>
    public Task AddAsync(BorrowRecord record);

    /// <summary>
    /// Updates an existing borrow record in the repository.
    /// </summary>
    public Task UpdateAsync(BorrowRecord record);

    /// <summary>
    /// Deletes a borrow record by its unique identifier.
    /// </summary>
    public Task DeleteAsync(int id);
}
