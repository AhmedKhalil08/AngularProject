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
        public UserController(UserManager<ApplicationUser> userManager,
            ICurrentUserService currentUser)
        {
            _userManager = userManager;
            _currentUser = currentUser;
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
        public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateProfileDto dto)
        {
            var user = await _userManager.FindByIdAsync(_currentUser.UserId);
            if (user == null) throw new NotFoundException("UserNotFound");
            user.FullName= dto.FullName;
            user.PhoneNumber= dto.PhoneNumber;
            user.ProfileImageUrl= dto.ProfileImageUrl;
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
    }
}
