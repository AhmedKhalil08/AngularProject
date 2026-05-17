//using NETCore.MailKit.Core;
using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Services;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Services.EmailService;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace ECommerce.Infrastructure.Services
{
    // ECommerce.Application/Common/Result.cs

    public class MailConfService : IMailConfService
    {
        private readonly IEmailService _emailService;
        private readonly IConfiguration _config;
        private readonly UserManager<ApplicationUser> _userManager;

        public MailConfService(
                     UserManager<ApplicationUser> userManager,

                    IEmailService emailService,
                    IConfiguration config)
        {


            _userManager = userManager;
            _config = config;
            _emailService = emailService;
        }
        public async Task<Result<string>> ConfirmEmail(string userId, string token)
        {

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
                return Result<string>.Failure("Invalid confirmation link");

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return Result<string>.Failure("Invalid confirmation link");

            if (user.EmailConfirmed)
                return Result<string>.Success("Email already confirmed");

            var decodedToken = Uri.UnescapeDataString(token);
            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

            if (!result.Succeeded)
                return Result<string>.Failure("Invalid confirmation link");

            return Result<string>.Success("Email confirmed successfully. You can now log in.");
        }
        public async Task<Result<string>> SendOrderStatusUpdateAsync(string userEmail, int orderId, string newStatus)
        {
            try
            {
                var subject = $"Update on your Order #{orderId}";
                var body = $@"
            <div style='font-family: Arial, sans-serif; padding: 20px; border: 1px solid #ddd; border-radius: 10px;'>
                <h2 style='color: #4CAF50;'>Order Status Update</h2>
                <p>Dear Customer,</p>
                <p>We wanted to let you know that the status of your order <strong>#{orderId}</strong> has been updated.</p>
                <p>New Status: <span style='font-size: 16px; font-weight: bold; color: #2196F3;'>{newStatus}</span></p>
                <hr>
                <p style='font-size: 12px; color: #777;'>Thank you for shopping with us!</p>
            </div>";
                await _emailService.SendCustomEmailAsync(userEmail, subject, body);
                return Result<string>.Success("Order status email sent successfully.");
            }
            catch (Exception ex)
            {
                return Result<string>.Failure($"Failed to send email: {ex.Message}");
            }
        }
    }

}

