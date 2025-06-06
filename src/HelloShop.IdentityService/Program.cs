// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.IdentityService.Authentication;
using HelloShop.IdentityService.Authorization;
using HelloShop.IdentityService.Constants;
using HelloShop.IdentityService.Entities;
using HelloShop.IdentityService.Infrastructure;
using HelloShop.IdentityService.Services;
using HelloShop.IdentityService.Workers;
using HelloShop.ServiceDefaults.Authorization;
using HelloShop.ServiceDefaults.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddControllers();

builder.AddNpgsqlDbContext<IdentityServiceDbContext>(connectionName: DbConstants.ConnectionStringName, configureDbContextOptions: options =>
{
    new NpgsqlDbContextOptionsBuilder(options).MigrationsHistoryTable(DbConstants.MigrationsHistoryTableName);
    options.UseSnakeCaseNamingConvention();
});

builder.Services.AddSingleton<MigrationService<IdentityServiceDbContext>>().AddHostedService<DataSeeder>();

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

builder.Services.AddOpenApi();
builder.Services.AddPermissionDefinitions();
builder.Services.AddAuthorization().AddDistributedMemoryCache().AddHttpClient().AddHttpContextAccessor().AddTransient<IPermissionChecker, LocalPermissionChecker>().AddCustomAuthorization();
builder.Services.AddModelMapper().AddModelValidator();
builder.Services.AddCustomLocalization();
builder.Services.AddSingleton(TimeProvider.System);

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseAuthorization();

app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.MapControllers();

app.UseOpenApi();
app.MapGroup("api/Permissions").MapPermissionDefinitions("Permissions");
app.UseCustomLocalization();

await app.Services.GetRequiredService<MigrationService<IdentityServiceDbContext>>().ExecuteAsync();

await app.RunAsync();
