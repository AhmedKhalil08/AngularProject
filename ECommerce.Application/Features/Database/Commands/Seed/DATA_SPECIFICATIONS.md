# Seeding Data Specifications

## Complete Data Reference

This document provides detailed information about all seeded data.

---

## 1. APPLICATION USERS (6 Records)

### Admin User
```
ID: Generated (GUID)
Username: admin@ecommerce.com
Email: admin@ecommerce.com
FullName: System Administrator
Role: Admin
IsActive: true
EmailConfirmed: true
CreatedAt: DateTime.UtcNow
ProfileImageUrl: https://via.placeholder.com/150?text=Admin
Password: SecurePassword123!
```

### Seller Users (2)

**Seller 1:**
```
ID: Generated (GUID)
Username: seller@ecommerce.com
Email: seller@ecommerce.com
FullName: John Seller
Role: Seller
IsActive: true
ProfileImageUrl: https://via.placeholder.com/150?text=Seller
```

**Seller 2:**
```
ID: Generated (GUID)
Username: seller2@ecommerce.com
Email: seller2@ecommerce.com
FullName: Alice Smith
Role: Seller
IsActive: true
ProfileImageUrl: https://via.placeholder.com/150?text=Seller2
```

### Customer Users (3)

**Customer 1:**
```
ID: Generated (GUID)
Username: customer1@ecommerce.com
Email: customer1@ecommerce.com
FullName: Robert Johnson
Role: Customer
IsActive: true
ProfileImageUrl: https://via.placeholder.com/150?text=Customer1
```

**Customer 2:**
```
ID: Generated (GUID)
Username: customer2@ecommerce.com
Email: customer2@ecommerce.com
FullName: Emma Williams
Role: Customer
IsActive: true
ProfileImageUrl: https://via.placeholder.com/150?text=Customer2
```

**Customer 3:**
```
ID: Generated (GUID)
Username: customer3@ecommerce.com
Email: customer3@ecommerce.com
FullName: Michael Brown
Role: Customer
IsActive: true
ProfileImageUrl: https://via.placeholder.com/150?text=Customer3
```

---

## 2. CATEGORIES (5 Records)

### 1. Electronics
```
ID: Auto-generated
Name: Electronics
Description: Latest electronic devices, gadgets, and accessories for tech enthusiasts.
ImageUrl: https://via.placeholder.com/200?text=Electronics
ParentCategoryId: null
```

### 2. Fashion
```
ID: Auto-generated
Name: Fashion
Description: Trendy clothing, footwear, and fashion accessories for all seasons.
ImageUrl: https://via.placeholder.com/200?text=Fashion
ParentCategoryId: null
```

### 3. Home & Kitchen
```
ID: Auto-generated
Name: Home & Kitchen
Description: Everything for your home and kitchen, from furniture to appliances.
ImageUrl: https://via.placeholder.com/200?text=Home
ParentCategoryId: null
```

### 4. Sports & Outdoors
```
ID: Auto-generated
Name: Sports & Outdoors
Description: Sports equipment, outdoor gear, and fitness accessories.
ImageUrl: https://via.placeholder.com/200?text=Sports
ParentCategoryId: null
```

### 5. Books & Media
```
ID: Auto-generated
Name: Books & Media
Description: Books, e-books, audiobooks, and multimedia content.
ImageUrl: https://via.placeholder.com/200?text=Books
ParentCategoryId: null
```

---

## 3. SELLER PROFILES (2 Records)

### 1. TechHub Store
```
ID: Auto-generated
UserId: seller@ecommerce.com ID
StoreName: TechHub Store
StoreDescription: Premium electronics and gadgets with warranty and excellent customer service.
LogoUrl: https://via.placeholder.com/150?text=TechHub
IsApproved: true
TotalEarnings: $15,250.50
CreatedAt: 6 months ago
```

