using ECommerce.Application.DTOs;
using ECommerce.Application.Exceptions;
using ECommerce.Application.Interfaces.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.ContactMessages.Queries.GetMessageById
{
    public class GetContactMessageByIdQueryHandler : IRequestHandler<GetContactMessageByIdQuery, ContactMessageDto>
    {
        private readonly IContactMessageRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public GetContactMessageByIdQueryHandler(IContactMessageRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ContactMessageDto> Handle(GetContactMessageByIdQuery request, CancellationToken cancellationToken)
        {
            var message = await _repository.GetByIdAsync(request.Id);
            if (message == null) throw new NotFoundException("Message Not Found");

            message.IsRead = true;
            await _repository.UpdateAsync(message);
            await _unitOfWork.SaveChangesAsync();

            return new ContactMessageDto
            {
                Id = message.Id,
                Name = message.Name,
                Email = message.Email,
                Subject = message.Subject,
                Message = message.Message,
                SentAt = message.SentAt,
                IsRead = true
            };
        }
    }
}
