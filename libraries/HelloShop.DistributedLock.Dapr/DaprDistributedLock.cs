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

            string? lockOwner = new StackTrace().GetFrame(1)?.GetMethod()?.DeclaringType?.Name;
            lockOwner ??= Guid.NewGuid().ToString();

#pragma warning disable DAPR_DISTRIBUTEDLOCK // 类型仅用于评估，在将来的更新中可能会被更改或删除。取消此诊断以继续。
            TryLockResponse response = await daprClient.Lock("lockstore", resourceId, lockOwner, expiryInSeconds, cancellationToken);
#pragma warning restore DAPR_DISTRIBUTEDLOCK // 类型仅用于评估，在将来的更新中可能会被更改或删除。取消此诊断以继续。

            return new DaprDistributedLockResult(response);
        }
    }
}