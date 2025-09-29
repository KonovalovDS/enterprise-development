namespace Application.Dtos;

/// <summary>
/// Represents a dto for a customer with additional information.
/// </summary>
public class CustomerDto
{
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
    /// Additional information Customer borrowed books count.
    /// </summary>
    public int? BorrowCount { get; set; }
}