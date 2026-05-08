using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.OrderItems.Queries.GetOrderItemById
{
    public class GetOrderItemByIdQuery: IRequest<OrderItemDto>
    {
        public int Id { get; set; }
    }
}
