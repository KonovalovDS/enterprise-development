using Microsoft.AspNetCore.Identity;

namespace Library.Domain.Entities;

/// <summary>
/// Represents an application user with optional linkage to a <see cref="Customer"/> entity.
/// Inherits from <see cref="IdentityUser"/> for ASP.NET Core Identity features.
/// </summary>
public class ApplicationUser : IdentityUser
{
    /// <summary>
    /// Optional reference to a <see cref="Customer"/> entity.
    /// If set, this links the user account to a specific customer in the system.
    /// </summary>
    public int? CustomerId { get; set; }
}
