// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using Microsoft.Extensions.Logging;

namespace HelloShop.OrderingService.Services
{
    public class MessageService(ILogger<MessageService> logger) : IEmailSender, ISmsSender
    {
        public Task SendEmailAsync(string email, string subject, string message, CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Sending email to {Email} with subject {Subject} and message {Message}", email, subject, message);
            return Task.CompletedTask;
        }

        public Task SendSmsAsync(string number, string message, CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Sending SMS to {Number} with message {Message}", number, message);
            return Task.CompletedTask;
        }
    }
}
