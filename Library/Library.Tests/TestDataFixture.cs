using Library.Domain.DataSeeders;
using Library.Domain.Entities;

namespace Library.Tests;

/// <summary>
/// Test data fixture for the library.
/// Provides collections of books, customers and borrow records.
/// </summary>
public class TestDataFixture
{
    /// <summary>
    /// List of books for testing.
    /// </summary>
    public List<Book> Books { get; } = DataSeeder.BooksTestData;

    /// <summary>
    /// List of customers for testing.
    /// </summary>
    public List<Customer> Customers { get; } = DataSeeder.CustomersTestData;

    /// <summary>
    /// List of borrow records for testing.
    /// </summary>
    public List<BorrowRecord> BorrowRecords { get; } = DataSeeder.BorrowRecordsTestData;
}