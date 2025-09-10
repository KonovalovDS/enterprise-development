using Microsoft.EntityFrameworkCore;

using library.Domain.Entities;
using library.Domain.Interfaces;
using library.Infrastructure.Persistence;

namespace library.Infrastructure.Repositories;

public class BookRepository : IBookRepository {
    private readonly AppDbContext _context;

    public BookRepository(AppDbContext context) {
        _context = context;
    }

    public async Task<IEnumerable<Book>> GetAllAsync() => 
        await _context.Books.ToListAsync();

    public async Task<Book?> GetByIdAsync(int id) => 
        await _context.Books.FirstOrDefaultAsync(b => b.Id == id);

    public async Task<bool> ExistsById(int id) => 
        await _context.Books.AnyAsync(b => b.Id == id);

    public async Task AddAsync(Book book) {
        if (book == null) throw new ArgumentNullException(nameof(book));
        await _context.Books.AddAsync(book);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Book book) {
        if (book == null) throw new ArgumentNullException(nameof(book));
        var existingBook = await _context.Books.FindAsync(book.Id);
        if (existingBook == null)
            throw new KeyNotFoundException($"Book with Id {book.Id} not found.");
        existingBook.Update(book.Name, book.Author, book.PublicationYear, book.Publisher, book.PublishingType);
        _context.Books.Update(existingBook);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id) {
        var book = await _context.Books.FindAsync(id);
        if (book == null)
            throw new KeyNotFoundException($"Book with Id {id} not found.");
        _context.Books.Remove(book);
        await _context.SaveChangesAsync();
    }
}
