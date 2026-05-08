using ECommerce.Application.Interfaces.Persistence;
using MediatR;
using System.Threading;
using System.Threading.Tasks;


namespace ECommerce.Application.Features.Banners.Commands.DeleteBanner
{
    public class DeleteBannerCommandHandler : IRequestHandler<DeleteBannerCommand, bool>
    {
        private readonly IBannerRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteBannerCommandHandler(IBannerRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteBannerCommand request, CancellationToken cancellationToken)
        {
            var item = await _repository.GetByIdAsync(request.Id);
            if (item == null) return false;

            await _repository.DeleteAsync(item.Id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
