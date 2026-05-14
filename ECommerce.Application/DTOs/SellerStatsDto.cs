using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.DTOs
{
    public class SellerStatsDto
    {
        public int TotalProducts { get; set; }
        public decimal TotalEarnings { get; set; }
        public int TotalOrders { get; set; }
        public List<MonthlySalesDto> MonthlySales { get; set; }
        public List<OrderStatusStatsDto> OrderStatusStats { get; set; }
        public List<TopProductDto> TopProducts { get; set; }
    }
}
