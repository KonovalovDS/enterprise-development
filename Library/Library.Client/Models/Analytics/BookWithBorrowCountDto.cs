namespace Library.Client.Models.Analytics;

/// <summary>
/// Represents a dto for a book with borrows count.
/// </summary>
public class BookWithBorrowCountDto
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
    public required string Publisher { get; set; }

    /// <summary>
    /// Type of publishing.
    /// </summary>
    public required string PublishingType { get; set; }

    /// <summary>
    /// Year the book was published.
    /// </summary>
    public required int PublicationYear { get; set; }

    /// <summary>
    /// Borrowed books count.
    /// </summary>
    public required int Count { get; set; } = 0;
}
