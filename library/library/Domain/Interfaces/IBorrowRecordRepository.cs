using library.Domain.Entities;

namespace library.Domain.Interfaces;

/// <summary>
/// Provides methods for managing <see cref="BorrowRecord"/> entities in the repository.
/// </summary>
public interface IBorrowRecordRepository {
    public Task<IEnumerable<BorrowRecord>> GetAllAsync();
    public Task<BorrowRecord?> GetByIdAsync(int id);
    public Task<bool> ExistsById(int id);
    public Task AddAsync(BorrowRecord record);
    public Task UpdateAsync(BorrowRecord record);
    public Task DeleteAsync(int id);
}
