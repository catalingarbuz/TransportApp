using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransportApp.Core.Entities;

namespace TransportApp.Infrastructure.EntityConfigurations;

/// <summary>
/// This class is used to configure the Booking entity for Entity Framework Core.
/// It specifies the properties of the Booking entity, their constraints, and the relationships with other entities.
/// </summary>
public class BookingsConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.Property(e => e.Id)
            .IsRequired();
        builder.HasKey(x => x.Id); 
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

        builder.HasOne(e => e.User)
            .WithMany(e => e.Bookings)
            .HasForeignKey(e => e.UserId)
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

