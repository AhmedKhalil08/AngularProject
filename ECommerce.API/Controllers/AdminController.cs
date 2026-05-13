using ECommerce.Application.DTOs;
using ECommerce.Application.Exceptions;
using ECommerce.Application.Features.Banners.Commands.CreateBanner;
using ECommerce.Application.Features.Banners.Commands.DeleteBanner;
using ECommerce.Application.Features.Banners.Commands.UpdateBanner;
using ECommerce.Application.Features.Banners.Queries.GetAllBanners;
using ECommerce.Application.Features.PromoCodes.Commands.CreatePromoCode;
using ECommerce.Application.Features.PromoCodes.Commands.DeletePromoCode;
using ECommerce.Application.Features.PromoCodes.Queries.GetAllPromoCodes;
using ECommerce.Application.Features.SellerProfiles.Commands.ApproveSellerProfile;
using ECommerce.Application.Features.SellerProfiles.Queries.GetAllSellerProfiles;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = nameof(UserRole.Admin))]
    public class AdminController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;
        public AdminController(IMediator mediator, UserManager<ApplicationUser> userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        [HttpGet("users")]
        public IActionResult GetAllUsers()
        {
            var users = _userManager.Users.Select(u => new UserDto
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                Role = u.Role.ToString(),
                IsActive=u.IsActive,
                 IsDeleted=u.IsDeleted

            }).ToList();
            return Ok(users);
        }

        [HttpPut("users/{id}/status")]
        public async Task<IActionResult> ToggleUserStatus(string id, [FromBody] bool isActive)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new NotFoundException("user Not Found");
            }
            user.IsActive = isActive;
            await _userManager.UpdateAsync(user);
            return Ok(new { message = isActive ? "User Activated" : "User Banned" });
        }

        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) { throw new NotFoundException("User Not Found"); }
            user.IsActive = false;
            user.IsDeleted = true;
            await _userManager.UpdateAsync(user);
            return NoContent();
        }

        [HttpGet("customers")]
        public IActionResult GetAllCustomers([FromQuery] string? search,
                    [FromQuery] string? status,
                    [FromQuery] int page = 1,
                    [FromQuery] int pageSize = 9)
        {
            var query = _userManager.Users.Where(u => u.Role == UserRole.Customer);
            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(u => u.FullName.Contains(search) || u.Email.Contains(search));

            query = status switch
            {
                "active" => query.Where(u => u.IsActive && !u.IsDeleted),
                "banned" => query.Where(u => !u.IsActive && !u.IsDeleted),
                "deleted" => query.Where(u => u.IsDeleted),
                _ => query
            };

            var totalCount = query.Count();
            var items = query.Skip((page - 1) * pageSize).Take(pageSize).Select(u => new UserDto
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                ProfileImageUrl = u.ProfileImageUrl,
                Role = u.Role.ToString(),
                IsActive = u.IsActive,
                IsDeleted = u.IsDeleted
            }).ToList();
            return Ok(new PagedResult<UserDto>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            });
        }
        [HttpPut("users/{id}/restore")]
        public async Task<IActionResult> RestoreUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) throw new NotFoundException("User Not Found");
            user.IsActive = true;
            user.IsDeleted = false;
            await _userManager.UpdateAsync(user);
            return Ok(new { message = "User Restored" });
        }

        [HttpGet("sellers")]
        public async Task<IActionResult> GetAllSellers()
        {
            var result = await _mediator.Send(new GetAllSellerProfilesQuery());
            return Ok(result);
        }


        [HttpPut("sellers/{id}/approve")]
        public async Task<IActionResult> ApproveSeller(int id, [FromBody] ApproveSellerProfileCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        // --------------------------------------------------Banner -----------------------------------------------
        #region Banners

        [HttpGet("banners")]
        public async Task<IActionResult> GetAllBanners()
        {
            var result = await _mediator.Send(new GetAllBannersQuery());
            return Ok(result);
        }

        [HttpPost("banners")]
        public async Task<IActionResult> CreateBanner([FromBody] CreateBannerCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpPut("banners/{id}")]
        public async Task<IActionResult> UpdateBanner(int id, [FromBody] UpdateBannerCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpDelete("banners/{id}")]
        public async Task<IActionResult> DeleteBanner(int id)
        {
            var result = await _mediator.Send(new DeleteBannerCommand { Id = id });
            if (!result) throw new NotFoundException("Banner Not Found");
            return NoContent();
        }
        #endregion

        //-----------------------------------------------PROMO--------------------------------------------------
        #region Promo 
        [HttpGet("promocodes")]
        public async Task<IActionResult> GetAllPromoCodes()
        {
            var result = await _mediator.Send(new GetAllPromoCodesQuery());
            return Ok(result);
        }

        [HttpPost("promocodes")]
        public async Task<IActionResult> CreatePromoCode([FromBody] CreatePromoCodeCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("promocodes/{id}")]
        public async Task<IActionResult> DeletePromoCode(int id)
        {
            var result = await _mediator.Send(new DeletePromoCodeCommand { Id = id });
            if (!result) throw new NotFoundException("Promo Not Found");
            return NoContent();
        }
        #endregion
    }
}
