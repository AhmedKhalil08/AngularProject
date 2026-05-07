using ECommerce.Application.DTOs;
using MediatR;
using System.Collections.Generic;

namespace ECommerce.Application.Features.ProductImages.Queries.GetAllProductImages
{
    public class GetAllProductImagesQuery : IRequest<List<ProductImageDto>>
    {
    }
}
