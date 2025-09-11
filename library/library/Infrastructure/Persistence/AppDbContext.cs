using library.Domain.Entities;
using library.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using static Grpc.Core.Metadata;

namespace library.Infrastructure.Persistence;

public class AppDbContext : DbContext {
    public DbSet<Book> Books { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<BorrowRecord> BorrowRecords { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Customer>(c => {
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
                .HasMaxLength(10);
            c.Property(c => c.RegisterDate)
                .IsRequired()
                .HasColumnType("date");
        });

        modelBuilder.Entity<Book>(b => {
            b.HasKey(b => b.Id);
            b.Property(b => b.Id)
                .ValueGeneratedOnAdd();
            b.Property(b => b.Code)
               .IsRequired()
               .HasMaxLength(3);
            b.Property(b => b.Author)
               .IsRequired()
               .HasMaxLength(128);
            b.Property(b => b.Name)
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

        modelBuilder.Entity<BorrowRecord>(b => {
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
