using library.Domain.Entities;
using library.Domain.Enums;
using static System.Reflection.Metadata.BlobBuilder;

namespace library.Infrastructure.Persistence;

public static class DbSeeder
{
    public static void Seed(AppDbContext context)
    {
        if (!context.Customers.Any())
        {
            var customers = new List<Customer>
            {
                new("Ivanov Ivan Ivanovich", "Samara, ul. Moskovskaya", "88005553535"),
                new("Volkov Alexander Yurevych", "Balakovo, ul. Lenina", "89271244028"),
                new("Smirnova Anna Sergeevna", "Samara, ul. Leningradskaya", "89272378492"),
                new("Kuznetsov Aleksey Vladimirovich", "Samara, ul. Sovetskaya", "89278459475"),
                new("Popova Elena Viktorovna", "Samara, ul. Kuybysheva", "89278451488"),
                new("Sokolov Dmitriy Andreevich", "Samara, ul. Molodogvardeyskaya", "89347549450"),
                new("Lebedeva Mariya Alekseevna", "Samara, ul. Oktyabrskaya", "89234858593"),
                new("Morozov Nikolay Petrovich", "Samara, ul. Gagarina", "89245439587"),
                new("Petrov Artyom Aleksandrovich", "Samara, ul. Krasnoarmeyskaya", "89349548395"),
                new("Volkova Ekaterina Mikhaylovna", "Samara, ul. Pervomayskaya", "89235489935")
            };
            context.Customers.AddRange(customers);
            context.SaveChanges();
        }

        if (!context.Books.Any())
        {
            var books = new List<Book>
            {
                new("", "J.K. Rowling", "The Philosopher's Stone", 1997, Publisher.NewEraPublishing, PublishingType.Hardcover),
                new("", "George Orwell", "1984", 1949, Publisher.WhiteLine, PublishingType.Hardcover),
                new("", "Jane Austen", "Pride and Prejudice", 1813, Publisher.IntellectPublishing, PublishingType.Hardcover),
                new("", "F. Scott Fitzgerald", "The Great Gatsby", 1925, Publisher.TheBinding, PublishingType.Hardcover),
                new("", "Harper Lee", "To Kill a Mockingbird", 1960, Publisher.GoldenPage, PublishingType.Hardcover),
                new("", "Mark Twain", "Adventures of Huckleberry Finn", 1884, Publisher.BeaconPress, PublishingType.Paperback),
                new("", "Charles Dickens", "Great Expectations", 1861, Publisher.EchoOfThought, PublishingType.Paperback),
                new("", "Ernest Hemingway", "The Old Man and the Sea", 1952, Publisher.WhiteLine, PublishingType.Paperback),
                new("", "Leo Tolstoy", "Anna Karenina", 1878, Publisher.TheBinding, PublishingType.Paperback),
                new("", "Dan Brown", "The Da Vinci Code", 2003, Publisher.WhiteLine, PublishingType.Ebook),
                new("", "Suzanne Collins", "The Hunger Games", 2008, Publisher.IntellectPublishing, PublishingType.Ebook),
                new("", "J.R.R. Tolkien", "The Hobbit", 1937, Publisher.TheBinding, PublishingType.Ebook),
                new("", "Stephen King", "The Shining", 1977, Publisher.EchoOfThought, PublishingType.Audiobook),
                new("", "Neil Gaiman", "American Gods", 2001, Publisher.EchoOfThought, PublishingType.Audiobook),
                new("", "Agatha Christie", "Murder on the Orient Express", 1934, Publisher.TheBinding, PublishingType.Audiobook),
                new("", "Margaret Atwood", "The Handmaid's Tale", 1985, Publisher.WhiteLine, PublishingType.Audiobook),
                new("", "Aldous Huxley", "Brave New World", 1932, Publisher.TheBinding, PublishingType.LimitedEdition),
                new("", "William Golding", "Lord of the Flies", 1954, Publisher.EchoOfThought, PublishingType.LimitedEdition),
                new("", "Ray Bradbury", "Fahrenheit 451", 1953, Publisher.GoldenPage, PublishingType.LimitedEdition),
                new("", "H.G. Wells", "The Time Machine", 1895, Publisher.EchoOfThought, PublishingType.LimitedEdition)
            };
            context.Books.AddRange(books);
            context.SaveChanges();
        }

        if (!context.BorrowRecords.Any())
        {
            var borrowRecords = new List<BorrowRecord>
            {
                new(1, 3, new DateOnly(2025, 4, 19), 12),
                new(5, 4, new DateOnly(2025, 5, 18), 29),
                new(8, 2, new DateOnly(2025, 3, 10), 21),
                new(19, 8, new DateOnly(2025, 7, 15), 6),
                new(14, 7, new DateOnly(2025, 5, 6), 7),
                new(15, 3, new DateOnly(2025, 4, 20), 27),
                new(19, 9, new DateOnly(2025, 6, 16), 7),
                new(3, 4, new DateOnly(2025, 2, 30), 10),
                new(7, 4, new DateOnly(2025, 5, 7), 15),
                new(3, 9, new DateOnly(2025, 5, 14), 20),
                new(6, 2, new DateOnly(2025, 8, 8), 19),
                new(9, 10, new DateOnly(2025, 9, 11), 8),
                new(2, 10, new DateOnly(2025, 3, 17), 13),
                new(2, 7, new DateOnly(2025, 4, 27), 8),
                new(5, 3, new DateOnly(2025, 4, 10), 20),
                new(10, 9, new DateOnly(2025, 7, 19), 16),
                new(4, 1, new DateOnly(2025, 5, 16), 15),
                new(9, 2, new DateOnly(2025, 1, 23), 25),
                new(14, 2, new DateOnly(2025, 7, 30), 30),
                new(16, 4, new DateOnly(2025, 8, 19), 6)
            };
            context.BorrowRecords.AddRange(borrowRecords);
            context.SaveChanges();
        }
    }
};
