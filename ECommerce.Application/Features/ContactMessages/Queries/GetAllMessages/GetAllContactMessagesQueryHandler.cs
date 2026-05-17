using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.ContactMessages.Queries.GetAllMessages
{
    public class GetAllContactMessagesQueryHandler : IRequestHandler<GetAllContactMessagesQuery, List<ContactMessageDto>>
    {
        private readonly IContactMessageRepository _repository;

        public GetAllContactMessagesQueryHandler(IContactMessageRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ContactMessageDto>> Handle(GetAllContactMessagesQuery request, CancellationToken cancellationToken)
        {
            var messages = await _repository.GetAllOrderedAsync();

            return messages.Select(m => new ContactMessageDto
            {
                Id = m.Id,
                Name = m.Name,
                Email = m.Email,
                Subject = m.Subject,
                Message = m.Message,
                SentAt = m.SentAt,
                IsRead = m.IsRead
            }).ToList();
        }
    }
}
