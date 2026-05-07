using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Addresses.Commands.DeleteAddress
{
    public class DeleteAddressCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
