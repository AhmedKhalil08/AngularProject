
using ECommerce.API.Extensions;
using ECommerce.API.Middlewares;
using ECommerce.Application;
using ECommerce.Application.Extensions;
using ECommerce.Application.Features.PromoCodes.Commands.CreatePromoCode;
using ECommerce.Application.Interfaces.Persistence;
using ECommerce.Application.Mapping;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure;
using ECommerce.Infrastructure.Persistence.Contexts;
using ECommerce.Infrastructure.Persistence.Repositories;
using ECommerce.Infrastructure.Persistence.Seeding;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;


namespace ECommerce.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
           

            // Add services to the container.

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

    //        builder.Services.AddMediatR(cfg =>
    //cfg.RegisterServicesFromAssembly(typeof(CreatePromoCodeCommandHandler).Assembly));




            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            //builder.Services.AddOpenApi();
            builder.Services.AddApplication();
            builder.Services.AddInfrastructure(builder.Configuration);
            //Swagger test
            builder.Services.AddSwaggerGen();
            // CORS for Angular later
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngular", policy =>
                            policy.WithOrigins("http://localhost:4200")
                                .AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowCredentials());
             });
            builder.Services.AddDatabaseSeeding();
            // Global Exception 
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();
            // For User Services
            builder.Services.AddHttpContextAccessor();
            var app = builder.Build();
            MapsterConfig.RegisterMappings();
            // SEED 
            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                await DataSeeder.SeedAllAsync(userManager, context);
            }
            app.UseExceptionHandler();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                //app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowAngular");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            await app.ExecuteDatabaseSeedingAsync();
            app.Run();
        }
    }
}
