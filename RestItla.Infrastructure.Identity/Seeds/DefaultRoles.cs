using Microsoft.AspNetCore.Identity;

using RestItla.Domain.Enum;
using RestItla.Infrastructure.Identity.Entities;

namespace RestItla.Infrastructure.Identity.Seeds
{
    public static class DefaultRoles
    {

        public static async Task SeedAsync(UserManager<ApplicationUser> userManager,
                                           RoleManager<IdentityRole> roleManager)
        {
            HashSet<string?> identityRoles = roleManager.Roles
                                                         .Select(i => i.Name)
                                                         .ToHashSet();
            foreach (var type in Enum.GetNames<Role>())
            {
                if (!identityRoles.Contains(type))
                {
                    await roleManager.CreateAsync(new IdentityRole(type));
                }
            }
        }
    }

}