namespace Application.Dtos;

/// <summary>
/// Represents a dto for a record of a book borrowed by a customer with additional information.
/// </summary>
public class BorrowRecordDto
{
    /// <summary>
    /// Identifier of the borrowed book.
    /// </summary>
    public required int BookId { get; set; }

    /// <summary>
    /// Identifier of the customer who borrowed the book.
    /// </summary>
    public required int CustomerId { get; set; }

    /// <summary>
    /// Date when the book was borrowed.
    /// </summary>
    public required DateOnly BorrowDate { get; set; }

    /// <summary>
    /// Duration of the borrow in days.
    /// </summary>
    public required int BorrowDuration { get; set; }

    /// <summary>
    /// Additional information with Book Title.
    /// </summary>
    public string? BookTitle { get; set; }

    /// <summary>
    /// Additional information with Customer Name.
    /// </summary>
    public string? CustomerName { get; set; }
}