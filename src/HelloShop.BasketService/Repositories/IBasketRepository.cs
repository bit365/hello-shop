// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.BasketService.Entities;

namespace HelloShop.BasketService.Repositories
{
    public interface IBasketRepository
    {
        Task<CustomerBasket?> GetBasketAsync(int customerId, CancellationToken cancellationToken = default);

        Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket, CancellationToken cancellationToken = default);

        Task DeleteBasketAsync(int customerId, CancellationToken cancellationToken = default);
    }
}
