// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

namespace HelloShop.OrderingService.Entities.Idempotency
{
    public class ClientRequest
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }

        public DateTimeOffset Time { get; set; }
    }
}
