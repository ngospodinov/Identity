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

        builder.Property(x => x.ReviewedAt);

        builder.HasOne(x => x.DataOwnerUser)
            .WithMany()
            .HasForeignKey(x => x.DataOwnerUserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(x => x.RequesterUser)
            .WithMany()
            .HasForeignKey(x => x.RequesterUserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(x => x.RequestedItem)
            .WithMany()
            .HasForeignKey(x => x.RequestedItemId)
            .OnDelete(DeleteBehavior.SetNull);
        
        builder.HasIndex(x => x.DataOwnerUserId).HasDatabaseName("ix_access_requests_owner_id");
        builder.HasIndex(x => x.RequesterUserId).HasDatabaseName("ix_access_requests_requester_id");
    }
}