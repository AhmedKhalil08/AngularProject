using MediatR;

namespace ECommerce.Application.Features.Database.Commands.Seed;

/// <summary>
/// Master command to seed the entire database with initial data.
/// This command orchestrates the seeding of all entities across the application.
/// </summary>
public class SeedDatabaseCommand : IRequest<bool>
{
    /// <summary>
    /// Optional flag to control whether to skip seeding if data already exists.
    /// Default: true (will not re-seed if data exists).
    /// </summary>
    public bool SkipIfDataExists { get; set; } = true;
}
