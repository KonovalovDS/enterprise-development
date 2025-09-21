using library.Domain.Enums;

namespace Tests;

/// <summary>
/// Unit tests for domain layer.
/// </summary>  
public class DomainTests(TestDataFixture fixture) : IClassFixture<TestDataFixture>
{
    private readonly TestDataFixture _fixture = fixture;

    /// <summary>
    /// Checks that all borrowed books are sorted by name.
    /// </summary>
    [Fact]
    public void AllBorrowedBooksSorted()
    {
        var currentDate = new DateOnly(2025, 6, 1);
        var expectedCount = 3;

        var allBorrowedBooks = _fixture.BorrowRecords
            .Where(r => r.BorrowDate <= currentDate && r.BorrowDate.AddDays(r.BorrowDuration) >= currentDate)
            .Select(r => _fixture.Books.First(b => b.Id == r.BookId))
            .Distinct()
            .OrderBy(b => b.Name)
            .ToList();

        Assert.Equal(allBorrowedBooks.Count, expectedCount);
    }

    /// <summary>
    /// Verifies top five customers by borrow count in a period.
    /// </summary>
    [Fact]
    public void TopFiveCustomersInfo()
    {
        var periodStart = new DateOnly(2025, 1, 1);
        var periodEnd = new DateOnly(2025, 6, 30);
        var expectedCount = 5;
        var expectedNames = new List<string> 
        {
            "Smirnova Anna Sergeevna",
            "Kuznetsov Aleksey Vladimirovich",
            "Volkov Alexander Yurevych",
            "Lebedeva Mariya Alekseevna",
            "Petrov Artyom Aleksandrovich"
        };

        var topFiveCustomers = _fixture.BorrowRecords
            .Where(r => r.BorrowDate >= periodStart && r.BorrowDate.AddDays(r.BorrowDuration) <= periodEnd)
            .GroupBy(r => r.CustomerId)
            .OrderByDescending(x => x.Count())
            .Take(5)
            .Select(g => _fixture.Customers.First(c => c.Id == g.Key))
            .ToList();

        var topFiveCustomerNames = topFiveCustomers.Select(c => c.Name).ToList();

        Assert.Equal(expectedCount, topFiveCustomers.Count);
        Assert.Equal(expectedNames, topFiveCustomerNames!);
    }

    /// <summary>
    /// Ensures customers with the longest borrows are correctly identified and sorted.
    /// </summary>
    [Fact]
    public void LongestBorrowsSorted()
    {
        var expectedMaxDuration = 30;
        var expectedNames = new List<string> 
        {
            "Petrov Artyom Aleksandrovich",
            "Volkov Alexander Yurevych"
        };

        var customersWithMaxDuration = _fixture.BorrowRecords
            .GroupBy(r => r.CustomerId)
            .Select(x => new
            {
                Customer = _fixture.Customers.First(c => c.Id == x.Key),
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

        var topCustomerNames = topCustomers.Select(c => c.Name).ToList();

        Assert.Equal(expectedMaxDuration, maxDuration);
        Assert.Equal(expectedNames, topCustomerNames!);
    }

    /// <summary>
    /// Checks top five publishers by borrow count in the last year.
    /// </summary>
    [Fact]
    public void TopFivePublishersLastYear()
    {
        var periodEnd = new DateOnly(2025, 9, 1);
        var periodStart = periodEnd.AddYears(-1);
        var expectedCount = 5;
        var expectedPublishers = new List<Publisher>
        {
            Publisher.WhiteLine,
            Publisher.GoldenPage,
            Publisher.EchoOfThought,
            Publisher.TheBinding,
            Publisher.IntellectPublishing
        };

        var topPublishers = _fixture.BorrowRecords
            .Where(r => r.BorrowDate >= periodStart && r.BorrowDate.AddDays(r.BorrowDuration) <= periodEnd)
            .Select(r => _fixture.Books.FirstOrDefault(b => b.Id == r.BookId)?.Publisher)
            .GroupBy(p => p)
            .Select(x => new
            {
                Publisher = x.Key,
                Count = x.Count()
            })
            .OrderByDescending(x => x.Count)
            .Take(5)
            .ToList();

        Assert.Equal(topPublishers.Count, expectedCount);
        Assert.Equal(expectedPublishers, topPublishers.Select(p => (Publisher)p.Publisher!));
    }

    /// <summary>
    /// Verifies top five least popular books by borrow count.
    /// </summary>
    [Fact]
    public void TopFiveLeastPopularBooks()
    {
        var expectedCount = 5;
        var expectedNames = new List<string>
        {
            "Brave New World", 
            "Lord of the Flies", 
            "The Hobbit", 
            "The Hunger Games", 
            "The Shining"
        };

        var bookCounts = _fixture.Books
            .Select(b => new
            {
                Book = b,
                BorrowCount = _fixture.BorrowRecords.Count(r => r.BookId == b.Id)
            })
            .OrderBy(x => x.BorrowCount)
            .ThenBy(x => x.Book.Name)
            .Take(5)
            .ToList();

        var leastPopularBookNames = bookCounts.Select(x => x.Book.Name).ToList();

        Assert.Equal(expectedCount, bookCounts.Count);
        Assert.Equal(expectedNames, leastPopularBookNames!);
    }
};