### 2. Fashion Forward
```
ID: Auto-generated
UserId: seller2@ecommerce.com ID
StoreName: Fashion Forward
StoreDescription: Curated collection of contemporary fashion, accessories, and lifestyle products.
LogoUrl: https://via.placeholder.com/150?text=Fashion
IsApproved: true
TotalEarnings: $8,920.75
CreatedAt: 4 months ago
```

---

## 4. PRODUCTS (10 Records)

| # | Name | Price | Stock | Category | Status | Description |
|---|------|-------|-------|----------|--------|-------------|
| 1 | Wireless Noise-Canceling Headphones Pro | $349.99 | 145 | Electronics | Available | Premium over-ear Bluetooth headphones with active noise cancellation, 40-hour battery life |
| 2 | Ultra HD 4K Smart Television 55-inch | $799.99 | 32 | Electronics | Available | Crystal-clear 4K resolution with HDR support, built-in smart apps |
| 3 | Professional Digital Camera DSLR | $1,299.99 | 18 | Electronics | Available | 24MP full-frame sensor, 4K video recording, weather-sealed |
| 4 | Men's Classic Cotton Business Shirt | $39.99 | 280 | Fashion | Available | Premium 100% organic cotton, wrinkle-resistant |
| 5 | Women's Comfortable Running Sneakers | $89.99 | 156 | Fashion | Available | Lightweight ergonomic design with cushioning technology |
| 6 | Stainless Steel Chef's Knife 8-inch | $59.99 | 220 | Home & Kitchen | Available | Professional-grade with high-carbon stainless steel blade |
| 7 | Automatic Bean-to-Cup Espresso Machine | $599.99 | 25 | Home & Kitchen | Available | Automatic grinding, built-in milk frother, programmable |
| 8 | Yoga Mat Premium Non-Slip | $49.99 | 189 | Sports & Outdoors | Available | 6mm thick, carrying strap, ideal for yoga and fitness |
| 9 | The Clean Code Handbook | $34.99 | 340 | Books & Media | Available | A Handbook of Agile Software Craftsmanship |
| 10 | Portable Bluetooth Speaker Waterproof | $79.99 | 267 | Electronics | Available | 12-hour battery, 360-degree sound, rugged design |

---

## 5. PRODUCT IMAGES (30+ Records)

Each product has:
- 1 Main Image (IsMain = true)
- 2 Additional Images (IsMain = false)

**URL Pattern:**
```
https://via.placeholder.com/400?text=Product_{ProductId}_{ImageName}
```

**Examples:**
```
Product 1:
- https://via.placeholder.com/400?text=Product_1_Main
- https://via.placeholder.com/400?text=Product_1_Image2
- https://via.placeholder.com/400?text=Product_1_Image3

Product 2:
- https://via.placeholder.com/400?text=Product_2_Main
- https://via.placeholder.com/400?text=Product_2_Image2
- https://via.placeholder.com/400?text=Product_2_Image3
```

---

## 6. BANNERS (5 Records)

| # | Title | DisplayOrder | Link | Active |
|---|-------|--------------|------|--------|
| 1 | Summer Sale - Up to 50% Off | 1 | /products?filter=sale | true |
| 2 | New Arrivals This Week | 2 | /products?filter=new | true |
| 3 | Premium Electronics Collection | 3 | /products?category=electronics | true |
| 4 | Fashion Forward Spring Collection | 4 | /products?category=fashion | true |
| 5 | Free Shipping on Orders Over 50 Dollars | 5 | /products | true |

**Image URLs:**
```
https://via.placeholder.com/1200x300?text=Summer+Sale
https://via.placeholder.com/1200x300?text=New+Arrivals
https://via.placeholder.com/1200x300?text=Electronics
https://via.placeholder.com/1200x300?text=Fashion
https://via.placeholder.com/1200x300?text=Free+Shipping
```

---

## 7. PROMO CODES (5 Records)

