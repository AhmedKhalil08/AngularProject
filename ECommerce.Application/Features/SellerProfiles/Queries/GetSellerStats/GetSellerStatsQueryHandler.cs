using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Persistence;
using ECommerce.Application.Interfaces.Services;
using ECommerce.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Features.SellerProfiles.Queries.GetSellerStats
{
    public class GetSellerStatsQueryHandler : IRequestHandler<GetSellerStatsQuery, SellerStatsDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUser;

        public GetSellerStatsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<SellerStatsDto> Handle(GetSellerStatsQuery request, CancellationToken cancellationToken)
        {
            var sellerId = _currentUser.UserId;

            var totalProducts = await _context.Products
                .CountAsync(p => p.SellerId == sellerId && !p.IsDeleted);

            var sellerProfile = await _context.SellerProfiles
                .FirstOrDefaultAsync(s => s.UserId == sellerId);

            var totalEarnings = sellerProfile?.TotalEarnings ?? 0;

            var orderItems = await _context.OrderItems
                .Include(oi => oi.Product)
                .Include(oi => oi.Order)
                .Where(oi => oi.Product.SellerId == sellerId)
                .ToListAsync();

            var totalOrders = orderItems.Select(oi => oi.OrderId).Distinct().Count();

            var monthlySales = orderItems
                .GroupBy(oi => new { oi.Order.OrderDate.Year, oi.Order.OrderDate.Month })
                .Select(g => new MonthlySalesDto
                {
                    Month = $"{g.Key.Year}-{g.Key.Month:D2}",
                    Revenue = g.Sum(oi => oi.Quantity * oi.UnitPrice),
                    Orders = g.Select(oi => oi.OrderId).Distinct().Count()
                })
                .OrderBy(m => m.Month)
                .ToList();

            var orderStatusStats = orderItems
                .GroupBy(oi => oi.Order.Status)
                .Select(g => new OrderStatusStatsDto
                {
                    Status = g.Key.ToString(),
                    Count = g.Select(oi => oi.OrderId).Distinct().Count()
                }).ToList();

            var topProducts = orderItems
                .GroupBy(oi => oi.Product.Name)
                .Select(g => new TopProductDto
                {
                    ProductName = g.Key,
                    TotalSold = g.Sum(oi => oi.Quantity),
                    Revenue = g.Sum(oi => oi.Quantity * oi.UnitPrice)
                })
                .OrderByDescending(p => p.TotalSold)
                .Take(5)
                .ToList();

            return new SellerStatsDto
            {
                TotalProducts = totalProducts,
                TotalEarnings = totalEarnings,
                TotalOrders = totalOrders,
                MonthlySales = monthlySales,
                OrderStatusStats = orderStatusStats,
                TopProducts = topProducts
            };
        }
    }
}