using ECommerce.Application.DTOs;
using ECommerce.Application.Exceptions;
using ECommerce.Application.Interfaces.Persistence;
using ECommerce.Application.Interfaces.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Addresses.Queries.GetAddressById
{
    public class GetAddressByIdQueryHandler : IRequestHandler<GetAddressByIdQuery, AddressDto>
    {
        private readonly IAddressRepository _repository;
        private readonly ICurrentUserService _currentUser;

        public GetAddressByIdQueryHandler(IAddressRepository repository, ICurrentUserService currentUser)
        {
            _repository = repository;
            _currentUser= currentUser;
        }

        public async Task<AddressDto> Handle(GetAddressByIdQuery request, CancellationToken cancellationToken)
        {
            var address = await _repository.GetByIdAsync(request.Id);
            if (address == null)
                throw new NotFoundException("Address not found");

            if (address.UserId != _currentUser.UserId)
                throw new ForbiddenAccessException("This is not your address");

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
