using AutoMapper;
using Library.Application.Contracts.AnalyticsDtos;
using Library.Domain.Interfaces;

namespace Library.Application.Services;

/// <summary>
/// Provides analytics operations related to books, customers and borrow records.
/// </summary>
/// <param name="borrowRecordRepository">Repository for accessing borrow records.</param>
/// <param name="bookRepository">Repository for accessing books.</param>
/// <param name="customerRepository">Repository for accessing customers.</param>
/// <param name="mapper">Mapper for dtos.</param>
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
    public async Task<List<BookWithBorrowCountDto>> GetAllBorrowedBooksByDateSortedAsync(DateOnly date)
    {
        var records = await borrowRecordRepository.GetAllAsync();
        var books = await bookRepository.GetAllAsync();

        var allBorrowedBooks = records
            .Where(r => r.BorrowDate <= date && r.BorrowDate.AddDays(r.BorrowDuration) >= date)
            .GroupBy(r => r.BookId)
            .Join(books,
                r => r.Key,
                b => b.Id,
                (r, b) =>
                {
                    var dto = mapper.Map<BookWithBorrowCountDto>(b);
                    dto.Count = r.Count();
                    return dto;
                })
            .OrderBy(b => b.Title)
            .ToList();

        return allBorrowedBooks;
    }

    /// <summary>
    /// Returns the top five customers with the most borrows within a specified date range.
    /// </summary>
    /// <param name="start">Start date of the period.</param>
    /// <param name="end">End date of the period.</param>
    public async Task<List<CustomerWithBorrowCountDto>> GetTopFiveCustomersAsync(DateOnly start, DateOnly end)
    {
        var records = await borrowRecordRepository.GetAllAsync();
        var customers = await customerRepository.GetAllAsync();

        var topFiveCustomersWithCount = records
            .Where(r => r.BorrowDate >= start && r.BorrowDate.AddDays(r.BorrowDuration) <= end)
            .GroupBy(r => r.CustomerId)
            .Select(x =>
            {
                var customer = customers.First(c => c.Id == x.Key);
                var dto = mapper.Map<CustomerWithBorrowCountDto>(customer);
                dto.Count = x.Count();
                return dto;
            })
            .OrderByDescending(x => x.Count)
            .ThenBy(x => x.Name)
            .Take(5)
            .ToList();

        return topFiveCustomersWithCount;
    }

    /// <summary>
    /// Returns customers who have borrowed books for the longest duration.
    /// </summary>
    public async Task<List<CustomerWithDurationDto>> GetCustomersWithLongestBorrowsAsync()
    {
        var records = await borrowRecordRepository.GetAllAsync();
        var customers = await customerRepository.GetAllAsync();

        var customersWithMaxDuration = records
            .GroupBy(r => r.CustomerId)
            .Select(x =>
            {
                var customer = customers.First(c => c.Id == x.Key);
                var dto = mapper.Map<CustomerWithDurationDto>(customer);
                dto.Duration = x.Max(r => r.BorrowDuration);
                return dto;
            })
            .ToList();

        if (customersWithMaxDuration.Count == 0)
            return [];

        var maxDuration = customersWithMaxDuration.Max(x => x.Duration);

        var topCustomers = customersWithMaxDuration
            .Where(x => x.Duration == maxDuration)
            .OrderBy(c => c.Name)
            .ToList();

        return topCustomers;
    }

    /// <summary>
    /// Returns the top five publishers with borrowed books count within a specified period.
    /// </summary>
    /// <param name="start">Start date of the period.</param>
    /// <param name="end">End date of the period.</param>
    public async Task<List<PublisherDto>> GetTopFivePublishersByDateAsync(DateOnly start, DateOnly end)
    {
        var records = await borrowRecordRepository.GetAllAsync();
        var books = await bookRepository.GetAllAsync();

        var topFivePublishers = records
            .Where(r => r.BorrowDate >= start && r.BorrowDate.AddDays(r.BorrowDuration) <= end)
            .Select(r => books.First(b => b.Id == r.BookId).Publisher)
            .GroupBy(p => p)
            .OrderByDescending(g => g.Count())
            .Take(5)
            .Select(g => new PublisherDto
            {
                Publisher = g.Key.ToString(),
                Count = g.Count()
            })
            .ToList();

        return topFivePublishers;
    }

    /// <summary>
    /// Returns the top five least popular books based on borrow count within the specified period.
    /// </summary>
    /// <param name="start">Start date of the period.</param>
    /// <param name="end">End date of the period.</param>
    public async Task<List<BookWithBorrowCountDto>> GetTopFiveLeastPopularBooksAsync(DateOnly start, DateOnly end)
    {
        var records = await borrowRecordRepository.GetAllAsync();
        var books = await bookRepository.GetAllAsync();

        var recordsInPeriod = records
            .Where(r => r.BorrowDate >= start && r.BorrowDate.AddDays(r.BorrowDuration) <= end)
            .ToList();

        var topFiveBooksWithCount = books
        .Select(b =>
        {
            var dto = mapper.Map<BookWithBorrowCountDto>(b);
            dto.Count = recordsInPeriod.Count(r => r.BookId == b.Id);
            return dto;
        })
        .OrderBy(x => x.Count)
        .ThenBy(x => x.Title)
        .Take(5)
        .ToList();

        return topFiveBooksWithCount;
    }
}
