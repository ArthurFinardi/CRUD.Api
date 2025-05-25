using CRUD.Domain.Entities;
using CRUD.Infrastructure.EventStore;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<EventData> EventStore { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Document).IsRequired().HasMaxLength(14);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Phone).IsRequired().HasMaxLength(15);
            entity.Property(e => e.StateRegistration).HasMaxLength(20);
            entity.Property(e => e.Type).IsRequired();

            entity.OwnsOne(e => e.Address, address =>
            {
                address.Property(a => a.ZipCode).IsRequired().HasMaxLength(9);
                address.Property(a => a.Street).IsRequired().HasMaxLength(200);
                address.Property(a => a.Number).IsRequired().HasMaxLength(10);
                address.Property(a => a.Neighborhood).IsRequired().HasMaxLength(100);
                address.Property(a => a.City).IsRequired().HasMaxLength(100);
                address.Property(a => a.State).IsRequired().HasMaxLength(2);
            });

            entity.HasIndex(e => e.Document).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();
        });

        modelBuilder.Entity<EventData>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.EventType).IsRequired();
            entity.Property(e => e.Data).IsRequired();
            entity.HasIndex(e => e.AggregateId);
            entity.HasIndex(e => e.Timestamp);
        });
    }
} 