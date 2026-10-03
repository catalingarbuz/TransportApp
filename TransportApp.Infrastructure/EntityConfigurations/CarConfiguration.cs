using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransportApp.Core.Entities;

namespace TransportApp.Infrastructure.EntityConfigurations;

/// <summary>
/// This is the entity configuration for the User entity, generally the Entity Framework will figure out most of the configuration but,
/// for some specifics such as unique keys, indexes and foreign keys it is better to explicitly specify them.
/// Note that the EntityTypeBuilder implements a Fluent interface, meaning it is a highly declarative interface using method-chaining.
/// </summary>
public class CarConfiguration : IEntityTypeConfiguration<Car>
{
    public void Configure(EntityTypeBuilder<Car> builder)
    {
        builder.Property(e => e.Id) 
            .IsRequired(); 
        builder.HasKey(x => x.Id); 
        builder.Property(e => e.Brand)
            .HasMaxLength(255)
            .IsRequired();
        builder.Property(e => e.Model)
            .HasMaxLength(255)
            .IsRequired();
        builder.Property(e => e.RegistrationNumber)
            .HasMaxLength(255)
            .IsRequired();
        builder.HasAlternateKey(e => e.RegistrationNumber);
        builder.Property(e => e.NumberOfSeats)
            .IsRequired();
        builder.Property(e => e.CreatedAt)
            .IsRequired();
        builder.Property(e => e.UpdatedAt)
            .IsRequired();
        builder.Property(e => e.DriverId)
            .IsRequired(false);

        builder.HasOne(e => e.Driver)
            .WithMany()
            .HasForeignKey(e => e.DriverId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

