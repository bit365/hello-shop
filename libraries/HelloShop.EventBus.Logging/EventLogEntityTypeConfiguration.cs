// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.EventBus.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace HelloShop.EventBus.Logging
{
    public class EventLogEntityTypeConfiguration : IEntityTypeConfiguration<DistributedEventLog>
    {
        private static readonly JsonSerializerOptions s_jsonSerializerOptions = new(JsonSerializerDefaults.Web);

        public void Configure(EntityTypeBuilder<DistributedEventLog> builder)
        {
            builder.HasKey(x => x.EventId);
            builder.Property(x => x.EventTypeName).HasMaxLength(64);
            builder.Property(x => x.Status).HasConversion<string>();

            builder.Property(x => x.DistributedEvent).HasConversion(v => JsonSerializer.Serialize(v, v.GetType(), s_jsonSerializerOptions), v => JsonSerializer.Deserialize<DistributedEvent>(v, s_jsonSerializerOptions)!);
        }
    }
}