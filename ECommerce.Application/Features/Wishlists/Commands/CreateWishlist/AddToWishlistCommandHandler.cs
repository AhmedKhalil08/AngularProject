using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Persistence;
using MediatR;
using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Wishlists.Commands.CreateWishlist
{
    public class AddToWishlistCommandHandler : IRequestHandler<AddToWishlistCommand, WishlistDto>
    {
        private readonly IWishlistRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public AddToWishlistCommandHandler(IWishlistRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<WishlistDto> Handle(AddToWishlistCommand request, CancellationToken cancellationToken)
        {
            var wishlist = new Wishlist
            {
                UserId = request.UserId,
                ProductId = request.ProductId,
                AddedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(wishlist);
            await _unitOfWork.SaveChangesAsync();

            return new WishlistDto
            {
                Id = wishlist.Id,
                AddedAt = wishlist.AddedAt
            };
        }
    }
}
