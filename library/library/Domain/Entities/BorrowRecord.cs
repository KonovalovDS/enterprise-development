namespace library.Domain.Entities;

/// <summary>
/// Represents a record of a book borrowed by a customer.
/// </summary>
public class BorrowRecord 
{
    public int Id { get; set; }
    public required int BookId { get; set; }
    public required int CustomerId { get; set; }
    public required DateOnly BorrowDate { get; set; }
    public required int BorrowDuration { get; set; }
}
