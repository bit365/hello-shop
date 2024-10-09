// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.ServiceDefaults.DistributedEvents.Abstractions;
using System.Diagnostics.CodeAnalysis;

namespace HelloShop.OrderingService.Entities.EventLogs
{
    public enum DistributedEventStatus { NotPublished, InProgress, Published, PublishedFailed }

    public class DistributedEventLog
    {
        public Guid EventId { get; set; }

        public required string EventTypeName { get; set; }

        public required DistributedEvent DistributedEvent { get; set; }

        public DistributedEventStatus Status { get; set; } = DistributedEventStatus.NotPublished;

        public int TimesSent { get; set; }

        public DateTimeOffset CreationTime { get; set; } = DateTimeOffset.UtcNow;

        public required Guid TransactionId { get; set; }

        /// <summary>
        /// EF Core cannot set navigation properties using a constructor.
        /// The constructor can be public, private, or have any other accessibility. 
        /// </summary>
        private DistributedEventLog() { }

        [SetsRequiredMembers]
        public DistributedEventLog(DistributedEvent @event, Guid transactionId)
        {
            EventId = @event.Id;
            EventTypeName = @event.GetType().Name;
            DistributedEvent = @event;
            TransactionId = transactionId;
        }
    }
}
