namespace Library.Application.Contracts.BorrowRecordDtos;

/// <summary>
/// Represents a dto for a borrow record in the library.
/// </summary>
public class BorrowRecordGetDto
{
    /// <summary>
    /// Unique identifier of the borrow record.
    /// </summary>
    public required int Id { get; set; }

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
}