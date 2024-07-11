// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using Garnet;

namespace GarnetWindowsService
{
    public class GarnetService(ILoggerFactory loggerFactory) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (!stoppingToken.IsCancellationRequested)
            {
                using var server = new GarnetServer([]);

                ILogger logger = loggerFactory.CreateLogger<GarnetService>();

                logger.LogInformation("Starting Garnet server...");

                try
                {
                    server.Start();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while starting the Garnet server.");
                }

                logger.LogInformation("Garnet server started.");

                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
        }
    }
}
