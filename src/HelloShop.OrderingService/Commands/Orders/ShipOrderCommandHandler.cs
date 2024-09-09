// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.OrderingService.Entities.Orders;
using HelloShop.OrderingService.Infrastructure;
using HelloShop.OrderingService.Services;
using MediatR;

namespace HelloShop.OrderingService.Commands.Orders
{
    public class ShipOrderCommandHandler(OrderingServiceDbContext dbContext) : IRequestHandler<ShipOrderCommand, bool>
    {
        public async Task<bool> Handle(ShipOrderCommand request, CancellationToken cancellationToken)
        {
            var orderToUpdate = await dbContext.Set<Order>().FindAsync([request.OrderNumber], cancellationToken: cancellationToken);

            if (orderToUpdate == null)
            {
                return false;
            }

            orderToUpdate.OrderStatus = OrderStatus.Shipped;

            return await dbContext.SaveChangesAsync(cancellationToken) > 0;
        }
    }

    public class ShipOrderIdentifiedCommandHandler(IMediator mediator, IClientRequestManager requestManager) : IdentifiedCommandHandler<ShipOrderCommand, bool>(mediator, requestManager)
    {
        protected override bool CreateResultForDuplicateRequest() => true;
    }
}
