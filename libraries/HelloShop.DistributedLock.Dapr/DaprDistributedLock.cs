// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using Dapr.Client;
using System.Diagnostics;

namespace HelloShop.DistributedLock.Dapr
{
    public class DaprDistributedLock(DaprClient daprClient) : IDistributedLock
    {
        public async Task<IDistributedLockResult> LockAsync(string resourceId, int expiryInSeconds = default, CancellationToken cancellationToken = default)
        {
            expiryInSeconds = expiryInSeconds == default ? 60 : expiryInSeconds;

            // The CallerMemberNameAttribute is used to get the name of the calling method.
            string? lockOwner = new StackTrace().GetFrame(1)?.GetMethod()?.DeclaringType?.Name;

#pragma warning disable CS0618
            TryLockResponse response = await daprClient.Lock("lockstore", resourceId, lockOwner, expiryInSeconds, cancellationToken);
#pragma warning restore CS0618

            return new DaprDistributedLockResult(response);
        }
    }
}