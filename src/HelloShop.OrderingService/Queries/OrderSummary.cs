// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

namespace HelloShop.OrderingService.Queries
{
    public class OrderSummary
    {
        public int OrderId { get; init; }

        public required string Status { get; init; }

        public DateTimeOffset OrderDate { get; init; }
    }
}
