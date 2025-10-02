using Domain.Enums;

/// <summary>
/// Represents a dto for a book in the library.
/// </summary>
public class BookGetDto
{
    /// <summary>
    /// Unique identifier of the book.
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// Inventory number of the book.
    /// </summary>
    public required string InventoryNumber { get; set; }

    /// <summary>
    /// Short code representing the book.
    /// </summary>
    public required string Code { get; set; }

    /// <summary>
    /// Author of the book.
    /// </summary>
    public required string Author { get; set; }

    /// <summary>
    /// Title of the book.
    /// </summary>
    public required string Title { get; set; }

    /// <summary>
    /// Publisher of the book.
    /// </summary>
    public required Publisher Publisher { get; set; }

    /// <summary>
    /// Type of publishing.
    /// </summary>
    public required PublishingType PublishingType { get; set; }

    /// <summary>
    /// Year the book was published.
    /// </summary>
    public required int PublicationYear { get; set; }
}
