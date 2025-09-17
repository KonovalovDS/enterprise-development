namespace library.Domain.Entities;

/// <summary>
/// Represents a record of a book borrowed by a customer.
/// </summary>
public class BorrowRecord 
{
    public int Id { get; private set; }
    public int BookId { get; private set; }
    public int CustomerId { get; private set; }
    public DateOnly BorrowDate { get; private set; }
    public int BorrowDuration { get; private set; }

    /// <summary>
    /// Private constructor for EF or serialization.
    /// </summary>
    private BorrowRecord() { }

    /// <summary>
    /// Initializes a new borrow record without an ID.
    /// </summary>
    /// <param name="bookId">Identifier of the borrowed book.</param>
    /// <param name="customerId">Identifier of the customer.</param>
    /// <param name="borrowDuration">Borrow duration in days (1-31).</param>
    /// <param name="borrowDate">Optional borrow date. Defaults to current date if null.</param>
    /// <exception cref="ArgumentException">Thrown if borrowDuration is not between 1 and 31.</exception>
    public BorrowRecord(
        int bookId, 
        int customerId, 
        int borrowDuration, 
        DateOnly? borrowDate = null)
    {
        if (borrowDuration <= 0 || borrowDuration > 31)
            throw new ArgumentException("Borrow duration must be positive number and less or equal than 31", nameof(borrowDuration));
        BookId = bookId;
        CustomerId = customerId;
        BorrowDuration = borrowDuration;
        BorrowDate = borrowDate ?? DateOnly.FromDateTime(DateTime.Now);
    }

    /// <summary>
    /// Initializes a new borrow record with an ID.
    /// Used for generating in-memory data for unit-tests.
    /// </summary>
    /// <param name="id">Unique identifier of the borrow record.</param>
    /// <param name="bookId">Identifier of the borrowed book.</param>
    /// <param name="customerId">Identifier of the customer.</param>
    /// <param name="borrowDate">Borrow date.</param>
    /// <param name="borrowDuration">Borrow duration in days (1-31).</param>
    /// <exception cref="ArgumentException">Thrown if borrowDuration is not between 1 and 31.</exception>
    public BorrowRecord(
        int id, 
        int bookId, 
        int customerId, 
        DateOnly borrowDate, 
        int borrowDuration) 
    {
        if (borrowDuration <= 0 || borrowDuration > 31) 
            throw new ArgumentException("Borrow duration must be positive number and less or equal than 31", nameof(borrowDuration));
        Id = id;
        BookId = bookId;
        CustomerId = customerId;
        BorrowDate = borrowDate;
        BorrowDuration = borrowDuration;
    }

    /// <summary>
    /// Updates the borrow record details.
    /// </summary>
    /// <param name="bookId">Identifier of the borrowed book.</param>
    /// <param name="customerId">Identifier of the customer.</param>
    /// <param name="borrowDuration">Borrow duration in days (1-31).</param>
    /// <param name="borrowDate">Optional borrow date. Defaults to current date if null.</param>
    /// <exception cref="ArgumentException">Thrown if borrowDuration is not between 1 and 31.</exception>
    public void Update(
        int bookId, 
        int customerId, 
        int borrowDuration, 
        DateOnly? borrowDate = null) 
    {
        if (borrowDuration <= 0 || borrowDuration > 31)
            throw new ArgumentException("Borrow duration must be positive number and less or equal than 31", nameof(borrowDuration));
        BookId = bookId;
        CustomerId = customerId;
        BorrowDuration = borrowDuration;
        BorrowDate = borrowDate ?? DateOnly.FromDateTime(DateTime.Now);
    }
}
