using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Addresses.Queries.GetAllAddresses
{
    public class GetAllAddressesQueryHandler : IRequestHandler<GetAllAddressesQuery, List<AddressDto>>
    {
        private readonly IAddressRepository _repository;

        public GetAllAddressesQueryHandler(IAddressRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<AddressDto>> Handle(GetAllAddressesQuery request, CancellationToken cancellationToken)
        {
            var addresses = await _repository.GetAllAsync();
            return addresses
                .Where(a => a.UserId == request.UserId && !a.IsDeleted)
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
