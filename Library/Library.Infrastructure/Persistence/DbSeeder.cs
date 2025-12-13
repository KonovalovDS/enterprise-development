using Library.Domain.DataSeeders;
using Library.Domain.Entities;
using Library.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Persistence;

/// <summary>
/// Provides helper methods to seed the database with initial test data for customers, books, and borrow records.
/// Ensures that the primary key sequences are correctly set after inserting data.
/// </summary>
public static class DbSeeder
{
    /// <summary>
    /// Seeds the <see cref="Customer"/> table with initial test data if it is empty.
    /// After inserting, updates the primary key sequence to match the maximum existing ID.
    /// </summary>
    /// <param name="context">The database context used to access the Customers table.</param>
    public static async Task SeedCustomersAsync(AppDbContext context)
    {
        if (!context.Customers.Any())
        {
            context.Customers.AddRange(DataSeeder.CustomersTestData);
            await context.SaveChangesAsync();
        }
        await context.Database.ExecuteSqlRawAsync(
            @"SELECT setval(pg_get_serial_sequence('""Customers""', 'Id'), (SELECT MAX(""Id"") FROM ""Customers""));"
        );
    }

    /// <summary>
    /// Seeds the <see cref="Book"/> table with initial test data if it is empty.
    /// After inserting, updates the primary key sequence to match the maximum existing ID.
    /// </summary>
    /// <param name="context">The database context used to access the Books table.</param>
    public static async Task SeedBooksAsync(AppDbContext context) 
    { 
        if (!context.Books.Any()) 
        { 
            context.Books.AddRange(DataSeeder.BooksTestData); 
            await context.SaveChangesAsync(); 
        }
        await context.Database.ExecuteSqlRawAsync(
            @"SELECT setval(pg_get_serial_sequence('""Books""', 'Id'), (SELECT MAX(""Id"") FROM ""Books""));"
        );
    }

    /// <summary>
    /// Seeds the <see cref="BorrowRecord"/> table with initial test data if it is empty.
    /// After inserting, updates the primary key sequence to match the maximum existing ID.
    /// </summary>
    /// <param name="context">The database context used to access the BorrowRecords table.</param>
    public static async Task SeedBorrowRecordsAsync(AppDbContext context) 
    { 
        if (!context.BorrowRecords.Any()) 
        { 
            context.BorrowRecords.AddRange(DataSeeder.BorrowRecordsTestData); 
            await context.SaveChangesAsync(); 
        }
        await context.Database.ExecuteSqlRawAsync(
            @"SELECT setval(pg_get_serial_sequence('""BorrowRecords""', 'Id'), (SELECT MAX(""Id"") FROM ""BorrowRecords""));"
        );
    }

    public static async Task SeedUsersAndRolesAsync(
        AppDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        var roles = Enum.GetNames(typeof(UserRole));
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        var adminEmail = "admin@library.com";
        var admin = await userManager.FindByEmailAsync(adminEmail);
        if (admin == null)
        {
            admin = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };
            await userManager.CreateAsync(admin, "Password123!");
            await userManager.AddToRoleAsync(admin, UserRole.Admin.ToString());
        }

        var customers = context.Customers.ToList();
        foreach (var customer in customers)
        {
            var userEmail = $"{customer.Name.Replace(" ", "").ToLower()}@library.com";
            var user = await userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = userEmail,
                    Email = userEmail,
                    EmailConfirmed = true,
                    CustomerId = customer.Id
                };
                await userManager.CreateAsync(user, "UserPassword123!");
                await userManager.AddToRoleAsync(user, UserRole.User.ToString());
            }
        }
    }
}
