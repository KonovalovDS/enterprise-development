namespace Library.Client.Models.Analytics;

/// <summary>
/// Represents a dto for a publisher with its name and the number of borrowed books.
/// </summary>
public class PublisherDto
{
    /// <summary>
    /// Publisher name.
    /// </summary>
    public required string Publisher { get; set; }

    /// <summary>
    /// Number of borrowed books of this publisher.
    /// </summary>
    public required int Count { get; set; } = 0;
}
