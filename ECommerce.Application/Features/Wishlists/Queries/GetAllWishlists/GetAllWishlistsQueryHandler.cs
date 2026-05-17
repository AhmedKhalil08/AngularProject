using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Persistence;
using ECommerce.Application.Interfaces.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Wishlists.Queries.GetAllWishlists
{
    public  class GetAllWishlistsQueryHandler : IRequestHandler<GetAllWishlistsQuery,List<WishlistDto>>
    {
        private readonly IWishlistRepository _repository;
        private readonly ICurrentUserService _currentUser;

        public GetAllWishlistsQueryHandler(IWishlistRepository repository, ICurrentUserService currentUser)
        {
            _repository = repository;
            _currentUser= currentUser;
        }

        public async Task<List<WishlistDto>> Handle(GetAllWishlistsQuery request, CancellationToken cancellationToken)
        {
            var wishlists = await _repository.GetAllAsync();

            return wishlists
                .Where(w => w.UserId == _currentUser.UserId && !w.IsDeleted)
                .Select(w => new WishlistDto
                {
                    Id = w.Id,
                     ProductId = w.ProductId,
                    ProductName = w.Product?.Name,
                    ProductPrice = w.Product?.Price ?? 0,
                    ProductImageUrl = w.Product?.Images?.FirstOrDefault()?.ImageUrl,
                    AddedAt = w.AddedAt
                }).ToList();
        }
    }
}
