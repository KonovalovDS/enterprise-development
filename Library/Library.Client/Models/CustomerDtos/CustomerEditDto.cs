namespace Library.Client.Models.CustomerDtos;

/// <summary>
/// Represents a dto for a customer that needed to create or edit.
/// </summary>
public class CustomerEditDto
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
}
