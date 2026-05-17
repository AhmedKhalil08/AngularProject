using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.SellerProfiles.Commands.DeleteSellerProfileByAdmin
{
    public class DeleteSellerProfileByAdminCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
