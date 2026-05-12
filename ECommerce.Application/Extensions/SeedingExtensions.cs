using ECommerce.Application.Features.Database.Commands.Seed;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Application.Extensions;

/// <summary>
/// Extension methods for registering MediatR with seeding commands.
/// </summary>
public static class SeedingExtensions
{
    /// <summary>
    /// Registers the database seeding commands in the DI container.
    /// </summary>
    /// <param name="services">The service collection to add seeding services to.</param>
    /// <returns>The updated service collection for method chaining.</returns>
    public static IServiceCollection AddDatabaseSeeding(this IServiceCollection services)
    {
        // MediatR is typically already registered, but this can be used to ensure handlers are available
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(SeedDatabaseCommand).Assembly);
        });

        return services;
    }
}
