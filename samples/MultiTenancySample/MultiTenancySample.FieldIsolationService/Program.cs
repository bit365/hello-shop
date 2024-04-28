using Microsoft.EntityFrameworkCore;
using MultiTenancySample.FieldIsolationService.EntityFrameworks;
using MultiTenancySample.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContextFactory<FieldIsolationServiceDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default"));
}, lifetime: ServiceLifetime.Scoped);

builder.Services.AddScoped<DataSeeding>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddMultiTenancy();

var app = builder.Build();

app.UseMultiTenancy();

var serviceProvider = app.Services.CreateScope().ServiceProvider;
var dataSeeding = serviceProvider.GetRequiredService<DataSeeding>();
await dataSeeding.SeedDataAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
