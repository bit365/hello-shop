using HelloShop.IdentityService;
using HelloShop.IdentityService.Constants;
using HelloShop.IdentityService.DataSeeding;
using HelloShop.IdentityService.Entities;
using HelloShop.IdentityService.EntityFrameworks;
using HelloShop.ServiceDefaults.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<IdentityServiceDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString(DbConstants.ConnectionStringName));
});

builder.Services.AddIdentity<User, Role>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 5;
    options.SignIn.RequireConfirmedAccount = false;
    options.ClaimsIdentity.SecurityStampClaimType = "securitystamp";
}).AddEntityFrameworkStores<IdentityServiceDbContext>();

const string issuerSigningKey = HelloShop.ServiceDefaults.Constants.IdentityConstants.IssuerSigningKey;

builder.Services.AddAuthentication(options =>
{
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CustomJwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters.ValidateIssuer = false;
    options.TokenValidationParameters.ValidateAudience = false;
    options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(issuerSigningKey));
}).AddCustomJwtBearer(options =>
{
    options.IssuerSigningKey = issuerSigningKey;
    options.SecurityAlgorithm = SecurityAlgorithms.HmacSha256;
});

builder.Services.AddDataSeedingProviders();
builder.Services.AddOpenApi();
builder.Services.AddPermissionDefinitions();

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseDataSeedingProviders();
app.UseOpenApi();
app.MapGroup("api/Permissions").MapPermissionDefinitions("Permissions");

app.Run();
