using ECommerce.Application.Features.Database.Commands.Seed;
using ECommerce.Infrastructure.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.API.Extensions
{
    public static class DatabaseInitializationExtension
    {
        public static async Task ExecuteDatabaseSeedingAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<Program>>();

            try
            {
                var context = services.GetRequiredService<ApplicationDbContext>();
                await context.Database.MigrateAsync();

                var mediator = services.GetRequiredService<IMediator>();
                await mediator.Send(new SeedDatabaseCommand { SkipIfDataExists = true });

                logger.LogInformation("Database migrated and seeded successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while migrating or seeding the database.");
            }
        }
    }

}
