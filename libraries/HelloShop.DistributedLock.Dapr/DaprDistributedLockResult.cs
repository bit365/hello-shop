// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using Dapr.Client;

namespace HelloShop.DistributedLock.Dapr
{
#pragma warning disable DAPR_DISTRIBUTEDLOCK // 类型仅用于评估，在将来的更新中可能会被更改或删除。取消此诊断以继续。
    public class DaprDistributedLockResult(TryLockResponse tryLockResponse) : IDistributedLockResult
#pragma warning restore DAPR_DISTRIBUTEDLOCK // 类型仅用于评估，在将来的更新中可能会被更改或删除。取消此诊断以继续。
    {
        public async ValueTask DisposeAsync()
        {
            await tryLockResponse.DisposeAsync().ConfigureAwait(false);
            GC.SuppressFinalize(this);
        }
    }
}
