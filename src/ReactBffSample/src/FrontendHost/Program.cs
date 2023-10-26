using Duende.Bff.Yarp;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddBff()
    .AddRemoteApis();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "cookie";
    options.DefaultChallengeScheme = "oidc";
    options.DefaultSignOutScheme = "oidc";
}).AddCookie("cookie", options =>
{
    options.Cookie.Name = "__Host-bff-abe";
    options.Cookie.SameSite = SameSiteMode.Strict;
}).AddOpenIdConnect("oidc", options =>
{
    // Duende Software
    //options.Authority = "https://demo.duendesoftware.com";
    //options.ClientId = "interactive.confidential.short";
    //options.ClientSecret = "secret";

    // AUTH0
    //options.Authority = "https://dev-6b0mu6pvkqz2xf0v.us.auth0.com/";
    //options.ClientId = "3M90HmrkSs7hcbi1A7C8K4LUMHyEAAPP";

    // B2C jwt-test-app
    options.Authority = "https://iamvresdnadev001.b2clogin.com/iamvresdnadev001.onmicrosoft.com/b2c_1a_signup_signin/v2.0/";
    options.ClientId = "3e68a722-16f7-4582-90d1-514a16fa6919";

    // Common Fields

    options.ResponseType = "code";
    options.ResponseMode = "query";

    options.GetClaimsFromUserInfoEndpoint = true;
    options.MapInboundClaims = false;
    options.SaveTokens = true;

    options.Scope.Clear();
    options.Scope.Add("openid");
    options.Scope.Add("profile");

    // B2C jwt-test-app
    options.Scope.Add(options.ClientId);
    //options.Scope.Add("https://iamvresdnadev001.onmicrosoft.com/app-desktop-eval-bff/read.access");
    //options.Scope.Add("api");
    //options.Scope.Add("offline_access");
    //options.ClientSecret = "McZ8Q~s6l21IvVtCUUoPk8GD361aAhI1jjOKLcnm";

    options.TokenValidationParameters = new()
    {
        NameClaimType = "name",
        RoleClaimType = "role"
    };
});

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseBff();
app.UseAuthorization();
app.MapBffManagementEndpoints();

app.MapControllers()
    .RequireAuthorization()
    .AsBffApiEndpoint();

// app.MapRemoteBffApiEndpoint("/todos", "https://localhost:5020/todos")
//     .RequireAccessToken(Duende.Bff.TokenType.User);

app.MapFallbackToFile("index.html"); ;

app.Run();
