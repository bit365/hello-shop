// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace HelloShop.EventBus.Abstractions
{
    public class EventBusOptions
    {
        public Dictionary<string, Type> EventTypes { get; } = [];

        public JsonSerializerOptions JsonSerializerOptions { get; } = new(DefaultSerializerOptions);

        internal static readonly JsonSerializerOptions DefaultSerializerOptions = new()
        {
            TypeInfoResolver = JsonSerializer.IsReflectionEnabledByDefault ? new DefaultJsonTypeInfoResolver() : JsonTypeInfoResolver.Combine()
        };
    }
}