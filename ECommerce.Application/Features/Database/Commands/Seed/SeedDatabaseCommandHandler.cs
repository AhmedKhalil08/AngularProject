using Microsoft.EntityFrameworkCore;
using System.Linq;
using ECommerce.Application.Interfaces.Persistence;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Application.Features.Database.Commands.Seed;

/// <summary>
/// Handler for seeding the entire database with realistic e-commerce dummy data.
/// Follows CQRS pattern and EF Core best practices.
/// All data operations include proper checks to avoid duplication.
/// </summary>
public class SeedDatabaseCommandHandler : IRequestHandler<SeedDatabaseCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    public SeedDatabaseCommandHandler(
        IApplicationDbContext context,
        UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<bool> Handle(SeedDatabaseCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Check if data already exists to prevent duplication
            //if (request.SkipIfDataExists && await _context.Categories.AnyAsync(cancellationToken))
            //{
            //    return true; // Data already seeded
            //}

            // Seed data in the correct order based on dependencies
            await SeedApplicationRoles(cancellationToken);
            await SeedApplicationUsers(cancellationToken);
            await SeedCategories(cancellationToken);
            await SeedSellerProfiles(cancellationToken);
            await SeedProducts(cancellationToken);
            await SeedProductImages(cancellationToken);
            await SeedBanners(cancellationToken);
            await SeedPromoCodes(cancellationToken);
            await SeedAddresses(cancellationToken);
            await SeedCarts(cancellationToken);
            await SeedReviews(cancellationToken);
            await SeedWishlists(cancellationToken);
            await SeedOrders(cancellationToken);
            await SeedOrderItemsWithShipments(cancellationToken);
            await SeedPayments(cancellationToken);
            await SeedContactMessages(cancellationToken);

            return true;
        }
        catch (Exception ex)
        {
            // Log the exception in production
            throw new InvalidOperationException("Failed to seed database.", ex);
        }
    }

    /// <summary>
    /// Seed ApplicationUser data with different roles.
    /// </summary>
    /// 
    private async Task SeedApplicationRoles(CancellationToken cancellationToken)
    {


        var roleNames = Enum.GetNames(typeof(UserRole));

        foreach (var roleName in roleNames)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = roleName,
                    NormalizedName = roleName.ToUpper()
                });
            }

        }
    }
    private async Task SeedApplicationUsers(CancellationToken cancellationToken)
    {
        if (await _context.Users.AnyAsync(cancellationToken))
            return;

        var users = new[]
        {
            new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "admin@ecommerce.com",
                Email = "admin@ecommerce.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = true,
                AccessFailedCount = 0,
                FullName = "System Administrator",
                Role = UserRole.Admin,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false,
                ProfileImageUrl ="https://ui-avatars.com/api/?name=System+Administrator&background=7c6ff7&color=fff"
            },
            new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "seller@ecommerce.com",
                Email = "seller@ecommerce.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = true,
                AccessFailedCount = 0,
                FullName = "John Seller",
                Role = UserRole.Seller,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false,
                ProfileImageUrl ="https://ui-avatars.com/api/?name=John+Seller&background=f59e0b&color=fff"
            },
            new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "seller2@ecommerce.com",
                Email = "seller2@ecommerce.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = true,
                AccessFailedCount = 0,
                FullName = "Alice Smith",
                Role = UserRole.Seller,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false,
                ProfileImageUrl = "https://ui-avatars.com/api/?name=Alice+Smith&background=f59e0b&color=fff"
            },
            new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "customer1@ecommerce.com",
                Email = "customer1@ecommerce.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = true,
                AccessFailedCount = 0,
                FullName = "Robert Johnson",
                Role = UserRole.Customer,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false,
                ProfileImageUrl = "https://ui-avatars.com/api/?name=Robert+Johnson&background=10b981&color=fff"
            },
            new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "customer2@ecommerce.com",
                Email = "customer2@ecommerce.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = true,
                AccessFailedCount = 0,
                FullName = "Emma Williams",
                Role = UserRole.Customer,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false,
                ProfileImageUrl = "https://ui-avatars.com/api/?name=Emma+Williams&background=10b981&color=fff"
            },
            new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "customer3@ecommerce.com",
                Email = "customer3@ecommerce.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = true,
                AccessFailedCount = 0,
                FullName = "Michael Brown",
                Role = UserRole.Customer,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false,
                ProfileImageUrl = "https://ui-avatars.com/api/?name=Michael+Brown&background=10b981&color=fff"
            }
        };

        foreach (var user in users)
        {
            var result = await _userManager.CreateAsync(user, "P@ssw0rd_123Eco!");
            if (result.Succeeded)
            {
                // Optionally assign roles if not using the Role property directly
                //await _userManager.AddToRoleAsync(user, user.Role.ToString());
            }
            else
            {
                // Handle creation failure (e.g., log errors)
                throw new InvalidOperationException($"Failed to create user {user.UserName}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }
    }
    /// <summary>
    /// Seed Category data with hierarchical structure.
    /// </summary>
    private async Task SeedCategories(CancellationToken cancellationToken)
    {
        if (await _context.Categories.AnyAsync(cancellationToken))
            return;

        var categories = new List<Category>
        {
            new Category
            {
                Name = "Electronics",
                Description = "Latest electronic devices, gadgets, and accessories for tech enthusiasts.",
                ImageUrl = "https://via.placeholder.com/200?text=Electronics",
                ParentCategoryId = null
            },
            new Category
            {
                Name = "Fashion",
                Description = "Trendy clothing, footwear, and fashion accessories for all seasons.",
                ImageUrl = "https://via.placeholder.com/200?text=Fashion",
                ParentCategoryId = null
            },
            new Category
            {
                Name = "Home & Kitchen",
                Description = "Everything for your home and kitchen, from furniture to appliances.",
                ImageUrl = "https://via.placeholder.com/200?text=Home",
                ParentCategoryId = null
            },
            new Category
            {
                Name = "Sports & Outdoors",
                Description = "Sports equipment, outdoor gear, and fitness accessories.",
                ImageUrl = "https://via.placeholder.com/200?text=Sports",
                ParentCategoryId = null
            },
            new Category
            {
                Name = "Books & Media",
                Description = "Books, e-books, audiobooks, and multimedia content.",
                ImageUrl = "https://via.placeholder.com/200?text=Books",
                ParentCategoryId = null
            }
        };

        await _context.Categories.AddRangeAsync(categories, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Seed SellerProfile data linked to users with Seller role.
    /// </summary>
    private async Task SeedSellerProfiles(CancellationToken cancellationToken)
    {
        if (await _context.SellerProfiles.AnyAsync(cancellationToken))
            return;

        var sellerUsers = await _context.Users
            .Where(u => u.Role == UserRole.Seller)
            .ToListAsync(cancellationToken);

        var sellerProfiles = new List<SellerProfile>();

        if (sellerUsers.Count > 0)
        {
            sellerProfiles.Add(new SellerProfile
            {
                UserId = sellerUsers[0].Id,
                StoreName = "TechHub Store",
                StoreDescription = "Premium electronics and gadgets with warranty and excellent customer service.",
                LogoUrl = "https://ui-avatars.com/api/?name=TechHub+Store&background=f59e0b&color=fff",
                IsApproved = true,
                TotalEarnings = 15250.50m,
                CreatedAt = DateTime.UtcNow.AddMonths(-6)
            });
        }

        if (sellerUsers.Count > 1)
        {
            sellerProfiles.Add(new SellerProfile
            {
                UserId = sellerUsers[1].Id,
                StoreName = "Fashion Forward",
                StoreDescription = "Curated collection of contemporary fashion, accessories, and lifestyle products.",
                LogoUrl = "https://ui-avatars.com/api/?name=Fashion+Forward&background=f59e0b&color=fff",
                IsApproved = true,
                TotalEarnings = 8920.75m,
                CreatedAt = DateTime.UtcNow.AddMonths(-4)
            });
        }

        if (sellerProfiles.Count > 0)
        {
            await _context.SellerProfiles.AddRangeAsync(sellerProfiles, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    /// <summary>
    /// Seed Product data with realistic pricing and inventory.
    /// </summary>
    private async Task SeedProducts(CancellationToken cancellationToken)
    {
        if (await _context.Products.AnyAsync(cancellationToken))
            return;

        var categories = await _context.Categories.ToListAsync(cancellationToken);
        var sellers = await _context.SellerProfiles.ToListAsync(cancellationToken);

        if (categories.Count == 0 || sellers.Count == 0)
            return;

        var products = new List<Product>
        {
            new Product
            {
                Name = "Wireless Noise-Canceling Headphones Pro",
                Description = "Premium over-ear Bluetooth headphones with active noise cancellation, 40-hour battery life, and touch controls.",
                Price = 349.99m,
                Stock = 145,
                CategoryId = categories.FirstOrDefault(c => c.Name == "Electronics")?.Id ?? 1,
                Status = ProductStatus.Available,
                SellerId = sellers.FirstOrDefault()?.UserId
            },
            new Product
            {
                Name = "Ultra HD 4K Smart Television 55-inch",
                Description = "Crystal-clear 4K resolution with HDR support, built-in smart apps, and immersive sound system.",
                Price = 799.99m,
                Stock = 32,
                CategoryId = categories.FirstOrDefault(c => c.Name == "Electronics")?.Id ?? 1,
                Status = ProductStatus.Available,
                SellerId = sellers.FirstOrDefault()?.UserId
            },
            new Product
            {
                Name = "Professional Digital Camera DSLR",
                Description = "24MP full-frame sensor, 4K video recording, professional autofocus, and weather-sealed body.",
                Price = 1299.99m,
                Stock = 18,
                CategoryId = categories.FirstOrDefault(c => c.Name == "Electronics")?.Id ?? 1,
                Status = ProductStatus.Available,
                SellerId = sellers.FirstOrDefault()?.UserId
            },
            new Product
            {
                Name = "Men's Classic Cotton Business Shirt",
                Description = "Premium quality 100% organic cotton, wrinkle-resistant, perfect for office or casual wear.",
                Price = 39.99m,
                Stock = 280,
                CategoryId = categories.FirstOrDefault(c => c.Name == "Fashion")?.Id ?? 2,
                Status = ProductStatus.Available,
                SellerId = sellers.Count > 1 ? sellers[1].UserId : sellers[0].UserId
            },
            new Product
            {
                Name = "Women's Comfortable Running Sneakers",
                Description = "Lightweight, ergonomic design with cushioning technology for all-day comfort and support.",
                Price = 89.99m,
                Stock = 156,
                CategoryId = categories.FirstOrDefault(c => c.Name == "Fashion")?.Id ?? 2,
                Status = ProductStatus.Available,
                SellerId = sellers.Count > 1 ? sellers[1].UserId : sellers[0].UserId
            },
            new Product
            {
                Name = "Stainless Steel Chef's Knife 8-inch",
                Description = "Professional-grade chef's knife with high-carbon stainless steel blade and ergonomic handle.",
                Price = 59.99m,
                Stock = 220,
                CategoryId = categories.FirstOrDefault(c => c.Name == "Home & Kitchen")?.Id ?? 3,
                Status = ProductStatus.Available,
                SellerId = sellers.FirstOrDefault()?.UserId
            },
            new Product
            {
                Name = "Automatic Bean-to-Cup Espresso Machine",
                Description = "Professional espresso maker with automatic grinding, built-in milk frother, and programmable settings.",
                Price = 599.99m,
                Stock = 25,
                CategoryId = categories.FirstOrDefault(c => c.Name == "Home & Kitchen")?.Id ?? 3,
                Status = ProductStatus.Available,
                SellerId = sellers.FirstOrDefault()?.UserId
            },
            new Product
            {
                Name = "Yoga Mat Premium Non-Slip",
                Description = "6mm thick non-slip yoga mat with carrying strap, ideal for yoga, pilates, and fitness workouts.",
                Price = 49.99m,
                Stock = 189,
                CategoryId = categories.FirstOrDefault(c => c.Name == "Sports & Outdoors")?.Id ?? 4,
                Status = ProductStatus.Available,
                SellerId = sellers.Count > 1 ? sellers[1].UserId : sellers[0].UserId
            },
            new Product
            {
                Name = "The Clean Code Handbook",
                Description = "A Handbook of Agile Software Craftsmanship - Essential reading for software developers.",
                Price = 34.99m,
                Stock = 340,
                CategoryId = categories.FirstOrDefault(c => c.Name == "Books & Media")?.Id ?? 5,
                Status = ProductStatus.Available,
                SellerId = sellers.FirstOrDefault()?.UserId
            },
            new Product
            {
                Name = "Portable Bluetooth Speaker Waterproof",
                Description = "Waterproof portable speaker with 12-hour battery, 360-degree sound, and rugged design.",
                Price = 79.99m,
                Stock = 267,
                CategoryId = categories.FirstOrDefault(c => c.Name == "Electronics")?.Id ?? 1,
                Status = ProductStatus.Available,
                SellerId = sellers.FirstOrDefault()?.UserId
            }
        };

        await _context.Products.AddRangeAsync(products, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Seed ProductImage data for each product.
    /// </summary>
    private async Task SeedProductImages(CancellationToken cancellationToken)
    {
        if (await _context.ProductImages.AnyAsync(cancellationToken))
            return;

        var products = await _context.Products.ToListAsync(cancellationToken);

        var productImages = new List<ProductImage>();

        foreach (var product in products)
        {
            // Main image
            productImages.Add(new ProductImage
            {
                ProductId = product.Id,
                ImageUrl = $"https://via.placeholder.com/400?text=Product_{product.Id}_Main",
                IsMain = true
            });

            // Additional images
            for (int i = 2; i <= 3; i++)
            {
                productImages.Add(new ProductImage
                {
                    ProductId = product.Id,
                    ImageUrl = $"https://via.placeholder.com/400?text=Product_{product.Id}_Image{i}",
                    IsMain = false
                });
            }
        }

        await _context.ProductImages.AddRangeAsync(productImages, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Seed Banner data for promotional displays.
    /// </summary>
    private async Task SeedBanners(CancellationToken cancellationToken)
    {
        if (await _context.Banners.AnyAsync(cancellationToken))
            return;

        var banners = new List<Banner>
        {
            new Banner
            {
                Title = "Summer Sale - Up to 50% Off",
                ImageUrl = "https://via.placeholder.com/1200x300?text=Summer+Sale",
                Link = "/products?filter=sale",
                IsActive = true,
                DisplayOrder = 1
            },
            new Banner
            {
                Title = "New Arrivals This Week",
                ImageUrl = "https://via.placeholder.com/1200x300?text=New+Arrivals",
                Link = "/products?filter=new",
                IsActive = true,
                DisplayOrder = 2
            },
            new Banner
            {
                Title = "Premium Electronics Collection",
                ImageUrl = "https://via.placeholder.com/1200x300?text=Electronics",
                Link = "/products?category=electronics",
                IsActive = true,
                DisplayOrder = 3
            },
            new Banner
            {
                Title = "Fashion Forward Spring Collection",
                ImageUrl = "https://via.placeholder.com/1200x300?text=Fashion",
                Link = "/products?category=fashion",
                IsActive = true,
                DisplayOrder = 4
            },
            new Banner
            {
                Title = "Free Shipping on Orders Over 50 Dollars",
                ImageUrl = "https://via.placeholder.com/1200x300?text=Free+Shipping",
                Link = "/products",
                IsActive = true,
                DisplayOrder = 5
            }
        };

        await _context.Banners.AddRangeAsync(banners, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Seed PromoCode data for discount campaigns.
    /// </summary>
    private async Task SeedPromoCodes(CancellationToken cancellationToken)
    {
        if (await _context.PromoCodes.AnyAsync(cancellationToken))
            return;

        var promoCodes = new List<PromoCode>
        {
            new PromoCode
            {
                Code = "WELCOME20",
                DiscountPercent = 20m,
                MaxUsageCount = 100,
                CurrentUsageCount = 34,
                ExpiryDate = DateTime.UtcNow.AddMonths(3)
            },
            new PromoCode
            {
                Code = "SUMMER50",
                DiscountPercent = 50m,
                MaxUsageCount = 50,
                CurrentUsageCount = 12,
                ExpiryDate = DateTime.UtcNow.AddMonths(2)
            },
            new PromoCode
            {
                Code = "LOYALTY15",
                DiscountPercent = 15m,
                MaxUsageCount = 200,
                CurrentUsageCount = 67,
                ExpiryDate = DateTime.UtcNow.AddMonths(6)
            },
            new PromoCode
            {
                Code = "FLASH30",
                DiscountPercent = 30m,
                MaxUsageCount = 75,
                CurrentUsageCount = 45,
                ExpiryDate = DateTime.UtcNow.AddDays(7)
            },
            new PromoCode
            {
                Code = "VIPFREE",
                DiscountPercent = 100m,
                MaxUsageCount = 10,
                CurrentUsageCount = 2,
                ExpiryDate = DateTime.UtcNow.AddMonths(1)
            }
        };

        await _context.PromoCodes.AddRangeAsync(promoCodes, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Seed Address data for customer shipping addresses.
    /// </summary>
    private async Task SeedAddresses(CancellationToken cancellationToken)
    {
        if (await _context.Addresses.AnyAsync(cancellationToken))
            return;

        var customers = await _context.Users
            .Where(u => u.Role == UserRole.Customer)
            .ToListAsync(cancellationToken);

        var addresses = new List<Address>();

        if (customers.Count > 0)
        {
            addresses.AddRange(new[]
            {
                new Address
                {
                    UserId = customers[0].Id,
                    FullName = "Robert Johnson",
                    Street = "123 Oak Street",
                    City = "New York",
                    State = "NY",
                    Country = "United States",
                    ZipCode = "10001",
                    Phone = "+1 (212) 555-0101",
                    IsDefault = true
                },
                new Address
                {
                    UserId = customers[0].Id,
                    FullName = "Robert Johnson",
                    Street = "456 Park Avenue",
                    City = "New York",
                    State = "NY",
                    Country = "United States",
                    ZipCode = "10022",
                    Phone = "+1 (212) 555-0102",
                    IsDefault = false
                }
            });
        }

        if (customers.Count > 1)
        {
            addresses.AddRange(new[]
            {
                new Address
                {
                    UserId = customers[1].Id,
                    FullName = "Emma Williams",
                    Street = "789 Maple Drive",
                    City = "Los Angeles",
                    State = "CA",
                    Country = "United States",
                    ZipCode = "90001",
                    Phone = "+1 (213) 555-0201",
                    IsDefault = true
                },
                new Address
                {
                    UserId = customers[1].Id,
                    FullName = "Emma Williams",
                    Street = "321 Cedar Lane",
                    City = "Los Angeles",
                    State = "CA",
                    Country = "United States",
                    ZipCode = "90028",
                    Phone = "+1 (213) 555-0202",
                    IsDefault = false
                }
            });
        }

        if (customers.Count > 2)
        {
            addresses.Add(new Address
            {
                UserId = customers[2].Id,
                FullName = "Michael Brown",
                Street = "654 Elm Street",
                City = "Chicago",
                State = "IL",
                Country = "United States",
                ZipCode = "60601",
                Phone = "+1 (312) 555-0303",
                IsDefault = true
            });
        }

        if (addresses.Count > 0)
        {
            await _context.Addresses.AddRangeAsync(addresses, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    /// <summary>
    /// Seed Cart data for each customer.
    /// </summary>
    private async Task SeedCarts(CancellationToken cancellationToken)
    {
        if (await _context.Carts.AnyAsync(cancellationToken))
            return;

        var customers = await _context.Users
            .Where(u => u.Role == UserRole.Customer)
            .ToListAsync(cancellationToken);

        var carts = customers.Select(customer => new Cart
        {
            UserId = customer.Id
        }).ToList();

        if (carts.Count > 0)
        {
            await _context.Carts.AddRangeAsync(carts, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    /// <summary>
    /// Seed Review data for products.
    /// </summary>
    private async Task SeedReviews(CancellationToken cancellationToken)
    {
        if (await _context.Reviews.AnyAsync(cancellationToken))
            return;

        var products = await _context.Products.Take(5).ToListAsync(cancellationToken);
        var customers = await _context.Users
            .Where(u => u.Role == UserRole.Customer)
            .ToListAsync(cancellationToken);

        if (products.Count < 5 || customers.Count < 3)
            return;

        var reviews = new List<Review>
        {
            new Review
            {
                ProductId = products[0].Id,
                UserId = customers[0].Id,
                Rating = 5,
                Comment = "Excellent quality and fast shipping! Highly recommended.",
                CreatedAt = DateTime.UtcNow.AddDays(-30)
            },
            new Review
            {
                ProductId = products[0].Id,
                UserId = customers[1].Id,
                Rating = 4,
                Comment = "Great product, but packaging could be better.",
                CreatedAt = DateTime.UtcNow.AddDays(-25)
            },
            new Review
            {
                ProductId = products[1].Id,
                UserId = customers[2].Id,
                Rating = 5,
                Comment = "Perfect! Exceeded my expectations.",
                CreatedAt = DateTime.UtcNow.AddDays(-20)
            },
            new Review
            {
                ProductId = products[2].Id,
                UserId = customers[0].Id,
                Rating = 4,
                Comment = "Good value for money. Delivery was on time.",
                CreatedAt = DateTime.UtcNow.AddDays(-15)
            },
            new Review
            {
                ProductId = products[3].Id,
                UserId = customers[1].Id,
                Rating = 5,
                Comment = "Fantastic! My new favorite store.",
                CreatedAt = DateTime.UtcNow.AddDays(-10)
            },
            new Review
            {
                ProductId = products[4].Id,
                UserId = customers[2].Id,
                Rating = 3,
                Comment = "Average quality, but acceptable.",
                CreatedAt = DateTime.UtcNow.AddDays(-5)
            }
        };

        await _context.Reviews.AddRangeAsync(reviews, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Seed Wishlist data for customer wishlists.
    /// </summary>
    private async Task SeedWishlists(CancellationToken cancellationToken)
    {
        if (await _context.Wishlists.AnyAsync(cancellationToken))
            return;

        var products = await _context.Products.Take(8).ToListAsync(cancellationToken);
        var customers = await _context.Users
            .Where(u => u.Role == UserRole.Customer)
            .ToListAsync(cancellationToken);

        if (products.Count < 6 || customers.Count < 3)
            return;

        var wishlists = new List<Wishlist>
        {
            new Wishlist
            {
                UserId = customers[0].Id,
                ProductId = products[0].Id,
                AddedAt = DateTime.UtcNow.AddDays(-10)
            },
            new Wishlist
            {
                UserId = customers[0].Id,
                ProductId = products[2].Id,
                AddedAt = DateTime.UtcNow.AddDays(-8)
            },
            new Wishlist
            {
                UserId = customers[1].Id,
                ProductId = products[1].Id,
                AddedAt = DateTime.UtcNow.AddDays(-7)
            },
            new Wishlist
            {
                UserId = customers[1].Id,
                ProductId = products[3].Id,
                AddedAt = DateTime.UtcNow.AddDays(-5)
            },
            new Wishlist
            {
                UserId = customers[2].Id,
                ProductId = products[4].Id,
                AddedAt = DateTime.UtcNow.AddDays(-3)
            },
            new Wishlist
            {
                UserId = customers[2].Id,
                ProductId = products[5].Id,
                AddedAt = DateTime.UtcNow.AddDays(-1)
            }
        };

        await _context.Wishlists.AddRangeAsync(wishlists, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Seed Order data with complete transaction history.
    /// </summary>
    private async Task SeedOrders(CancellationToken cancellationToken)
    {
        if (await _context.Orders.AnyAsync(cancellationToken))
            return;
        var products = await _context.Products.ToListAsync(cancellationToken);
        var customers = await _context.Users
            .Where(u => u.Role == UserRole.Customer)
            .ToListAsync(cancellationToken);
        var addresses = await _context.Addresses.ToListAsync(cancellationToken);
        var promoCodes = await _context.PromoCodes.ToListAsync(cancellationToken);

        
        if (customers.Count < 3 || addresses.Count < 1)
            return;

        var orders = new List<Order>
        {
            new Order
            {
                UserId = customers[0].Id,
                OrderDate = DateTime.UtcNow.AddDays(-45),
                TotalAmount = 1249.98m,
                Status = OrderStatus.Delivered,
                Notes = "Leave at front door if not home.",
                PaymentMethod = PaymentMethod.CreditCard,
                ShippingAddressId = addresses[0].Id,
                PromoCodeId = promoCodes.Count > 0 ? promoCodes[0].Id : null
            },
            new Order
            {
                UserId = customers[0].Id,
                OrderDate = DateTime.UtcNow.AddDays(-20),
                TotalAmount = 179.97m,
                Status = OrderStatus.Delivered,
                Notes = null,
                PaymentMethod = PaymentMethod.PayPal,
                ShippingAddressId = addresses[0].Id,
                PromoCodeId = null
            },
            new Order
            {
                UserId = customers[1].Id,
                OrderDate = DateTime.UtcNow.AddDays(-15),
                TotalAmount = 749.99m,
                Status = OrderStatus.Shipped,
                Notes = "Expedited shipping requested.",
                PaymentMethod = PaymentMethod.CreditCard,
                ShippingAddressId = addresses.Count > 2 ? addresses[2].Id : addresses[0].Id,
                PromoCodeId = promoCodes.Count > 1 ? promoCodes[1].Id : null
            },
            new Order
            {
                UserId = customers[2].Id,
                OrderDate = DateTime.UtcNow.AddDays(-8),
                TotalAmount = 89.99m,
                Status = OrderStatus.Confirmed,
                Notes = null,
                PaymentMethod = PaymentMethod.CashOnDelivery,
                ShippingAddressId = addresses.Count > 4 ? addresses[4].Id : addresses[0].Id,
                PromoCodeId = null
            },
            new Order
            {
                UserId = customers[1].Id,
                OrderDate = DateTime.UtcNow.AddDays(-2),
                TotalAmount = 599.99m,
                Status = OrderStatus.Pending,
                Notes = "Standard delivery is fine.",
                PaymentMethod = PaymentMethod.Wallet,
                ShippingAddressId = addresses.Count > 3 ? addresses[3].Id : addresses[0].Id,
                PromoCodeId = promoCodes.Count > 2 ? promoCodes[2].Id : null
            }
        };

        await _context.Orders.AddRangeAsync(orders, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Seed OrderItem data linking orders to products.
    /// </summary>
    private async Task SeedOrderItemsWithShipments(CancellationToken cancellationToken)
    {
        if (await _context.OrderItems.AnyAsync(cancellationToken)) return;

        var orders = await _context.Orders.ToListAsync(cancellationToken);
        var products = await _context.Products.ToListAsync(cancellationToken);

        foreach (var order in orders)
        {
            // 1. اختار عينة منتجات للأوردر ده (مثلاً أول 6 منتجات)
            var orderProducts = products.Take(6).ToList();

            // 2. تجميع المنتجات حسب البائع
            var sellerGroups = orderProducts.GroupBy(p => p.SellerId);

            foreach (var group in sellerGroups)
            {
                // 3. لكل بائع في الأوردر، نكريت شحنة واحدة
                var shipment = new Shipment
                {
                    OrderId = order.Id,
                    SellerId = group.Key, // الـ SellerId
                    Status = ShipmentStatus.Processing,
                    TrackingNumber = $"TRK-{order.Id}-{group.Key.Substring(0, 4)}",
                    ShippingFee = 10.00m,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Shipments.Add(shipment);
                await _context.SaveChangesAsync(cancellationToken); // عشان الـ ShipmentId يتولد

                // 4. نضيف المنتجات كـ OrderItems مربوطة بالشحنة دي
                foreach (var product in group)
                {
                    var orderItem = new OrderItem
                    {
                        OrderId = order.Id,
                        ProductId = product.Id,
                        Quantity = 1,
                        UnitPrice = product.Price,
                        ShipmentId = shipment.Id // الربط السليم
                    };
                    _context.OrderItems.Add(orderItem);
                }
            }
        }
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Seed Payment data for all orders.
    /// </summary>
    private async Task SeedPayments(CancellationToken cancellationToken)
    {
        if (await _context.Payments.AnyAsync(cancellationToken))
            return;

        var orders = await _context.Orders.ToListAsync(cancellationToken);

        if (orders.Count == 0)
            return;

        var payments = new List<Payment>
        {
            new Payment
            {
                OrderId = orders[0].Id,
                Amount = orders[0].TotalAmount,
                TransactionId = "TXN-2024-001",
                Method = PaymentMethod.CreditCard,
                Status = PaymentStatus.completed,
                PaidAt = orders[0].OrderDate.AddMinutes(5)
            },
            new Payment
            {
                OrderId = orders[1].Id,
                Amount = orders[1].TotalAmount,
                TransactionId = "TXN-2024-002",
                Method = PaymentMethod.PayPal,
                Status = PaymentStatus.completed,
                PaidAt = orders[1].OrderDate.AddMinutes(10)
            },
            new Payment
            {
                OrderId = orders[2].Id,
                Amount = orders[2].TotalAmount,
                TransactionId = "TXN-2024-003",
                Method = PaymentMethod.CreditCard,
                Status = PaymentStatus.completed,
                PaidAt = orders[2].OrderDate.AddMinutes(3)
            },
            new Payment
            {
                OrderId = orders[3].Id,
                Amount = orders[3].TotalAmount,
                TransactionId = null,
                Method = PaymentMethod.CashOnDelivery,
                Status = PaymentStatus.Pending,
                PaidAt = null
            },
            new Payment
            {
                OrderId = orders[4].Id,
                Amount = orders[4].TotalAmount,
                TransactionId = "TXN-2024-004",
                Method = PaymentMethod.Wallet,
                Status = PaymentStatus.Pending,
                PaidAt = null
            }
        };

        await _context.Payments.AddRangeAsync(payments, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Seed Shipment data for orders from each seller.
    /// </summary>
    /// 
    private async Task SeedContactMessages(CancellationToken cancellationToken)
    {
        if (await _context.ContactMessages.AnyAsync(cancellationToken))
            return;

        var messages = new List<ContactMessage>
    {
        new ContactMessage
        {
            Name = "Ahmed Hassan",
            Email = "ahmed@example.com",
            Subject = "Order Delivery Issue",
            Message = "My order has been delayed for 3 days, can you help?",
            SentAt = DateTime.UtcNow.AddDays(-5),
            IsRead = false
        },
        new ContactMessage
        {
            Name = "Sara Mohamed",
            Email = "sara@example.com",
            Subject = "Payment Problem",
            Message = "I was charged twice for my last order.",
            SentAt = DateTime.UtcNow.AddDays(-3),
            IsRead = false
        },
        new ContactMessage
        {
            Name = "John Smith",
            Email = "john@example.com",
            Subject = "Return Request",
            Message = "I would like to return a product I bought last week.",
            SentAt = DateTime.UtcNow.AddDays(-1),
            IsRead = true
        }
    };

        await _context.ContactMessages.AddRangeAsync(messages, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

}
