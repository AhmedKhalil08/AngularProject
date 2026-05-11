# Database Seeding Documentation

## Overview

This documentation covers the comprehensive database seeding mechanism implemented using **MediatR** and **Entity Framework Core** following **CQRS** (Command Query Responsibility Segregation) pattern and **Clean Architecture** principles.

## Architecture

### Components

1. **SeedDatabaseCommand** - CQRS Command that initiates the seeding process
2. **SeedDatabaseCommandHandler** - CQRS Handler that orchestrates all seeding operations
3. **SeedingExtensions** - DI extension for registering seeding services

## Features

✅ **CQRS Pattern Compliance**
- All logic contained within the MediatR handler
- Clear separation of concerns
- Single Responsibility Principle

✅ **EF Core Best Practices**
- `DbContext` injected via constructor
- `UserManager` injected for user management
- `CancellationToken` passed to all async EF operations
- Existence checks prevent duplicate data insertion

✅ **Data Quality**
- 10+ Products with realistic e-commerce data
- 5+ Categories with hierarchical structure
- 6 Users with different roles (Admin, Sellers, Customers)
- 2 Seller Profiles with approval status and earnings
- 5+ Promotional Codes with discount percentages
- Complete order history with 5 orders
- Address data with default address support
- Product reviews and ratings
- Wishlist entries
- Payment records with transaction tracking

✅ **Relationship Integrity**
- Proper Foreign Key relationships
- Navigation Properties correctly linked
- Cascading saves through SaveChangesAsync

## Seeding Data Structure

### 1. ApplicationUsers (6 records)
- **1 Admin** - System administrator with full privileges
- **2 Sellers** - Store owners with seller profiles
- **3 Customers** - Regular users who can browse and purchase

```
User Roles:
- Admin: admin@ecommerce.com
- Seller: seller@ecommerce.com, seller2@ecommerce.com
- Customer: customer1/2/3@ecommerce.com
```

### 2. Categories (5 records)
- Electronics
- Fashion
- Home & Kitchen
- Sports & Outdoors
- Books & Media

### 3. SellerProfiles (2 records)
- **TechHub Store** - Electronics specialist ($15,250.50 earnings)
- **Fashion Forward** - Fashion focus ($8,920.75 earnings)

### 4. Products (10 records)
Professional e-commerce products with:
- Realistic pricing ($34.99 - $1,299.99)
- Stock quantities (18 - 340 units)
- Product status (Available, OutOfStock, Discontinued)
- Seller associations

**Sample Products:**
- Wireless Noise-Canceling Headphones Pro: $349.99
- Ultra HD 4K Smart Television 55-inch: $799.99
- Professional Digital Camera DSLR: $1,299.99
- Men's Classic Cotton Business Shirt: $39.99
- Professional Stainless Steel Chef's Knife: $59.99
- Automatic Bean-to-Cup Espresso Machine: $599.99
- Yoga Mat Premium Non-Slip: $49.99
- The Clean Code Handbook: $34.99
- Portable Bluetooth Speaker Waterproof: $79.99

### 5. ProductImages (30+ records)
- Main image per product (IsMain = true)
- 2 additional images per product
- Placeholder URLs for demonstration

### 6. Banners (5 records)
Promotional banners with:
- Display order
- Active status
- Links to product filters
- Placeholder images

**Banner Examples:**
- Summer Sale - Up to 50% Off
- New Arrivals This Week
- Premium Electronics Collection
- Fashion Forward Spring Collection
- Free Shipping on Orders Over $50

### 7. PromoCodes (5 records)
Discount codes with:
- Discount percentages (15% - 100%)
- Usage limits and tracking
- Expiry dates

**Promo Codes:**
- WELCOME20: 20% off (expires in 3 months)
- SUMMER50: 50% off (expires in 2 months)
- LOYALTY15: 15% off (expires in 6 months)
- FLASH30: 30% off (expires in 7 days)
- VIPFREE: 100% off/Free (expires in 1 month)

### 8. Addresses (5+ records)
Customer shipping addresses with:
- Default address marking
- Complete address details
- Phone numbers
- User associations

### 9. Carts (3 records)
Shopping carts for each customer

### 10. Reviews (6 records)
Product reviews with:
- 3-5 star ratings
- Realistic comments
- Creation timestamps

### 11. Wishlists (6 records)
Customer wishlist items with product associations

### 12. Orders (5 records)
Complete order history with:
- Order status (Pending, Confirmed, Shipped, Delivered)
- Total amounts
- Shipping addresses
- Promo code associations
- Payment method
- Order notes

**Order Statuses:**
- Delivered (2 orders)
- Shipped (1 order)
- Confirmed (1 order)
- Pending (1 order)

### 13. OrderItems (6 records)
Individual line items for each order with:
- Product quantity
- Unit pricing
- Order association

### 14. Payments (5 records)
Payment records with:
- Transaction IDs
- Payment method (CreditCard, PayPal, Wallet, CashOnDelivery)
- Payment status (Pending, Completed)
- Paid timestamps

## Usage

### 1. Register in Startup/Program.cs

```csharp
using ECommerce.Application.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddApplicationServices();
builder.Services.AddDatabaseSeeding();

var app = builder.Build();
```

### 2. Call the Seeding Command

**In Program.cs (during startup):**

```csharp
using ECommerce.Application.Features.Database.Commands.Seed;
using MediatR;

var mediator = app.Services.GetRequiredService<IMediator>();
await mediator.Send(new SeedDatabaseCommand { SkipIfDataExists = true });
```

**In a Controller:**

```csharp
[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("seed-database")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> SeedDatabase(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new SeedDatabaseCommand { SkipIfDataExists = true },
            cancellationToken
        );

        return result ? Ok("Database seeded successfully") : BadRequest("Seeding failed");
    }
}
```

