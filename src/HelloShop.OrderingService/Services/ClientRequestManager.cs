// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.OrderingService.Entities.Idempotency;
using HelloShop.OrderingService.Infrastructure;

namespace HelloShop.OrderingService.Services
{
    public class ClientRequestManager(OrderingServiceDbContext dbContext) : IClientRequestManager
    {
        public async Task<bool> ExistAsync(Guid id) => await dbContext.FindAsync<ClientRequest>(id) is not null;

        public async Task CreateRequestForCommandAsync<T>(Guid id)
        {
            if (await ExistAsync(id))
            {
                throw new Exception($"Request with {id} already exists");
            }

            await dbContext.AddAsync(new ClientRequest
            {
                Id = id,
                Name = typeof(T).Name,
                Time = DateTime.UtcNow
            });

            await dbContext.SaveChangesAsync();
        }
    }
}
