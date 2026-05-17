using ECommerce.Application.DTOs;
using ECommerce.Domain.Entities;
using Mapster;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace ECommerce.Application.Mapping
{
    public class MapsterConfig
    {
        public static void RegisterMappings()
        {
            // Order
            TypeAdapterConfig<Order, OrderDto>.NewConfig()
                .Map(dest => dest.UserName, src => src.User.UserName)
                .Map(dest => dest.Address, src => src.ShippingAddress)
                .MaxDepth(2);

            // OrderItem - التعديل هنا
            TypeAdapterConfig<OrderItem, OrderItemDto>.NewConfig()
     .Map(dest => dest.ProductId, src => src.ProductId)
     .Map(dest => dest.Quantity, src => src.Quantity)
     .Map(dest => dest.Price, src => src.UnitPrice)
     .Map(dest => dest.ProductName, src => src.Product.Name)
     // السطر ده هو الأهم: بيمنع المابستر إنه يحاول يملأ أي حاجة تانية بتوقعاته
     .IgnoreNonMapped(true);

            // Payment - لازم نلغي الـ UserName منه مؤقتاً للتأكد
            TypeAdapterConfig<Payment, PaymentDto>.NewConfig()
         .Map(dest => dest.Id, src => src.Id)
         .Map(dest => dest.Amount, src => src.Amount)
         .Map(dest => dest.TransactionId, src => src.TransactionId)
         .Map(dest => dest.Method, src => src.Method)
         .Map(dest => dest.Status, src => src.Status)
         .Map(dest => dest.PaidAt, src => src.PaidAt)
         // لو الـ PaymentDto فيه UserName، هاته من المسار ده بالظبط:
         .Map(dest => dest.UserName, src => src.Order.User.UserName)
         .IgnoreNonMapped(true);

            // Product
            TypeAdapterConfig<Product, ProductDto>.NewConfig()
                .Map(dest => dest.CategoryName, src => src.Category.Name)
                .Map(dest => dest.ImageUrls, src => src.Images.Select(img => img.ImageUrl).ToList())
                .Map(dest => dest.SellerName, src => src.Seller.StoreName);
            TypeAdapterConfig<Category,ProductDto>.NewConfig().
                Map(dest => dest.CategoryName, src => src.Name);

            TypeAdapterConfig<Shipment, ShipmentDto>.NewConfig()
            .Map(dest => dest.Items, src => src.OrderItems);
            TypeAdapterConfig<Shipment, SellerShipmentDto>.NewConfig()
            .Map(dest => dest.Items, src => src.OrderItems)
            .Map(dest=> dest.SellerName, src => src.Seller.User.FullName);
            //.Map(dest => dest.Id, src => src.OrderId);
            TypeAdapterConfig<Review, ReviewDto>.NewConfig()
            .Map(dest => dest.UserFullName, src => src.User.FullName);

            TypeAdapterConfig<OrderItem, OrderItemDto>.NewConfig()
            .Map(dest => dest.ProductName, src => src.Product.Name)
            .Map(dest => dest.Price, src => src.UnitPrice);


        }
    }
}