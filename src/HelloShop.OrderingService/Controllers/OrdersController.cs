﻿// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using AutoMapper;
using HelloShop.OrderingService.Commands;
using HelloShop.OrderingService.Commands.Orders;
using HelloShop.OrderingService.Models.Orders;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Claims;

namespace HelloShop.OrderingService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController(ILogger<OrdersController> logger, IMediator mediator, IMapper mapper) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromHeader(Name = "x-request-id")] Guid requestId, CreateOrderRequest request)
        {
            using (logger.BeginScope(new List<KeyValuePair<string, object>> { new("IdentifiedCommandId", requestId) }))
            {
                string? nameIdentifier = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                string? userName = User.FindFirst(ClaimTypes.Name)?.Value;

                if (!int.TryParse(nameIdentifier, out int userId) || string.IsNullOrWhiteSpace(userName))
                {
                    throw new InvalidOperationException("User id or name not found in claims.");
                }

                CreateOrderCommand createOrderCommand = mapper.Map<CreateOrderCommand>(request, opts => opts.AfterMap((src, dest) =>
                {
                    dest.UserId = userId;
                    dest.UserName = userName;
                }));

                var createOrderIdentifiedCommand = new IdentifiedCommand<CreateOrderCommand, bool>(createOrderCommand, requestId);

                logger.LogInformation("Sending create order command.");

                try
                {
                    var result = await mediator.Send(createOrderIdentifiedCommand);
                    return result ? Ok() : Problem(detail: "Create order failed to process.", statusCode: 500);

                }
                catch (ApplicationException ex) when (ex.InnerException is FluentValidation.ValidationException validationException)
                {
                    logger.LogWarning("Validation error in create order command.");

                    ModelStateDictionary modelState = validationException.Errors.Aggregate(ModelState, (acc, error) =>
                    {
                        acc.AddModelError(error.PropertyName, error.ErrorMessage);
                        return acc;
                    });

                    return ValidationProblem(modelState);
                }
            }
        }
    }
}