using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Infrastructure.EntityConfigurations;

/// <summary>
/// This is the entity configuration for the User entity, generally the Entity Framework will figure out most of the configuration but,
/// for some specifics such as unique keys, indexes and foreign keys it is better to explicitly specify them.
/// Note that the EntityTypeBuilder implements a Fluent interface, meaning it is a highly declarative interface using method-chaining.
/// </summary>
public class BookingsConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.Property(e => e.Id) // This specifies which property is configured.
            .IsRequired(); // Here it is specified if the property is required, meaning it cannot be null in the database.
        builder.HasKey(x => x.Id); // Here it is specifies that the property Id is the primary key.
        builder.Property(e => e.CreatedAt)
            .IsRequired();
        builder.Property(e => e.UpdatedAt)
            .IsRequired();
        builder.Property(e => e.UserId)
            .IsRequired();
        builder.Property(e => e.CarId)
            .IsRequired();
        builder.Property(e => e.RouteId)
            .IsRequired();
        builder.Property(e => e.BookingDate)
            .IsRequired();
        builder.Property(e => e.DepartureDate)
            .IsRequired();
        builder.Property(e => e.DeparturePlace)
            .HasMaxLength(40)
            .IsRequired();
        builder.Property(e => e.ArrivalPlace)
            .HasMaxLength(40)
            .IsRequired();

        builder.HasOne(e => e.User)
            .WithMany(e => e.Bookings)
            .HasForeignKey(e => e.CarId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Driver)
            .WithMany(e => e.Bookings)
            .HasForeignKey(e => e.DriverId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Car)
           .WithMany(e => e.Bookings)
           .HasForeignKey(e => e.CarId)
           .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Route)
           .WithMany(e => e.Bookings)
           .HasForeignKey(e => e.RouteId)
           .OnDelete(DeleteBehavior.Cascade);
    }
}
