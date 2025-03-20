// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.BasketService.DistributedEvents.EventHandling;
using HelloShop.BasketService.DistributedEvents.Events;
using HelloShop.BasketService.Repositories;
using HelloShop.BasketService.Services;
using HelloShop.EventBus.Abstractions;
using HelloShop.EventBus.Dapr;
using HelloShop.ServiceDefaults.Extensions;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add extensions services to the container.

const string issuerSigningKey = HelloShop.ServiceDefaults.Constants.IdentityConstants.IssuerSigningKey;

builder.Services.AddAuthentication().AddJwtBearer(options =>
{
    options.TokenValidationParameters.ValidateIssuer = false;
    options.TokenValidationParameters.ValidateAudience = false;
    options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(issuerSigningKey));
});

builder.Services.AddHttpContextAccessor();
builder.AddRedisDistributedCache("cache");
builder.Services.AddSingleton<IBasketRepository, DistributedCacheBasketRepository>();

builder.Services.AddGrpc().AddJsonTranscoding();
builder.Services.AddGrpcSwagger();
builder.Services.AddOpenApi();

builder.Services.AddDataSeedingProviders();
builder.Services.AddCustomLocalization();
builder.Services.AddModelMapper().AddModelValidator();
builder.Services.AddLocalization().AddPermissionDefinitions();
builder.Services.AddAuthorization().AddRemotePermissionChecker().AddCustomAuthorization();
builder.Services.AddCors();

builder.AddDaprEventBus().AddSubscription<OrderStartedDistributedEvent, OrderStartedDistributedEventHandler>();

// End addd extensions services to the container.

var app = builder.Build();

app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client.").WithTags("Welcome");

// Configure extensions request pipeline.
app.MapDaprEventBus();
app.UseAuthentication().UseAuthorization();
app.MapGrpcService<GreeterService>();
app.MapGrpcService<CustomerBasketService>();
app.UseDataSeedingProviders();
app.UseCustomLocalization();
app.UseOpenApi();
app.MapGroup("api/Permissions").MapPermissionDefinitions("Permissions");
// End configure extensions request pipeline.
app.Run();
