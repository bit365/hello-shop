// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using MediatR;

namespace HelloShop.OrderingService.Commands.Orders
{
    public class CreateOrderCommand : IRequest<bool>
    {
        public int UserId { get; set; }

        public required string UserName { get; set; }

        #region Order Items

        public required IEnumerable<CreateOrderCommandItem> OrderItems { get; set; }

        #endregion

        #region PaymentMethod

        public required string CardAlias { get; set; }

        public required string CardNumber { get; set; }

        public required string CardHolderName { get; set; }

        public string? CardSecurityNumber { get; set; }

        public DateTimeOffset? CardExpiration { get; set; }

        #endregion

        #region Address

        public required string Country { get; set; }

        public required string State { get; set; }

        public required string City { get; set; }

        public required string Street { get; set; }

        public required string ZipCode { get; set; }

        #endregion

        public class CreateOrderCommandItem
        {
            public int ProductId { get; set; }

            public required string ProductName { get; set; }

            public required string PictureUrl { get; set; }

            public decimal UnitPrice { get; set; }

            public int Units { get; set; }
        }
    }
}
