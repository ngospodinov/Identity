using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Mappings;


public class AccessGrantMapping : IEntityTypeConfiguration<AccessGrant>
{
    public void Configure(EntityTypeBuilder<AccessGrant> builder)
    {
        builder.ToTable("access_grants");

        builder.HasKey(g => g.Id);

        builder.Property(g => g.Category)
            .IsRequired();

        builder.Property(g => g.GrantedAt)
            .IsRequired();

        builder.HasOne(g => g.User)
            .WithMany(u => u.AccessGrants)
            .HasForeignKey(g => g.UserId);
    }
}
