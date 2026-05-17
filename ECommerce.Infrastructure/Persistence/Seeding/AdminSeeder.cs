using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Infrastructure.Persistence.Seeding
{
    public class AdminSeeder
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
        {
            var adminEmail = "admin@ecommerce.com";

            if (await userManager.FindByEmailAsync(adminEmail) != null)
                return;

            var admin = new ApplicationUser
            {
                UserName = "admin",
                Email = adminEmail,
                FullName = "System Admin",
                PhoneNumber = "0000000000",
                Role = UserRole.Admin,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(admin, "Admin@123");

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                Console.WriteLine($"Admin seed failed: {errors}");
            }
        }
    }
}
