// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.OrderingService.Extensions;
using MediatR;

namespace HelloShop.OrderingService.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling command {CommandName} {Request}", request.GetGenericTypeName(), request);

            var response = await next();

            logger.LogInformation("Command {CommandName} handled {Response}", request.GetGenericTypeName(), response);

            return response;
        }
    }
}
