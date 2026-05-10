using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Persistence;
using ECommerce.Application.Interfaces.Services;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Features.Products.Queries.GetMyProducts
{
    public class GetMyProductsQueryHandler : IRequestHandler<GetMyProductsQuery, List<ProductDto>>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICurrentUserService _currentUserService;

        public GetMyProductsQueryHandler(IProductRepository productRepository, ICurrentUserService currentUserService)
        {
            _productRepository = productRepository;

            _currentUserService = currentUserService;
        }

        public async Task<List<ProductDto>> Handle(GetMyProductsQuery request, CancellationToken cancellationToken)
        {
            var sellerId = _currentUserService.UserId;
 
            var products = await _productRepository 
                .Table
                .Where(p => p.SellerId == sellerId) 
                .ProjectToType<ProductDto>() 
                .ToListAsync(cancellationToken);

            return products;
        }
    }
}