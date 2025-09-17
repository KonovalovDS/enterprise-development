namespace library.Api.DTOs;

/// <summary>
/// Data Transfer Object for <see cref="Customer"/>.
/// </summary>
public class CustomerDto
{
    public string? Name { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public DateOnly? RegisterDate { get; set; }
}
