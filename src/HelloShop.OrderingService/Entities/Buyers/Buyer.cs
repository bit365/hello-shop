// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

namespace HelloShop.OrderingService.Entities.Buyers
{
    public class Buyer
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public List<PaymentMethod>? PaymentMethods { get; set; }
    }
}
