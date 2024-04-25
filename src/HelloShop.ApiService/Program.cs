using HelloShop.ApiService.Infrastructure;
using Yarp.ReverseProxy.Configuration;
using HelloShop.ServiceDefaults.Extensions;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerUI;
using HelloShop.ApiService.Extensions;
using HelloShop.ApiService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddControllers().AddDataAnnotationsLocalization();

builder.Services.AddHttpForwarderWithServiceDiscovery();

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .LoadFromMemory(routes: [], clusters: []);

builder.Services.AddSingleton<IConfiguredServiceEndPointResolver, ConfiguredServiceEndPointResolver>();
builder.Services.AddSingleton<IReverseProxyConfigProvider, CustomReverseProxyConfigProvider>();
builder.Services.AddTransient<IConfigureOptions<SwaggerUIOptions>, OpenApiConfigureOptions>();
builder.Services.AddTransient<IPermissionService, PermissionService>();

builder.Services.AddOpenApi();

var app = builder.Build();

app.MapDefaultEndpoints();

app.Services.GetRequiredService<IHostApplicationLifetime>().ApplicationStarted.Register(async () =>
{
    IReverseProxyConfigProvider provider = app.Services.GetRequiredService<IReverseProxyConfigProvider>();
    IReadOnlyList<RouteConfig> routes = await provider.GetRoutesAsync();
    IReadOnlyList<ClusterConfig> clusters = await provider.GetClustersAsync();
    app.Services.GetRequiredService<InMemoryConfigProvider>().Update(routes, clusters);
});

app.MapReverseProxy();

app.MapControllers();

app.UseOpenApi();

app.Run();
