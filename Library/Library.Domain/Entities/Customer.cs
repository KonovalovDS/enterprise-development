namespace Library.Domain.Entities;

/// <summary>
/// Represents a customer with personal details and registration date.
/// </summary>
public class Customer
{
    /// <summary>
    /// Unique identifier of the customer.
    /// </summary>
    public int Id { get; set; }

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
    public required DateOnly RegisterDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);
}
