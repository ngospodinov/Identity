using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Mappings;

public class NameMapping : IEntityTypeConfiguration<Name>
{
    public void Configure(EntityTypeBuilder<Name> builder)
    {
        builder.ToTable("names");
        
        builder.Property(x => x.Category).HasConversion<string>().HasMaxLength(32).IsRequired();

        builder.Property(x => x.FirstName).IsRequired().HasMaxLength(128);
        builder.Property(x => x.MiddleName).HasMaxLength(128);
        builder.Property(x => x.LastName).HasMaxLength(128);

        builder.Property(x => x.IsDefaultForCategory).IsRequired();
        
        builder.HasIndex(x => new { x.UserId, x.Category, x.IsDefaultForCategory })
            .IsUnique()
            .HasFilter("\"IsDeleted\" = false AND \"IsDefaultForCategory\" = true");
    }
}