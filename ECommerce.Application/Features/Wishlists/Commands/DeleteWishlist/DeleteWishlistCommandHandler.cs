using ECommerce.Application.Exceptions;
using ECommerce.Application.Interfaces.Persistence;
using ECommerce.Application.Interfaces.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Wishlists.Commands.DeleteWishlist
{
    public class DeleteWishlistCommandHandler : IRequestHandler<DeleteWishlistCommand, bool>
    {
        private readonly IWishlistRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public DeleteWishlistCommandHandler(IWishlistRepository repository, IUnitOfWork unitOfWork, ICurrentUserService currentUser)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<bool> Handle(DeleteWishlistCommand request, CancellationToken cancellationToken)
        {
            var wishlist = await _repository.GetByIdAsync(request.Id);
            if (wishlist == null) throw new NotFoundException("WishListNotFound");
            if (wishlist.UserId != _currentUser.UserId)
            {
                throw new ForbiddenAccessException("This is NOT YOUR WISHLIST !!!!");
            }
            wishlist.IsDeleted = true;

            await _repository.UpdateAsync(wishlist);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
