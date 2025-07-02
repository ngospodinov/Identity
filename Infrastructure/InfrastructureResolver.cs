using Application;
using Application.Repositories;
using Application.Services;
using Infrastructure.Messaging;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure;

public static class InfrastructureResolver
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("IdentityDb")));
        
        services.AddHttpContextAccessor();

        
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IDataItemRepository, DataItemRepository>();
        services.AddScoped<IAccessRequestRepository, AccessRequestRepository>();
        services.AddScoped<IAccessGrantRepository, AccessGrantRepository>();
        services.AddScoped<INameRepository, NameRepository>();
        
        services.AddScoped<ICurrentClientProvider, CurrentClientProvider>();
        services.AddScoped<IUserSyncService, UserSyncService>();
        services.AddScoped<INameService, NameService>();        
        
        var isEf = string.Equals(
            Environment.GetEnvironmentVariable("DOTNET_EF"),
            "true",
            StringComparison.OrdinalIgnoreCase);
        
      
        return services;
    }
}