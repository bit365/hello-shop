var builder = DistributedApplication.CreateBuilder(args);

var apiservice = builder.AddProject<Projects.HelloShop_ApiService>("helloshop.apiservice");

builder.AddProject<Projects.HelloShop_WebApp>("helloshop.webfrontend").WithReference(apiservice);

builder.AddProject<Projects.HelloShop_IdentityService>("helloshop.identityservice");

builder.AddProject<Projects.HelloShop_OrderingService>("helloshop.orderingservice");

builder.AddProject<Projects.HelloShop_ProductService>("helloshop.productservice");

builder.AddProject<Projects.HelloShop_BasketService>("helloshop.basketservice");

builder.Build().Run();
