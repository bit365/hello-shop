// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

namespace HelloShop.EventBus.RabbitMQ
{
    public class RabbitMQEventBusOptions
    {
        public string ExchangeName { get; set; } = "event-bus-exchange";

        public required string QueueName { get; set; }

        public int RetryCount { get; set; } = 10;
    }
}
