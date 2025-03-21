﻿// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

namespace HelloShop.EventBus.Dapr
{
    public class DaprEventBusOptions
    {
        public string PubSubName { get; set; } = "event-bus-pubsub";

        public bool RequireAuthenticatedDaprApiToken { get; set; } = false;

        public string? DaprApiToken { get; set; }
    }
}
