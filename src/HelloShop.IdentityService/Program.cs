using HelloShop.IdentityService.Constants;
using HelloShop.IdentityService.DataSeeding;
using HelloShop.IdentityService.Entities;
using HelloShop.IdentityService.EntityFrameworks;
using HelloShop.ServiceDefaults.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<IdentityServiceDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString(DbConstants.ConnectionStringName));
});

builder.Services.AddIdentityApiEndpoints<User>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 5;
    options.SignIn.RequireConfirmedAccount = false;
}).AddRoles<Role>().AddEntityFrameworkStores<IdentityServiceDbContext>();

builder.Services.AddDataSeedingProviders();
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapGroup("identity").MapIdentityApi<User>();

app.MapDefaultEndpoints();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseDataSeedingProviders();
app.UseOpenApi();

app.Run();
