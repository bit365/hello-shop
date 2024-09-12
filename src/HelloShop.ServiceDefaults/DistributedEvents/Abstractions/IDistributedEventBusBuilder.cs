// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using Microsoft.Extensions.DependencyInjection;

namespace HelloShop.ServiceDefaults.DistributedEvents.Abstractions
{
    public interface IDistributedEventBusBuilder
    {
        public IServiceCollection Services { get; }
    }
}
