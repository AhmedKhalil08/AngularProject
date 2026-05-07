using ECommerce.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Addresses.Commands.UpdateAddress
{
    public class UpdateAddressCommand : IRequest<AddressDto>
    {

        public int Id { get; set; }
        public string FullName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string Phone { get; set; }
        public bool IsDefault { get; set; }
    }
}
