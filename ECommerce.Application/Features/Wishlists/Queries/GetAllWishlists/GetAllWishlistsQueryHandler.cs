using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Wishlists.Queries.GetAllWishlists
{
    internal class GetAllWishlistsQueryHandler
    {
        private readonly IWishlistRepository _repository;

        public GetAllWishlistsQueryHandler(IWishlistRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<WishlistDto>> Handle(GetAllWishlistsQuery request, CancellationToken cancellationToken)
        {
            var wishlists = await _repository.GetAllAsync();

            return wishlists
                .Where(w => w.UserId == request.UserId && !w.IsDeleted)
                .Select(w => new WishlistDto
                {
                    Id = w.Id,
                    ProductName = w.Product?.Name,
                    ProductPrice = w.Product?.Price ?? 0,
                    ProductImageUrl = w.Product?.Images?.FirstOrDefault()?.ImageUrl,
                    AddedAt = w.AddedAt
                }).ToList();
        }
    }
}
