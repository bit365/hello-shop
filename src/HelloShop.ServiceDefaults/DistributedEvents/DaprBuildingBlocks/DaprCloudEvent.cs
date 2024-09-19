// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using Dapr;
using System.Text.Json.Serialization;

namespace HelloShop.ServiceDefaults.DistributedEvents.DaprBuildingBlocks
{
    public class DaprCloudEvent<TData>(TData data) : CloudEvent<TData>(data)
    {
        /// <summary>
        /// CloudEvent 'pubsubname' attribute.
        /// </summary>
        [JsonPropertyName("pubsubname")]
        public required string PubSubName { get; init; }

        /// <summary>
        /// CloudEvent 'topic' attribute.
        /// </summary>
        [JsonPropertyName("topic")]
        public required string Topic { get; init; }

        /// <summary>
        /// CloudEvent 'time' attribute.
        /// </summary>
        [JsonPropertyName("time")]
        public required DateTimeOffset Time { get; init; }
    }
}
