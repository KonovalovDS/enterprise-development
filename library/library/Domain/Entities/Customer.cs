namespace library.Domain.Entities;

/// <summary>
/// Represents a customer with personal details and registration date.
/// </summary>
public class Customer 
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Address { get; set; }
    public required string PhoneNumber { get; set; }
    public required DateOnly RegisterDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);
}
