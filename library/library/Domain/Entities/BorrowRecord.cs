namespace library.Domain.Entities;

public class BorrowRecord 
{
    public int Id { get; private set; }
    public int BookId { get; private set; }
    public int CustomerId { get; private set; }
    public DateOnly BorrowDate { get; private set; }
    public int BorrowDuration { get; private set; }

    private BorrowRecord() { }

    public BorrowRecord(int bookId, int customerId, int borrowDuration, DateOnly? borrowDate = null)
    {
        if (borrowDuration <= 0 || borrowDuration > 31)
            throw new ArgumentException("Borrow duration must be positive number and less or equal than 31", nameof(borrowDuration));
        BookId = bookId;
        CustomerId = customerId;
        BorrowDuration = borrowDuration;
        BorrowDate = borrowDate ?? DateOnly.FromDateTime(DateTime.Now);
    }

    public BorrowRecord(int id, int bookId, int customerId, DateOnly borrowDate, int borrowDuration) 
    {
        if (borrowDuration <= 0 || borrowDuration > 31) 
            throw new ArgumentException("Borrow duration must be positive number and less or equal than 31", nameof(borrowDuration));
        Id = id;
        BookId = bookId;
        CustomerId = customerId;
        BorrowDate = borrowDate;
        BorrowDuration = borrowDuration;
    }

    public void Update(int bookId, int customerId, int borrowDuration, DateOnly? borrowDate = null) {
        if (borrowDuration <= 0 || borrowDuration > 31)
            throw new ArgumentException("Borrow duration must be positive number and less or equal than 31", nameof(borrowDuration));
        BookId = bookId;
        CustomerId = customerId;
        BorrowDuration = borrowDuration;
        BorrowDate = borrowDate ?? DateOnly.FromDateTime(DateTime.Now);
    }
}
