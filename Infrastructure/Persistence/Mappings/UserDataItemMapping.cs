using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Mappings;

public class UserDataItemMapping : IEntityTypeConfiguration<UserDataItem>
{
    public void Configure(EntityTypeBuilder<UserDataItem> builder)
    {
        builder.ToTable("user_data_items");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Key)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(d => d.Value)
            .IsRequired();

        builder.Property(d => d.Category)
            .IsRequired();

        builder.Property(d => d.CreatedAt)
            .IsRequired();
    }
}