using ECommerce.Application.DTOs.Auth;
using ECommerce.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ECommerce.Domain.Entities;

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
            var result = await _authService.GoogleLoginAsync();
            return Ok(result);
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

        #region Arwa:confirm email
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
    }
}
