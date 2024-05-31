// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.ServiceDefaults.Constants;

namespace HelloShop.ServiceDefaults.Authorization;

public interface IAuthorizationResource
{
    string ResourceType => GetType().Name;

    string ResourceId => GetType().GetProperty(EntityConnstants.DefaultKey)?.GetValue(this)?.ToString() ?? throw new NotImplementedException();
}