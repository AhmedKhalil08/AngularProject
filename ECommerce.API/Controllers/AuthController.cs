using ECommerce.Application.DTOs;
using ECommerce.Application.DTOs.Auth;
using ECommerce.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IMailConfService _mailconfservice;

        public AuthController(IAuthService authService, IMailConfService mailconfservice)
        {
            _authService = authService;
            _mailconfservice = mailconfservice;

        }

        #region Register Customer 

        [HttpPost("register/customer")]
        public async Task<IActionResult> RegisterCustomer([FromBody] RegisterDto model)
        {
            var result = await _authService.RegisterCustomerAsync(model);
            return Ok(result);
        }
        #endregion

        #region Register Seller 


        [HttpPost("register/seller")]
        public async Task<IActionResult> RegisterSeller([FromBody] RegisterSellerDto model)
        {
            var result = await _authService.RegisterSellerAsync(model);
            return Ok(result);
        }

        #endregion

        #region Login 

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var result = await _authService.LoginAsync(model);
            return Ok(result);
        }
        #endregion

        #region Google Login

        [HttpGet("google-login")]
        public IActionResult GoogleLogin()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleCallback", "Auth", null, Request.Scheme)
            };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("google-callback")]
        public async Task<IActionResult> GoogleCallback()
        {
            var result = await _authService.ExternalLoginAsync();
            return Redirect("http://localhost:4200");
        }

        #endregion

        #region Change PAssword
        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto model)
        {
            var result = await _authService.ChangePasswordAsync(model);
            return Ok(new { message = "Password Changed Successfuly" });
        }
        #endregion

        #region confirm email
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string token)
        {
            var result = await _mailconfservice.ConfirmEmail(userId, token);

            if (result.IsSuccess)
                return Content($@"<html><body style='font-family:sans-serif;text-align:center;padding:50px'>
                <h2 style='color:green'> {result.Data}</h2>
                <p>You can now <a href='http://localhost:4200/auth/login'>login</a></p>
                </body></html>", "text/html");

            return Content($@"<html><body style='font-family:sans-serif;text-align:center;padding:50px'>
            <h2 style='color:red'>{result.Error}</h2>
            </body></html>", "text/html");
        }
        #endregion


        #region Logout
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("token", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
            });
            return Ok();
        }
        #endregion


        [HttpPost("become-seller")]
        [Authorize]
        public async Task<IActionResult> BecomeSeller([FromBody] BecomeSellerDto model)
        {
            await _authService.BecomeSellerAsync(model);
            return Ok(new { message = "You are now a seller!" });
        }

        #region Facebook 

        [HttpGet("facebook-login")]
        public IActionResult FacebookLogin()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("FacebookCallback", "Auth", null, Request.Scheme)
            };
            return Challenge(properties, FacebookDefaults.AuthenticationScheme);
        }

        [HttpGet("facebook-callback")]
        public async Task<IActionResult> FacebookCallback()
        {
            var result = await _authService.ExternalLoginAsync();
            return Redirect("http://localhost:4200");
        }
        #endregion

        #region ME
        [HttpGet("me")]
        [Authorize]
        public IActionResult Me()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var fullName = User.FindFirst(ClaimTypes.Name)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var expiration = User.FindFirst("exp")?.Value;

            return Ok(new AuthResponseDto
            {
                Email = email,
                FullName = fullName,
                Role = role,
                Expiration = DateTime.UtcNow.AddDays(7)
            });
        }
        #endregion



    }
}
