using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using SimpleWebAuth.Data;

namespace SimpleWebAuth;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddRazorPages();
        
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme; // "Cookies"
            options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme; // "oidc"

            // keep the logout controller in sync too to remove the cookie...
        })
        .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
        {
            options.Cookie.Name = "__HOST-SimpleWebAuth";
            //options.Cookie.SameSite = SameSiteMode.Lax; // Strict will not work due too top-scripting redirect
        })
        .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
        {

            options.Events = new OpenIdConnectEvents
            {
                OnAccessDenied = async (arg) =>
                {
                    var s = arg;
                    await Task.CompletedTask;
                },
                OnAuthenticationFailed = async (arg) =>
                {
                    var s = arg;
                    await Task.CompletedTask;
                },
                OnUserInformationReceived = async (arg) =>
                {
                    var s = arg;

                    var isAuth = arg.HttpContext.User.Identity.IsAuthenticated;

                    await Task.CompletedTask;
                }
                ,
                OnAuthorizationCodeReceived = async (arg) =>
                {
                    var s = arg;
                    await Task.CompletedTask;
                }
            };

            var oidcConfig =
                builder.Configuration.GetSection("OidcConfig:Entra")
                //builder.Configuration.GetSection("OidcConfig:Duende")
            ;

            options.Authority = oidcConfig["Authority"];
            options.ClientId = oidcConfig["ClientId"];
            options.ClientSecret = oidcConfig["ClientSecret"];


            options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
          
            options.ResponseType = OpenIdConnectResponseType.Code;
            options.UsePkce = true;

            options.Scope.Clear();
            options.Scope.Add("openid");
            options.Scope.Add("profile");

            options.MapInboundClaims = false;
            options.GetClaimsFromUserInfoEndpoint = true;
            options.SaveTokens = true;

        });


        // Add services to the container.
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(connectionString));
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        //builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
        //    .AddEntityFrameworkStores<ApplicationDbContext>()
        //    ;

        //var requireAuthPolicy = new AuthorizationPolicyBuilder()
        //.RequireAuthenticatedUser()
        //.Build();

        //builder.Services.AddAuthorizationBuilder()
        //    .SetFallbackPolicy(requireAuthPolicy);

        

        var app = builder.Build();


        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapRazorPages().RequireAuthorization();

        app.Run();
    }
}