# Visual Integration Guide

## 📊 Architecture Diagram

```
┌─────────────────────────────────────────────────────────────┐
│                        API Layer                             │
│  ┌────────────────────────────────────────────────────────┐ │
│  │  AdminController                                        │ │
│  │  [POST] /api/admin/seed-database                       │ │
│  └────────────┬─────────────────────────────────────────┘ │
└───────────────┼─────────────────────────────────────────────┘
                │
┌───────────────┼─────────────────────────────────────────────┐
│   Application │ Layer (MediatR)                              │
│  ┌────────────▼─────────────────────────────────────────┐  │
│  │  SeedDatabaseCommand (IRequest<bool>)                │  │
│  │  - SkipIfDataExists: bool                            │  │
│  └────────────┬─────────────────────────────────────────┘  │
│               │                                             │
│  ┌────────────▼─────────────────────────────────────────┐  │
│  │  SeedDatabaseCommandHandler (IRequestHandler)        │  │
│  │  - Handle(command, cancellationToken)                │  │
│  │  - 14 private seeding methods                        │  │
│  └────────────┬─────────────────────────────────────────┘  │
└───────────────┼─────────────────────────────────────────────┘
                │
┌───────────────┼─────────────────────────────────────────────┐
│  Infrastructure Layer (EF Core)                             │
│  ┌────────────▼─────────────────────────────────────────┐  │
│  │  ApplicationDbContext                                │  │
│  │  ├─ DbSet<ApplicationUser>                          │  │
│  │  ├─ DbSet<Category>                                 │  │
│  │  ├─ DbSet<Product>                                  │  │
│  │  ├─ DbSet<SellerProfile>                            │  │
│  │  ├─ DbSet<Order>                                    │  │
│  │  ├─ DbSet<Payment>                                  │  │
│  │  └─ ... (14 DbSets total)                          │  │
│  └────────────┬─────────────────────────────────────────┘  │
│               │                                             │
│  ┌────────────▼─────────────────────────────────────────┐  │
│  │  SQL Server Database                                 │  │
│  │  ├─ Users Table (6 records)                         │  │
│  │  ├─ Categories Table (5 records)                    │  │
│  │  ├─ Products Table (10 records)                     │  │
│  │  └─ ... (14 tables total, 200+ records)            │  │
│  └────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────┘
```

## 🔄 Execution Flow

```
Start Application
    ↓
Program.cs Startup
    ↓
Register Services
    ├─ AddDbContext
    ├─ AddIdentity
    ├─ AddMediatR
    └─ AddDatabaseSeeding()
    ↓
Build Application
    ↓
Create Service Scope
    ↓
Get IMediator from DI
    ↓
Send SeedDatabaseCommand
    ↓
SeedDatabaseCommandHandler.Handle()
    ├─ Check if data exists (SkipIfDataExists)
    │  └─ If YES → Return true
    │
    ├─ If NO → Proceed with seeding:
    │
    ├─ [1] SeedApplicationUsers()
    │  └─ Create 6 users with different roles
    │
    ├─ [2] SeedCategories()
    │  └─ Create 5 product categories
    │
    ├─ [3] SeedSellerProfiles()
    │  └─ Create 2 seller stores (linked to sellers)
    │
    ├─ [4] SeedProducts()
    │  └─ Create 10 products (linked to categories & sellers)
    │
    ├─ [5] SeedProductImages()
    │  └─ Create 30+ product images (linked to products)
    │
    ├─ [6] SeedBanners()
    │  └─ Create 5 promotional banners
    │
    ├─ [7] SeedPromoCodes()
    │  └─ Create 5 discount codes
    │
    ├─ [8] SeedAddresses()
    │  └─ Create 5+ customer addresses
    │
    ├─ [9] SeedCarts()
    │  └─ Create 3 shopping carts
    │
    ├─ [10] SeedReviews()
    │  └─ Create 6 product reviews
    │
    ├─ [11] SeedWishlists()
    │  └─ Create 6 wishlist items
    │
    ├─ [12] SeedOrders()
    │  └─ Create 5 orders with status tracking
    │
    ├─ [13] SeedOrderItems()
    │  └─ Create 6 order line items
    │
    └─ [14] SeedPayments()
       └─ Create 5 payment records
    ↓
Return true
    ↓
Application Running with Seeded Data ✓
```

