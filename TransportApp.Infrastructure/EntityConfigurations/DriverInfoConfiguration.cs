using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransportApp.Core.Entities;

namespace TransportApp.Infrastructure.EntityConfigurations;

/// <summary>
/// This is the entity configuration for the User entity, generally the Entity Framework will figure out most of the configuration but,
/// for some specifics such as unique keys, indexes and foreign keys it is better to explicitly specify them.
/// Note that the EntityTypeBuilder implements a Fluent interface, meaning it is a highly declarative interface using method-chaining.
/// </summary>
public class DriverInfoConfiguration : IEntityTypeConfiguration<DriverInfo>
{
    public void Configure(EntityTypeBuilder<DriverInfo> builder)
    {
        builder.Property(e => e.Id) // This specifies which property is configured.
            .IsRequired(); // Here it is specified if the property is required, meaning it cannot be null in the database.
        builder.HasKey(x => x.Id); // Here it is specifies that the property Id is the primary key.
        builder.Property(e => e.CompletedTrips)
            .IsRequired();
        builder.Property(e => e.YearExperience)
            .IsRequired();
        builder.Property(e => e.BirthDate)
            .IsRequired();
        builder.Property(e => e.DriverId)
          .IsRequired();
        builder.Property(e => e.CreatedAt)
            .IsRequired();
        builder.Property(e => e.UpdatedAt)
            .IsRequired();

        builder.HasOne(e => e.Driver)
           .WithOne(e => e.DriverInfo) // This provides the reverse mapping for the one-to-many relation. 
           .HasForeignKey<DriverInfo>(e => e.DriverId) // Here the foreign key column is specified. 
           .IsRequired()
           .OnDelete(DeleteBehavior.Cascade); // This specifies the delete behavior when the referenced entity is removed.
    }
}

