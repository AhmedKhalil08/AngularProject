using ECommerce.Application.DTOs;
using MediatR;

namespace ECommerce.Application.Features.ProductImages.Commands.CreateProductImage
{
    public class CreateProductImageCommand : IRequest<ProductImageDto>
    {
        public int ProductId { get; set; }
        public string ImageUrl { get; set; }
        public bool IsMain { get; set; }
    }
}
