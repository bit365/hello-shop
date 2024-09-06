// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using MediatR;

namespace HelloShop.OrderingService.Commands
{
    public class IdentifiedCommand<TRequest, TResponse>(TRequest command, Guid id) : IRequest<TResponse> where TRequest : IRequest<TResponse>
    {
        public TRequest Command { get; } = command;

        public Guid Id { get; } = id;
    }
}
