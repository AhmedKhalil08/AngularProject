using ECommerce.Application.DTOs.Auth;
using ECommerce.Application.Interfaces.Persistence;
using ECommerce.Application.Interfaces.Services;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ECommerce.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ISellerProfileRepository _sellerProfileRepo;
        private readonly IUnitOfWork _unitOfWork;


        public AuthService(UserManager<ApplicationUser> userManager,
                            IConfiguration configuration,
                            ISellerProfileRepository sellerProfileRepo,
                            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _configuration = configuration;
            _sellerProfileRepo = sellerProfileRepo;
            _unitOfWork = unitOfWork;
        }


        #region Register Custmer 
        public async Task<AuthResponseDto> RegisterCustomerAsync(RegisterDto DTO)
        {
            // exists?
            await ValidateEmailNotTaken(DTO.Email);
            // nope ? creating 
            var user = new ApplicationUser
            {
                UserName = DTO.UserName,
                Email = DTO.Email,
                FullName = DTO.FullName,
                PhoneNumber = DTO.PhoneNumber,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                Role = UserRole.Customer
            };
            // Return The response 
            return await CreateUserAndGenerateResponse(user, DTO.Password);
        }

        #endregion

        #region Register Seller
        public async Task<AuthResponseDto> RegisterSellerAsync(RegisterSellerDto DTO)
        {
            // checking
            await ValidateEmailNotTaken(DTO.Email);
            // App user
            var user = new ApplicationUser
            {
                UserName = DTO.UserName,
                Email = DTO.Email,
                FullName = DTO.FullName,
                PhoneNumber = DTO.PhoneNumber,
                Role = UserRole.Seller,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
            var response = await CreateUserAndGenerateResponse(user, DTO.Password);

            // Creating Seller Profile Automatic
            var profile = new SellerProfile
            {
                UserId = user.Id,
                StoreName = DTO.StoreName,
                StoreDescription = DTO.StoreDescription,
                IsApproved = false,
                TotalEarnings = 0,
                CreatedAt = DateTime.UtcNow
            };
            // saving the Profile 
            await _sellerProfileRepo.AddAsync(profile);
            await _unitOfWork.SaveChangesAsync();
            // Returning
            return response;
        }
        #endregion

        #region Login
        public async Task<AuthResponseDto> LoginAsync(LoginDto DTO)
        {
            // Find user by email or username
            var user = await _userManager.FindByEmailAsync(DTO.EmailOrUserName)
           ?? await _userManager.FindByNameAsync(DTO.EmailOrUserName);

            if (user == null)
                throw new Exception("Invalid email/username or password");

            //  Check password
            var isValid = await _userManager.CheckPasswordAsync(user, DTO.Password);
            if (!isValid)
                throw new Exception("Invalid email/username or password");

            // Check if account is active
            if (!user.IsActive)
                throw new Exception("Account is Banned");

            // Generate token 
            var token = GenerateJwtToken(user);

            // return
            return new AuthResponseDto
            {
                Token = token,
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role.ToString(),
                Expiration = DateTime.UtcNow.AddDays(7)

            };
        }
        #endregion

        #region Helpers 

        // Private helper — creates user and returns token response
        private async Task<AuthResponseDto> CreateUserAndGenerateResponse(ApplicationUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception(errors);
            }

            var token = GenerateJwtToken(user);

            return new AuthResponseDto
            {
                Token = token,
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role.ToString(),
                Expiration = DateTime.UtcNow.AddDays(7)
            };
        }

        // Private helper — checks if email is taken
        private async Task ValidateEmailNotTaken(string email)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser != null)
                throw new Exception("Email already registered");
        }
        #endregion

        #region Token 
        private string GenerateJwtToken(ApplicationUser user)
        {
            //  Define the claims (info inside the token)
            var claims = new[]
            {
                 new Claim(ClaimTypes.NameIdentifier, user.Id),
                 new Claim(ClaimTypes.Email, user.Email),
                 new Claim(ClaimTypes.Name, user.FullName),
                 new Claim(ClaimTypes.Role, user.Role.ToString())
             };

            // Create the signing key from appsettings.json
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            //  Create credentials using the key
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //  Build the token
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: credentials
            );

            // Convert to string and return
            return new JwtSecurityTokenHandler().WriteToken(token);


        }
        #endregion 
    }
}
