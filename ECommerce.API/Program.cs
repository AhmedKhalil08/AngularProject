
using ECommerce.Application;
using ECommerce.Application.Mapping;
using ECommerce.Infrastructure;
using ECommerce.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace ECommerce.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers()
                .AddJsonOptions(options => {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

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
                        .AllowAnyMethod());
            });
            var app = builder.Build();
            MapsterConfig.RegisterMappings();
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

            app.Run();
        }
    }
}
