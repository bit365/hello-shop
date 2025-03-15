// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using System.Text.Json;
using System.Text.Json.Serialization;

namespace HelloShop.ServiceDefaults.DistributedEvents.Abstractions
{
    public record DistributedEvent
    {
        public DistributedEvent()
        {
            Id = Guid.NewGuid();
            CreationTime = TimeProvider.System.GetUtcNow();
        }

        public Guid Id { get; set; }

        public DateTimeOffset CreationTime { get; set; }

        [JsonExtensionData]
        public Dictionary<string, JsonElement>? ExtensionData { get; set; }
    }
}
