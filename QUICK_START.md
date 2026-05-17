# Quick Start Guide - Database Seeding

## 5-Minute Setup

### Step 1: Verify Dependencies
Ensure your `Program.cs` has MediatR registered:

```csharp
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
```

### Step 2: Register Seeding Extension
Add to `Program.cs`:

```csharp
using ECommerce.Application.Extensions;

builder.Services.AddDatabaseSeeding();
```

### Step 3: Call Seeding During Startup
Add to `Program.cs` before building the app:

```csharp
var app = builder.Build();

// Seed the database
using (var scope = app.Services.CreateAsyncScope())
{
    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
    await mediator.Send(new SeedDatabaseCommand { SkipIfDataExists = true });
}

app.Run();
```

### Step 4: Run Your Application
```bash
dotnet run
```

That's it! Your database will be automatically seeded with:
- ✅ 6 Users (1 Admin, 2 Sellers, 3 Customers)
- ✅ 5 Categories
- ✅ 10 Products with images
- ✅ 5 Banners
- ✅ 5 Promo Codes
- ✅ Complete order history
- ✅ Reviews and wishlists

## What Gets Seeded?

| Entity | Count | Details |
|--------|-------|---------|
| Users | 6 | Admin, 2 Sellers, 3 Customers |
| Categories | 5 | Electronics, Fashion, Home, Sports, Books |
| Products | 10 | $34.99 - $1,299.99 price range |
| Product Images | 30+ | 3 images per product |
| Banners | 5 | Promotional banners |
| Promo Codes | 5 | 15%-100% discounts |
| Addresses | 5+ | Customer shipping addresses |
| Orders | 5 | Complete order history |
| Payments | 5 | Payment records |
| Reviews | 6 | Product ratings and comments |
| Wishlists | 6 | Customer favorite items |

## Test Credentials

```
Admin User:
Email: admin@ecommerce.com
Password: SecurePassword123!

Seller User:
Email: seller@ecommerce.com
Password: SecurePassword123!

Customer User:
Email: customer1@ecommerce.com
Password: SecurePassword123!
```

## Database Reset (Development Only)

To completely reset and reseed the database:

```csharp
// In your DbContext or using EF CLI:
dotnet ef database drop --force
dotnet ef database update
// Then run your application to reseed
```

## Troubleshooting

### ❌ "SeedDatabaseCommand not found"
**Fix:** Ensure you called `AddDatabaseSeeding()` in Program.cs

### ❌ "DbContext not registered"
**Fix:** Verify your DbContext is registered in Program.cs:
```csharp
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString)
);
```

### ❌ "Data not seeding"
**Fix:** Check that `SkipIfDataExists = true` is working. If database has partial data:
```csharp
// Set to false to force reseed (caution!)
new SeedDatabaseCommand { SkipIfDataExists = false }
```

## Advanced Usage

### Custom Seeding in API Controller

```csharp
[HttpPost("admin/seed")]
[Authorize(Roles = "Admin")]
public async Task<IActionResult> SeedDatabase()
{
    var result = await _mediator.Send(
        new SeedDatabaseCommand { SkipIfDataExists = true }
    );
    return result 
        ? Ok("Database seeded successfully") 
        : BadRequest("Seeding failed");
}
```

### Use in Unit Tests

```csharp
[SetUp]
public async Task Setup()
{
    // Seed test database
    await _mediator.Send(new SeedDatabaseCommand { SkipIfDataExists = false });
}
```

## Files Created

- ✅ `SeedDatabaseCommand.cs` - CQRS Command
- ✅ `SeedDatabaseCommandHandler.cs` - Command Handler (600+ lines)
- ✅ `SeedingExtensions.cs` - DI Extension
- ✅ `SEEDING_DOCUMENTATION.md` - Full Documentation
- ✅ `QUICK_START.md` - This file

## Next Steps

1. Run your application
2. Check database - should be populated with seed data
3. Test with provided credentials
4. Customize seed data as needed in handler
5. Add logging for production use

## Support

For issues or questions, refer to:
- Full documentation: `SEEDING_DOCUMENTATION.md`
- Entity models: `ECommerce.Domain/Entities/`
- DbContext: `ECommerce.Infrastructure/Persistence/Contexts/ApplicationDbContext.cs`
