// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

namespace HelloShop.OrderingService.Queries
{
    public interface IOrderQueries
    {
        Task<OrderDetails> GetOrderAsync(int id);

        Task<IEnumerable<OrderSummary>> GetOrdersFromUserAsync(int userId);
    }
}
