using ECommerce.Application.DTOs;
using MediatR;

namespace ECommerce.Application.Features.ProductImages.Commands.UpdateProductImage
{
    public class UpdateProductImageCommand : IRequest<ProductImageDto>
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public bool IsMain { get; set; }
    }
}
