using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;

namespace Application.Services;

/// <summary>
/// Provides analytics operations related to books, customers and borrow records.
/// </summary>
/// <param name="borrowRecordRepository">Repository for accessing borrow records.</param>
/// <param name="bookRepository">Repository for accessing books.</param>
/// <param name="customerRepository">Repository for accessing customers.</param>
public class AnalyticsService(
    IBorrowRecordRepository borrowRecordRepository,
    IBookRepository bookRepository,
    ICustomerRepository customerRepository)
{
    /// <summary>
    /// Repository for borrow records.
    /// </summary>
    private readonly IBorrowRecordRepository _borrowRecordRepository = borrowRecordRepository;

    /// <summary>
    /// Repository for books.
    /// </summary>
    private readonly IBookRepository _bookRepository = bookRepository;

    /// <summary>
    /// Repository for customers.
    /// </summary>
    private readonly ICustomerRepository _customerRepository = customerRepository;

    /// <summary>
    /// Returns all borrowed books and sorts them by name.
    /// </summary>
    /// <returns>The result contains a list of borrowed <see cref="Book"/> objects.</returns>
    public async Task<List<Book?>> GetAllBorrowedBooksSortedAsync()
    {
        var records = await _borrowRecordRepository.GetAllAsync();
        var books = await _bookRepository.GetAllAsync();

        var allBorrowedBooks = records
            .Select(r => books.FirstOrDefault(b => b.Id == r.BookId))
            .Where(b => b != null)
            .Distinct()!
            .OrderBy(b => b!.Title)
            .ToList()!;

        return allBorrowedBooks;
    }

    /// <summary>
    /// Returns the top five customers with the most borrows within a specified period.
    /// </summary>
    /// <param name="start">Start date of the period.</param>
    /// <param name="end">End date of the period.</param>
    /// <returns>The result contains a list of the top five <see cref="Customer"/> objects.</returns>
    public async Task<List<Customer?>> GetTopFiveCustomersAsync(DateOnly start, DateOnly end)
    {
        var records = await _borrowRecordRepository.GetAllAsync();
        var customers = await _customerRepository.GetAllAsync();

        var topFiveCustomers = records
            .Where(r => r.BorrowDate >= start && r.BorrowDate.AddDays(r.BorrowDuration) <= end)
            .GroupBy(r => r.CustomerId)
            .OrderByDescending(g => g.Count())
            .Take(5)
            .Select(g => customers.FirstOrDefault(c => c.Id == g.Key))
            .Where(c => c != null)!
            .ToList()!;

        return topFiveCustomers;
    }

    /// <summary>
    /// Returns customers who have borrowed books for the longest duration.
    /// </summary>
    /// <returns>The result contains a list of <see cref="Customer"/> objects with the longest borrow durations.</returns>
    public async Task<List<Customer>> GetCustomersWithLongestBorrowsAsync()
    {
        var records = await _borrowRecordRepository.GetAllAsync();
        var customers = await _customerRepository.GetAllAsync();

        var customersWithDurations = records
            .GroupBy(r => r.CustomerId)
            .Select(g => new
            {
                Customer = customers.FirstOrDefault(c => c.Id == g.Key),
                MaxDuration = g.Max(r => r.BorrowDuration)
            })
            .Where(x => x.Customer != null)
            .OrderByDescending(x => x.MaxDuration)
            .ToList();

        var maxDuration = customersWithDurations.FirstOrDefault()?.MaxDuration ?? 0;

        var customersWithLondestBorrows = customersWithDurations
            .Where(x => x.MaxDuration == maxDuration)
            .Select(x => x.Customer!)
            .OrderBy(c => c.Name)
            .ToList();

        return customersWithLondestBorrows;
    }

    /// <summary>
    /// Returns the top five publishers based on book borrows within a specified period.
    /// </summary>
    /// <param name="start">Start date of the period.</param>
    /// <param name="end">End date of the period.</param>
    /// <returns>The result contains a list of top five <see cref="Publisher"/> values.</returns>
    public async Task<List<Publisher?>> GetTopFivePublishersLastYearAsync(DateOnly start, DateOnly end)
    {
        var records = await _borrowRecordRepository.GetAllAsync();
        var books = await _bookRepository.GetAllAsync();

        var topFivePublishers = records
            .Where(r => r.BorrowDate >= start && r.BorrowDate.AddDays(r.BorrowDuration) <= end)
            .Select(r => books.FirstOrDefault(b => b.Id == r.BookId)?.Publisher)
            .Where(p => p != null)!
            .GroupBy(p => p)
            .OrderByDescending(g => g.Count())
            .Take(5)
            .Select(g => g.Key!)
            .ToList()!;

        return topFivePublishers;
    }

    /// <summary>
    /// Returns the top five least popular books based on borrow count.
    /// </summary>
    /// <returns>The result contains a list of the least popular <see cref="Book"/> objects.</returns>
    public async Task<List<Book>> GetTopFiveLeastPopularBooksAsync()
    {
        var records = await _borrowRecordRepository.GetAllAsync();
        var books = await _bookRepository.GetAllAsync();

        var topFiveLeastPopularBooks = books
            .Select(b => new
            {
                Book = b,
                BorrowCount = records.Count(r => r.BookId == b.Id)
            })
            .OrderBy(x => x.BorrowCount)
            .ThenBy(x => x.Book.Title)
            .Take(5)
            .Select(x => x.Book)
            .ToList();

        return topFiveLeastPopularBooks;
    }
}
