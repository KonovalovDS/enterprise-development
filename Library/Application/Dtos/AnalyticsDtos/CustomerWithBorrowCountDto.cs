namespace Application.Dtos.AnalyticsDtos;

/// <summary>
/// Represents a dto for a customer with borrowed books count.
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
    /// Address of the customer.
    /// </summary>
    public required string Address { get; set; }

    /// <summary>
    /// Phone number of the customer.
    /// </summary>
    public required string PhoneNumber { get; set; }

    /// <summary>
    /// Date when the customer was registered.
    /// Defaults to the current date.
    /// </summary>
    public required DateOnly RegisterDate { get; set; }

    /// <summary>
    /// Borrowed books count.
    /// </summary>
    public required int Count { get; set; } = 0;
}
