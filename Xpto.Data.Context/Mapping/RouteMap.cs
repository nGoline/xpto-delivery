
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Xpto.Domain.Entities;

namespace Xpto.Data.Context.Mapping
{
    public class RouteMap : IEntityTypeConfiguration<Route>
    {
        public void Configure(EntityTypeBuilder<Route> builder)
        {            
            // Primary Key
            builder.HasKey(t => t.Id);

            // Properties
            builder.Property(t => t.FromId)
                .IsRequired();

            builder.Property(t => t.ToId)
                .IsRequired();

            builder.Ignore(t => t.ValidationResult);
        }
    }
}