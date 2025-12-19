namespace Library.Client.Models.ClaimDtos;

/// <summary>
/// Data Transfer Object representing a single claim for a user.
/// Typically used to store claim type and its corresponding value.
/// </summary>
/// <param name="Type">The type of the claim (e.g., role, email, permission).</param>
/// <param name="Value">The value associated with the claim type.</param>
public record ClaimDto(
    string Type,
    string Value
);