## 🔀 Data Flow Diagram

```
ApplicationUsers (6)
    ├─── UserManager.CreateAsync() ─┐
    │                                │
    │    ├─ Admin                    │
    │    ├─ Seller1 ─┬─────→ SellerProfile1
    │    ├─ Seller2 ─┤─────→ SellerProfile2
    │    ├─ Customer1 ├─────→ Address1, Address2, Cart1
    │    ├─ Customer2 ├─────→ Address3, Address4, Cart2
    │    └─ Customer3 └─────→ Address5, Cart3
    │
    └─→ Order Author
        └─→ Review Author
        └─→ Wishlist Author

Categories (5)
    └─→ Product Category (10 products)
        └─→ ProductImages (30+)
        └─→ Review Products
        └─→ Wishlist Products
        └─→ CartItem Products
        └─→ OrderItem Products

SellerProfiles (2)
    └─→ Product Seller

Products (10)
    ├─→ ProductImages (3 per product)
    ├─→ Reviews (6 total)
    ├─→ CartItems (0 on seed)
    ├─→ Wishlists (6 total)
    └─→ OrderItems (6 total)

Orders (5)
    ├─→ OrderItems (6 total)
    ├─→ Payments (5 total)
    ├─→ Addresses (shipping)
    ├─→ Users (customer)
    └─→ PromoCodes (optional)

PromoCodes (5)
    └─→ Orders (applied to some)

Banners (5)
    └─ Independent (no relationships)
```

## 📋 Dependency Matrix

```
                    │ User │ Cat │ Prod│ Sel │ Img │ Ban │ Prm │ Addr│ Cart│ Rev │ Wish│ Ord │ OI  │ Pay │
────────────────────┼──────┼─────┼─────┼─────┼─────┼─────┼─────┼─────┼─────┼─────┼─────┼─────┼─────┼─────┤
ApplicationUser     │      │     │     │     │     │     │     │  X  │  X  │  X  │  X  │  X  │     │     │
Category            │      │     │  X  │     │     │     │     │     │     │     │     │     │     │     │
Product             │      │     │     │     │  X  │     │     │     │     │  X  │  X  │     │  X  │     │
SellerProfile       │  X   │     │  X  │     │     │     │     │     │     │     │     │     │     │     │
ProductImage        │      │     │  X  │     │     │     │     │     │     │     │     │     │     │     │
Banner              │      │     │     │     │     │     │     │     │     │     │     │     │     │     │
PromoCode           │      │     │     │     │     │     │     │     │     │     │     │  X  │     │     │
Address             │  X   │     │     │     │     │     │     │     │     │     │     │  X  │     │     │
Cart                │  X   │     │     │     │     │     │     │     │     │     │     │     │     │     │
Review              │  X   │     │  X  │     │     │     │     │     │     │     │     │     │     │     │
Wishlist            │  X   │     │  X  │     │     │     │     │     │     │     │     │     │     │     │
Order               │  X   │     │     │     │     │     │  ○  │  X  │     │     │     │     │  X  │     │
OrderItem           │      │     │  X  │     │     │     │     │     │     │     │     │  X  │     │     │
Payment             │      │     │     │     │     │     │     │     │     │     │     │  X  │     │     │
```

**Legend:**
- `X` = Required dependency (creates after parent)
- `○` = Optional dependency (foreign key nullable)
- Empty = No dependency

## 🧩 Component Interaction

