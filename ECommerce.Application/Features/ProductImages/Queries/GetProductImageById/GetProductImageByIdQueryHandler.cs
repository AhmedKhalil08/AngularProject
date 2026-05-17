using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Persistence;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.ProductImages.Queries.GetProductImageById
{
    public class GetProductImageByIdQueryHandler : IRequestHandler<GetProductImageByIdQuery, ProductImageDto>
    {
        private readonly IProductImageRepository _repository;

        public GetProductImageByIdQueryHandler(IProductImageRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProductImageDto> Handle(GetProductImageByIdQuery request, CancellationToken cancellationToken)
        {
            var image = await _repository.GetByIdAsync(request.Id);
            if (image == null) return null;

            return new ProductImageDto
            {
                Id = image.Id,
                ProductId = image.ProductId,
                ImageUrl = image.ImageUrl,
                IsMain = image.IsMain
            };
        }
    }
}
