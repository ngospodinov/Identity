using Application;
using Identity.Middleware;
using Infrastructure;
using Infrastructure.Messaging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(IApplicationMarker).Assembly));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Identity API",
        Version = "v1"
    });
    
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            ClientCredentials = new OpenApiOAuthFlow()
            {
                TokenUrl = new Uri("https://localhost:5001/connect/token"),
                Scopes = new Dictionary<string, string>
                {
                    {
                        "IdentityProjectApi", "Access to the Identity API"
                    }
                }
            }
        }
    });
    
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "oauth2",
                    Type = ReferenceType.SecurityScheme
                }
            }, new List<string>()
        }
    });
    
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "https://localhost:5001"; 
        options.TokenValidationParameters.ValidateAudience = false;
        options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" }; 
        options.MapInboundClaims = false;
        
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine("JWT Auth failed: " + context.Exception.Message);
                return Task.CompletedTask;
            }
        };
    });
    
builder.Services.AddAuthorization();
builder.Services.AddHostedService<UserEventsConsumer>();

builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseMiddleware<ExceptionHandler>();
app.UseAuthorization();

app.MapControllers();

app.Run();

