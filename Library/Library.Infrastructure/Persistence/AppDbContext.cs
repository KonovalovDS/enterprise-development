using Microsoft.EntityFrameworkCore;
using Library.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Library.Infrastructure.Persistence;

/// <summary>
/// EF Core database context for the library application.
/// </summary>
/// <param name="options">The options for this context.</param>
public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext(options)
{
    /// <summary>
    /// DbSet of books in the library.
    /// </summary>
    public DbSet<Book> Books { get; set; }

    /// <summary>
    /// DbSet of customers in the library.
    /// </summary>
    public DbSet<Customer> Customers { get; set; }

    /// <summary>
    /// DbSet of borrow records, representing books borrowed by customers.
    /// </summary>
    public DbSet<BorrowRecord> BorrowRecords { get; set; }

    /// <summary>
    /// Configures the EF Core model.
    /// </summary>
    /// <param name="modelBuilder">Model builder instance.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder) 
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Customer>(c => 
        {
            c.HasKey(c => c.Id);
            c.Property(c => c.Id)
                .ValueGeneratedOnAdd();
            c.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(128);
            c.Property(c => c.Address)
                .IsRequired()
                .HasMaxLength(128);
            c.Property(c => c.PhoneNumber)
                .IsRequired()
                .HasMaxLength(13);
            c.Property(c => c.RegisterDate)
                .IsRequired()
                .HasColumnType("date");
        });

        modelBuilder.Entity<Book>(b => 
        {
            b.HasKey(b => b.Id);
            b.Property(b => b.Id)
                .ValueGeneratedOnAdd();
            b.Property(b => b.InventoryNumber)
                .IsRequired()
                .HasMaxLength(6);
            b.Property(b => b.Code)
               .IsRequired()
               .HasMaxLength(2);
            b.Property(b => b.Author)
               .IsRequired()
               .HasMaxLength(128);
            b.Property(b => b.Title)
               .IsRequired()
               .HasMaxLength(128);
            b.Property(b => b.PublicationYear)
               .IsRequired();
            b.Property(b => b.Publisher)
               .HasConversion<string>()
               .IsRequired();
            b.Property(b => b.PublishingType)
               .HasConversion<string>()
               .IsRequired();
        });

        modelBuilder.Entity<BorrowRecord>(b => 
        {
            b.HasKey(b => b.Id);
            b.Property(b => b.Id)
                .ValueGeneratedOnAdd();
            b.HasOne<Book>()
                .WithMany()
                .HasForeignKey(b => b.BookId)
                .OnDelete(DeleteBehavior.Cascade);
            b.HasOne<Customer>()
                .WithMany()
                .HasForeignKey(b => b.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);
            b.Property(b => b.BorrowDate)
                .IsRequired()
                .HasColumnType("date");
            b.Property(b => b.BorrowDuration)
                .IsRequired();
        });
    }
}
