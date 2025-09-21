using library.Domain.Entities;

namespace Tests;

public class TestDataFixture 
{
    public List<Book> Books { get; } = TestDataSeeder.BooksTestData;
    public List<Customer> Customers { get; } = TestDataSeeder.CustomersTestData;
    public List<BorrowRecord> BorrowRecords { get; } = TestDataSeeder.BorrowRecordsTestData;
}