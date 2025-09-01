using IdentityProjectUI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();

builder.Services.AddAuthentication(o =>
{
    o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme; 
    o.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie()
.AddOpenIdConnect(options =>
{
    options.Authority = "https://localhost:5001";
    options.ClientId = "interactive";
    options.ClientSecret = "49C1A7E1-0C79-4A89-A3D6-A37998FB86B0";
    
    options.Scope.Add("openid");
    options.Scope.Add("profile");
    options.Scope.Add("email");
    options.Scope.Add("IdentityProjectApi");

    options.SaveTokens = true;
    options.ResponseType = "code";
    options.GetClaimsFromUserInfoEndpoint = true;
    
    options.ClaimActions.MapJsonKey("email", "email");
    
    options.Events = new OpenIdConnectEvents
    {
        OnTokenResponseReceived = r =>
        {
            var idToken = r.TokenEndpointResponse.IdToken;
            return Task.CompletedTask;
        }
    };
    
    options.SignedOutCallbackPath = "/signout-callback-oidc"; 
    options.SignedOutRedirectUri  = "/";        
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<AccessTokenHandler>();

builder.Services.AddHttpClient<IdentityApiClient>(c =>
{
    c.BaseAddress = new Uri(builder.Configuration["Api:BaseUrl"]!);
}).AddHttpMessageHandler<AccessTokenHandler>();


var app = builder.Build();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();



app.Run();