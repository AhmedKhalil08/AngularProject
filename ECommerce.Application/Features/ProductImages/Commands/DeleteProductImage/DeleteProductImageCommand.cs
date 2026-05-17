using MediatR;

namespace ECommerce.Application.Features.ProductImages.Commands.DeleteProductImage
{
    public class DeleteProductImageCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}