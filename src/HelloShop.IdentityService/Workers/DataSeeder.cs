// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.


using HelloShop.IdentityService.Entities;
using HelloShop.IdentityService.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HelloShop.IdentityService.Workers
{
    public class DataSeeder(IServiceScopeFactory scopeFactory) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = scopeFactory.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<IdentityServiceDbContext>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            TimeProvider timeProvider = scope.ServiceProvider.GetRequiredService<TimeProvider>();

            await dbContext.Database.CreateExecutionStrategy().ExecuteAsync(async () =>
            {
                await using var transaction = await dbContext.Database.BeginTransactionAsync(stoppingToken);

                var adminRole = await roleManager.Roles.SingleOrDefaultAsync(x => x.Name == "AdminRole", stoppingToken);

                if (adminRole == null)
                {
                    adminRole = new Role { Name = "AdminRole", CreationTime = timeProvider.GetUtcNow() };
                    await roleManager.CreateAsync(adminRole);
                }

                var guestRole = await roleManager.Roles.SingleOrDefaultAsync(x => x.Name == "GuestRole", stoppingToken);

                if (guestRole == null)
                {
                    guestRole = new Role { Name = "GuestRole", CreationTime = timeProvider.GetUtcNow() };
                    await roleManager.CreateAsync(guestRole);
                }

                var adminUser = await userManager.FindByNameAsync("admin");

                if (adminUser == null)
                {
                    adminUser = new User
                    {
                        UserName = "admin",
                        Email = "admin@test.com",
                        CreationTime = timeProvider.GetUtcNow()
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
                        Email = "guest@test.com",
                        CreationTime = timeProvider.GetUtcNow()
                    };
                    await userManager.CreateAsync(guestUser, guestUser.UserName);
                }

                await userManager.AddToRoleAsync(guestUser, "GuestRole");

                if (!await dbContext.Set<PermissionGranted>().AnyAsync(cancellationToken: stoppingToken))
                {
                    dbContext.AddRange(
                        new PermissionGranted { PermissionName = "Catalog.Products", RoleId = 1 },
                        new PermissionGranted { PermissionName = "Catalog.Products.Create", RoleId = 1 },
                        new PermissionGranted { PermissionName = "Catalog.Products.Update", RoleId = 1 },
                        new PermissionGranted { PermissionName = "Catalog.Products.Delete", RoleId = 1 },
                        new PermissionGranted { PermissionName = "Catalog.Products.Details", RoleId = 1 }
                    );
                    await dbContext.SaveChangesAsync(stoppingToken);
                }

                await transaction.CommitAsync(stoppingToken);
            });
        }
    }
}
