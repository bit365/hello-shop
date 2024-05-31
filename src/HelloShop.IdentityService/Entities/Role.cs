// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using Microsoft.AspNetCore.Identity;

namespace HelloShop.IdentityService.Entities
{
    public class Role : IdentityRole<int>
    {
        public DateTimeOffset CreationTime { get; set; } = TimeProvider.System.GetUtcNow();
    }
}
