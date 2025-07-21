using Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
        
    }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
    
    public DbSet<UserDataItem> UserDataItems { get; set; }
    
    public DbSet<AccessGrant> AccessGrants { get; set; }
    
    public DbSet<AccessRequest> AccessRequests { get; set; }
    
    public DbSet<ProcessedEvent> ProcessedEvents { get; set; }
    
    public DbSet<Name> Names { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../WebApi")) 
                .AddJsonFile("appsettings.Development.json")
                .Build();

            optionsBuilder.UseNpgsql(config.GetConnectionString("IdentityDb"));
        }
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        
    }
}