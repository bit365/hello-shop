// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using Dapr.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloShop.ServiceDefaults.DistributedLocks
{
#pragma warning disable CS0618
    public class DaprDistributedLockResult(TryLockResponse tryLockResponse) : IDistributedLockResult
#pragma warning restore CS0618
    {
        public async ValueTask DisposeAsync()
        {
            await tryLockResponse.DisposeAsync().ConfigureAwait(false);
            GC.SuppressFinalize(this);
        }
    }
}