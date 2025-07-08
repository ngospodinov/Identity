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
        
        builder.Property(g => g.Id).ValueGeneratedOnAdd();

        builder.Property(g => g.Category)
            .IsRequired();

        builder.Property(g => g.GrantedAt)
            .IsRequired();

        builder.Property(g => g.RevokedAt);
          

        builder.HasOne(g => g.User)
            .WithMany(u => u.AccessGrants)
            .HasForeignKey(g => g.UserId);
        
        builder.HasOne(g => g.Institution)
            .WithMany(i => i.AccessGrants)
            .HasForeignKey(g => g.InstitutionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(g => new { g.UserId, g.Category }).IsUnique();
    }
}
