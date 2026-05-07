using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Persistence;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.ProductImages.Queries.GetAllProductImages
{
    public class GetAllProductImagesQueryHandler : IRequestHandler<GetAllProductImagesQuery, List<ProductImageDto>>
    {
        private readonly IProductImageRepository _repository;

        public GetAllProductImagesQueryHandler(IProductImageRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ProductImageDto>> Handle(GetAllProductImagesQuery request, CancellationToken cancellationToken)
        {
            var images = await _repository.GetAllAsync();
            return images.Select(i => new ProductImageDto
            {
                Id = i.Id,
                ProductId = i.ProductId,
                ImageUrl = i.ImageUrl,
                IsMain = i.IsMain
            }).ToList();
        }
    }
}
