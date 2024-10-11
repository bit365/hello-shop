// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.AppHost.Extensions;
using Aspire.Hosting.Dapr;

var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache", port: 6379);

var rabbitmq = builder.AddRabbitMQ("rabbitmq").WithManagementPlugin();

var identityService = builder.AddProject<Projects.HelloShop_IdentityService>("identityservice")
    .WithDaprSidecar();

DaprSidecarOptions daprSidecarOptions = new() { ResourcesPaths = ["DaprComponents"] };

var orderingService = builder.AddProject<Projects.HelloShop_OrderingService>("orderingservice")
    .WithReference(identityService)
    .WithDaprSidecar(options =>
    {
        options.WithOptions(daprSidecarOptions).WithReference(rabbitmq);
    });

var productService = builder.AddProject<Projects.HelloShop_ProductService>("productservice")
    .WithReference(identityService)
    .WithDaprSidecar(options =>
    {
        options.WithOptions(daprSidecarOptions).WithReference(rabbitmq);
    }); ;

var basketService = builder.AddProject<Projects.HelloShop_BasketService>("basketservice")
    .WithReference(identityService)
    .WithReference(cache)
    .WithDaprSidecar(options =>
    {
        options.WithOptions(daprSidecarOptions).WithReference(rabbitmq);
    });

var apiservice = builder.AddProject<Projects.HelloShop_ApiService>("apiservice")
.WithReference(identityService)
.WithReference(orderingService)
.WithReference(productService)
.WithReference(basketService)
.WithDaprSidecar();

builder.AddProject<Projects.HelloShop_WebApp>("webapp")
    .WithReference(apiservice)
    .WithDaprSidecar();

builder.Build().Run();
