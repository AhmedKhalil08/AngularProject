using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Addresses.Commands.UpdateAddress
{
    public class UpdateAddressCommandHandler : IRequestHandler<UpdateAddressCommand, AddressDto>
    {
        private readonly IAddressRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateAddressCommandHandler(IAddressRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<AddressDto> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
        {
            var address = await _repository.GetByIdAsync(request.Id);

            address.FullName = request.FullName;
            address.Street = request.Street;
            address.City = request.City;
            address.State = request.State;
            address.Country = request.Country;
            address.ZipCode = request.ZipCode;
            address.Phone = request.Phone;
            address.IsDefault = request.IsDefault;

            await _repository.UpdateAsync(address);
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
