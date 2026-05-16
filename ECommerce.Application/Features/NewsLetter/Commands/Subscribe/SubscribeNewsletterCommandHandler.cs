using ECommerce.Application.Interfaces.Persistence;
using ECommerce.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.NewsLetter.Commands.Subscribe
{
    public class SubscribeNewsletterCommandHandler : IRequestHandler<SubscribeNewsletterCommand, bool>
    {
        private readonly INewsletterRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public SubscribeNewsletterCommandHandler(INewsletterRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(SubscribeNewsletterCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repository.GetByEmailAsync(request.Email);
            if (existing != null)
            {
                if (!existing.IsActive)
                {
                    existing.IsActive = true;
                    await _repository.UpdateAsync(existing);
                    await _unitOfWork.SaveChangesAsync();
                }
                return true;
            }

            var subscriber = new NewsletterSubscriber
            {
                Email = request.Email,
                SubscribedAt = DateTime.UtcNow,
                IsActive = true
            };

            await _repository.AddAsync(subscriber);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
