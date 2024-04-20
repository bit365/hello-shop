using HelloShop.ProductService.Constants;
using HelloShop.ProductService.EntityFrameworks;
using Microsoft.EntityFrameworkCore;
using HelloShop.ServiceDefaults.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddControllers();

// Add extensions services to the container.
builder.Services.AddDbContext<ProductServiceDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString(DbConstants.ConnectionStringName));
});
builder.Services.AddHttpClient().AddHttpContextAccessor().AddDistributedMemoryCache();
builder.Services.AddDataSeedingProviders();
builder.Services.AddCustomLocalization();
builder.Services.AddOpenApi();
builder.Services.AddModelMapper().AddModelValidator();
builder.Services.AddLocalization().AddPermissionDefinitions();
builder.Services.AddAuthorization().AddRemotePermissionChecker().AddCustomAuthorization();
// End addd extensions services to the container.

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.MapControllers();

// Configure extensions request pipeline.
app.UseDataSeedingProviders();
app.UseCustomLocalization();
app.UseOpenApi();
app.MapGroup("api/Permissions").MapPermissionDefinitions("Permissions");
// End configure extensions request pipeline.

app.Run();
