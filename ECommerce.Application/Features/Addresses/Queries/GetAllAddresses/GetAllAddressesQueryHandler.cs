using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Persistence;
using ECommerce.Application.Interfaces.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Addresses.Queries.GetAllAddresses
{
    public class GetAllAddressesQueryHandler : IRequestHandler<GetAllAddressesQuery, List<AddressDto>>
    {
        private readonly IAddressRepository _repository;
        private readonly ICurrentUserService _currentUser;
        public GetAllAddressesQueryHandler(IAddressRepository repository , ICurrentUserService currentUser)
        {
            _repository = repository;
            _currentUser= currentUser;
        }

        public async Task<List<AddressDto>> Handle(GetAllAddressesQuery request, CancellationToken cancellationToken)
        {
            var addresses = await _repository.GetAllAsync();
            return addresses
                .Where(a => a.UserId == _currentUser.UserId && !a.IsDeleted)
                .Select(a => new AddressDto
                {
                    Id = a.Id,
                    FullName = a.FullName,
                    Street = a.Street,
                    City = a.City,
                    State = a.State,
                    Country = a.Country,
                    ZipCode = a.ZipCode,
                    Phone = a.Phone,
                    IsDefault = a.IsDefault
                }).ToList();
        }

    }
}
