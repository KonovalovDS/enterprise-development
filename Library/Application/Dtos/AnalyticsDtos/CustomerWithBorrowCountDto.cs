namespace Application.Dtos.AnalyticsDtos;

/// <summary>
/// Represents a dto for a customer with name and borrowed books count.
/// </summary>
public class CustomerWithBorrowCountDto
{
    /// <summary>
    /// Unique identifier of the customer.
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// Full name of the customer.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Borrowed books count.
    /// </summary>
    public required int Count { get; set; } = 0;
}
