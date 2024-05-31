// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

namespace HelloShop.ApiService.Infrastructure
{
    public class ConfiguredServiceEndPoint
    {
        public required string ServiceName { get; set; }

        public IReadOnlyCollection<string>? Endpoints { get; set; }
    }
}