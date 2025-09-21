using library.Domain.Enums;

namespace library.Domain.Entities;

/// <summary>
/// Represents a book entity with author, name, publisher, and publishing details.
/// </summary>
public class Book 
{
    public int Id { get; private set; }
    public string Code { get; private set; }
    public string Author { get; private set; }
    public string Name { get; private set; }
    public Publisher Publisher { get; private set; }
    public PublishingType PublishingType { get; private set; }
    public int PublicationYear { get; private set; }

    /// <summary>
    /// Private constructor for EF or serialization.
    /// </summary>
    private Book() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Book"/> class without an ID.
    /// </summary>
    /// <param name="code">Book code. If null or whitespace, it will be generated.</param>
    /// <param name="author">Author of the book. Cannot be null.</param>
    /// <param name="name">Title of the book. Cannot be null.</param>
    /// <param name="year">Publication year.</param>
    /// <param name="publisher">Publisher of the book.</param>
    /// <param name="publishingType">Type of publishing.</param>
    /// <exception cref="ArgumentNullException">Thrown if author or name is null.</exception>
    public Book(
        string code, 
        string author, 
        string name, 
        int year, 
        Publisher publisher, 
        PublishingType publishingType)
    {
        Author = author ?? throw new ArgumentNullException(nameof(author));
        Name = name ?? throw new ArgumentNullException(nameof(name));
        PublicationYear = year;
        Publisher = publisher;
        PublishingType = publishingType;
        Code = string.IsNullOrWhiteSpace(code) ? GenerateCode() : code;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Book"/> class with an ID.
    /// Used for generating in-memory data for unit-tests.
    /// </summary>
    /// <param name="id">Unique identifier of the book.</param>
    /// <param name="code">Book code. If null or whitespace, it will be generated.</param>
    /// <param name="author">Author of the book. Cannot be null.</param>
    /// <param name="name">Name (title) of the book. Cannot be null.</param>
    /// <param name="year">Publication year.</param>
    /// <param name="publisher">Publisher of the book.</param>
    /// <param name="publishingType">Type of publishing.</param>
    /// <exception cref="ArgumentNullException">Thrown if author or name is null.</exception>
    public Book(
        int id, 
        string? code, 
        string? author, 
        string? name, 
        int year, 
        Publisher publisher, 
        PublishingType publishingType) 
    {
        Id = id;
        Author = author ?? throw new ArgumentNullException(nameof(author));
        Name = name ?? throw new ArgumentNullException(nameof(name));
        PublicationYear = year;
        Publisher = publisher;
        PublishingType = publishingType;
        Code = string.IsNullOrWhiteSpace(code) ? GenerateCode() : code;
    }

    /// <summary>
    /// Example of code geenrator for book based on it's author and name.
    /// </summary>
    /// <returns>Generated code string.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the book name is not set.</exception>
    public string GenerateCode() 
    {
        if (string.IsNullOrWhiteSpace(Name)) 
            throw new InvalidOperationException("Book Name must be set before generating code.");
        return $"{char.ToUpper(Name[0])}{(int)Publisher}";
    }

    /// <summary>
    /// Updates the book's details and regenerates the code.
    /// </summary>
    /// <param name="name">New name (title) of the book.</param>
    /// <param name="author">New author of the book.</param>
    /// <param name="publicationYear">New publication year.</param>
    /// <param name="publisher">New publisher.</param>
    /// <param name="pusblishingType">New publishing type.</param>
    public void Update(
        string? name, 
        string? author, 
        int publicationYear, 
        Publisher publisher, 
        PublishingType pusblishingType) 
    {
        Name = name;
        Author = author;
        PublicationYear = publicationYear;
        Publisher = publisher;
        PublishingType = pusblishingType;
        Code = GenerateCode();
    }
}
