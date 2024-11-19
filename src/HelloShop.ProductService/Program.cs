// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.ProductService.Constants;
using HelloShop.ProductService.Infrastructure;
using HelloShop.ServiceDefaults.DistributedEvents.Abstractions;
using HelloShop.ServiceDefaults.DistributedEvents.DaprBuildingBlocks;
using HelloShop.ServiceDefaults.DistributedLocks;
using HelloShop.ServiceDefaults.Extensions;
using Microsoft.EntityFrameworkCore;

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
builder.AddDaprDistributedEventBus().AddSubscriptionFromAssembly();
builder.Services.AddSingleton<IDistributedLock, DaprDistributedLock>();
// End addd extensions services to the container.

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseAuthorization();

app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.MapControllers();

// Configure extensions request pipeline.
app.UseDataSeedingProviders();
app.UseCustomLocalization();
app.UseOpenApi();
app.MapGroup("api/Permissions").MapPermissionDefinitions("Permissions");
app.MapDaprDistributedEventBus();
// End configure extensions request pipeline.

app.Run();

/// <summary>
///  The test project requires a public Program type.
/// </summary>
public partial class Program { }