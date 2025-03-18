// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using CommunityToolkit.Aspire.Hosting.Dapr;
using HelloShop.AppHost.Extensions;

var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache", port: 6380).WithLifetime(ContainerLifetime.Persistent).WithPersistence();

var rabbitmqUser = builder.AddParameter("rabbitmqUser", secret: true);
var rabbitmqPassword = builder.AddParameter("rabbitmqPassword", secret: true);
var rabbitmq = builder.AddRabbitMQ("rabbitmq", rabbitmqUser, rabbitmqPassword).WithLifetime(ContainerLifetime.Persistent).WithManagementPlugin();

var identityService = builder.AddProject<Projects.HelloShop_IdentityService>("identityservice")
    .WithDaprSidecar();

DaprSidecarOptions daprSidecarOptions = new() { ResourcesPaths = ["DaprComponents"] };

var orderingService = builder.AddProject<Projects.HelloShop_OrderingService>("orderingservice")
    .WithReference(identityService)
    .WithDaprSidecar(options =>
    {
        options.WithOptions(daprSidecarOptions).WithReferenceAndWaitFor(rabbitmq).WithReferenceAndWaitFor(cache);
    });

var productService = builder.AddProject<Projects.HelloShop_ProductService>("productservice")
    .WithReference(identityService).WaitFor(identityService)
    .WithDaprSidecar(options =>
    {
        options.WithOptions(daprSidecarOptions).WithReferenceAndWaitFor(rabbitmq).WithReferenceAndWaitFor(cache);
    });

var basketService = builder.AddProject<Projects.HelloShop_BasketService>("basketservice")
    .WithReference(identityService).WaitFor(identityService)
    .WithReference(cache).WaitFor(cache)
    .WithDaprSidecar(options =>
    {
        options.WithOptions(daprSidecarOptions).WithReferenceAndWaitFor(rabbitmq).WithReferenceAndWaitFor(cache);
    });

var apiservice = builder.AddProject<Projects.HelloShop_ApiService>("apiservice")
.WithReference(identityService).WaitFor(identityService)
.WithReference(orderingService).WaitFor(orderingService)
.WithReference(productService).WaitFor(productService)
.WithReference(basketService).WaitFor(basketService)
.WithDaprSidecar();

builder.AddProject<Projects.HelloShop_WebApp>("webapp")
    .WithReference(apiservice).WaitFor(apiservice)
    .WithDaprSidecar();

builder.Build().Run();
