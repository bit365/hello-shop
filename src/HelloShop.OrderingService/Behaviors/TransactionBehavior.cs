// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.OrderingService.Extensions;
using HelloShop.OrderingService.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace HelloShop.OrderingService.Behaviors
{
    public class TransactionBehavior<TRequest, TResponse>(OrderingServiceDbContext dbContext, ILoggerFactory loggerFactory) : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger = loggerFactory.CreateLogger<TransactionBehavior<TRequest, TResponse>>();

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            TResponse? response = default;

            string typeName = request.GetGenericTypeName();

            try
            {
                if (dbContext.Database.CurrentTransaction != null)
                {
                    return await next();
                }

                var strategy = dbContext.Database.CreateExecutionStrategy();

                await strategy.ExecuteAsync(async () =>
                {
                    using var transaction = await dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

                    using (_logger.BeginScope(new List<KeyValuePair<string, object>> { new("TransactionContext", transaction.TransactionId) }))
                    {
                        _logger.LogInformation("Begin transaction {TransactionId} for {CommandName} {Command}", transaction.TransactionId, typeName, request);

                        response = await next();

                        _logger.LogInformation("Commit transaction {TransactionId} for {CommandName}", transaction.TransactionId, typeName);
                    }

                    await transaction.CommitAsync();
                });

                return response ?? throw new ApplicationException($"Command {typeName} returned null response");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Handling transaction for {CommandName} {Request}", typeName, request);
                throw;
            }
        }
    }
}
