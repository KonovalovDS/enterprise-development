using Microsoft.AspNetCore.Identity;

namespace Library.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public int? CustomerId { get; set; }
}