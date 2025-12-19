namespace Library.Client.Models.Filter;

/// <summary>
/// Represents filtering and sorting options for querying collections.
/// </summary>
public class FilterOptions
{
    /// <summary>
    /// Optional search term used to filter results by matching text.
    /// </summary>
    public string? SearchTerm { get; set; }

    /// <summary>
    /// Sort order for the results.
    /// Defaults to "asc" for ascending order. Can be set to "desc" for descending order.
    /// </summary>
    public string SortOrder { get; set; } = "asc";
}
