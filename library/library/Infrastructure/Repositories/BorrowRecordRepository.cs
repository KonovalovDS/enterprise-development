using Microsoft.EntityFrameworkCore;

using library.Domain.Entities;
using library.Domain.Interfaces;
using library.Infrastructure.Persistence;

namespace library.Infrastructure.Repositories;

/// <summary>
/// Repository for managing BorrowRecord entities.
/// Provides CRUD methods for records.
/// </summary>
public class BorrowRecordRepository : IBorrowRecordRepository 
{
    private readonly AppDbContext _context;

    /// <summary>
    /// Initializes a new instance of <see cref="BorrowRecordRepository"/>.
    /// </summary>
    public BorrowRecordRepository(AppDbContext context) 
    {
        _context = context;
    }

    /// <summary>Gets all borrow records.</summary>
    public async Task<IEnumerable<BorrowRecord>> GetAllAsync() => 
        await _context.BorrowRecords.ToListAsync();

    /// <summary>Gets a borrow record by its ID.</summary>
    /// <param name="id">Record ID.</param>
    /// <returns>The <see cref="BorrowRecord"/> if found; otherwise, null.</returns>
    public async Task<BorrowRecord?> GetByIdAsync(int id) => 
        await _context.BorrowRecords.FirstOrDefaultAsync(r => r.Id == id);

    /// <summary>Checks if a borrow record exists by ID.</summary>
    /// <param name="id">Record ID.</param>
    /// <returns>True if exists; otherwise, false.</returns>
    public async Task<bool> ExistsById(int id) => 
        await _context.BorrowRecords.AnyAsync(b => b.Id == id);

    /// <summary>Adds a new borrow record.</summary>
    /// <param name="record">Record to add.</param>
    public async Task AddAsync(BorrowRecord record) 
    {
        if (record == null) throw new ArgumentNullException(nameof(record));

        await _context.BorrowRecords.AddAsync(record);
        await _context.SaveChangesAsync();
    }

    /// <summary>Updates an existing borrow record.</summary>
    /// <param name="record">Record with updated data.</param>
    public async Task UpdateAsync(BorrowRecord record) 
    {
        if (record == null) throw new ArgumentNullException(nameof(record));
        var existingRecord = await _context.BorrowRecords.FindAsync(record.Id);
        if (existingRecord == null)
            throw new KeyNotFoundException($"Record with Id {record.Id} not found.");

        existingRecord.BookId = record.BookId;
        existingRecord.CustomerId = record.CustomerId;
        existingRecord.BorrowDuration = record.BorrowDuration;
        existingRecord.BorrowDate = record.BorrowDate;

        _context.BorrowRecords.Update(existingRecord);
        await _context.SaveChangesAsync();
    }

    /// <summary>Deletes a borrow record by ID.</summary>
    /// <param name="id">Record ID.</param>
    public async Task DeleteAsync(int id) 
    {
        var record = await _context.BorrowRecords.FindAsync(id);
        if (record == null)
            throw new KeyNotFoundException($"Record with Id {id} not found.");

        _context.BorrowRecords.Remove(record);
        await _context.SaveChangesAsync();
    }
}