| Code | Discount | Max Uses | Current Uses | Expires In |
|------|----------|----------|--------------|-----------|
| WELCOME20 | 20% | 100 | 34 | 3 months |
| SUMMER50 | 50% | 50 | 12 | 2 months |
| LOYALTY15 | 15% | 200 | 67 | 6 months |
| FLASH30 | 30% | 75 | 45 | 7 days |
| VIPFREE | 100% | 10 | 2 | 1 month |

---

## 8. ADDRESSES (5+ Records)

### Address 1 - Robert Johnson (Primary)
```
UserId: customer1@ecommerce.com
FullName: Robert Johnson
Street: 123 Oak Street
City: New York
State: NY
Country: United States
ZipCode: 10001
Phone: +1 (212) 555-0101
IsDefault: true
```

### Address 2 - Robert Johnson (Secondary)
```
UserId: customer1@ecommerce.com
FullName: Robert Johnson
Street: 456 Park Avenue
City: New York
State: NY
Country: United States
ZipCode: 10022
Phone: +1 (212) 555-0102
IsDefault: false
```

### Address 3 - Emma Williams (Primary)
```
UserId: customer2@ecommerce.com
FullName: Emma Williams
Street: 789 Maple Drive
City: Los Angeles
State: CA
Country: United States
ZipCode: 90001
Phone: +1 (213) 555-0201
IsDefault: true
```

### Address 4 - Emma Williams (Secondary)
```
UserId: customer2@ecommerce.com
FullName: Emma Williams
Street: 321 Cedar Lane
City: Los Angeles
State: CA
Country: United States
ZipCode: 90028
Phone: +1 (213) 555-0202
IsDefault: false
```

### Address 5 - Michael Brown (Primary)
```
UserId: customer3@ecommerce.com
FullName: Michael Brown
Street: 654 Elm Street
City: Chicago
State: IL
Country: United States
ZipCode: 60601
Phone: +1 (312) 555-0303
IsDefault: true
```

---

## 9. CARTS (3 Records)

| UserEmail | CartId | Items Count | Status |
|-----------|--------|------------|--------|
| customer1@ecommerce.com | Auto | 0 | Active |
| customer2@ecommerce.com | Auto | 0 | Active |
| customer3@ecommerce.com | Auto | 0 | Active |

---

## 10. REVIEWS (6 Records)

| ProductId | UserEmail | Rating | Comment | CreatedAt |
|-----------|-----------|--------|---------|-----------|
| Product 1 | customer1@ecommerce.com | 5 | Excellent quality and fast shipping! Highly recommended. | -30 days |
| Product 1 | customer2@ecommerce.com | 4 | Great product, but packaging could be better. | -25 days |
| Product 2 | customer3@ecommerce.com | 5 | Perfect! Exceeded my expectations. | -20 days |
| Product 3 | customer1@ecommerce.com | 4 | Good value for money. Delivery was on time. | -15 days |
| Product 4 | customer2@ecommerce.com | 5 | Fantastic! My new favorite store. | -10 days |
| Product 5 | customer3@ecommerce.com | 3 | Average quality, but acceptable. | -5 days |

---

## 11. WISHLISTS (6 Records)

| UserEmail | ProductId | ProductName | AddedAt |
|-----------|-----------|-------------|---------|
| customer1@ecommerce.com | 1 | Wireless Headphones | -10 days |
| customer1@ecommerce.com | 3 | Camera DSLR | -8 days |
| customer2@ecommerce.com | 2 | Smart TV | -7 days |
| customer2@ecommerce.com | 4 | Business Shirt | -5 days |
| customer3@ecommerce.com | 5 | Running Shoes | -3 days |
| customer3@ecommerce.com | 6 | Chef's Knife | -1 day |

---

## 12. ORDERS (5 Records)

### Order 1
```
OrderId: Auto
UserId: customer1@ecommerce.com
OrderDate: -45 days
TotalAmount: $1,249.98
Status: Delivered
ShippingAddress: 123 Oak Street, New York, NY 10001
PaymentMethod: CreditCard
PromoCode: WELCOME20 (20% off)
Notes: Leave at front door if not home
```

