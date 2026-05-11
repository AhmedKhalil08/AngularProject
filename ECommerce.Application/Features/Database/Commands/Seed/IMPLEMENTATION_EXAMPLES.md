# Implementation Examples

This file provides real-world implementation examples for the database seeding mechanism.

## Example 1: Basic Implementation in Program.cs

```csharp
using ECommerce.Application.Extensions;
using ECommerce.Application.Features.Database.Commands.Seed;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Add Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Add MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// Add Seeding Extension
builder.Services.AddDatabaseSeeding();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // Seed database on startup in development
    using (var scope = app.Services.CreateAsyncScope())
    {
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        try
        {
            var result = await mediator.Send(
                new SeedDatabaseCommand { SkipIfDataExists = true },
                CancellationToken.None
            );

            if (result)
            {
                Console.WriteLine("✓ Database seeding completed successfully");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✗ Database seeding failed: {ex.Message}");
        }
    }
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
```

## Example 2: API Endpoint for Seeding

```csharp
using ECommerce.Application.Features.Database.Commands.Seed;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<AdminController> _logger;

    public AdminController(IMediator mediator, ILogger<AdminController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Seeds the database with initial data.
    /// Admin access required.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success message</returns>
    [HttpPost("seed-database")]
    public async Task<IActionResult> SeedDatabase(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Starting database seeding...");

            var result = await _mediator.Send(
                new SeedDatabaseCommand { SkipIfDataExists = true },
                cancellationToken
            );

            if (!result)
            {
                _logger.LogWarning("Database seeding returned false");
                return BadRequest(new { message = "Database seeding failed" });
            }

            _logger.LogInformation("Database seeding completed successfully");
            return Ok(new { message = "Database seeded successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during database seeding");
            return StatusCode(500, new { message = "An error occurred during seeding", error = ex.Message });
        }
    }

    /// <summary>
    /// Force reseed the database (caution: in development only).
    /// </summary>
    [HttpPost("reseed-database")]
    public async Task<IActionResult> ReseedDatabase(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogWarning("Force reseeding database - this will recreate seed data");

            var result = await _mediator.Send(
                new SeedDatabaseCommand { SkipIfDataExists = false },
                cancellationToken
            );

            return result
                ? Ok(new { message = "Database reseeded successfully" })
                : BadRequest(new { message = "Database reseeding failed" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during force database reseeding");
            return StatusCode(500, new { message = "An error occurred during reseeding", error = ex.Message });
        }
    }
}
```

## Example 3: Integration with Hosted Service

```csharp
using ECommerce.Application.Features.Database.Commands.Seed;
using MediatR;
using Microsoft.Extensions.Hosting;

namespace ECommerce.Infrastructure.Services;

/// <summary>
/// Background service that seeds the database on application startup.
/// </summary>
public class DatabaseSeedingService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DatabaseSeedingService> _logger;

    public DatabaseSeedingService(IServiceProvider serviceProvider, ILogger<DatabaseSeedingService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Starting database seeding service...");

            using (var scope = _serviceProvider.CreateAsyncScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                var result = await mediator.Send(
                    new SeedDatabaseCommand { SkipIfDataExists = true },
                    cancellationToken
                );

                if (result)
                {
                    _logger.LogInformation("✓ Database seeding completed successfully");
                }
                else
                {
                    _logger.LogWarning("⚠ Database seeding returned false");
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "✗ Error during database seeding");
            throw;
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Database seeding service stopped");
        return Task.CompletedTask;
    }
}

// Register in Program.cs:
builder.Services.AddHostedService<DatabaseSeedingService>();
```

## Example 4: Unit Testing the Handler

```csharp
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using ECommerce.Application.Features.Database.Commands.Seed;
using ECommerce.Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;

namespace ECommerce.Application.Tests.Features.Database;

[TestFixture]
public class SeedDatabaseCommandHandlerTests
{
    private Mock<ApplicationDbContext> _mockContext;
    private Mock<UserManager<ApplicationUser>> _mockUserManager;
    private SeedDatabaseCommandHandler _handler;
    private CancellationToken _cancellationToken;

    [SetUp]
    public void Setup()
    {
        _mockContext = new Mock<ApplicationDbContext>();
        _mockUserManager = CreateMockUserManager();
        _handler = new SeedDatabaseCommandHandler(_mockContext.Object, _mockUserManager.Object);
        _cancellationToken = CancellationToken.None;
    }

    [Test]
    public async Task Handle_WhenDataExists_ShouldReturnTrue()
    {
        // Arrange
        var command = new SeedDatabaseCommand { SkipIfDataExists = true };

        _mockContext.Setup(x => x.Categories.AnyAsync(_cancellationToken))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    public async Task Handle_WhenDataDoesNotExist_ShouldSeedDatabase()
    {
        // Arrange
        var command = new SeedDatabaseCommand { SkipIfDataExists = true };

        _mockContext.Setup(x => x.Categories.AnyAsync(_cancellationToken))
            .ReturnsAsync(false);

        _mockContext.Setup(x => x.Users.AnyAsync(_cancellationToken))
            .ReturnsAsync(false);

        _mockContext.Setup(x => x.SellerProfiles.AnyAsync(_cancellationToken))
            .ReturnsAsync(false);

        _mockContext.Setup(x => x.Products.AnyAsync(_cancellationToken))
            .ReturnsAsync(false);

        _mockContext.Setup(x => x.ProductImages.AnyAsync(_cancellationToken))
            .ReturnsAsync(false);

        _mockContext.Setup(x => x.Banners.AnyAsync(_cancellationToken))
            .ReturnsAsync(false);

        _mockContext.Setup(x => x.PromoCodes.AnyAsync(_cancellationToken))
            .ReturnsAsync(false);

        _mockContext.Setup(x => x.Addresses.AnyAsync(_cancellationToken))
            .ReturnsAsync(false);

        _mockContext.Setup(x => x.Carts.AnyAsync(_cancellationToken))
            .ReturnsAsync(false);

        _mockContext.Setup(x => x.Reviews.AnyAsync(_cancellationToken))
            .ReturnsAsync(false);

        _mockContext.Setup(x => x.Wishlists.AnyAsync(_cancellationToken))
            .ReturnsAsync(false);

        _mockContext.Setup(x => x.Orders.AnyAsync(_cancellationToken))
            .ReturnsAsync(false);

        _mockContext.Setup(x => x.OrderItems.AnyAsync(_cancellationToken))
            .ReturnsAsync(false);

        _mockContext.Setup(x => x.Payments.AnyAsync(_cancellationToken))
            .ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    public async Task Handle_WithExceptionDuringSeeding_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var command = new SeedDatabaseCommand { SkipIfDataExists = true };

        _mockContext.Setup(x => x.Categories.AnyAsync(_cancellationToken))
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidOperationException>(
            async () => await _handler.Handle(command, _cancellationToken)
        );

        Assert.That(ex.Message, Does.Contain("Failed to seed database"));
    }

    private Mock<UserManager<ApplicationUser>> CreateMockUserManager()
    {
        var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
        return new Mock<UserManager<ApplicationUser>>(
            userStoreMock.Object,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null
        );
    }
}
```

