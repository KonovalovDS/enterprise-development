using library.Domain.Entities;
using library.Domain.Enums;

namespace Tests;

/// <summary>
/// Provides test data for seeding the database.
/// </summary>
public static class TestDataSeeder
{
    /// <summary>
    /// A list of sample <see cref="Customer"/> objects for testing.
    /// </summary>
    public static List<Customer> CustomersTestData =>
    [
        new Customer { Id = 1, Name = "Ivanov Ivan Ivanovich", Address = "Samara, ul. Moskovskaya", PhoneNumber = "88005553535", RegisterDate = new DateOnly(2025, 1, 1)},
        new Customer { Id = 2, Name = "Volkov Alexander Yurevych", Address = "Balakovo, ul. Lenina", PhoneNumber = "89271244028", RegisterDate = new DateOnly(2025, 1, 1)},
        new Customer { Id = 3, Name = "Smirnova Anna Sergeevna", Address = "Samara, ul. Leningradskaya", PhoneNumber = "89272378492", RegisterDate = new DateOnly(2025, 1, 1)},
        new Customer { Id = 4, Name = "Kuznetsov Aleksey Vladimirovich", Address = "Samara, ul. Sovetskaya", PhoneNumber = "89278459475", RegisterDate = new DateOnly(2025, 1, 1)},
        new Customer { Id = 5, Name = "Popova Elena Viktorovna", Address = "Samara, ul. Kuybysheva", PhoneNumber = "89278451488", RegisterDate = new DateOnly(2025, 1, 1)},
        new Customer { Id = 6, Name = "Sokolov Dmitriy Andreevich", Address = "Samara, ul. Molodogvardeyskaya", PhoneNumber = "89347549450", RegisterDate = new DateOnly(2025, 1, 1)},
        new Customer { Id = 7, Name = "Lebedeva Mariya Alekseevna", Address = "Samara, ul. Oktyabrskaya", PhoneNumber = "89234858593", RegisterDate = new DateOnly(2025, 1, 1)},
        new Customer { Id = 8, Name = "Morozov Nikolay Petrovich", Address = "Samara, ul. Gagarina", PhoneNumber = "89245439587", RegisterDate = new DateOnly(2025, 1, 1)},
        new Customer { Id = 9, Name = "Petrov Artyom Aleksandrovich", Address = "Samara, ul. Krasnoarmeyskaya", PhoneNumber = "89349548395", RegisterDate = new DateOnly(2025, 1, 1) },
        new Customer { Id = 10, Name = "Volkova Ekaterina Mikhaylovna", Address = "Samara, ul. Pervomayskaya", PhoneNumber = "89235489935", RegisterDate = new DateOnly(2025, 1, 1)}
    ];

