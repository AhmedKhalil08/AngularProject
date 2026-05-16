using ECommerce.Application.DTOs;
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
//using NETCore.MailKit.Core;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Data;
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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICurrentUserService _currentUserService;
        private readonly IEmailService _emailService;

       // private readonly IHttpContextAccessor _httpContextAccessor;
      //  private readonly ICurrentUserService _currentUserService;
        //private readonly IEmailService _emailService;
       



        public AuthService(UserManager<ApplicationUser> userManager,
                            IConfiguration configuration,
                            ISellerProfileRepository sellerProfileRepo,
                            IUnitOfWork unitOfWork,
                    IHttpContextAccessor httpContextAccessor,
                    ICurrentUserService currentUserService,
                    IEmailService emailService
                  
            )
        {
            _userManager = userManager;
            _configuration = configuration;
            _sellerProfileRepo = sellerProfileRepo;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _currentUserService = currentUserService;
            _emailService = emailService;
            
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
                throw new UnauthorizedAccessException("Invalid email/username or password");
            //  Check password
            var isValid = await _userManager.CheckPasswordAsync(user, DTO.Password);
            if (!isValid)
                throw new UnauthorizedAccessException("Invalid email/username or password");
            
            if (!user.EmailConfirmed)
            {
                throw new ForbiddenAccessException("Please Confirm Your Email First");
            }

            // Check if account is active
            if (!user.IsActive)
                throw new ForbiddenAccessException("Account is Banned");

            // Generate token 
            var token = GenerateJwtToken(user);

            // return
            _httpContextAccessor.HttpContext.Response.Cookies.Append("token", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(7)
            });

            return new AuthResponseDto
            {
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role.ToString(),
                Expiration = DateTime.UtcNow.AddDays(7)
            };
        }
        #endregion

        #region External Logins
        public async Task<AuthResponseDto> ExternalLoginAsync()
        {
            // Read the Google response from the current request
            var result = await _httpContextAccessor.HttpContext
                .AuthenticateAsync(IdentityConstants.ExternalScheme);

            if (!result.Succeeded)
                throw new UnauthorizedAccessException("Google authentication failed");

            // Extract user info from Google's response
            var claims = result.Principal.Identities.FirstOrDefault()?.Claims;
            var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var fullName = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            if (email == null)
                throw new BadRequestException("Could not retrieve email from Google");

            // Check if user already exists
            var user = await _userManager.FindByEmailAsync(email);

            // If not, create them automatically
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    FullName = fullName ?? email,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    Role = UserRole.Customer,
                    EmailConfirmed = true
                };

                var createResult = await _userManager.CreateAsync(user);
                if (!createResult.Succeeded)
                {
                    var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                    throw new BadRequestException(errors);
                }
            }

            // Generate JWT and return — same as regular login
            var token = GenerateJwtToken(user);

            _httpContextAccessor.HttpContext.Response.Cookies.Append("token", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(7)
            });

            return new AuthResponseDto
            {
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role.ToString(),
                Expiration = DateTime.UtcNow.AddDays(7)
            };
        }
        #endregion


        #region Change Password
        public async Task<bool> ChangePasswordAsync(ChangePasswordDto model)
        {
            // Get current user from JWT token
            var userId = _currentUserService.UserId;
            if (userId == null)
                throw new UnauthorizedAccessException("You are not logged in");

            // Find user in DB
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("User not found");

            // Check if account is active
            if (!user.IsActive)
                throw new ForbiddenAccessException("Account is banned");

            // Change password
            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new BadRequestException(errors);
            }

            return true;
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
                throw new BadRequestException(errors);
            }
            /*start of Arwa's code'*/
            else
            {
                ///Arwa//
                var token2 = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var encodedToken = Uri.EscapeDataString(token2);
                var clientUrl = _configuration["AppUrl"];
                var confirmationLink = $"{clientUrl}/confirm-email?userId={user.Id}&token={encodedToken}";

                await _emailService.SendEmailConf(new EmailDto()
                {
                    To = user.Email,

                    Body = $@"
                         <h2>Welcome!</h2>
                            <p>Please confirm your account by 
                             <a href='{confirmationLink}'>clicking here</a>.
                        </p>
                         <p>Or copy this link: <br> {confirmationLink}</p>"
                });
                
            }
            /*end*/

                var token = GenerateJwtToken(user);

            _httpContextAccessor.HttpContext.Response.Cookies.Append("token", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(7)
            });

            return new AuthResponseDto
            {
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
                throw new BadRequestException("Email already registered");
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


        // Confirm Email
        public async Task<string> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("User not found");

            var decodedToken = Uri.UnescapeDataString(token);
            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

            if (!result.Succeeded)
                throw new BadRequestException("Invalid confirmation token");

            return "Email confirmed successfully. You can now log in.";
        }



       
        public async Task BecomeSellerAsync(BecomeSellerDto dto)
        {
            var userId = _currentUserService.UserId;
            if (userId == null)
                throw new UnauthorizedAccessException("You are not logged in");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("User not found");

            if (user.Role == UserRole.Seller)
                throw new BadRequestException("You are already a seller");

            user.Role = UserRole.Seller;
            await _userManager.UpdateAsync(user);

            var profile = new SellerProfile
            {
                UserId = user.Id,
                StoreName = dto.StoreName,
                StoreDescription = dto.StoreDescription,
                LogoUrl = dto.LogoUrl,
                IsApproved = false,
                TotalEarnings = 0,
                CreatedAt = DateTime.UtcNow
            };

            await _sellerProfileRepo.AddAsync(profile);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
