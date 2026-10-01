using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransportApp.Core.Entities;

namespace TransportApp.Infrastructure.EntityConfigurations;

/// <summary>
/// This class is used to configure the Route entity for Entity Framework Core. It specifies the properties of the Route entity, their data types, and any constraints or relationships with other entities.
/// </summary>
public class RouteConfiguration : IEntityTypeConfiguration<Route>
{
    public void Configure(EntityTypeBuilder<Route> builder)
    {
        builder.Property(e => e.Id) 
            .IsRequired();
        builder.HasKey(x => x.Id); 
        builder.Property(e => e.StartingLocationId)
            .IsRequired();
        builder.Property(e => e.FinalLocationId)
            .IsRequired();
        builder.Property(e => e.DepartureTime)
            .IsRequired();
        builder.Property(e => e.ArrivalTime)
            .IsRequired();
        builder.HasOne(e => e.StartingLocation)
            .WithMany();
        builder.HasOne(e => e.FinalLocation)
            .WithMany();
        builder.Property(e => e.CreatedAt)
            .IsRequired();
        builder.Property(e => e.UpdatedAt)
            .IsRequired();
    }
}