    /// <summary>
    /// A list of sample <see cref="Book"/> objects for testing.
    /// </summary>
    public static List<Book> BooksTestData =>
    [
        new Book { Id = 1, InventoryNumber = "A1B2C3", Code = "7R", Author = "J.K. Rowling", Title = "The Philosopher's Stone", PublicationYear = 1997, Publisher = Publisher.NewEraPublishing, PublishingType = PublishingType.Hardcover },
        new Book { Id = 2, InventoryNumber = "F6G7H8", Code = "9O", Author = "George Orwell", Title = "1984", PublicationYear = 1949, Publisher = Publisher.WhiteLine, PublishingType = PublishingType.Hardcover },
        new Book { Id = 3, InventoryNumber = "J1K2L3", Code = "3A", Author = "Jane Austen", Title = "Pride and Prejudice", PublicationYear = 1813, Publisher = Publisher.IntellectPublishing, PublishingType = PublishingType.Hardcover },
        new Book { Id = 4, InventoryNumber = "M4N5O6", Code = "5F", Author = "F. Scott Fitzgerald", Title = "The Great Gatsby", PublicationYear = 1925, Publisher = Publisher.TheBinding, PublishingType = PublishingType.Hardcover },
        new Book { Id = 5, InventoryNumber = "P7Q8R9", Code = "0L", Author = "Harper Lee", Title = "To Kill a Mockingbird", PublicationYear = 1960, Publisher = Publisher.GoldenPage, PublishingType = PublishingType.Hardcover },
        new Book { Id = 6, InventoryNumber = "S1T2U3", Code = "4T", Author = "Mark Twain", Title = "Adventures of Huckleberry Finn", PublicationYear = 1884, Publisher = Publisher.BeaconPress, PublishingType = PublishingType.Paperback },
        new Book { Id = 7, InventoryNumber = "V4W5X6", Code = "1D", Author = "Charles Dickens", Title = "Great Expectations", PublicationYear = 1861, Publisher = Publisher.EchoOfThought, PublishingType = PublishingType.Paperback },
        new Book { Id = 8, InventoryNumber = "Y7Z8A1", Code = "2H", Author = "Ernest Hemingway", Title = "The Old Man and the Sea", PublicationYear = 1952, Publisher = Publisher.WhiteLine, PublishingType = PublishingType.Paperback },
        new Book { Id = 9, InventoryNumber = "B2C3D4", Code = "8T", Author = "Leo Tolstoy", Title = "Anna Karenina", PublicationYear = 1878, Publisher = Publisher.TheBinding, PublishingType = PublishingType.Paperback },
        new Book { Id = 10, InventoryNumber = "E5F6G7", Code = "3B", Author = "Dan Brown", Title = "The Da Vinci Code", PublicationYear = 2003, Publisher = Publisher.WhiteLine, PublishingType = PublishingType.Ebook },
        new Book { Id = 11, InventoryNumber = "H8I9J0", Code = "8C", Author = "Suzanne Collins", Title = "The Hunger Games", PublicationYear = 2008, Publisher = Publisher.IntellectPublishing, PublishingType = PublishingType.Ebook },
        new Book { Id = 12, InventoryNumber = "K1L2M3", Code = "7T", Author = "J.R.R. Tolkien", Title = "The Hobbit", PublicationYear = 1937, Publisher = Publisher.TheBinding, PublishingType = PublishingType.Ebook },
        new Book { Id = 13, InventoryNumber = "N4O5P6", Code = "7K", Author = "Stephen King", Title = "The Shining", PublicationYear = 1977, Publisher = Publisher.EchoOfThought, PublishingType = PublishingType.Audiobook },
        new Book { Id = 14, InventoryNumber = "Q7R8S9", Code = "1G", Author = "Neil Gaiman", Title = "American Gods", PublicationYear = 2001, Publisher = Publisher.EchoOfThought, PublishingType = PublishingType.Audiobook },
        new Book { Id = 15, InventoryNumber = "T1U2V3", Code = "4C", Author = "Agatha Christie", Title = "Murder on the Orient Express", PublicationYear = 1934, Publisher = Publisher.TheBinding, PublishingType = PublishingType.Audiobook },
        new Book { Id = 16, InventoryNumber = "W4X5Y6", Code = "5A", Author = "Margaret Atwood", Title = "The Handmaid's Tale", PublicationYear = 1985, Publisher = Publisher.WhiteLine, PublishingType = PublishingType.Audiobook },
        new Book { Id = 17, InventoryNumber = "Z7A8B9", Code = "2H", Author = "Aldous Huxley", Title = "Brave New World", PublicationYear = 1932, Publisher = Publisher.TheBinding, PublishingType = PublishingType.LimitedEdition },
        new Book { Id = 18, InventoryNumber = "C1D2E3", Code = "4G", Author = "William Golding", Title = "Lord of the Flies", PublicationYear = 1954, Publisher = Publisher.EchoOfThought, PublishingType = PublishingType.LimitedEdition },
        new Book { Id = 19, InventoryNumber = "F4G5H6", Code = "3B", Author = "Ray Bradbury", Title = "Fahrenheit 451", PublicationYear = 1953, Publisher = Publisher.GoldenPage, PublishingType = PublishingType.LimitedEdition },
        new Book { Id = 20, InventoryNumber = "I7J8K9", Code = "5W", Author = "H.G. Wells", Title = "The Time Machine", PublicationYear = 1895, Publisher = Publisher.EchoOfThought, PublishingType = PublishingType.LimitedEdition }
    ];

