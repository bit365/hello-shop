// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.OrderingService.Entities.Orders;
using HelloShop.OrderingService.Infrastructure;
using HelloShop.OrderingService.Services;
using MediatR;

namespace HelloShop.OrderingService.Commands.Orders
{
    public class CancelOrderCommandHandler(OrderingServiceDbContext dbContext) : IRequestHandler<CancelOrderCommand, bool>
    {
        public async Task<bool> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
        {
            var orderToUpdate = await dbContext.Set<Order>().FindAsync([request.OrderNumber], cancellationToken);

            if (orderToUpdate == null)
            {
                return false;
            }

            orderToUpdate.OrderStatus = OrderStatus.Cancelled;

            return await dbContext.SaveChangesAsync(cancellationToken) > 0;
        }
    }

    public class CancelOrderIdentifiedCommandHandler(IMediator mediator, IClientRequestManager requestManager) : IdentifiedCommandHandler<CancelOrderCommand, bool>(mediator, requestManager)
    {
        protected override bool CreateResultForDuplicateRequest() => true;
    }
}
