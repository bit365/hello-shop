// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.


using HelloShop.EventBus.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HelloShop.EventBus.Logging
{
    public class DistributedEventWorker(IServiceScopeFactory serviceScopeFactory) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = serviceScopeFactory.CreateScope();

                var eventBus = scope.ServiceProvider.GetRequiredService<IEventBus>();
                var eventLogService = scope.ServiceProvider.GetRequiredService<IDistributedEventLogService>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<DistributedEventWorker>>();

                try
                {
                    var failedEventLogs = await eventLogService.RetrieveEventLogsFailedToPublishAsync(stoppingToken);

                    foreach (var eventLog in failedEventLogs)
                    {
                        DistributedEvent @event = eventLog.DistributedEvent;

                        try
                        {
                            await eventLogService.MarkEventAsInProgressAsync(@event.Id, stoppingToken);
                            await eventBus.PublishAsync(@event, stoppingToken);
                            await eventLogService.MarkEventAsPublishedAsync(@event.Id, stoppingToken);
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex, "Publish through event bus failed for {EventId}.", @event.Id);
                            await eventLogService.MarkEventAsFailedAsync(@event.Id, stoppingToken);
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while retrieving failed event logs.");
                }

                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
        }
    }
}
