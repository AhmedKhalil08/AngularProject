using ECommerce.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Orders.Queries.GetOrderDetails
{
    public class GetOrderDetailsQuery:IRequest<OrderDetailsDto>
    {
        public int OrderId { get; set; }
    }
}
