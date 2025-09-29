using AutoMapper;

using Application.Dtos;
using Domain.Interfaces;

namespace Application.Services;

/// <summary>
/// Provides analytics operations related to books, customers and borrow records.
/// </summary>
/// <param name="borrowRecordRepository">Repository for accessing borrow records.</param>
/// <param name="bookRepository">Repository for accessing books.</param>
/// <param name="customerRepository">Repository for accessing customers.</param>
/// <param name="mapper">Mapper for dtos and entities.</param>
public class AnalyticsService(
    IBorrowRecordRepository borrowRecordRepository,
    IBookRepository bookRepository,
    ICustomerRepository customerRepository,
    IMapper mapper
)
{
    /// <summary>
    /// Returns all books borrowed on a specific date, sorted alphabetically by title.
    /// </summary>
    /// <param name="date">The specific date for filtering borrowed books.</param>
    public async Task<List<BookDto>> GetAllBorrowedBooksByDateSortedAsync(DateOnly date)
    {
        var records = await borrowRecordRepository.GetAllAsync();
        var books = await bookRepository.GetAllAsync();

        var allBorrowedBooks = records
            .Where(r => r.BorrowDate <= date && r.BorrowDate.AddDays(r.BorrowDuration) >= date)
            .Join(books,
                  r => r.BookId,
                  b => b.Id,
                  (r, b) => b)
            .Distinct()
            .OrderBy(b => b.Title)
            .ToList();

        var resultDto = mapper.Map<List<BookDto>>(allBorrowedBooks);
        return resultDto;
    }

    /// <summary>
    /// Returns the top five customers with the most borrows within a specified date range.
    /// </summary>
    /// <param name="start">Start date of the period.</param>
    /// <param name="end">End date of the period.</param>
    public async Task<List<CustomerDto>> GetTopFiveCustomersAsync(DateOnly start, DateOnly end)
    {
        var records = await borrowRecordRepository.GetAllAsync();
        var customers = await customerRepository.GetAllAsync();

        var topFiveCustomersWithCount = records
            .Where(r => r.BorrowDate >= start && r.BorrowDate.AddDays(r.BorrowDuration) <= end)
            .GroupBy(r => r.CustomerId)
            .Select(g => new
            {
                Customer = customers.First(c => c.Id == g.Key),
                BorrowCount = g.Count()
            })
            .OrderByDescending(x => x.BorrowCount)
            .Take(5)
            .ToList();

        var resultDto = mapper.Map<List<CustomerDto>>(topFiveCustomersWithCount.Select(x => x.Customer));
        for (var i = 0; i < resultDto.Count; i++)
        {
            resultDto[i].BorrowCount = topFiveCustomersWithCount[i].BorrowCount;
        }
        return resultDto;
    }

    /// <summary>
    /// Returns customers who have borrowed books for the longest duration.
    /// </summary>
    public async Task<List<CustomerDto>> GetCustomersWithLongestBorrowsAsync()
    {
        var records = await borrowRecordRepository.GetAllAsync();
        var customers = await customerRepository.GetAllAsync();

        var customersWithMaxDuration = records
            .GroupBy(r => r.CustomerId)
            .Select(x => new
            {
                Customer = customers.First(c => c.Id == x.Key),
                MaxDuration = x.Max(r => r.BorrowDuration)
            })
            .OrderByDescending(x => x.MaxDuration)
            .ToList();

        var maxDuration = customersWithMaxDuration.Max(x => x.MaxDuration);

        var topCustomers = customersWithMaxDuration
            .Where(x => x.MaxDuration == maxDuration)
            .Select(x => x.Customer)
            .OrderBy(c => c.Name)
            .ToList();

        var resultDto = mapper.Map<List<CustomerDto>>(topCustomers);
        return resultDto;
    }

    /// <summary>
    /// Returns the top five publishers based on book borrows within a specified period.
    /// </summary>
    /// <param name="start">Start date of the period.</param>
    /// <param name="end">End date of the period.</param>
    public async Task<List<string>> GetTopFivePublishersByDateAsync(DateOnly start, DateOnly end)
    {
        var records = await borrowRecordRepository.GetAllAsync();
        var books = await bookRepository.GetAllAsync();

        var topFivePublishers = records
            .Where(r => r.BorrowDate >= start && r.BorrowDate.AddDays(r.BorrowDuration) <= end)
            .Select(r => books.First(b => b.Id == r.BookId).Publisher)
            .GroupBy(p => p)
            .OrderByDescending(g => g.Count())
            .Take(5)
            .Select(g => g.Key.ToString())
            .ToList();

        return topFivePublishers;
    }

    /// <summary>
    /// Returns the top five least popular books based on borrow count.
    /// </summary>
    public async Task<List<BookDto>> GetTopFiveLeastPopularBooksAsync()
    {
        var records = await borrowRecordRepository.GetAllAsync();
        var books = await bookRepository.GetAllAsync();

        var topFiveBooksWithCount = books
        .Select(b => new
        {
            Book = b,
            BorrowCount = records.Count(r => r.BookId == b.Id)
        })
        .OrderBy(x => x.BorrowCount)
        .ThenBy(x => x.Book.Title)
        .Take(5)
        .ToList();

        var resultDto = mapper.Map<List<BookDto>>(topFiveBooksWithCount.Select(x => x.Book));
        for (var i = 0; i < resultDto.Count; i++)
        {
            resultDto[i].BorrowCount = topFiveBooksWithCount[i].BorrowCount;
        }
        return resultDto;
    }
}
