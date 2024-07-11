// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using GarnetWindowsService;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddWindowsService(options =>
{
    options.ServiceName = "Garnet Service";
});

builder.Services.AddHostedService<GarnetService>();

var host = builder.Build();

host.Run();