using Domain.DataSeeders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public static class DbSeeder
{
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
}