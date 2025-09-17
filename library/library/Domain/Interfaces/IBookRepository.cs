using library.Domain.Entities;

namespace library.Domain.Interfaces;

/// <summary>
/// Provides methods for managing <see cref="Book"/> entities in the repository.
/// </summary>
public interface IBookRepository {
    public Task<IEnumerable<Book>> GetAllAsync();
    public Task<Book?> GetByIdAsync(int id);
    public Task<bool> ExistsById(int id);
    public Task AddAsync(Book book);
    public Task UpdateAsync(Book book);
    public Task DeleteAsync(int id);
}
