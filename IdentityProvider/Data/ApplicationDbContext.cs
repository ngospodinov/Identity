using IdentityProvider.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityProvider.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<OutboxMessage> Outbox => Set<OutboxMessage>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<OutboxMessage>(e =>
        {
            e.ToTable("outbox_messages");

            e.HasKey(x => x.Id);

            e.Property(x => x.Type)
                .IsRequired()
                .HasMaxLength(128);

            e.Property(x => x.Payload)
                .IsRequired();

            e.Property(x => x.CreatedOnUtc)
                .IsRequired();                

            e.Property(x => x.ProcessedOnUtc)
                .IsRequired(false);

            e.Property(x => x.Error)
                .IsRequired(false);

            e.HasIndex(x => x.CreatedOnUtc)
                .HasFilter("\"ProcessedOnUtc\" IS NULL");
        });
    }
}