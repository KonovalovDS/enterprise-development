namespace library.Api.DTOs;

public class BorrowRecordDto
{
    public int BookId { get; set; }
    public int CustomerId { get; set; }
    public DateOnly BorrowDate { get; set; }
    public int BorrowDuration { get; set; }

    public string BookName { get; set; }
    public string CustomerName { get; set; }
}
