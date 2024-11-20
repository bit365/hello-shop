// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

namespace HelloShop.OrderingService.Queries
{
    public class OrderDetailsItem
    {
        public int ProductId { get; init; }

        public required string ProductName { get; init; }

        public int Units { get; init; }

        public double UnitPrice { get; init; }

        public required string PictureUrl { get; init; }
    }
}