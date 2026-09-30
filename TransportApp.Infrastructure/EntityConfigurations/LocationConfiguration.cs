using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransportApp.Core.Entities;

namespace TransportApp.Infrastructure.EntityConfigurations
{
    public class LocationConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.Property(e => e.Id)
                .IsRequired();
            builder.HasKey(x => x.Id);
            builder.Property(e => e.City)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(e => e.Country)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(e => e.Latitude)
                .IsRequired(false);
            builder.Property(e => e.Longitude)
                .IsRequired(false);
            builder.Property(e => e.Adress)
                .IsRequired(false)
                .HasMaxLength(300);
        }
    }
}
