using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Products.Commands.RestoreProduct
{
    public class RestoreProductCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
