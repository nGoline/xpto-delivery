using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Xpto.Domain.Entities;

namespace Xpto.Data.Context.Mapping
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Primary Key
            builder.HasKey(t => t.Id);

            // Properties
            builder.Property(t => t.Name)
                   .HasMaxLength(160)
                   .IsRequired();

            builder.Property(t => t.Email)
                   .HasMaxLength(160) 
                   .IsRequired();

            builder.Ignore(t => t.ValidationResult);
        }
    }
}