    /// <summary>
    /// A list of sample <see cref="BorrowRecord"/> objects for testing.
    /// </summary>
    public static List<BorrowRecord> BorrowRecordsTestData =>
    [
        new BorrowRecord { Id = 1, BookId = 1, CustomerId = 3, BorrowDate = new DateOnly(2025, 4, 19), BorrowDuration = 12 },
        new BorrowRecord { Id = 2, BookId = 5, CustomerId = 4, BorrowDate = new DateOnly(2025, 5, 18), BorrowDuration = 29 },
        new BorrowRecord { Id = 3, BookId = 8, CustomerId = 2, BorrowDate = new DateOnly(2025, 3, 10), BorrowDuration = 30 },
        new BorrowRecord { Id = 4, BookId = 19, CustomerId = 8, BorrowDate = new DateOnly(2025, 7, 15), BorrowDuration = 6 },
        new BorrowRecord { Id = 5, BookId = 14, CustomerId = 7, BorrowDate = new DateOnly(2025, 5, 6), BorrowDuration = 7 },
        new BorrowRecord { Id = 6, BookId = 15, CustomerId = 3, BorrowDate = new DateOnly(2025, 4, 20), BorrowDuration = 27 },
        new BorrowRecord { Id = 7, BookId = 19, CustomerId = 9, BorrowDate = new DateOnly(2025, 6, 16), BorrowDuration = 7 },
        new BorrowRecord { Id = 8, BookId = 3, CustomerId = 4, BorrowDate = new DateOnly(2025, 2, 27), BorrowDuration = 10 },
        new BorrowRecord { Id = 9, BookId = 7, CustomerId = 4, BorrowDate = new DateOnly(2025, 5, 7), BorrowDuration = 15 },
        new BorrowRecord { Id = 10, BookId = 3, CustomerId = 9, BorrowDate = new DateOnly(2025, 5, 14), BorrowDuration = 30 },
        new BorrowRecord { Id = 11, BookId = 6, CustomerId = 2, BorrowDate = new DateOnly(2025, 8, 8), BorrowDuration = 19 },
        new BorrowRecord { Id = 12, BookId = 9, CustomerId = 10, BorrowDate = new DateOnly(2025, 9, 11), BorrowDuration = 8 },
        new BorrowRecord { Id = 13, BookId = 2, CustomerId = 10, BorrowDate = new DateOnly(2025, 3, 17), BorrowDuration = 13 },
        new BorrowRecord { Id = 14, BookId = 2, CustomerId = 7, BorrowDate = new DateOnly(2025, 4, 27), BorrowDuration = 8 },
        new BorrowRecord { Id = 15, BookId = 5, CustomerId = 3, BorrowDate = new DateOnly(2025, 4, 10), BorrowDuration = 20 },
        new BorrowRecord { Id = 16, BookId = 10, CustomerId = 9, BorrowDate = new DateOnly(2025, 7, 19), BorrowDuration = 16 },
        new BorrowRecord { Id = 17, BookId = 4, CustomerId = 1, BorrowDate = new DateOnly(2025, 5, 18), BorrowDuration = 20 },
        new BorrowRecord { Id = 18, BookId = 9, CustomerId = 2, BorrowDate = new DateOnly(2025, 1, 23), BorrowDuration = 25 },
        new BorrowRecord { Id = 19, BookId = 14, CustomerId = 2, BorrowDate = new DateOnly(2025, 7, 30), BorrowDuration = 30 },
        new BorrowRecord { Id = 20, BookId = 16, CustomerId = 4, BorrowDate = new DateOnly(2025, 8, 19), BorrowDuration = 6 }
    ];
}
