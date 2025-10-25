namespace Library.Application.Contracts.CustomerDtos;

/// <summary>
/// Represents a dto for a customer.
/// </summary>
public class CustomerGetDto
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
}
