using library.Domain.Entities;

namespace Tests;

/// <summary>
/// Test data fixture for the library.
/// Provides collections of books, customers and borrow records.
/// </summary>
public class TestDataFixture 
{
    /// <summary>
    /// List of books for testing.
    /// </summary>
    public List<Book> Books { get; } = TestDataSeeder.BooksTestData;

    /// <summary>
    /// List of customers for testing.
    /// </summary>
    public List<Customer> Customers { get; } = TestDataSeeder.CustomersTestData;

    /// <summary>
    /// List of borrow records for testing.
    /// </summary>
    public List<BorrowRecord> BorrowRecords { get; } = TestDataSeeder.BorrowRecordsTestData;
}