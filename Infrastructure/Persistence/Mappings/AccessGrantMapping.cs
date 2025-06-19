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
        
        builder.HasOne(g => g.DataOwnerUser)
            .WithMany()
            .HasForeignKey(g => g.DataOwnerUserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(g => g.RequesterUser)
            .WithMany()
            .HasForeignKey(g => g.RequesterUserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasIndex(g => new { g.DataOwnerUserId, g.RequesterUserId, g.Category, g.RequestedItemId, g.RevokedAt }).IsUnique();
    }
}
