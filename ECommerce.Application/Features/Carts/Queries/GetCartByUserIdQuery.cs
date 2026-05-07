using ECommerce.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Carts.Queries
{
    public class GetCartByUserIdQuery : IRequest<CartDto>
    {
        public string UserId { get; set; }
    }
}
