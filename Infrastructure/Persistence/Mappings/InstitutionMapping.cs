using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Mappings;

public class InstitutionMapping : IEntityTypeConfiguration<Institution>
{
    public void Configure(EntityTypeBuilder<Institution> builder)
    {
        builder.ToTable("institutions");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(i => i.ContactEmail)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(i => i.ClientId).IsRequired();
        
        builder.HasMany(i => i.AccessGrants)
            .WithOne(g => g.Institution)
            .HasForeignKey(g => g.InstitutionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(i => i.AccessRequests)
            .WithOne(r => r.Institution)
            .HasForeignKey(r => r.InstitutionId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasIndex(i => i.ClientId).IsUnique();
    }
}