// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

namespace HelloShop.OrderingService.Entities.Orders
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public required string ProductName { get; set; }

        public required string PictureUrl { get; set; }

        public decimal UnitPrice { get; set; }

        public int Units { get; set; }
    }
}
