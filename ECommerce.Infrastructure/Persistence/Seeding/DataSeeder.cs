using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Infrastructure.Persistence.Seeding
{
    public class DataSeeder
    {
        public static async Task SeedAllAsync(
                    UserManager<ApplicationUser> userManager,
                    ApplicationDbContext context)
        {
            await AdminSeeder.SeedAsync(userManager);
        }
    }
}
