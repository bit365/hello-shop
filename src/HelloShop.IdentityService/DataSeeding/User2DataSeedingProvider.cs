using HelloShop.IdentityService.Entities;
using HelloShop.IdentityService.EntityFrameworks;
using HelloShop.ServiceDefaults.Infrastructure;

namespace HelloShop.IdentityService.DataSeeding
{
    public class User2DataSeedingProvider : IDataSeedingProvider
    {
        public async Task SeedingAsync(IServiceProvider serviceProvider)
        {
            var dbContext = serviceProvider.GetRequiredService<IdentityServiceDbContext>();

            var guestUser = dbContext.Set<User>().SingleOrDefault(x => x.UserName == "guest2");

            if (guestUser is null)
            {
                await dbContext.Set<User>().AddAsync(new User
                {
                    UserName = "guest2",
                    PasswordHash = "AQAAAAEAACcQAAAAEJ"
                });

                await dbContext.SaveChangesAsync();
            }
        }
    }
}
