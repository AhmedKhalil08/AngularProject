using ECommerce.Application.Exceptions;
using ECommerce.Application.Interfaces.Persistence;
using ECommerce.Application.Interfaces.Services;
using MediatR;

namespace ECommerce.Application.Features.Addresses.Commands.DeleteAddress
{
    public class DeleteAddressCommandHandler : IRequestHandler<DeleteAddressCommand, bool>
    {

        private readonly IAddressRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public DeleteAddressCommandHandler(IAddressRepository repository, IUnitOfWork unitOfWork, ICurrentUserService currentUser)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<bool> Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
        {
            var address = await _repository.GetByIdAsync(request.Id);
            if (address == null)
                throw new NotFoundException("Address not found");

            if (address.UserId != _currentUser.UserId)
                throw new ForbiddenAccessException("This is not your address");

            address.IsDeleted = true;

            await _repository.UpdateAsync(address);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}

