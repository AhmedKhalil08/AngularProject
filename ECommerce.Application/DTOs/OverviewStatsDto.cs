using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.DTOs
{
    public class OverviewStatsDto
    {
        public int TotalCustomers { get; set; }
        public int TotalSellers { get; set; }
        public int TotalAdmins { get; set; }
        public int TotalOrders { get; set; }
        public int TotalProducts { get; set; }
        public int TotalCategories { get; set; }
        public decimal TotalRevenue { get; set; }
        public int PendingSellers { get; set; }
        public int BannedUsers { get; set; }
        public List<MonthlySalesDto> MonthlySales { get; set; }
        public List<OrderStatusStatsDto> OrderStatusStats { get; set; }
        public List<TopProductDto> TopProducts { get; set; }

    }
}
