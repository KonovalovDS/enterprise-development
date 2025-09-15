using Microsoft.EntityFrameworkCore;

using library.Domain.Entities;
using library.Domain.Interfaces;
using library.Infrastructure.Persistence;

namespace library.Infrastructure.Repositories;

public class BorrowRecordRepository : IBorrowRecordRepository {
    private readonly AppDbContext _context;

    public BorrowRecordRepository(AppDbContext context) {
        _context = context;
    }

    public async Task<IEnumerable<BorrowRecord>> GetAllAsync() => 
        await _context.BorrowRecords.ToListAsync();

    public async Task<BorrowRecord?> GetByIdAsync(int id) => 
        await _context.BorrowRecords.FirstOrDefaultAsync(r => r.Id == id);

    public async Task<bool> ExistsById(int id) => 
        await _context.BorrowRecords.AnyAsync(b => b.Id == id);

    public async Task AddAsync(BorrowRecord record) {
        if (record == null) throw new ArgumentNullException(nameof(record));
        await _context.BorrowRecords.AddAsync(record);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(BorrowRecord record) {
        if (record == null) throw new ArgumentNullException(nameof(record));
        var existingRecord = await _context.BorrowRecords.FindAsync(record.Id);
        if (existingRecord == null)
            throw new KeyNotFoundException($"Record with Id {record.Id} not found.");
        existingRecord.Update(record.BookId, record.CustomerId, record.BorrowDuration, record.BorrowDate);
        _context.BorrowRecords.Update(record);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id) {
        var record = await _context.BorrowRecords.FindAsync(id);
        if (record == null)
            throw new KeyNotFoundException($"Record with Id {id} not found.");
        _context.BorrowRecords.Remove(record);
        await _context.SaveChangesAsync();
    }
}
