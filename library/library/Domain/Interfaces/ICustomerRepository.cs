using library.Domain.Entities;

namespace library.Domain.Interfaces;

/// <summary>
/// Provides methods for managing <see cref="Customer"/> entities in the repository.
/// </summary>
public interface ICustomerRepository {
    public Task<IEnumerable<Customer>> GetAllAsync();
    public Task<Customer?> GetByIdAsync(int id);
    public Task<bool> ExistsById(int id);
    public Task AddAsync(Customer customer);
    public Task UpdateAsync(Customer customer);
    public Task DeleteAsync(int id);
}
