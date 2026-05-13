using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Carts.Commands.SyncCart
{
    public class CartItemSyncDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }

    }
}
