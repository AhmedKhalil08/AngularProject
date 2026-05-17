using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Persistence;
using MediatR;
using Mapster;
using ECommerce.Application.Interfaces.Services; // 👈 1. لازم نعمل Import للـ Mapster

namespace ECommerce.Application.Features.Products.Queries.GetAllProducts
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, List<ProductDto>>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICurrentUserService _currentUserService;

        public GetAllProductsQueryHandler(IProductRepository productRepository, ICurrentUserService currentUserService)
        {
            _productRepository = productRepository;
            _currentUserService = currentUserService;
        }

        public async Task<List<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {


            var products = await _productRepository.GetAllAsync();

            if (_currentUserService.Role == "Admin")
            {
                return products.Adapt<List<ProductDto>>();
            }
            else
            {
                var userProducts = products.Where(p => p.IsDeleted == false).ToList();
                return userProducts.Adapt<List<ProductDto>>();
            }
        }
    }
}