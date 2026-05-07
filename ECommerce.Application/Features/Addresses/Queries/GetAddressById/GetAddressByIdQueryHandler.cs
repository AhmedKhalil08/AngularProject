using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Addresses.Queries.GetAddressById
{
    public class GetAddressByIdQueryHandler : IRequestHandler<GetAddressByIdQuery, AddressDto>
    {
        private readonly IAddressRepository _repository;

        public GetAddressByIdQueryHandler(IAddressRepository repository)
        {
            _repository = repository;
        }

        public async Task<AddressDto> Handle(GetAddressByIdQuery request, CancellationToken cancellationToken)
        {
            var address = await _repository.GetByIdAsync(request.Id);

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
