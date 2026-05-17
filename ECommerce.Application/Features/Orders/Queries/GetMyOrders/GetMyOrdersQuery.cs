using ECommerce.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Orders.Queries.GetMyOrders
{
    public class GetMyOrdersQuery: IRequest<List<OrderDto>>
    {
    }
}
