// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

namespace HelloShop.ServiceDefaults.Authorization;

public record struct ResourceInfo(string ResourceType, string ResourceId) : IAuthorizationResource
{
    public override readonly string ToString() => $"{ResourceType}:{ResourceId}";
}

