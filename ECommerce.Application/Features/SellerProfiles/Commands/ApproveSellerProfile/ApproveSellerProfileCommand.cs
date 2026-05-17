using ECommerce.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.SellerProfiles.Commands.ApproveSellerProfile
{
    public class ApproveSellerProfileCommand : IRequest<SellerProfileDto>
    {
        public int Id { get; set; }
        public bool IsApproved { get; set; }
    }
}
