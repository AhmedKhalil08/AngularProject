using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Persistence;
using ECommerce.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Addresses.Commands.CreateAddress
{
    public class CreateAddressCommandHandler : IRequestHandler<CreateAddressCommand, AddressDto>
    {
        private readonly IAddressRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateAddressCommandHandler(IAddressRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<AddressDto> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
        {
            var address = new Address
            {
                UserId = request.UserId,
                FullName = request.FullName,
                Street = request.Street,
                City = request.City,
                State = request.State,
                Country = request.Country,
                ZipCode = request.ZipCode,
                Phone = request.Phone,
                IsDefault = request.IsDefault
            };

            await _repository.AddAsync(address);
            await _unitOfWork.SaveChangesAsync();

            return new AddressDto
            {
                Id = address.Id,
                FullName = address.FullName,
                Street = address.Street,
                City = address.City,
                State = address.State,
                Country = address.Country,
                ZipCode = address.ZipCode,
                Phone = address.Phone,
                IsDefault = address.IsDefault
            };
        }
    }
}

