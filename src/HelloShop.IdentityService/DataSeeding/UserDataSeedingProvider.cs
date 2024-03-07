using HelloShop.IdentityService.Entities;
using HelloShop.IdentityService.EntityFrameworks;
using HelloShop.ServiceDefaults.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace HelloShop.IdentityService.DataSeeding
{
    public class UserDataSeedingProvider(UserManager<User> userManager, RoleManager<Role> roleManager) : IDataSeedingProvider
    {
        public async Task SeedingAsync(IServiceProvider serviceProvider)
        {
            var adminRole = await roleManager.FindByNameAsync("AdminRole");

            if (adminRole == null)
            {
                await roleManager.CreateAsync(new Role
                {
                    Name = "AdminRole"
                });
            }

            var guestRole = await roleManager.FindByNameAsync("GuestRole");

            if (guestRole == null)
            {
                await roleManager.CreateAsync(new Role
                {
                    Name = "GuestRole"
                });
            }

            var adminUser = await userManager.FindByNameAsync("admin");

            if (adminUser == null)
            {
                await userManager.CreateAsync(new User
                {
                    UserName = "admin",
                    Email = "admin@test.com"
                },"admin");
            }

            if (adminUser!=null)
            {
                await userManager.AddToRolesAsync(adminUser, ["AdminRole", "GuestRole"]);
            }

            var guestUser = await userManager.FindByNameAsync("guest");

            if (guestUser == null)
            {
                await userManager.CreateAsync(new User
                {
                    UserName = "guest",
                    Email = "guest@test.com"
                },"guest");
            }

            if (guestUser!=null)
            {
                await userManager.AddToRoleAsync(guestUser, "GuestRole");
            }
        }
    }
}
