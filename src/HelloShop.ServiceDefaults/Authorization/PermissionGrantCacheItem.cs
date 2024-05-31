// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

namespace HelloShop.ServiceDefaults.Authorization;

public class PermissionGrantCacheItem(bool isGranted = false)
{
    public bool IsGranted { get; set; } = isGranted;

    public static string CreateCacheKey(int roleId, string name, string? resourceType = null, string? resourceId = null)
    {
        string cacheKey = $"acl:role:{roleId}:{name}";

        if (!string.IsNullOrWhiteSpace(resourceType))
        {
            cacheKey += $":{resourceType}";
        }

        if (!string.IsNullOrWhiteSpace(resourceId))
        {
            cacheKey += $":{resourceId}";
        }

        return cacheKey;
    }
}
