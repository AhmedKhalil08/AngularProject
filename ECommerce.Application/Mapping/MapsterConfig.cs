using ECommerce.Application.DTOs;
using ECommerce.Domain.Entities;
using Mapster;
namespace ECommerce.Application.Mapping
{
    public class MapsterConfig
    {
        public static void RegisterMappings()
        {
            // 1. Order -> OrderDto
            TypeAdapterConfig<Order, OrderDto>.NewConfig()
                .Map(dest => dest.UserName, src => src.User.UserName)
                .Map(dest => dest.Address, src => src.ShippingAddress)
                .PreserveReference(true);

            // 2. OrderItem -> OrderItemDto
            TypeAdapterConfig<OrderItem, OrderItemDto>.NewConfig()
                .Map(dest => dest.Price, src => (int)src.UnitPrice);

            TypeAdapterConfig<Payment, PaymentDto>.NewConfig();

            TypeAdapterConfig<Address, AddressDto>.NewConfig();

            TypeAdapterConfig<Product, ProductDto>.NewConfig()
               .Map(dest => dest.CategoryName, src => src.Category.Name)

    // بنجيب لينكات الصور من لستة الـ ProductImages
                .Map(dest => dest.ImageUrls, src => src.Images.Select(img => img.ImageUrl).ToList())

    // بنجيب بيانات البائع من جدول الـ SellerProfile (أو User حسب ما إنت مسميه)
                .Map(dest => dest.SellerName, src => src.Seller.StoreName)
                 .Map(dest => dest.storeDes, src => src.Seller.StoreDescription);
        }
    }

}

