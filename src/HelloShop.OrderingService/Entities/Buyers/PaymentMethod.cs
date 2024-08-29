// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

namespace HelloShop.OrderingService.Entities.Buyers
{
    public class PaymentMethod
    {
        public int Id { get; set; }

        public int BuyerId { get; set; }

        public required string Alias { get; set; }

        public required string CardNumber { get; set; }

        public required string CardHolderName { get; set; }

        public string? SecurityNumber { get; set; }

        public DateTimeOffset? Expiration { get; set; }
    }
}
