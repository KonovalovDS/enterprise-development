using library.Domain.Entities;

namespace library.Api.DTOs;

public class CustomerDto
{
    public string? Name { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public DateOnly? RegisterDate { get; set; }
}
