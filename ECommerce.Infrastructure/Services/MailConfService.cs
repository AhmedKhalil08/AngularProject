using ECommerce.Infrastructure.Persistence.Repositories;
using ECommerce.Application.DTOs.Auth;
using ECommerce.Application.Exceptions;
using ECommerce.Application.Interfaces.Persistence;
using ECommerce.Application.Interfaces.Services;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using ECommerce.Infrastructure.Services.EmailService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
//using NETCore.MailKit.Core;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.Mvc;
using ECommerce.Application.DTOs;

namespace ECommerce.Infrastructure.Services
{
    // ECommerce.Application/Common/Result.cs
   
    public class MailConfService:IMailConfService
    {
        private readonly IEmailService _emailService;
        private readonly IConfiguration _config;
        private readonly UserManager<ApplicationUser> _userManager;

        public MailConfService(
                     UserManager<ApplicationUser> userManager,
                           
                    IEmailService emailService,
                    IConfiguration config) {


            _userManager = userManager;
            _config = config;
            _emailService = emailService;
        }
        public async Task<Result<string>> ConfirmEmail( string userId,  string token)
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
                return  Result<string>.Failure("Invalid confirmation link");

             return Result<string>.Success("Email confirmed successfully. You can now log in.");
        }
    }
}
