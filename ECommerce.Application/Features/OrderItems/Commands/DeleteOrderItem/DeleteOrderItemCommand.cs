using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.OrderItems.Commands.DeleteOrderItem
{
    internal class DeleteOrderItemCommand: IRequest<bool>
    {
        public int Id { get; set; }
    }
}
