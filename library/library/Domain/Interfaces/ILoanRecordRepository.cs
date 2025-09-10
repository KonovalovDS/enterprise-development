using library.Domain.Entities;

namespace library.Domain.Interfaces;

public interface IBorrowRecordRepository {
    public Task<IEnumerable<BorrowRecord>> GetAllAsync();
    public Task<BorrowRecord?> GetByIdAsync(int id);
    public Task<bool> ExistsById(int id);
    public Task AddAsync(BorrowRecord record);
    public Task UpdateAsync(BorrowRecord record);
    public Task DeleteAsync(int id);
}
