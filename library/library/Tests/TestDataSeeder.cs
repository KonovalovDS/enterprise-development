using library.Domain.Entities;
using library.Domain.Enums;

namespace library.Tests;

/// <summary>
/// Provides test data for seeding the database.
/// </summary>
public static class TestDataSeeder
{
    /// <summary>
    /// Returns a list of sample <see cref="Customer"/> objects for testing.
    /// </summary>
    /// <returns>List of <see cref="Customer"/> instances.</returns>
    public static List<Customer> GetCustomersTestData()
    {
        return new List<Customer>
        {
            new(1, "Ivanov Ivan Ivanovich", "Samara, ul. Moskovskaya", "88005553535"),
            new(2, "Volkov Alexander Yurevych", "Balakovo, ul. Lenina", "89271244028"),
            new(3, "Smirnova Anna Sergeevna", "Samara, ul. Leningradskaya", "89272378492"),
            new(4, "Kuznetsov Aleksey Vladimirovich", "Samara, ul. Sovetskaya", "89278459475"),
            new(5, "Popova Elena Viktorovna", "Samara, ul. Kuybysheva", "89278451488"),
            new(6, "Sokolov Dmitriy Andreevich", "Samara, ul. Molodogvardeyskaya", "89347549450"),
            new(7, "Lebedeva Mariya Alekseevna", "Samara, ul. Oktyabrskaya", "89234858593"),
            new(8, "Morozov Nikolay Petrovich", "Samara, ul. Gagarina", "89245439587"),
            new(9, "Petrov Artyom Aleksandrovich", "Samara, ul. Krasnoarmeyskaya", "89349548395"),
            new(10, "Volkova Ekaterina Mikhaylovna", "Samara, ul. Pervomayskaya", "89235489935")
        };
    }

    /// <summary>
    /// Returns a list of sample <see cref="Book"/> objects for testing.
    /// </summary>
    /// <returns>List of <see cref="Book"/> instances.</returns>
    public static List<Book> GetBooksTestData()
    {
        return new List<Book>
        {
            new(1, "", "J.K. Rowling", "The Philosopher's Stone", 1997, Publisher.NewEraPublishing, PublishingType.Hardcover),
            new(2, "", "George Orwell", "1984", 1949, Publisher.WhiteLine, PublishingType.Hardcover),
            new(3, "", "Jane Austen", "Pride and Prejudice", 1813, Publisher.IntellectPublishing, PublishingType.Hardcover),
            new(4, "", "F. Scott Fitzgerald", "The Great Gatsby", 1925, Publisher.TheBinding, PublishingType.Hardcover),
            new(5, "", "Harper Lee", "To Kill a Mockingbird", 1960, Publisher.GoldenPage, PublishingType.Hardcover),
            new(6, "", "Mark Twain", "Adventures of Huckleberry Finn", 1884, Publisher.BeaconPress, PublishingType.Paperback),
            new(7, "", "Charles Dickens", "Great Expectations", 1861, Publisher.EchoOfThought, PublishingType.Paperback),
            new(8, "", "Ernest Hemingway", "The Old Man and the Sea", 1952, Publisher.WhiteLine, PublishingType.Paperback),
            new(9, "", "Leo Tolstoy", "Anna Karenina", 1878, Publisher.TheBinding, PublishingType.Paperback),
            new(10, "", "Dan Brown", "The Da Vinci Code", 2003, Publisher.WhiteLine, PublishingType.Ebook),
            new(11, "", "Suzanne Collins", "The Hunger Games", 2008, Publisher.IntellectPublishing, PublishingType.Ebook),
            new(12, "", "J.R.R. Tolkien", "The Hobbit", 1937, Publisher.TheBinding, PublishingType.Ebook),
            new(13, "", "Stephen King", "The Shining", 1977, Publisher.EchoOfThought, PublishingType.Audiobook),
            new(14, "", "Neil Gaiman", "American Gods", 2001, Publisher.EchoOfThought, PublishingType.Audiobook),
            new(15, "", "Agatha Christie", "Murder on the Orient Express", 1934, Publisher.TheBinding, PublishingType.Audiobook),
            new(16, "", "Margaret Atwood", "The Handmaid's Tale", 1985, Publisher.WhiteLine, PublishingType.Audiobook),
            new(17, "", "Aldous Huxley", "Brave New World", 1932, Publisher.TheBinding, PublishingType.LimitedEdition),
            new(18, "", "William Golding", "Lord of the Flies", 1954, Publisher.EchoOfThought, PublishingType.LimitedEdition),
            new(19, "", "Ray Bradbury", "Fahrenheit 451", 1953, Publisher.GoldenPage, PublishingType.LimitedEdition),
            new(20, "", "H.G. Wells", "The Time Machine", 1895, Publisher.EchoOfThought, PublishingType.LimitedEdition)
        };
    }

    /// <summary>
    /// Returns a list of sample <see cref="BorrowRecord"/> objects for testing.
    /// </summary>
    /// <returns>List of <see cref="BorrowRecord"/> instances.</returns>
    public static List<BorrowRecord> GetBorrowRecordsTestData()
    {
        return new List<BorrowRecord>
        {
            new(1, 1, 3, new DateOnly(2025, 4, 19), 12),
            new(2, 5, 4, new DateOnly(2025, 5, 18), 29),
            new(3, 8, 2, new DateOnly(2025, 3, 10), 30),
            new(4, 19, 8, new DateOnly(2025, 7, 15), 6),
            new(5, 14, 7, new DateOnly(2025, 5, 6), 7),
            new(6, 15, 3, new DateOnly(2025, 4, 20), 27),
            new(7, 19, 9, new DateOnly(2025, 6, 16), 7),
            new(8, 3, 4, new DateOnly(2025, 2, 27), 10),
            new(9, 7, 4, new DateOnly(2025, 5, 7), 15),
            new(10, 3, 9, new DateOnly(2025, 5, 14), 30),
            new(11, 6, 2, new DateOnly(2025, 8, 8), 19),
            new(12, 9, 10, new DateOnly(2025, 9, 11), 8),
            new(13, 2, 10, new DateOnly(2025, 3, 17), 13),
            new(14, 2, 7, new DateOnly(2025, 4, 27), 8),
            new(15, 5, 3, new DateOnly(2025, 4, 10), 20),
            new(16, 10, 9, new DateOnly(2025, 7, 19), 16),
            new(17, 4, 1, new DateOnly(2025, 5, 16), 15),
            new(18, 9, 2, new DateOnly(2025, 1, 23), 25),
            new(19, 14, 2, new DateOnly(2025, 7, 30), 30),
            new(20, 16, 4, new DateOnly(2025, 8, 19), 6)
        };
    }
}
