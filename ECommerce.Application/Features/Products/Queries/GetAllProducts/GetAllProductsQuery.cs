using ECommerce.Application.DTOs;
using MediatR;
using System.Collections.Generic;

namespace ECommerce.Application.Features.Products.Queries.GetAllProducts
{
    public class GetAllProductsQuery : IRequest<List<ProductDto>>
    {
    }
}
