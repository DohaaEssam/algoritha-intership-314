using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using testvezeeta.Core_Layer.Domain;

namespace testvezeeta.Infrastructure_Layer
{
    public static class SeedDataService
    {
        public static async Task Initialize(IServiceProvider serviceProvider, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            string adminEmail = "admin@example.com";
            string adminPassword = "Admin@123";

            await SeedRoles(roleManager);
            await SeedAdminUser(userManager, adminEmail, adminPassword);
        }

        private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {

            string[] roleNames = { "Admin", "Patient","Doctor" };

            foreach (var roleName in roleNames)
            {
                var roleExists = await roleManager.RoleExistsAsync(roleName);

                if (!roleExists)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        private static async Task SeedAdminUser(UserManager<ApplicationUser> userManager, string email, string password)
        {
            var adminUser = await userManager.FindByEmailAsync(email);

            if (adminUser == null)
            {
                var user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                };

                var result = await userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }
    }
}
