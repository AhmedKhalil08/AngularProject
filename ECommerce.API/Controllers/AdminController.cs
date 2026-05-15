using ECommerce.Application.DTOs;
using ECommerce.Application.DTOs.Auth;
using ECommerce.Application.Exceptions;
using ECommerce.Application.Features.Banners.Commands.CreateBanner;
using ECommerce.Application.Features.Banners.Commands.DeleteBanner;
using ECommerce.Application.Features.Banners.Commands.UpdateBanner;
using ECommerce.Application.Features.Banners.Queries.GetAllBanners;
using ECommerce.Application.Features.ContactMessages.Commands.MarkAsRead;
using ECommerce.Application.Features.ContactMessages.Queries;
using ECommerce.Application.Features.ContactMessages.Queries.GetAllMessages;
using ECommerce.Application.Features.ContactMessages.Queries.GetMessageById;
using ECommerce.Application.Features.PromoCodes.Commands.CreatePromoCode;
using ECommerce.Application.Features.PromoCodes.Commands.DeletePromoCode;
using ECommerce.Application.Features.PromoCodes.Queries.GetAllPromoCodes;
using ECommerce.Application.Features.SellerProfiles.Commands.ApproveSellerProfile;
using ECommerce.Application.Features.SellerProfiles.Commands.DeleteSellerProfileByAdmin;
using ECommerce.Application.Features.SellerProfiles.Queries.GetAllSellerProfiles;
using ECommerce.Application.Interfaces.Persistence;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = nameof(UserRole.Admin))]
    public class AdminController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IApplicationDbContext _context;
        public AdminController(IMediator mediator, UserManager<ApplicationUser> userManager, IApplicationDbContext context)
        {
            _mediator = mediator;
            _userManager = userManager;
            _context= context;
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
        public async Task<IActionResult> GetAllSellers(
            [FromQuery] string? search,
            [FromQuery] string? status,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 9)
        {
            var result = await _mediator.Send(new GetAllSellerProfilesQuery
            {
                Search = search,
                Status = status,
                Page = page,
                PageSize = pageSize
            });
            return Ok(result);
        }

        [HttpPut("sellers/{id}/approve")]
        public async Task<IActionResult> ApproveSeller(int id, [FromBody] ApproveSellerProfileCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpGet("admins")]
        public IActionResult GetAllAdmins()
        {
            var admins = _userManager.Users
                .Where(u => u.Role == UserRole.Admin)
                .Select(u => new UserDto
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
            return Ok(admins);
        }
        [HttpPost("admins")]
        public async Task<IActionResult> CreateAdmin([FromBody] CreateAdminDto dto)
        {
            var admin = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                FullName = dto.FullName,
                Role = UserRole.Admin,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(admin, dto.Password);
            if (!result.Succeeded)
                throw new BadRequestException(string.Join(", ", result.Errors.Select(e => e.Description)));

            return Ok(new { message = "Admin created successfully" });
        }
        // CHARTS
        [HttpGet("overview")]
        public async Task<IActionResult> GetOverviewStats()
        {
            var totalCustomers = await _userManager.Users.CountAsync(u => u.Role == UserRole.Customer && !u.IsDeleted);
            var totalSellers = await _userManager.Users.CountAsync(u => u.Role == UserRole.Seller && !u.IsDeleted);
            var totalAdmins = await _userManager.Users.CountAsync(u => u.Role == UserRole.Admin && !u.IsDeleted);
            var bannedUsers = await _userManager.Users.CountAsync(u => !u.IsActive && !u.IsDeleted);
            var pendingSellers = await _context.SellerProfiles.CountAsync(s => !s.IsApproved && !s.IsDeleted);
            var totalOrders = await _context.Orders.CountAsync();
            var totalProducts = await _context.Products.CountAsync(p => !p.IsDeleted);
            var totalCategories = await _context.Categories.CountAsync();
            var totalRevenue = await _context.Orders
                .Where(o => o.Status == OrderStatus.Delivered)
                .SumAsync(o => o.TotalAmount);

            var monthlySales = await _context.Orders
                .GroupBy(o => new { o.OrderDate.Year, o.OrderDate.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Revenue = g.Sum(o => o.TotalAmount),
                    Orders = g.Count()
                })
                .OrderBy(m => m.Year).ThenBy(m => m.Month)
                .ToListAsync();


            var monthlySalesDto = monthlySales.Select(m => new MonthlySalesDto
            {
                Month = $"{m.Year}-{m.Month:D2}",
                Revenue = m.Revenue,
                Orders = m.Orders
            }).ToList();

            var orderStatusStats = await _context.Orders
                .GroupBy(o => o.Status)
                .Select(g => new OrderStatusStatsDto
                {
                    Status = g.Key.ToString(),
                    Count = g.Count()
                }).ToListAsync();
            var topProducts = await _context.OrderItems
                .GroupBy(oi => oi.Product.Name)
                 .Select(g => new TopProductDto
                {
                  ProductName = g.Key,
                 TotalSold = g.Sum(oi => oi.Quantity),
                 Revenue = g.Sum(oi => oi.Quantity * oi.UnitPrice)
                     })
                .OrderByDescending(p => p.TotalSold)
                .Take(5)
                .ToListAsync();

            return Ok(new OverviewStatsDto
            {
                TotalCustomers = totalCustomers,
                TotalSellers = totalSellers,
                TotalAdmins = totalAdmins,
                TotalOrders = totalOrders,
                TotalProducts = totalProducts,
                TotalCategories = totalCategories,
                TotalRevenue = totalRevenue,
                PendingSellers = pendingSellers,
                BannedUsers = bannedUsers,
                MonthlySales = monthlySalesDto,
                OrderStatusStats = orderStatusStats,
                TopProducts = topProducts
            });
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


        #region Messages 


        [HttpGet("messages")]
        public async Task<IActionResult> GetAllMessages()
        {
            var result = await _mediator.Send(new GetAllContactMessagesQuery());
            return Ok(result);
        }

        [HttpPut("messages/{id}/read")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var result = await _mediator.Send(new MarkAsReadCommand { Id = id });
            if (!result) throw new NotFoundException("Message Not Found");
            return Ok(new { message = "Marked as read" });
        }


        [HttpGet("messages/{id}")]
        public async Task<IActionResult> GetMessageById(int id)
        {
            var result = await _mediator.Send(new GetContactMessageByIdQuery { Id = id });
            return Ok(result);
        }
        #endregion
    }
}
