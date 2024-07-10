// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.BasketService.Entities;
using Microsoft.Extensions.Caching.Distributed;

namespace HelloShop.BasketService.Repositories
{
    public class DistributedCacheBasketRepository(IDistributedCache cache) : IBasketRepository
    {
        private const string BasketKeyPrefix = "basket";

        private static string GetBasketKey(int customerId) => $"{BasketKeyPrefix}:{customerId}";

        public async Task DeleteBasketAsync(int customerId, CancellationToken token = default) => await cache.RemoveAsync(GetBasketKey(customerId), token);

        public async Task<CustomerBasket?> GetBasketAsync(int customerId, CancellationToken token = default) => await cache.GetObjectAsync<CustomerBasket>(GetBasketKey(customerId), token);

        public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket, CancellationToken token = default)
        {
            DistributedCacheEntryOptions options = new() { SlidingExpiration = TimeSpan.MaxValue };

            await cache.SetObjectAsync(GetBasketKey(basket.BuyerId), basket, options, token);

            return await GetBasketAsync(basket.BuyerId, token);
        }
    }
}
