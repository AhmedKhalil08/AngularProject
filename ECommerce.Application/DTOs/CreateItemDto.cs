using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.DTOs
{
    public class CreateItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
