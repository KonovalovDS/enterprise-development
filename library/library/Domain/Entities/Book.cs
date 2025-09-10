using library.Domain.Enums;

namespace library.Domain.Entities;

public class Book {
    public required int Id { get; set; }
    public required string Code { get; set; }
    public required string Author { get; set; }
    public required string Name { get; set; }
    public required Publisher Publisher { get; set; }
    public required PublishingType PublisherType { get; set; }
    public required int PublicationYear { get; set; }

    private Book() { }

    public Book(string code, string author, string name, int year, Publisher publisher, PublishingType publisherType) {
        Author = author ?? throw new ArgumentNullException(nameof(author));
        Name = name ?? throw new ArgumentNullException(nameof(name));
        PublicationYear = year;
        Publisher = publisher;
        PublisherType = publisherType;
        Code = string.IsNullOrWhiteSpace(code) ? GenerateCode() : code;
    }

    public string GenerateCode() {
        if (string.IsNullOrWhiteSpace(Name)) 
            throw new InvalidOperationException("Book Name must be set before generating code.");
        return $"{char.ToUpper(Name[0])}{(int)Publisher}";
    }

    public void Update(string name, string author, int publicationYear, Publisher publisher, PublishingType pusblisherType) {
        Name = name;
        Author = author;
        PublicationYear = publicationYear;
        Publisher = publisher;
        PublisherType = pusblisherType;
    }
}
