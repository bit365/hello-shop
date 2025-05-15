// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using CommunityToolkit.Aspire.Hosting.Dapr;
using HelloShop.AppHost.Extensions;

var builder = DistributedApplication.CreateBuilder(args);

var postgreUser = builder.AddParameter("postgreUser", secret: true);
var postgrePassword = builder.AddParameter("postgrePassword", secret: true);
var postgres = builder.AddPostgres("postgres", postgreUser, postgrePassword, port: 5432).WithPgAdmin();
var identitydb = postgres.AddDatabase("identitydb");
var productdb = postgres.AddDatabase("productdb");
var orderingdb = postgres.AddDatabase("orderingdb");

var redisPassword = builder.AddParameter("redisPassword", secret: true);
var cache = builder.AddRedis("cache", port: 6379, password: redisPassword).WithLifetime(ContainerLifetime.Persistent).WithPersistence();

var rabbitmqUser = builder.AddParameter("rabbitmqUser", secret: true);
var rabbitmqPassword = builder.AddParameter("rabbitmqPassword", secret: true);
var rabbitmq = builder.AddRabbitMQ("rabbitmq", rabbitmqUser, rabbitmqPassword).WithLifetime(ContainerLifetime.Persistent).WithManagementPlugin();

var identityService = builder.AddProject<Projects.HelloShop_IdentityService>("identityservice")
    .WithReference(identitydb).WaitFor(identitydb)
    .WithDaprSidecar();

DaprSidecarOptions daprSidecarOptions = new() { ResourcesPaths = ["DaprComponents"] };

var orderingService = builder.AddProject<Projects.HelloShop_OrderingService>("orderingservice")
    .WithReference(identityService)
    .WithReference(orderingdb).WaitFor(orderingdb)
    .WithDaprSidecar(options =>
    {
        options.WithOptions(daprSidecarOptions).WithReferenceAndWaitFor(rabbitmq).WithReferenceAndWaitFor(cache);
    });

var productService = builder.AddProject<Projects.HelloShop_ProductService>("productservice")
    .WithReference(identityService).WaitFor(identityService)
    .WithReference(productdb).WaitFor(productdb)
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
