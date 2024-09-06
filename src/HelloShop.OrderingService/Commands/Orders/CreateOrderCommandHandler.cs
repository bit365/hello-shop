// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.OrderingService.Services;
using MediatR;

namespace HelloShop.OrderingService.Commands.Orders
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, bool>
    {
        public Task<bool> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    public class CreateOrderIdentifiedCommandHandler(IMediator mediator, IClientRequestManager requestManager) : IdentifiedCommandHandler<CreateOrderCommand, bool>(mediator, requestManager)
    {
        protected override bool CreateResultForDuplicateRequest() => default;
    }
}
