using Microsoft.EntityFrameworkCore;

using library.Domain.Entities;
using library.Domain.Interfaces;
using library.Infrastructure.Persistence;

namespace library.Infrastructure.Repositories;

/// <summary>
/// Repository for managing Customer entities.
/// Provides CRUD methods for customers.
/// </summary>
public class CustomerRepository(AppDbContext context) : ICustomerRepository 
{
    private readonly AppDbContext _context = context;

    /// <summary>
    /// Gets all customers.
    /// </summary>
    public async Task<IEnumerable<Customer>> GetAllAsync() =>
        await _context.Customers.ToListAsync();

    /// <summary>
    /// Gets a customer by its ID.
    /// </summary>
    /// <param name="id">Customer ID.</param>
    /// <returns>The <see cref="Customer"/> if found; otherwise, null.</returns>
    public async Task<Customer?> GetByIdAsync(int id) => 
        await _context.Customers.FirstOrDefaultAsync(c => c.Id == id);

    /// <summary>
    /// Checks if a customer exists by ID.
    /// </summary>
    /// <param name="id">Customer ID.</param>
    /// <returns>True if exists; otherwise, false.</returns>
    public async Task<bool> ExistsById(int id) => 
        await _context.Customers.AnyAsync(b => b.Id == id);

    /// <summary>
    /// Adds a new customer.
    /// </summary>
    /// <param name="customer">Customer to add.</param>
    public async Task AddAsync(Customer customer) 
    {
        if (customer == null) throw new ArgumentNullException(nameof(customer));

        await _context.Customers.AddAsync(customer);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Updates an existing customer.
    /// </summary>
    /// <param name="customer">Customer with updated data.</param>
    public async Task UpdateAsync(Customer customer) 
    {
        if (customer == null) throw new ArgumentNullException(nameof(customer));
        var existingCustomer = await _context.Customers.FindAsync(customer.Id);
        if (existingCustomer == null)
            throw new KeyNotFoundException($"Customer with Id {customer.Id} not found.");

        existingCustomer.Name = customer.Name;
        existingCustomer.Address = customer.Address;
        existingCustomer.PhoneNumber = customer.PhoneNumber;

        _context.Customers.Update(existingCustomer);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes a customer by ID.
    /// </summary>
    /// <param name="id">Customer ID.</param>
    public async Task DeleteAsync(int id) 
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null)
            throw new KeyNotFoundException($"Customer with Id {id} not found.");

        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();
    }
}
