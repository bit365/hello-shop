// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.OrderingService.Services;
using MediatR;

namespace HelloShop.OrderingService.Commands
{
    public abstract class IdentifiedCommandHandler<TRequest, TResponse>(IMediator mediator, IClientRequestManager requestManager) : IRequestHandler<IdentifiedCommand<TRequest, TResponse>, TResponse> where TRequest : IRequest<TResponse>
    {
        protected virtual TResponse? CreateResultForDuplicateRequest() => default;

        public async Task<TResponse> Handle(IdentifiedCommand<TRequest, TResponse> request, CancellationToken cancellationToken)
        {
            if (await requestManager.ExistAsync(request.Id))
            {
                return CreateResultForDuplicateRequest() ?? throw new NotImplementedException();
            }

            await requestManager.CreateRequestForCommandAsync<TRequest>(request.Id);

            return await mediator.Send(request.Command, cancellationToken);
        }
    }
}
