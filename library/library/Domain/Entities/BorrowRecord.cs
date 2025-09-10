namespace library.Domain.Entities;

public class BorrowRecord {
    public required int Id { get; set; }
    public required int BookId { get; set; }
    public required int CustomerId { get; set; }
    public required DateOnly BorrowDate { get; set; }
    public required int BorrowDuration { get; set; }

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
