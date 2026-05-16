using ECommerce.Application.Interfaces.Persistence;
using ECommerce.Application.Interfaces.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Wishlists.Commands.ClearWishList
{
    public class ClearWishlistCommandHandler : IRequestHandler<ClearWishlistCommand, bool>
    {
        private readonly IWishlistRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public ClearWishlistCommandHandler(IWishlistRepository repository, IUnitOfWork unitOfWork, ICurrentUserService currentUser)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<bool> Handle(ClearWishlistCommand request, CancellationToken cancellationToken)
        {
            var items = await _repository.GetAllAsync();
            var userItems = items.Where(w => w.UserId == _currentUser.UserId && !w.IsDeleted).ToList();

            foreach (var item in userItems)
            {
                item.IsDeleted = true;
                await _repository.UpdateAsync(item);
            }

            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
