// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.DistributedLock.Dapr;
using HelloShop.EventBus.Abstractions;
using HelloShop.EventBus.Dapr;
using HelloShop.ProductService.Constants;
using HelloShop.ProductService.Infrastructure;
using HelloShop.ServiceDefaults.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddControllers();

const string issuerSigningKey = HelloShop.ServiceDefaults.Constants.IdentityConstants.IssuerSigningKey;

builder.Services.AddAuthentication().AddJwtBearer(options =>
{
    options.TokenValidationParameters.ValidateIssuer = false;
    options.TokenValidationParameters.ValidateAudience = false;
    options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(issuerSigningKey));
});

// Add extensions services to the container.
builder.AddNpgsqlDbContext<ProductServiceDbContext>(connectionName: DbConstants.ConnectionStringName, configureDbContextOptions: options =>
{
    new NpgsqlDbContextOptionsBuilder(options).MigrationsHistoryTable(DbConstants.MigrationsHistoryTableName);
    options.UseSnakeCaseNamingConvention();
});
builder.Services.AddHttpClient().AddHttpContextAccessor().AddDistributedMemoryCache();
builder.Services.AddDataSeedingProviders();
builder.Services.AddCustomLocalization();
builder.Services.AddOpenApi();
builder.Services.AddModelMapper().AddModelValidator();
builder.Services.AddLocalization().AddPermissionDefinitions();
builder.Services.AddAuthorization().AddRemotePermissionChecker().AddCustomAuthorization();
builder.AddDaprEventBus().AddSubscriptionFromAssembly();
builder.Services.AddDaprDistributedLock();
builder.Services.AddSingleton(TimeProvider.System);
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
app.MapDaprEventBus();
// End configure extensions request pipeline.

app.Run();

/// <summary>
///  The test project requires a public Program type.
/// </summary>
public partial class Program { }