// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

namespace HelloShop.OrderingService.Models.Orders
{
    public class BasketItem
    {
        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public required string ProductName { get; set; }

        public required string PictureUrl { get; set; }

        public decimal UnitPrice { get; set; }
    }
}
