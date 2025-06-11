using Duende.IdentityServer;
using IdentityProvider.Data;
using IdentityProvider.Models;
using IdentityProvider.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace IdentityProvider;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddRazorPages();

        builder.Services.AddCors(options => options.AddDefaultPolicy(p =>
            p.WithOrigins("https://localhost:5003")
                .AllowAnyHeader()
                .AllowAnyMethod()
        ));
        
        var isEf = Environment.GetEnvironmentVariable("DOTNET_EF") == "true";
        
        builder.Services.AddScoped<OutboxInterceptor>();
        builder.Services.AddScoped<DeletionService>();


        

        builder.Services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("IdentityServerDb"));
            
            if (!isEf)
            {
                options.AddInterceptors(sp.GetRequiredService<OutboxInterceptor>());
            }
        });
        
        if (!isEf)
        {
            builder.Services.AddHostedService<OutboxDispatcher>();
        }
           
        
        builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        builder.Services
            .AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
            })
            .AddConfigurationStore(opts =>
                opts.ConfigureDbContext = b => b.UseNpgsql(
                    builder.Configuration.GetConnectionString("IdentityServerDb"),
                    sql => sql.MigrationsAssembly("IdentityProvider")))
            .AddOperationalStore(opts =>
                opts.ConfigureDbContext = b => b.UseNpgsql(
                    builder.Configuration.GetConnectionString("IdentityServerDb"),
                    sql => sql.MigrationsAssembly("IdentityProvider")))
            .AddAspNetIdentity<ApplicationUser>();


        builder.Services.AddAuthentication()
            .AddGoogle(options =>
            {
                options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                
                options.ClientId = "348846350973-fqvfmmq9d61pk3q415ome8ac7tj0kq74.apps.googleusercontent.com";
                options.ClientSecret = "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";
                
                options.Scope.Add("openid");
                options.Scope.Add("email");
                options.Scope.Add("profile");
                
                options.SaveTokens = true;
                options.Events.OnRedirectToAuthorizationEndpoint = ctx =>
                {
                    if (ctx.Properties?.Items is { } items &&
                        items.TryGetValue("prompt", out var prompt) &&
                        !string.IsNullOrWhiteSpace(prompt))
                    {
                        var sep = ctx.RedirectUri.Contains('?') ? "&" : "?";
                        ctx.Response.Redirect($"{ctx.RedirectUri}{sep}prompt={Uri.EscapeDataString(prompt)}");
                        return Task.CompletedTask;
                    }

                    ctx.Response.Redirect(ctx.RedirectUri);
                    return Task.CompletedTask;
                };
            });

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseSerilogRequestLogging();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseStaticFiles();
        app.UseCors();
        app.UseRouting();
        app.UseIdentityServer();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapRazorPages()
            .RequireAuthorization();

        return app;
    }
}