```
┌─────────────────────────────────────────────────┐
│  SeedDatabaseCommandHandler                      │
│                                                  │
│  Dependencies Injected:                          │
│  ├─ ApplicationDbContext context                │
│  └─ UserManager<ApplicationUser> userManager    │
│                                                  │
│  For each entity:                               │
│  ├─ Check: await context.Set<Entity>           │
│  │         .AnyAsync(cancellationToken)         │
│  │                                              │
│  │ If NO data exists:                          │
│  │ ├─ Create new instances                     │
│  │ ├─ Set all properties                       │
│  │ ├─ Link foreign keys                        │
│  │ ├─ AddRangeAsync(entities)                  │
│  │ └─ SaveChangesAsync(cancellationToken)      │
│  │                                              │
│  └─ Return true                                │
│                                                  │
└─────────────────────────────────────────────────┘
```

## 📱 Integration Sequence

```
Time  Event                        Code Location
────  ────────────────────────────  ──────────────────────────
 0    Application Start
      ↓
 1    Program.cs Main()            Program.cs
      ├─ AddDatabaseSeeding()      Extensions/SeedingExtensions.cs
      ├─ Build app
      └─ CreateAsyncScope()
      ↓
 2    GetRequiredService<IMediator>
      ├─ Resolve from DI
      └─ Send Command
      ↓
 3    MediatR Routing
      ├─ Find handler
      └─ Invoke Handle()
      ↓
 4    SeedDatabaseCommandHandler    SeedDatabaseCommandHandler.cs
      │
      ├─ Check Categories.AnyAsync()
      ├─ If skip = true && exists → Return true
      │
      ├─ SeedApplicationUsers()     (Line ~70)
      ├─ SeedCategories()           (Line ~110)
      ├─ SeedSellerProfiles()       (Line ~145)
      ├─ SeedProducts()             (Line ~185)
      ├─ SeedProductImages()        (Line ~245)
      ├─ SeedBanners()              (Line ~275)
      ├─ SeedPromoCodes()           (Line ~300)
      ├─ SeedAddresses()            (Line ~345)
      ├─ SeedCarts()                (Line ~395)
      ├─ SeedReviews()              (Line ~410)
      ├─ SeedWishlists()            (Line ~450)
      ├─ SeedOrders()               (Line ~485)
      ├─ SeedOrderItems()           (Line ~535)
      └─ SeedPayments()             (Line ~570)
      ↓
 5    Database Committed
      ├─ SaveChangesAsync()
      └─ All records persisted
      ↓
 6    Return true
      └─ Handler completes
      ↓
 7    Application Ready
      └─ With seeded data ✓
```

## 🎨 State Diagram

```
┌──────────────────────────────────────────────────────────┐
│ Database States During Seeding                           │
└──────────────────────────────────────────────────────────┘

  START
   │
   ├─→ [Empty Database]
   │   └─→ Command sent (SkipIfDataExists = true)
   │       └─→ AnyAsync check fails (no data)
   │
   ├─→ [Seeding Phase]
   │   ├─ Users being inserted
   │   ├─ Categories being inserted
   │   ├─ Products being inserted
   │   └─ ... (all 14 entity types)
   │
   ├─→ [Partial Data]
   │   └─→ (if seeding fails here, incomplete data exists)
   │       └─→ Next seeding attempt may skip or merge
   │
   └─→ [Complete Seeded Database]
       ├─ 6 Users
       ├─ 5 Categories
       ├─ 10 Products
       ├─ ... (all 200+ records)
       └─ Ready for use ✓
```

## 🔐 Security Flow

```
User Request
    │
    ├─→ [Authenticate]
    │   └─→ JWT Token verified
    │
    ├─→ [Authorize]
    │   └─→ [Authorize(Roles = "Admin")]
    │       └─→ Check user role
    │
    └─→ [SeedDatabase Endpoint]
        └─→ If authorized:
            ├─ Execute SeedDatabaseCommand
            ├─ Update database
            └─ Return success

        └─→ If not authorized:
            └─ Return 403 Forbidden
```

## 📊 Entity Count Growth

