
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Xpto.Domain.Entities;

namespace Xpto.Data.Context.Mapping
{
    public class MapPointMap : IEntityTypeConfiguration<MapPoint>
    {
        public void Configure(EntityTypeBuilder<MapPoint> builder)
        {
            // Primary Key
            builder.HasKey(t => t.Id);

            // Properties
            builder.Property(t => t.Name)
                   .HasMaxLength(160)
                   .IsRequired();

            builder.Property(t => t.Latitude)
                   .IsRequired();

            builder.Property(t => t.Longitude)
                   .IsRequired();

            builder.Ignore(t => t.ValidationResult);

            builder.Ignore(t => t.Coordinate);
        }
    }
}