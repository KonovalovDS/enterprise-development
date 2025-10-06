using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories;

/// <summary>
/// Repository for managing BorrowRecord entities.
/// Provides CRUD methods for records.
/// </summary>
/// <param name="context">The application's database context used for data access.</param>
public class BorrowRecordRepository(AppDbContext context) : IBorrowRecordRepository 
{
    /// <summary>
    /// Gets all borrow records.
    /// </summary>
    public async Task<IEnumerable<BorrowRecord>> GetAllAsync() => 
        await context.BorrowRecords.ToListAsync();

    /// <summary>
    /// Gets a borrow record by its ID.
    /// </summary>
    /// <param name="id">Record ID.</param>
    /// <returns>The <see cref="BorrowRecord"/> if found; otherwise, null.</returns>
    public async Task<BorrowRecord?> GetByIdAsync(int id) => 
        await context.BorrowRecords.FirstOrDefaultAsync(r => r.Id == id);

    /// <summary>
    /// Checks if a borrow record exists by ID.
    /// </summary>
    /// <param name="id">Record ID.</param>
    /// <returns>True if exists; otherwise, false.</returns>
    public async Task<bool> ExistsById(int id) => 
        await context.BorrowRecords.AnyAsync(b => b.Id == id);

    /// <summary>
    /// Adds a new borrow record.
    /// </summary>
    /// <param name="record">Record to add.</param>
    public async Task AddAsync(BorrowRecord record) 
    {
        await context.BorrowRecords.AddAsync(record);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Updates an existing borrow record.
    /// </summary>
    /// <param name="record">Record with updated data.</param>
    public async Task UpdateAsync(BorrowRecord record) 
    {
        var existingRecord = await context.BorrowRecords.FindAsync(record.Id) ?? 
            throw new KeyNotFoundException($"Record with Id {record.Id} not found.");

        existingRecord.BookId = record.BookId;
        existingRecord.CustomerId = record.CustomerId;
        existingRecord.BorrowDuration = record.BorrowDuration;
        existingRecord.BorrowDate = record.BorrowDate;

        context.BorrowRecords.Update(existingRecord);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes a borrow record by ID.
    /// </summary>
    /// <param name="id">Record ID.</param>
    public async Task DeleteAsync(int id) 
    {
        var record = await context.BorrowRecords.FindAsync(id) ??
            throw new KeyNotFoundException($"Record with Id {id} not found.");

        context.BorrowRecords.Remove(record);
        await context.SaveChangesAsync();
    }
}
