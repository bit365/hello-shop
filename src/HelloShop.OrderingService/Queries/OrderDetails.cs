// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

namespace HelloShop.OrderingService.Queries
{
    public class OrderDetails
    {
        public int OrderId { get; init; }

        public required string Status { get; init; }

        public DateTimeOffset OrderDate { get; init; }

        public required string Country { get; init; }

        public required string State { get; init; }

        public required string City { get; init; }

        public required string Street { get; init; }

        public required string ZipCode { get; init; }

        public string? Description { get; init; }

        public required List<OrderDetailsItem> OrderItems { get; set; }
    }
}