## Example 5: Conditional Seeding Based on Environment

```csharp
var builder = WebApplication.CreateBuilder(args);

// ... other configurations ...

var app = builder.Build();

// Seed database only in development and staging
if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    using (var scope = app.Services.CreateAsyncScope())
    {
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

        try
        {
            logger.LogInformation("Environment: {0}", app.Environment.EnvironmentName);
            logger.LogInformation("Starting database seeding...");

            var result = await mediator.Send(new SeedDatabaseCommand { SkipIfDataExists = true });

            if (result)
                logger.LogInformation("✓ Database seeding completed");
            else
                logger.LogWarning("⚠ Database seeding returned false");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "✗ Database seeding failed");
            // Don't throw - allow application to continue
        }
    }
}

app.Run();
```

## Example 6: Custom Seed Data Extension

```csharp
/// <summary>
/// Extension to add custom seeding logic for specific test scenarios.
/// </summary>
public class CustomSeedingService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public CustomSeedingService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task SeedTestDataAsync(CancellationToken cancellationToken)
    {
        // Create test user
        var testUser = new ApplicationUser
        {
            UserName = "test@example.com",
            Email = "test@example.com",
            FullName = "Test User",
            Role = UserRole.Customer,
            IsActive = true
        };

        await _userManager.CreateAsync(testUser, "TestPassword123!");

        // Create test products
        var category = new Category
        {
            Name = "Test Category",
            Description = "For testing purposes"
        };

        await _context.Categories.AddAsync(category, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var product = new Product
        {
            Name = "Test Product",
            Description = "A test product",
            Price = 99.99m,
            Stock = 10,
            CategoryId = category.Id,
            Status = ProductStatus.Available
        };

        await _context.Products.AddAsync(product, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
```

## Example 7: Seeding with Progress Reporting

```csharp
public class SeedDatabaseCommandHandlerWithProgress : IRequestHandler<SeedDatabaseCommand, bool>
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IProgress<SeedingProgress> _progress;

    public SeedDatabaseCommandHandlerWithProgress(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        IProgress<SeedingProgress> progress = null)
    {
        _context = context;
        _userManager = userManager;
        _progress = progress;
    }

    public async Task<bool> Handle(SeedDatabaseCommand request, CancellationToken cancellationToken)
    {
        try
        {
            ReportProgress(SeedingStage.Starting, 0);

            if (request.SkipIfDataExists && await _context.Categories.AnyAsync(cancellationToken))
            {
                ReportProgress(SeedingStage.Completed, 100);
                return true;
            }

            ReportProgress(SeedingStage.Users, 10);
            await SeedApplicationUsers(cancellationToken);

            ReportProgress(SeedingStage.Categories, 20);
            await SeedCategories(cancellationToken);

            ReportProgress(SeedingStage.Products, 50);
            await SeedProducts(cancellationToken);

            ReportProgress(SeedingStage.Orders, 75);
            await SeedOrders(cancellationToken);

            ReportProgress(SeedingStage.Completed, 100);
            return true;
        }
        catch (Exception ex)
        {
            ReportProgress(SeedingStage.Error, 0, ex.Message);
            throw new InvalidOperationException("Failed to seed database.", ex);
        }
    }

    private void ReportProgress(SeedingStage stage, int percentage, string message = null)
    {
        _progress?.Report(new SeedingProgress
        {
            Stage = stage,
            Percentage = percentage,
            Message = message ?? stage.ToString()
        });
    }
}

public class SeedingProgress
{
    public SeedingStage Stage { get; set; }
    public int Percentage { get; set; }
    public string Message { get; set; }
}

public enum SeedingStage
{
    Starting,
    Users,
    Categories,
    Products,
    Orders,
    Completed,
    Error
}
```

## Usage Examples

All of these implementations can be used in your application to provide flexible, robust database seeding capabilities. Choose the approach that best fits your needs.
