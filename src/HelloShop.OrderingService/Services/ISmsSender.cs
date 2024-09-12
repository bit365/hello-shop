// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

namespace HelloShop.OrderingService.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message, CancellationToken cancellationToken = default);
    }
}
