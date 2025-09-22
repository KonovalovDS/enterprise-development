using library.Domain.Enums;

namespace library.Domain.Entities;

/// <summary>
/// Represents a book entity with author, name, publisher, and publishing details.
/// </summary>
public class Book 
{
    public int Id { get; set; }
    public required string InventoryNumber { get; set; }
    public required string Code { get; set; }
    public required string Author { get; set; }
    public required string Title { get; set; }
    public required Publisher Publisher { get; set; }
    public required PublishingType PublishingType { get; set; }
    public required int PublicationYear { get; set; }
}
