using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Carts.Commands.ClearCart
{
    public class ClearCartCommand : IRequest<bool>
    {
        public string UserId { get; set; }
    }
}
