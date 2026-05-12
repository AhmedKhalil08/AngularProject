using ECommerce.Application.DTOs.Auth;
using ECommerce.Application.Interfaces.Services;
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
            try
            {
                var result = await _authService.RegisterCustomerAsync(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        #endregion

        #region Register Seller 


        [HttpPost("register/seller")]
        public async Task<IActionResult> RegisterSeller([FromBody] RegisterSellerDto model)
        {
            try
            {
                var result = await _authService.RegisterSellerAsync(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        #endregion

        #region Login 

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            try
            {
                var result = await _authService.LoginAsync(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        #endregion

        #region Arwa:confirm email
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string token)
        {
            var result = await _mailconfservice.ConfirmEmail(userId,token);
            return Ok(result);

        }


        //[HttpPost("resend-confirmation")]
        //public async Task<IActionResult> ResendConfirmation([FromBody] string email)
        //{
        //    var user = await _userManager.FindByEmailAsync(email);
        //    if (user is null)
        //        return NotFound("User not found");

        //    if (user.EmailConfirmed)
        //        return BadRequest("Email is already confirmed");

        //    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        //    var encodedToken = Uri.EscapeDataString(token);
        //    var clientUrl = _config["ClientUrl"];
        //    var confirmationLink = $"{clientUrl}/confirm-email?userId={user.Id}&token={encodedToken}";

        //    await _emailService.SendEmailAsync(new EmailDto
        //    {
        //        To = user.Email,
        //        Subject = "Resend - Confirm Your Email",
        //        Body = $"<p>Click <a href='{confirmationLink}'>here</a> to confirm your email.</p>"
        //    });

        //    return Ok("Confirmation email resent. Please check your inbox.");
        //}


        #endregion
    }
}
