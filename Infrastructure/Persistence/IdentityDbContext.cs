using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class IdentityDbContext : DbContext
{
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
        : base(options)
    {
    }

    public DbSet<UserEntity> Users { get; set; }
    
    public DbSet<UserDataItem> UserDataItems { get; set; }
    
    public DbSet<Institution> Institutions { get; set; }
    
    public DbSet<AccessGrant> AccessGrants { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IdentityDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
        
        var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        var date = new DateTime(2025, 03, 01, 0, 0, 0, DateTimeKind.Utc);
        
        modelBuilder.Entity<UserEntity>().HasData(new UserEntity
        {
            Id = userId,
            Username = "testuser",
            Email = "test@example.com",
            CreatedAt = date
        });

        modelBuilder.Entity<UserDataItem>().HasData(
            new UserDataItem
            {
                Id = 1,
                UserId = userId,
                Key = "GPA",
                Value = "3.8",
                Category = Domain.Enums.DataCategory.Academic,
                CreatedAt = date
            },
            new UserDataItem
            {
                Id = 2,
                UserId = userId,
                Key = "IBAN",
                Value = "BG00TEST1234567890",
                Category = Domain.Enums.DataCategory.Financial,
                CreatedAt = date
            }
        );

        modelBuilder.Entity<AccessGrant>().HasData(new AccessGrant
        {
            Id = 1,
            UserId = userId,
            ClientId = "test-client", 
            Category = Domain.Enums.DataCategory.Academic,
            GrantedAt = date
        });
    }
}