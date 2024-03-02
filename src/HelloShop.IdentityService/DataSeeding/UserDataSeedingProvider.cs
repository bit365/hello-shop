using HelloShop.IdentityService.Entities;
using HelloShop.IdentityService.EntityFrameworks;
using HelloShop.ServiceDefaults.Infrastructure;

namespace HelloShop.IdentityService.DataSeeding
{
    public class UserDataSeedingProvider(IdentityServiceDbContext dbContext) : IDataSeedingProvider
    {
        public async Task SeedingAsync(IServiceProvider serviceProvider)
        {
            var guestUser = dbContext.Set<User>().SingleOrDefault(x => x.UserName == "guest");

            if (guestUser is null)
            {
                await dbContext.Set<User>().AddAsync(new User
                {
                    UserName = "guest",
                    PasswordHash = "AQAAAAEAACcQAAAAEJ"
                });

                await dbContext.SaveChangesAsync();
            }
        }
    }
}
