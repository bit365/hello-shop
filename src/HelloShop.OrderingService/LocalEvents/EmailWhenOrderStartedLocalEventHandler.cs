// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.OrderingService.Services;
using MediatR;

namespace HelloShop.OrderingService.LocalEvents
{
    public class EmailWhenOrderStartedLocalEventHandler(IEmailSender emailSender) : INotificationHandler<OrderStartedLocalEvent>
    {
        public Task Handle(OrderStartedLocalEvent notification, CancellationToken cancellationToken)
        {
            emailSender.SendEmailAsync(notification.Order.BuyerId.ToString(), "Order started", $"Your order {notification.Order.Id} has started", cancellationToken);

            return Task.CompletedTask;
        }
    }
}