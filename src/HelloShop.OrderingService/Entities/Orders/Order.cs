// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using System.Diagnostics.CodeAnalysis;

namespace HelloShop.OrderingService.Entities.Orders
{
    public class Order
    {
        public int Id { get; set; }

        public DateTimeOffset OrderDate { get; private set; } = DateTimeOffset.UtcNow;

        public required Address Address { get; init; }

        public OrderStatus OrderStatus { get; set; } = OrderStatus.Submitted;

        public int BuyerId { get; set; }

        public int? PaymentMethodId { get; set; }

        public required IReadOnlyCollection<OrderItem> OrderItems { get; init; }

        public string? Description { get; set; }

        /// <summary>
        /// EF Core cannot set navigation properties using a constructor.
        /// The constructor can be public, private, or have any other accessibility. 
        /// </summary>
        private Order() { }

        [SetsRequiredMembers]
        public Order(int buyerId, Address address, IEnumerable<OrderItem> orderItems)
        {
            Address = address;
            BuyerId = buyerId;
            OrderItems = orderItems.ToList();
        }
    }
}
