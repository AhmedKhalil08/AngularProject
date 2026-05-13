using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Persistence;
using MediatR;
using Mapster; // 👈 1. لازم نعمل Import للـ Mapster

namespace ECommerce.Application.Features.Products.Queries.GetAllProducts
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, List<ProductDto>>
    {
        private readonly IProductRepository _productRepository;

        public GetAllProductsQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
         

            var products = await _productRepository.GetAllAsync();

            return products.Adapt<List<ProductDto>>();
        }
    }
}