using ECommerce.Application.DTOs;
using MediatR;

namespace ECommerce.Application.Features.Addresses.Commands.CreateAddress
{
    public class CreateAddressCommand : IRequest<AddressDto>
    {
        //public string UserId { get; set; }
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
