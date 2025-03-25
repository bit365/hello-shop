// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.OrderingService.Extensions;
using HelloShop.OrderingService.Infrastructure;
using HelloShop.OrderingService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddServiceDefaults();
builder.AddApplicationServices();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapDefaultEndpoints();

app.MapApplicationEndpoints();

app.MapControllers();

await app.Services.GetRequiredService<MigrationService<OrderingServiceDbContext>>().ExecuteAsync();

await app.RunAsync();
