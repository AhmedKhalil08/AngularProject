using ECommerce.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Addresses.Queries.GetAllAddresses
{
    public class GetAllAddressesQuery : IRequest<List<AddressDto>>
    {
        //public string UserId { get; set; }
    }
}
