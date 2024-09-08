// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

namespace HelloShop.OrderingService.Models.Orders
{
    public class CreateOrderRequest
    {
        public required List<BasketItem> Items { get; init; }

        public required string Country { get; init; }

        public required string State { get; init; }

        public required string City { get; init; }

        public required string Street { get; init; }

        public required string ZipCode { get; init; }

        public required string CardAlias { get; init; }

        public required string CardNumber { get; init; }

        public required string CardHolderName { get; init; }

        public string? CardSecurityNumber { get; init; }

        public DateTimeOffset? CardExpiration { get; init; }
    }
}
