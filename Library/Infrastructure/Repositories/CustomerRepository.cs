using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories;

/// <summary>
/// Repository for managing Customer entities.
/// Provides CRUD methods for customers.
/// </summary>
/// <param name="context">The application's database context used for data access.</param>
public class CustomerRepository(AppDbContext context) : ICustomerRepository 
{
    /// <summary>
    /// Gets all customers.
    /// </summary>
    public async Task<IEnumerable<Customer>> GetAllAsync() =>
        await context.Customers.ToListAsync();

    /// <summary>
    /// Gets a customer by its ID.
    /// </summary>
    /// <param name="id">Customer ID.</param>
    /// <returns>The <see cref="Customer"/> if found; otherwise, null.</returns>
    public async Task<Customer?> GetByIdAsync(int id) => 
        await context.Customers.FirstOrDefaultAsync(c => c.Id == id);

    /// <summary>
    /// Checks if a customer exists by ID.
    /// </summary>
    /// <param name="id">Customer ID.</param>
    /// <returns>True if exists; otherwise, false.</returns>
    public async Task<bool> ExistsById(int id) => 
        await context.Customers.AnyAsync(b => b.Id == id);

    /// <summary>
    /// Adds a new customer.
    /// </summary>
    /// <param name="customer">Customer to add.</param>
    public async Task AddAsync(Customer customer) 
    {
        await context.Customers.AddAsync(customer);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Updates an existing customer.
    /// </summary>
    /// <param name="customer">Customer with updated data.</param>
    public async Task UpdateAsync(Customer customer) 
    {
        var existingCustomer = await context.Customers.FindAsync(customer.Id) ?? 
            throw new KeyNotFoundException($"Customer with Id {customer.Id} not found.");

        existingCustomer.Name = customer.Name;
        existingCustomer.Address = customer.Address;
        existingCustomer.PhoneNumber = customer.PhoneNumber;

        context.Customers.Update(existingCustomer);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes a customer by ID.
    /// </summary>
    /// <param name="id">Customer ID.</param>
    public async Task DeleteAsync(int id) 
    {
        var customer = await context.Customers.FindAsync(id) ??
            throw new KeyNotFoundException($"Customer with Id {id} not found.");

        context.Customers.Remove(customer);
        await context.SaveChangesAsync();
    }
}
