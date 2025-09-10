using library.Domain.Entities;
using library.Domain.Interfaces;
using library.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace library.Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository {
    private readonly AppDbContext _context;

    public CustomerRepository(AppDbContext context) {
        _context = context;
    }

    public async Task<IEnumerable<Customer>> GetAllAsync() =>
        await _context.Customers.ToListAsync();

    public async Task<Customer?> GetByIdAsync(int id) => 
        await _context.Customers.FirstOrDefaultAsync(c => c.Id == id);

    public async Task<bool> ExistsById(int id) => await _context.Customers.AnyAsync(b => b.Id == id);

    public async Task AddAsync(Customer customer) {
        if (customer == null) throw new ArgumentNullException(nameof(customer));
        await _context.Customers.AddAsync(customer);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Customer customer) {
        if (customer == null) throw new ArgumentNullException(nameof(customer));
        var existingCustomer = await _context.Customers.FindAsync(customer.Id);
        if (existingCustomer == null)
            throw new KeyNotFoundException($"Customer with Id {customer.Id} not found.");
        existingCustomer.Update(customer.Name, customer.Address, customer.PhoneNumber);
        _context.Customers.Update(existingCustomer);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id) {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null)
            throw new KeyNotFoundException($"Customer with Id {id} not found.");
        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();
    }
}
