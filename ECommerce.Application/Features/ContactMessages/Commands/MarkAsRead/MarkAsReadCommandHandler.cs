using ECommerce.Application.Exceptions;
using ECommerce.Application.Interfaces.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.ContactMessages.Commands.MarkAsRead
{
    public class MarkAsReadCommandHandler : IRequestHandler<MarkAsReadCommand, bool>
    {
        private readonly IContactMessageRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public MarkAsReadCommandHandler(IContactMessageRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(MarkAsReadCommand request, CancellationToken cancellationToken)
        {
            var message = await _repository.GetByIdAsync(request.Id);
            if (message == null) throw new NotFoundException("Message Not Found");
            message.IsRead = true;
            await _repository.UpdateAsync(message);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
