using ECommerce.Application.DTOs;
using MediatR;

namespace ECommerce.Application.Features.ProductImages.Queries.GetProductImageById
{
    public class GetProductImageByIdQuery : IRequest<ProductImageDto>
    {
        public int Id { get; set; }
    }
}