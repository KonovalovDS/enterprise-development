namespace library.Domain.Entities;

/// <summary>
/// Represents a customer with personal details and registration date.
/// </summary>
public class Customer 
{
    public int Id { get; private set; }
    public string? Name { get; private set; }
    public string? Address { get; private set; }
    public string? PhoneNumber { get; private set; }
    public DateOnly RegisterDate { get; init; } = DateOnly.FromDateTime(DateTime.Today);

    /// <summary>
    /// Private constructor for EF or serialization.
    /// </summary>
    private Customer() { }

    /// <summary>
    /// Initializes a new customer without an ID.
    /// </summary>
    /// <param name="name">Customer name. Cannot be empty.</param>
    /// <param name="address">Customer address. Must be at least 10 characters if provided.</param>
    /// <param name="phoneNumber">Customer phone number. Must be 11 digits if provided.</param>
    /// <exception cref="ArgumentException">Thrown if validation fails for name, address, or phone number.</exception>
    public Customer(
        string? name, 
        string? address, 
        string? phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));
        if (!string.IsNullOrWhiteSpace(address) && address.Length < 10)
            throw new ArgumentException("Address is too short or empty", nameof(address));
        if (!string.IsNullOrWhiteSpace(phoneNumber) && !System.Text.RegularExpressions.Regex.IsMatch(phoneNumber, @"^\d{11}$"))
            throw new ArgumentException("Phone number must be 11 digits long", nameof(phoneNumber));
        Name = name;
        Address = address;
        PhoneNumber = phoneNumber;
    }

    /// <summary>
    /// Initializes a new customer with an ID.
    /// Used for generating in-memory data for unit-tests.
    /// </summary>
    /// <param name="id">Unique identifier of the customer.</param>
    /// <param name="name">Customer name. Cannot be empty.</param>
    /// <param name="address">Customer address. Must be at least 10 characters if provided.</param>
    /// <param name="phoneNumber">Customer phone number. Must be 11 digits if provided.</param>
    /// <exception cref="ArgumentException">Thrown if validation fails for name, address, or phone number.</exception>
    public Customer(
        int id, 
        string? name, 
        string? address, 
        string? phoneNumber) 
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));
        if (!string.IsNullOrWhiteSpace(address) && address.Length < 10)
            throw new ArgumentException("Address is too short or empty", nameof(address));
        if (!string.IsNullOrWhiteSpace(phoneNumber) && !System.Text.RegularExpressions.Regex.IsMatch(phoneNumber, @"^\d{11}$"))
            throw new ArgumentException("Phone number must be 11 digits long", nameof(phoneNumber));
        Id = id;
        Name = name;
        Address = address;
        PhoneNumber = phoneNumber;
    }

    /// <summary>
    /// Updates the customer's details.
    /// </summary>
    /// <param name="name">Customer name. Cannot be empty.</param>
    /// <param name="address">Customer address. Must be at least 10 characters if provided.</param>
    /// <param name="phoneNumber">Customer phone number. Must be 10 digits if provided.</param>
    /// <exception cref="ArgumentException">Thrown if validation fails for name, address, or phone number.</exception>
    public void Update(
        string? name, 
        string? address, 
        string? phoneNumber) 
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));
        if (!string.IsNullOrWhiteSpace(address) && address.Length < 10)
            throw new ArgumentException("Address is too short or empty", nameof(address));
        if (!string.IsNullOrWhiteSpace(phoneNumber) && !System.Text.RegularExpressions.Regex.IsMatch(phoneNumber, @"^\d{10}$"))
            throw new ArgumentException("Phone number must be 10 digits long", nameof(phoneNumber));
        Name = name;
        Address = address;
        PhoneNumber = phoneNumber;
    }
}
