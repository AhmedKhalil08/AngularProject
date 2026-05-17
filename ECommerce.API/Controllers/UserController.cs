using ECommerce.Application.DTOs;
using ECommerce.Application.Exceptions;
using ECommerce.Application.Interfaces.Services;
using ECommerce.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICurrentUserService _currentUser;
        private readonly IFileService _fileService;
        public UserController(UserManager<ApplicationUser> userManager,
            ICurrentUserService currentUser,
            IFileService fileService)
        {
            _userManager = userManager;
            _currentUser = currentUser;
            _fileService = fileService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMyProfile()
        {
            var user = await _userManager.FindByIdAsync(_currentUser.UserId);
            if (user == null) throw new NotFoundException("USer Not FOund");
            return Ok(new UserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                ProfileImageUrl = user.ProfileImageUrl,
                Role = user.Role.ToString()
            });
        }
        [HttpPut]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateMyProfile([FromForm] UpdateProfileDto dto)
        {
            var user = await _userManager.FindByIdAsync(_currentUser.UserId);
            if (user == null) throw new NotFoundException("UserNotFound");

            user.FullName = dto.FullName;
            user.PhoneNumber = dto.PhoneNumber;

            if (dto.ProfileImage != null)
                user.ProfileImageUrl = await _fileService.UploadFileAsync(dto.ProfileImage, "profiles");
            else if (dto.ProfileImageUrl != null)
                user.ProfileImageUrl = dto.ProfileImageUrl;

            await _userManager.UpdateAsync(user);
            return Ok(new UserDto
            {
                Id = user.Id,
                ProfileImageUrl = user.ProfileImageUrl,
                Email = user.Email,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role.ToString()
            });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteMyAccount()
        {
            var user = await _userManager.FindByIdAsync(_currentUser.UserId);
            if (user == null) throw new NotFoundException("User Not Found");
            user.IsDeleted = true;
            await _userManager.UpdateAsync(user);
            Response.Cookies.Delete("token", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
            });
            return Ok(new { message = "Account deleted successfully" });
        }
    }
}
