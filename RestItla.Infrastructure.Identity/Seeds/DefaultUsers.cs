using Microsoft.AspNetCore.Identity;

using RestItla.Domain.Enum;
using RestItla.Infrastructure.Identity.Entities;

namespace RestItla.Infrastructure.Identity.Seeds
{
    public static class DefaultUsers
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager,
                                           RoleManager<IdentityRole> roleManager)
        {
            ApplicationUser mesero = new()
            {
                UserName = "basicuser@email.com",
                Email = "basicuser@email.com",
                Name = "BasicJohn",
                Surname = "BasicDoe",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            ApplicationUser? user = await userManager.FindByEmailAsync(mesero.Email);
            if (user == null)
            {
                await userManager.CreateAsync(mesero, "123Pa$$word!");
                await userManager.AddToRoleAsync(mesero, nameof(Role.Waiter));
            }

            ApplicationUser admin = new()
            {
                UserName = "adminuser@email.com",
                Email = "adminuser@email.com",
                Name = "John",
                Surname = "Doe",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            var user2 = await userManager.FindByEmailAsync(admin.Email);
            if (user2 == null)
            {
                await userManager.CreateAsync(admin, "123Pa$$word!");
                await userManager.AddToRoleAsync(admin, nameof(Role.Admin));
            }

            ApplicationUser superAdmin = new()
            {
                UserName = "superadminuser@email.com",
                Email = "superadminuser@email.com",
                Name = "SuperJohn",
                Surname = "SuperDoe",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            var user3 = await userManager.FindByEmailAsync(superAdmin.Email);
            if (user3 == null)
            {
                await userManager.CreateAsync(superAdmin, "123Pa$$word!");
                await userManager.AddToRoleAsync(superAdmin, nameof(Role.Waiter));
                await userManager.AddToRoleAsync(superAdmin, nameof(Role.Admin));
            }
        }
    }
}