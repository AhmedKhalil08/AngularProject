using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.SellerProfiles.Commands.DeleteSellerProfile
{
    public class DeleteSellerProfileCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