```
Entities Created During Seeding:

Users:              0 → 6
Categories:         0 → 5
SellerProfiles:     0 → 2
Products:           0 → 10
ProductImages:      0 → 30+
Banners:            0 → 5
PromoCodes:         0 → 5
Addresses:          0 → 5+
Carts:              0 → 3
Reviews:            0 → 6
Wishlists:          0 → 6
Orders:             0 → 5
OrderItems:         0 → 6
Payments:           0 → 5
                   ───────
TOTAL:              0 → 200+
```

## 🎯 Success Criteria

```
✓ All 6 users created with correct roles
✓ All 5 categories created
✓ All 10 products linked to categories
✓ All 30+ product images linked to products
✓ All seller profiles linked to sellers
✓ All addresses linked to customers
✓ All carts linked to customers
✓ All reviews linked to products and users
✓ All wishlists linked to products and users
✓ All orders linked to customers and addresses
✓ All order items linked to orders and products
✓ All payments linked to orders
✓ All promo codes created
✓ All banners created
✓ Database consistent and queryable
✓ No duplicate data on re-seeding
```

## 📈 Performance Characteristics

```
Operation               Estimated Time    Records
─────────────────────   ─────────────────  ──────
SeedApplicationUsers    10-20 ms           6
SeedCategories          5-10 ms            5
SeedSellerProfiles      5-10 ms            2
SeedProducts            10-20 ms           10
SeedProductImages       10-20 ms           30+
SeedBanners             5-10 ms            5
SeedPromoCodes          5-10 ms            5
SeedAddresses           5-10 ms            5+
SeedCarts               5-10 ms            3
SeedReviews             5-10 ms            6
SeedWishlists           5-10 ms            6
SeedOrders              10-20 ms           5
SeedOrderItems          5-10 ms            6
SeedPayments            5-10 ms            5
─────────────────────────────────────────────────
Total Seeding Time      100-200 ms         200+
```

## ✨ Visual Data Hierarchy

```
├─ Admin User (1)
│  └─ [No seed relationships]
│
├─ Seller1 (1)
│  ├─ SellerProfile: TechHub Store
│  │  ├─ Products: 5
│  │  │  ├─ ProductImages: 15
│  │  │  ├─ Reviews: 3
│  │  │  └─ Wishlists: 2
│  │  └─ Earnings: $15,250.50
│  │
│  └─ [No addresses]
│
├─ Seller2 (1)
│  ├─ SellerProfile: Fashion Forward
│  │  ├─ Products: 5
│  │  │  ├─ ProductImages: 15
│  │  │  ├─ Reviews: 3
│  │  │  └─ Wishlists: 2
│  │  └─ Earnings: $8,920.75
│  │
│  └─ [No addresses]
│
├─ Customer1 (1)
│  ├─ Addresses: 2
│  ├─ Cart: 1 (empty)
│  ├─ Orders: 2
│  │  ├─ Order1: $1,249.98 (Delivered)
│  │  │  ├─ OrderItems: 2
│  │  │  ├─ Payments: 1 (Completed)
│  │  │  └─ PromoCode: WELCOME20
│  │  │
│  │  └─ Order2: $179.97 (Delivered)
│  │     ├─ OrderItems: 1
│  │     ├─ Payments: 1 (Completed)
│  │     └─ PromoCode: None
│  │
│  ├─ Reviews: 2
│  └─ Wishlists: 2
│
├─ Customer2 (1)
│  ├─ Addresses: 2
│  ├─ Cart: 1 (empty)
│  ├─ Orders: 2
│  │  ├─ Order3: $749.99 (Shipped)
│  │  └─ Order5: $599.99 (Pending)
│  ├─ Reviews: 2
│  └─ Wishlists: 2
│
└─ Customer3 (1)
   ├─ Addresses: 1
   ├─ Cart: 1 (empty)
   ├─ Orders: 1
   │  └─ Order4: $89.99 (Confirmed)
   ├─ Reviews: 1
   └─ Wishlists: 1
```

---

This visual guide provides comprehensive architecture and data flow documentation for the seeding system.
