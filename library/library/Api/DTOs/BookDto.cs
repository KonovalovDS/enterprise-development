using library.Domain.Enums;

namespace library.Api.DTOs;

/// <summary>
/// Data Transfer Object for <see cref="Book"/>.
/// </summary>
public class BookDto
{
    public string? Code { get; set; }
    public string? Author { get; set; }
    public string? Name { get; set; }
    public Publisher Publisher { get; set; }
    public PublishingType PublishingType { get; set; }
    public int PublicationYear { get; set; }
}