**In a Console Application:**

```csharp
var mediator = serviceProvider.GetRequiredService<IMediator>();
var result = await mediator.Send(
    new SeedDatabaseCommand { SkipIfDataExists = true },
    CancellationToken.None
);

Console.WriteLine(result ? "Seeding completed successfully" : "Seeding failed");
```

## Key Design Decisions

### 1. Existence Checks
Before seeding each entity type, the handler checks if data already exists:

```csharp
if (await _context.Categories.AnyAsync(cancellationToken))
    return; // Skip if data exists
```

**Benefits:**
- Prevents duplicate data insertion
- Safe to call multiple times
- Idempotent operation

### 2. Seeding Order
Data is seeded in dependency order:

```
1. ApplicationUsers (no dependencies)
2. Categories (no dependencies)
3. SellerProfiles (depends on Users)
4. Products (depends on Categories, SellerProfiles)
5. ProductImages (depends on Products)
6. Banners (no dependencies)
7. PromoCodes (no dependencies)
8. Addresses (depends on Users)
9. Carts (depends on Users)
10. Reviews (depends on Products, Users)
11. Wishlists (depends on Users, Products)
12. Orders (depends on Users, Addresses, PromoCodes)
13. OrderItems (depends on Orders, Products)
14. Payments (depends on Orders)
```

### 3. Cancellation Token Support
All async operations properly support cancellation:

```csharp
await _context.SaveChangesAsync(cancellationToken);
await _context.Users.AnyAsync(cancellationToken);
```

### 4. Realistic Data
- Professional product names and descriptions
- Realistic pricing ($34.99 - $1,299.99)
- Proper inventory levels
- Authentic order and payment workflows
- Valid email addresses
- Phone numbers with proper formatting

## Configuration

### Skip if Data Exists (Default: true)

```csharp
// Skip seeding if data already exists
await mediator.Send(new SeedDatabaseCommand { SkipIfDataExists = true });

// Force reseed even if data exists (caution!)
// Only for development environments
await mediator.Send(new SeedDatabaseCommand { SkipIfDataExists = false });
```

## Error Handling

The handler wraps all operations in a try-catch block:

```csharp
try
{
    // Seeding operations
}
catch (Exception ex)
{
    throw new InvalidOperationException("Failed to seed database.", ex);
}
```

**Recommendation:** Add logging in production:

```csharp
catch (Exception ex)
{
    _logger.LogError(ex, "Database seeding failed");
    throw new InvalidOperationException("Failed to seed database.", ex);
}
```

## Best Practices Applied

✅ **CQRS Pattern**
- Command: SeedDatabaseCommand
- Handler: SeedDatabaseCommandHandler
- Clear separation of read/write operations

✅ **Dependency Injection**
- Constructor injection of DbContext and UserManager
- Loose coupling with external dependencies
- Testable architecture

✅ **Async/Await**
- All I/O operations are async
- CancellationToken support throughout
- Non-blocking database operations

✅ **Entity Framework Core**
- AddRangeAsync for batch inserts
- SaveChangesAsync after each entity type
- Proper navigation property setup
- Foreign key relationships maintained

✅ **Clean Code**
- Clear method names describing intent
- XML documentation for all methods
- Logical data organization
- Single Responsibility Principle

✅ **English Only**
- All code comments in English
- All dummy data strings in English
- No Arabic or other language content

## Testing

### Unit Testing Example

```csharp
[TestFixture]
public class SeedDatabaseCommandHandlerTests
{
    private Mock<ApplicationDbContext> _mockContext;
    private Mock<UserManager<ApplicationUser>> _mockUserManager;
    private SeedDatabaseCommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _mockContext = new Mock<ApplicationDbContext>();
        _mockUserManager = new Mock<UserManager<ApplicationUser>>(
            Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null
        );
        _handler = new SeedDatabaseCommandHandler(_mockContext.Object, _mockUserManager.Object);
    }

    [Test]
    public async Task Handle_WhenDataDoesNotExist_ShouldSeedDatabase()
    {
        // Arrange
        var command = new SeedDatabaseCommand { SkipIfDataExists = true };
        _mockContext.Setup(x => x.Categories.AnyAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.IsTrue(result);
    }
}
```

## Troubleshooting

### Issue: Foreign Key Constraint Violation
**Solution:** Ensure seeding order is correct. Check that parent entities are created before child entities.

### Issue: Duplicate Key Exception
**Solution:** Verify the `SkipIfDataExists` check is working. Clear database and retry.

### Issue: UserManager Not Available
**Solution:** Ensure `UserManager<ApplicationUser>` is registered in DI container in Program.cs:

```csharp
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
```

## Performance Considerations

- Seeding uses `AddRangeAsync` for batch operations
- SaveChangesAsync is called after each entity type to maintain relationships
- Existence checks are minimal and efficient
- Consider disabling change tracking for production bulk operations

## Security Notes

- Default passwords are used for testing only
- In production, use secure password generation
- Restrict seeding to Admin role only
- Use environment-based configuration
- Never seed sensitive data in production

## References

- **MediatR Documentation:** https://github.com/jbogard/MediatR
- **Entity Framework Core:** https://docs.microsoft.com/en-us/ef/core/
- **CQRS Pattern:** https://martinfowler.com/bliki/CQRS.html
- **Clean Architecture:** https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html

## Future Enhancements

- [ ] Add logging for seeding operations
- [ ] Create seed data factories for easier testing
- [ ] Add database reset command
- [ ] Implement progress reporting for large datasets
- [ ] Add environment-specific seeding (Dev, Staging, Prod)
- [ ] Create custom seed data profiles
- [ ] Add analytics data seeding
