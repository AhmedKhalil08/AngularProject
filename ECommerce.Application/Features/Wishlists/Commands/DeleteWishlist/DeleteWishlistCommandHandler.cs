using ECommerce.Application.Interfaces.Persistence;
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

        public DeleteWishlistCommandHandler(IWishlistRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteWishlistCommand request, CancellationToken cancellationToken)
        {
            var wishlist = await _repository.GetByIdAsync(request.Id);

            wishlist.IsDeleted = true;

            await _repository.UpdateAsync(wishlist);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
