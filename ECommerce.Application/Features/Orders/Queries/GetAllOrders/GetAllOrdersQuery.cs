using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Application.DTOs;
using MediatR;

namespace ECommerce.Application.Features.Orders.Queries.GetAllOrders
{
   public class GetAllOrdersQuery: IRequest<List<OrderDto>>
    {

    }
}
