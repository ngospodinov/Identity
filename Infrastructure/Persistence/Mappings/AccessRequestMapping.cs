using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Mappings;

public class AccessRequestMapping : IEntityTypeConfiguration<AccessRequest>
{
    public void Configure(EntityTypeBuilder<AccessRequest> builder)
    {
        builder.ToTable("access_requests");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.RequestedAt).IsRequired();
        
        builder.Property(x => x.ReviewedAt).IsRequired();

        builder.HasOne(x => x.User)
            .WithMany(x => x.AccessRequests)
            .HasForeignKey(x => x.UserId);
        
        builder.HasOne(x => x.Institution)
            .WithMany(x => x.AccessRequests)
            .HasForeignKey(x => x.InstitutionId);
        
        builder.HasOne(x => x.RequestedItem)
            .WithMany()
            .HasForeignKey(x => x.RequestedItemId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}