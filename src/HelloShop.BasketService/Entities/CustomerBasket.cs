// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

namespace HelloShop.BasketService.Entities
{
    public class CustomerBasket
    {
        public int BuyerId { get; set; }

        public List<BasketItem> Items { get; set; } = [];
    }
}
