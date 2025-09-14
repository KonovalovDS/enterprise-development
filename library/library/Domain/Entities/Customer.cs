namespace library.Domain.Entities;

public class Customer {
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public DateOnly RegisterDate { get; init; } = DateOnly.FromDateTime(DateTime.Today);

    private Customer() { }
    public Customer(int id, string? name, string? address, string? phoneNumber) {
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

    public void Update(string? name, string? address, string? phoneNumber) {
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
