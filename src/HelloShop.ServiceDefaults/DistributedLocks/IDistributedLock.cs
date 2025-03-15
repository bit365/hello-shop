// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

namespace HelloShop.ServiceDefaults.DistributedLocks
{
    public interface IDistributedLock
    {
        public abstract Task<IDistributedLockResult> LockAsync(string resourceId, int expiryInSeconds = default, CancellationToken cancellationToken = default);
    }
}
