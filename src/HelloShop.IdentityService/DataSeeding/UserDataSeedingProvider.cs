using HelloShop.IdentityService.Entities;
using HelloShop.IdentityService.EntityFrameworks;
using HelloShop.ServiceDefaults.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HelloShop.IdentityService.DataSeeding
{
    public class UserDataSeedingProvider(UserManager<User> userManager, RoleManager<Role> roleManager) : IDataSeedingProvider
    {
        public async Task SeedingAsync(IServiceProvider ServiceProvider)
        {
            var adminRole = await roleManager.Roles.SingleOrDefaultAsync(x => x.Name == "AdminRole");

            if (adminRole == null)
            {
                adminRole = new Role { Name = "AdminRole", };
                await roleManager.CreateAsync(adminRole);
            }

            var guestRole = await roleManager.Roles.SingleOrDefaultAsync(x => x.Name == "GuestRole");

            if (guestRole == null)
            {
                guestRole = new Role { Name = "GuestRole", };
                await roleManager.CreateAsync(guestRole);
            }

            var adminUser = await userManager.FindByNameAsync("admin");

            if (adminUser == null)
            {
                adminUser = new User
                {
                    UserName = "admin",
                    Email = "admin@test.com"
                };
                await userManager.CreateAsync(adminUser, adminUser.UserName);
            }

            await userManager.AddToRolesAsync(adminUser, ["AdminRole", "GuestRole"]);

            var guestUser = await userManager.FindByNameAsync("guest");

            if (guestUser == null)
            {
                guestUser = new User
                {
                    UserName = "guest",
                    Email = "guest@test.com"
                };
                await userManager.CreateAsync(guestUser, guestUser.UserName);
            }

            await userManager.AddToRoleAsync(guestUser, "GuestRole");

            if (userManager.Users.Count() < 30)
            {
                for (int i = 0; i < 30; i++)
                {
                    var user = new User
                    {
                        UserName = $"user{i}",
                        Email = $"test{i}@test.com",
                    };
                    await userManager.CreateAsync(user, user.UserName);
                }
            }
        }
    }
}
