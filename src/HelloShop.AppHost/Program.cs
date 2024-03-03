var builder = DistributedApplication.CreateBuilder(args);

var apiservice = builder.AddProject<Projects.HelloShop_ApiService>("apiservice");

builder.AddProject<Projects.HelloShop_IdentityService>("identityservice");

builder.AddProject<Projects.HelloShop_OrderingService>("orderingservice");

builder.AddProject<Projects.HelloShop_ProductService>("productservice");

builder.AddProject<Projects.HelloShop_BasketService>("basketservice");

builder.AddProject<Projects.HelloShop_WebApp>("webapp").WithReference(apiservice);

builder.Build().Run();
