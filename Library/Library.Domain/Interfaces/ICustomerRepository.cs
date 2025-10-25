using Library.Domain.Entities;

namespace Library.Domain.Interfaces;

/// <summary>
/// Provides methods for managing <see cref="Customer"/> entities in the repository.
/// </summary>
public interface ICustomerRepository 
{
    /// <summary>
    /// Returns all customers.
    /// </summary>
    public Task<IEnumerable<Customer>> GetAllAsync();

    /// <summary>
    /// Returns a customer by its unique identifier.
    /// Returns null if the customer does not exist.
    /// </summary>
    /// <param name="id">Customer ID.</param>
    public Task<Customer?> GetByIdAsync(int id);

    /// <summary>
    /// Checks if a customer exists by its unique identifier.
    /// </summary>
    /// <param name="id">Customer ID.</param>
    public Task<bool> ExistsById(int id);

    /// <summary>
    /// Adds a new customer to the repository.
    /// </summary>
    /// <param name="id">Customer to add.</param>
    public Task AddAsync(Customer customer);

    /// <summary>
    /// Updates an existing customer in the repository.
    /// </summary>
    /// <param name="id">Customer to update.</param>
    public Task UpdateAsync(Customer customer);

    /// <summary>
    /// Deletes a customer by its unique identifier.
    /// </summary>
    /// <param name="id">Customer ID.</param>
    public Task DeleteAsync(int id);
}
