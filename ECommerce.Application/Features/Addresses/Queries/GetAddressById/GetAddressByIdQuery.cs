using ECommerce.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Addresses.Queries.GetAddressById
{
    public class GetAddressByIdQuery : IRequest<AddressDto>
    {
        public int Id { get; set; }
    }
}
