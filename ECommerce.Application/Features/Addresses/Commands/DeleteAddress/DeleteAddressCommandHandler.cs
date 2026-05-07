using ECommerce.Application.Interfaces.Persistence;
using MediatR;

namespace ECommerce.Application.Features.Addresses.Commands.DeleteAddress
{
    public class DeleteAddressCommandHandler : IRequestHandler<DeleteAddressCommand, bool>
    {

        private readonly IAddressRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteAddressCommandHandler(IAddressRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
        {
            var address = await _repository.GetByIdAsync(request.Id);

            address.IsDeleted = true;

            await _repository.UpdateAsync(address);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}

