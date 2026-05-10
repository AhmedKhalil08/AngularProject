using ECommerce.Application.Interfaces.Persistence;
using ECommerce.Application.Interfaces.Services;
using ECommerce.Application.Services;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Persistence.Contexts;
using ECommerce.Infrastructure.Persistence.Repositories;
using ECommerce.Infrastructure.Services.EmailService;
using ECommerce.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;


namespace ECommerce.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Register your infrastructure services here
            // For example:
            // Identity Ahmed 
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            // KANDEL
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICartItemRepository, CartItemRepository>();

            services.AddScoped<IPromoCodeRepository, PromoCodeRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<IBannerRepository, BannerRepository>();
            services.AddScoped<IOrderItemRepository, OrderItemRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IWishlistRepository, WishlistRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();



            services.AddScoped<IProductImageRepository, ProductImageRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IFileService, FileService>();

            // Ahmed
            
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<ISellerProfileRepository, SellerProfileRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<IWishlistRepository, WishlistRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAuthService, AuthService>();
            

            
            // JWT Auth
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
             .AddJwtBearer(options =>
             {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                 ValidateIssuer = true,
                 ValidateAudience = true,
                 ValidateLifetime = true,
                 ValidateIssuerSigningKey = true,
                 ValidIssuer = configuration["Jwt:Issuer"],
                 ValidAudience = configuration["Jwt:Audience"],
                 IssuerSigningKey = new SymmetricSecurityKey(
                 Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                 };
                  }).AddGoogle(options =>
                  {
                      options.ClientId = configuration["Authentication:Google:ClientId"];
                      options.ClientSecret = configuration["Authentication:Google:ClientSecret"];
                  });
            //



            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            


            //Email Service
             services.AddScoped<IEmailService, EmailService>();
             services.AddHttpClient();


            return services;

        }

    }
}
