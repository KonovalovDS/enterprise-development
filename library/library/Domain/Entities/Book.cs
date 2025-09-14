using library.Domain.Enums;

namespace library.Domain.Entities;

public class Book {
    public int Id { get; set; }
    public string? Code { get; set; }
    public string? Author { get; set; }
    public string? Name { get; set; }
    public Publisher Publisher { get; set; }
    public PublishingType PublishingType { get; set; }
    public int PublicationYear { get; set; }

    private Book() { }

    public Book(int id, string? code, string? author, string? name, int year, Publisher publisher, PublishingType publishingType) {
        Id = id;
        Author = author ?? throw new ArgumentNullException(nameof(author));
        Name = name ?? throw new ArgumentNullException(nameof(name));
        PublicationYear = year;
        Publisher = publisher;
        PublishingType = publishingType;
        Code = string.IsNullOrWhiteSpace(code) ? GenerateCode() : code;
    }

    public string GenerateCode() {
        if (string.IsNullOrWhiteSpace(Name)) 
            throw new InvalidOperationException("Book Name must be set before generating code.");
        return $"{char.ToUpper(Name[0])}{(int)Publisher}";
    }

    public void Update(string? name, string? author, int publicationYear, Publisher publisher, PublishingType pusblishingType) {
        Name = name;
        Author = author;
        PublicationYear = publicationYear;
        Publisher = publisher;
        PublishingType = pusblishingType;
    }
}
