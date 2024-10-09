// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.OrderingService.Entities.EventLogs;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using HelloShop.ServiceDefaults.DistributedEvents.Abstractions;

namespace HelloShop.OrderingService.Infrastructure.EntityConfigurations.EventLogs
{
    public class DistributedEventLogEntityTypeConfiguration : IEntityTypeConfiguration<DistributedEventLog>
    {
        private static readonly JsonSerializerOptions s_jsonSerializerOptions = new(JsonSerializerDefaults.Web);

        public void Configure(EntityTypeBuilder<DistributedEventLog> builder)
        {
            builder.ToTable("DistributedEventLogs");

            builder.HasKey(x => x.EventId);
            builder.Property(x => x.EventTypeName).HasMaxLength(32);
            builder.Property(x => x.Status).HasConversion<string>();

            builder.Property(x => x.DistributedEvent).HasConversion(v => JsonSerializer.Serialize(v, v.GetType(), s_jsonSerializerOptions), v => JsonSerializer.Deserialize<DistributedEvent>(v, s_jsonSerializerOptions)!);
        }
    }
}
