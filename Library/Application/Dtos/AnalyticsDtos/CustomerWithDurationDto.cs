namespace Application.Dtos.AnalyticsDtos;

/// <summary>
/// Represents a dto for a customer with name and longest borrow duration.
/// </summary>
public class CustomerWithDurationDto
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
    /// Longest borrow duration of this customer.
    /// </summary>
    public required int Duration { get; set; } = 0;
}
