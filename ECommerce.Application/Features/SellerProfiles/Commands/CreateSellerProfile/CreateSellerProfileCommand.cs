using ECommerce.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.SellerProfiles.Commands.CreateSellerProfile
{
    public class CreateSellerProfileCommand : IRequest<SellerProfileDto>
    {
        public string UserId { get; set; }
        public string StoreName { get; set; }
        public string? StoreDescription { get; set; }
        public string? LogoUrl { get; set; }
    }
}
