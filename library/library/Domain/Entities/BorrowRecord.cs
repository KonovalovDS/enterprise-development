namespace library.Domain.Entities;

public class BorrowRecord {
    public required int Id { get; set; }
    public required int BookId { get; set; }
    public required int CustomerId { get; set; }
    public required DateOnly LoanDate { get; set; }
    public required int LoanDuration { get; set; }

    private BorrowRecord() { }
    public BorrowRecord(int bookId, int customerId, DateOnly loanDate, int loanDuration) {
        if (loanDuration <= 0 || loanDuration > 31) 
            throw new ArgumentException("Loan duration must be positive number and less or equal than 31", nameof(loanDuration));
        BookId = bookId;
        CustomerId = customerId;
        LoanDate = loanDate;
        LoanDuration = loanDuration;
    }

    public void Update(int bookId, int customerId, DateOnly loanDate, int loanDuration) {
        if (loanDuration <= 0 || loanDuration > 31)
            throw new ArgumentException("Loan duration must be positive number and less or equal than 31", nameof(loanDuration));
        BookId = bookId;
        CustomerId = customerId;
        LoanDate = loanDate;
        LoanDuration = loanDuration;
    }
}
