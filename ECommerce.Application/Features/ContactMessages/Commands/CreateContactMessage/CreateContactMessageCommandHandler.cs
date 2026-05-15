using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Persistence;
using ECommerce.Application.Interfaces.Services;
using ECommerce.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.ContactMessages.Commands.CreateContactMessage
{
    public class CreateContactMessageCommandHandler : IRequestHandler<CreateContactMessageCommand, ContactMessageDto>
    {
        private readonly IContactMessageRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationService _notificationService;
        public CreateContactMessageCommandHandler(
            IContactMessageRepository repository,
            IUnitOfWork unitOfWork,
            INotificationService notificationService)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
        }

        public async Task<ContactMessageDto> Handle(CreateContactMessageCommand request, CancellationToken cancellationToken)
        {
            var message = new ContactMessage
            {
                Name = request.Name,
                Email = request.Email,
                Subject = request.Subject,
                Message = request.Message,
                SentAt = DateTime.UtcNow,
                IsRead = false
            };

            await _repository.AddAsync(message);
            await _unitOfWork.SaveChangesAsync();

            await _notificationService.SendNewMessageNotification(
                message.Id, message.Name, message.Subject, message.SentAt);

            return new ContactMessageDto
            {
                Id = message.Id,
                Name = message.Name,
                Email = message.Email,
                Subject = message.Subject,
                Message = message.Message,
                SentAt = message.SentAt,
                IsRead = message.IsRead
            };
        }
    }
}

