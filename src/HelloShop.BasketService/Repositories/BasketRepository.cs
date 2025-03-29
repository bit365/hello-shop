// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.BasketService.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Hybrid;
using System.Threading;

namespace HelloShop.BasketService.Repositories
{
    public class BasketRepository(HybridCache cache) : IBasketRepository
    {
        private const string BasketKeyPrefix = "basket";

        private static string GetBasketKey(int customerId) => $"{BasketKeyPrefix}:{customerId}";

        public async Task DeleteBasketAsync(int customerId, CancellationToken cancellationToken = default) => await cache.RemoveAsync(GetBasketKey(customerId), cancellationToken);
        
        public async Task<CustomerBasket?> GetBasketAsync(int customerId, CancellationToken cancellationToken = default)
        {
            return await cache.GetOrCreateAsync(GetBasketKey(customerId), async cancel => await Task.FromResult<CustomerBasket?>(default), cancellationToken: cancellationToken);
        }

        public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket, CancellationToken cancellationToken = default)
        {
            HybridCacheEntryOptions options = new()
            {
                Expiration = TimeSpan.MaxValue,
                LocalCacheExpiration = TimeSpan.FromMinutes(5),
            };

            await cache.SetAsync(GetBasketKey(basket.BuyerId), basket, options, cancellationToken: cancellationToken);

            return await GetBasketAsync(basket.BuyerId, cancellationToken);
        }
    }
}
