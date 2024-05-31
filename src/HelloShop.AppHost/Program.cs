// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

var builder = DistributedApplication.CreateBuilder(args);

var identityService = builder.AddProject<Projects.HelloShop_IdentityService>("identityservice");

var orderingService = builder.AddProject<Projects.HelloShop_OrderingService>("orderingservice").WithReference(identityService);

var productService = builder.AddProject<Projects.HelloShop_ProductService>("productservice").WithReference(identityService);

var basketService = builder.AddProject<Projects.HelloShop_BasketService>("basketservice").WithReference(identityService);

var apiservice = builder.AddProject<Projects.HelloShop_ApiService>("apiservice")
.WithReference(identityService)
.WithReference(orderingService)
.WithReference(productService)
.WithReference(basketService);

builder.AddProject<Projects.HelloShop_WebApp>("webapp").WithReference(apiservice);

builder.Build().Run();
