using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Mappings;

public class SpecificRevocationMapping : IEntityTypeConfiguration<SpecificRevocation>
{
    public void Configure(EntityTypeBuilder<SpecificRevocation> builder)
    {
        builder.ToTable("specific_revocations");
        builder.HasKey(sr => sr.Id);
        
        builder.Property(sr => sr.AccessGrantId).IsRequired();
        builder.Property(sr => sr.UserDataItemId).IsRequired();

        builder.HasIndex(x => new { x.AccessGrantId, x.UserDataItemId })
                .IsUnique();
        
        builder.HasOne(sr => sr.AccessGrant)
            .WithMany(ag => ag.SpecificRevocations)
            .HasForeignKey(sr => sr.AccessGrantId).IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(sr => sr.UserDataItem)
            .WithMany(di => di.SpecificRevocations)
            .HasForeignKey(sr => sr.UserDataItemId).IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(sr => sr.UserDataItemId);
    }
}