// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using HelloShop.BasketService.Entities;


// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.BasketService.Protos;
using HelloShop.BasketService.Repositories;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace HelloShop.BasketService.Services
{
    [Authorize]
    public class CustomerBasketService(IBasketRepository repository, ILogger<CustomerBasketService> logger, IMapper mapper) : Basket.BasketBase
    {
        public override async Task<CustomerBasketResponse> GetBasket(Empty request, ServerCallContext context)
        {
            string? nameIdentifier = context.GetHttpContext().User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(nameIdentifier, out int userId))
            {
                throw new RpcException(new Status(StatusCode.Unauthenticated, "The caller is not authenticated."));
            }

            logger.LogDebug("Begin GetBasketById call from method {Method} for basket id {Id}", context.Method, userId);

            CustomerBasket? basket = await repository.GetBasketAsync(userId, context.CancellationToken);

            if (basket is not null)
            {
                mapper.Map<CustomerBasketResponse>(basket);
            }

            return new();
        }

        public override async Task<CustomerBasketResponse> UpdateBasket(UpdateBasketRequest request, ServerCallContext context)
        {
            string? nameIdentifier = context.GetHttpContext().User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(nameIdentifier, out int userId))
            {
                throw new RpcException(new Status(StatusCode.Unauthenticated, "The caller is not authenticated."));
            }

            logger.LogDebug("Begin UpdateBasket call from method {Method} for basket id {Id}", context.Method, userId);

            CustomerBasket customerBasket = mapper.Map<CustomerBasket>(request, opts => opts.AfterMap((src, dest) => dest.BuyerId = userId));

            CustomerBasket? result = await repository.UpdateBasketAsync(customerBasket, context.CancellationToken);

            result = result ?? throw new RpcException(new Status(StatusCode.NotFound, $"Basket with buyer id {userId} does not exist"));

            return mapper.Map<CustomerBasketResponse>(result);
        }

        public override async Task<Empty> DeleteBasket(Empty request, ServerCallContext context)
        {
            string? nameIdentifier = context.GetHttpContext().User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(nameIdentifier, out int userId))
            {
                throw new RpcException(new Status(StatusCode.Unauthenticated, "The caller is not authenticated."));
            }

            await repository.DeleteBasketAsync(userId, context.CancellationToken);

            return new();
        }
    }
}
