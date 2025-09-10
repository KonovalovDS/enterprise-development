using library.Domain.Entities;

namespace library.Domain.Interfaces;

public interface IBookRepository {
    public Task<IEnumerable<Book>> GetAllAsync();
    public Task<Book?> GetByIdAsync(int id);
    public Task<bool> ExistsById(int id);
    public Task AddAsync(Book book);
    public Task UpdateAsync(Book book);
    public Task DeleteAsync(int id);
}
