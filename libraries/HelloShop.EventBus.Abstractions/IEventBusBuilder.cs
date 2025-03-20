// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using Microsoft.Extensions.DependencyInjection;

namespace HelloShop.EventBus.Abstractions
{
    public interface IEventBusBuilder
    {
        public IServiceCollection Services { get; }
    }
}
