using library.Domain.Entities;
using library.Domain.Enums;
using System.Linq;
using Xunit;

namespace library.Tests;

public class DomainTests
{
    private readonly List<Book> _books = new();
    private readonly List<Customer> _customers = new();
    private readonly List<BorrowRecord> _borrowRecords = new();

    public DomainTests()
    {
        _books = TestDataSeeder.GetBooksTestData();
        _customers = TestDataSeeder.GetCustomersTestData();
        _borrowRecords = TestDataSeeder.GetBorrowRecordsTestData();
    }

    [Fact]
    public void AllBorrowedBooksSorted()
    {
        var allBorrowedBooks = _borrowRecords
            .Select(r => _books.FirstOrDefault(b => b.Id == r.BookId))
            .Where(b => b != null)
            .Distinct()
            .OrderBy(b => b.Name)
            .ToList();

        Assert.NotEmpty(allBorrowedBooks);
        Assert.True(allBorrowedBooks.Count == 14);
        for (var i = 1; i < allBorrowedBooks.Count; i++)
        {
            Assert.True(string.Compare(allBorrowedBooks[i - 1].Name, allBorrowedBooks[i].Name, StringComparison.Ordinal) <= 0);
        }
    }

    [Fact]
    public void TopFiveCustomersInfo()
    {
        var periodStart = new DateOnly(2025, 1, 1);
        var periodEnd = new DateOnly(2025, 6, 30);
        var expectedNames = new List<string> {
            "Smirnova Anna Sergeevna",
            "Kuznetsov Aleksey Vladimirovich",
            "Volkov Alexander Yurevych",
            "Lebedeva Mariya Alekseevna",
            "Petrov Artyom Aleksandrovich"
        };

        var topFiveCustomers = _borrowRecords
            .Where(r => r.BorrowDate >= periodStart && r.BorrowDate.AddDays(r.BorrowDuration) <= periodEnd)
            .GroupBy(r => r.CustomerId)
            .OrderByDescending(x => x.Count())
            .Take(5)
            .Select(g => _customers.FirstOrDefault(c => c.Id == g.Key))
            .ToList();

        Assert.NotEmpty(topFiveCustomers);
        Assert.Equal(5, topFiveCustomers.Count);
        Assert.Equal(expectedNames, topFiveCustomers.Select(c => c.Name).ToList());
    }

    [Fact]
    public void LongestBorrowsSorted()
    {
        var expectedMaxDuration = 30;
        var expectedNames = new List<string> {
            "Petrov Artyom Aleksandrovich",
            "Volkov Alexander Yurevych"
        };

        var customersWithMaxDuration = _borrowRecords
            .GroupBy(r => r.CustomerId)
            .Select(x => new
            {
                Customer = _customers.FirstOrDefault(c => c.Id == x.Key),
                MaxDuration = x.Max(r => r.BorrowDuration)
            })
            .Where(x => x.Customer != null)
            .OrderByDescending(x => x.MaxDuration)
            .ToList();

        var maxDurationEntry = customersWithMaxDuration.FirstOrDefault();
        Assert.NotNull(maxDurationEntry);
        var maxDuration = maxDurationEntry.MaxDuration;

        var topCustomers = customersWithMaxDuration
            .Where(x => x.MaxDuration == maxDuration)
            .Select(x => x.Customer)
            .OrderBy(c => c.Name)
            .ToList();

        var topCustomerNames = topCustomers.Select(c => c.Name).ToList();

        Assert.Equal(expectedMaxDuration, maxDuration);
        Assert.Equal(expectedNames, topCustomerNames);
    }

    [Fact]
    public void TopFivePublishersLastYear()
    {
        var periodEnd = DateOnly.FromDateTime(DateTime.Today);
        var periodStart = periodEnd.AddYears(-1);
        var expectedPublishers = new List<Publisher>
        {
            Publisher.WhiteLine,
            Publisher.GoldenPage,
            Publisher.EchoOfThought,
            Publisher.TheBinding,
            Publisher.IntellectPublishing
        };

        var topPublishers = _borrowRecords
            .Where(r => r.BorrowDate >= periodStart && r.BorrowDate.AddDays(r.BorrowDuration) <= periodEnd)
            .Select(r => _books.FirstOrDefault(b => b.Id == r.BookId)?.Publisher)
            .GroupBy(p => p)
            .Select(x => new
            {
                Publisher = x.Key,
                Count = x.Count()
            })
            .OrderByDescending(x => x.Count)
            .Take(5)
            .ToList();

        Assert.NotEmpty(topPublishers);
        Assert.True(topPublishers.Count == 5);
        foreach (var item in topPublishers)
        {
            Assert.True(expectedPublishers.Contains((Publisher)item.Publisher));
        }
    }

    [Fact]
    public void TopFiveLeastPopularBooks()
    {
        var expectedNames = new List<string>
        {
            "Brave New World", 
            "Lord of the Flies", 
            "The Hobbit", 
            "The Hunger Games", 
            "The Shining"
        };

        var bookCounts = _books
            .Select(b => new
            {
                Book = b,
                BorrowCount = _borrowRecords.Count(r => r.BookId == b.Id)
            })
            .OrderBy(x => x.BorrowCount)
            .ThenBy(x => x.Book.Name)
            .Take(5)
            .ToList();

        var leastPopularBookNames = bookCounts.Select(x => x.Book.Name).ToList();

        Assert.NotEmpty(bookCounts);
        Assert.Equal(5, bookCounts.Count);
        Assert.Equal(expectedNames, leastPopularBookNames);
    }
};
