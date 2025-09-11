namespace library.Domain.Entities;

public class BorrowRecord {
    public int Id { get; set; }
    public int BookId { get; set; }
    public int CustomerId { get; set; }
    public DateOnly BorrowDate { get; set; }
    public int BorrowDuration { get; set; }

    private BorrowRecord() { }
    public BorrowRecord(int bookId, int customerId, DateOnly borrowDate, int borrowDuration) {
        if (borrowDuration <= 0 || borrowDuration > 31) 
            throw new ArgumentException("Loan duration must be positive number and less or equal than 31", nameof(borrowDuration));
        BookId = bookId;
        CustomerId = customerId;
        BorrowDate = borrowDate;
        BorrowDuration = borrowDuration;
    }

    public void Update(int bookId, int customerId, DateOnly borrowDate, int borrowDuration) {
        if (borrowDuration <= 0 || borrowDuration > 31)
            throw new ArgumentException("Loan duration must be positive number and less or equal than 31", nameof(borrowDuration));
        BookId = bookId;
        CustomerId = customerId;
        BorrowDate = borrowDate;
        BorrowDuration = borrowDuration;
    }
}
