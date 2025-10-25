namespace Library.Application.Contracts.BorrowRecordDtos;

/// <summary>
/// Represents a dto for a borrow record that needed to create or edit.
/// </summary>
public class BorrowRecordEditDto
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
}