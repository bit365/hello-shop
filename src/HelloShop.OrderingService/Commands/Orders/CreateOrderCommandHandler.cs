// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using AutoMapper;
using HelloShop.OrderingService.Entities.Buyers;
using HelloShop.OrderingService.Entities.Orders;
using HelloShop.OrderingService.Infrastructure;
using HelloShop.OrderingService.LocalEvents;
using HelloShop.OrderingService.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HelloShop.OrderingService.Commands.Orders
{
    public class CreateOrderCommandHandler(IMediator mediator, OrderingServiceDbContext dbContext, IMapper mapper) : IRequestHandler<CreateOrderCommand, bool>
    {
        public async Task<bool> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            Address address = mapper.Map<Address>(request);

            IEnumerable<OrderItem> orderItems = mapper.Map<IEnumerable<OrderItem>>(request.OrderItems);

            Buyer? buyer = await dbContext.Set<Buyer>().FindAsync([request.UserId], cancellationToken);

            if (buyer == null)
            {
                buyer = new() { Id = request.UserId, Name = request.UserName };
                await dbContext.AddAsync(buyer, cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);
            }

            PaymentMethod? paymentMethod = await dbContext.Set<PaymentMethod>().SingleOrDefaultAsync(pm => pm.BuyerId == buyer.Id && pm.CardNumber == request.CardNumber, cancellationToken);

            if (paymentMethod == null)
            {
                paymentMethod = new() { BuyerId = buyer.Id, Alias = request.CardAlias, CardNumber = request.CardNumber, SecurityNumber = request.CardSecurityNumber, CardHolderName = request.CardHolderName, Expiration = request.CardExpiration };
                await dbContext.AddAsync(paymentMethod, cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);
            }

            Order order = new(buyer.Id, address, orderItems) { PaymentMethodId = paymentMethod.Id };

            await dbContext.AddAsync(order, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            await mediator.Publish(new OrderStartedLocalEvent(order), cancellationToken);

            return await Task.FromResult(true);
        }
    }

    public class CreateOrderIdentifiedCommandHandler(IMediator mediator, IClientRequestManager requestManager) : IdentifiedCommandHandler<CreateOrderCommand, bool>(mediator, requestManager)
    {
        protected override bool CreateResultForDuplicateRequest() => default;
    }
}