### Order 2
```
OrderId: Auto
UserId: customer1@ecommerce.com
OrderDate: -20 days
TotalAmount: $179.97
Status: Delivered
ShippingAddress: 123 Oak Street, New York, NY 10001
PaymentMethod: PayPal
PromoCode: None
Notes: None
```

### Order 3
```
OrderId: Auto
UserId: customer2@ecommerce.com
OrderDate: -15 days
TotalAmount: $749.99
Status: Shipped
ShippingAddress: 789 Maple Drive, Los Angeles, CA 90001
PaymentMethod: CreditCard
PromoCode: SUMMER50 (50% off)
Notes: Expedited shipping requested
```

### Order 4
```
OrderId: Auto
UserId: customer3@ecommerce.com
OrderDate: -8 days
TotalAmount: $89.99
Status: Confirmed
ShippingAddress: 654 Elm Street, Chicago, IL 60601
PaymentMethod: CashOnDelivery
PromoCode: None
Notes: None
```

### Order 5
```
OrderId: Auto
UserId: customer2@ecommerce.com
OrderDate: -2 days
TotalAmount: $599.99
Status: Pending
ShippingAddress: 321 Cedar Lane, Los Angeles, CA 90028
PaymentMethod: Wallet
PromoCode: LOYALTY15 (15% off)
Notes: Standard delivery is fine
```

---

## 13. ORDER ITEMS (6 Records)

| OrderId | ProductId | ProductName | Quantity | UnitPrice |
|---------|-----------|-------------|----------|-----------|
| Order 1 | 1 | Wireless Headphones | 1 | $349.99 |
| Order 1 | 2 | Smart TV | 1 | $799.99 |
| Order 2 | 6 | Chef's Knife | 3 | $59.99 |
| Order 3 | 3 | Camera DSLR | 1 | $1,299.99 |
| Order 4 | 8 | Yoga Mat | 2 | $49.99 |
| Order 5 | 7 | Espresso Machine | 1 | $599.99 |

---

## 14. PAYMENTS (5 Records)

| PaymentId | OrderId | Amount | Method | Status | TransactionId | PaidAt |
|-----------|---------|--------|--------|--------|---------------|--------|
| Auto | Order 1 | $1,249.98 | CreditCard | Completed | TXN-2024-001 | Order Date +5 min |
| Auto | Order 2 | $179.97 | PayPal | Completed | TXN-2024-002 | Order Date +10 min |
| Auto | Order 3 | $749.99 | CreditCard | Completed | TXN-2024-003 | Order Date +3 min |
| Auto | Order 4 | $89.99 | CashOnDelivery | Pending | None | Null |
| Auto | Order 5 | $599.99 | Wallet | Pending | TXN-2024-004 | Null |

---

## Data Relationships Summary

```
ApplicationUser (6)
├── SellerProfile (2) - only for Sellers
├── Cart (3) - one per Customer
├── Address (5+) - multiple per Customer
├── Order (5)
│   ├── OrderItem (6)
│   │   └── Product
│   ├── Payment (5)
│   └── PromoCode (optional)
├── Review (6)
│   └── Product
└── Wishlist (6)
    └── Product

Category (5)
└── Product (10)
    ├── ProductImage (30+)
    ├── CartItem (linked to Cart)
    └── OrderItem (linked to Order)

PromoCode (5)
└── Order (applied to some orders)

Banner (5)
```

---

## Password Information

All test users have the same password:
```
Password: SecurePassword123!
```

**Note:** In production, use secure password generation and hashing.

---

## Placeholder Image URLs

All images use placeholder service:
```
https://via.placeholder.com/{width}x{height}?text={description}
```

**Examples:**
```
https://via.placeholder.com/150?text=Admin
https://via.placeholder.com/200?text=Electronics
https://via.placeholder.com/400?text=Product_1_Main
https://via.placeholder.com/1200x300?text=Summer+Sale
```

In production, replace with actual CDN URLs.
