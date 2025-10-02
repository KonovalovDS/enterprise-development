namespace Application.Dtos.AnalyticsDtos;

/// <summary>
/// Represents a dto for a book with title, author and borrow count.
/// </summary>
public class BookWithBorrowCountDto
{
    /// <summary>
    /// Unique identifier of the book.
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// Title of the book.
    /// </summary>
    public required string Title { get; set; }

    /// <summary>
    /// Author of the book.
    /// </summary>
    public required string Author { get; set; }

    /// <summary>
    /// Borrowed books count.
    /// </summary>
    public required int Count { get; set; } = 0;
}
