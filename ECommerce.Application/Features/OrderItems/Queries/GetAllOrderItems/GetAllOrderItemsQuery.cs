using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace ECommerce.Application.Features.OrderItems.Queries.GetAllOrderItems
{
    public class GetAllOrderItemsQuery:IRequest<List<OrderItemDto>>
    {
    }
}
