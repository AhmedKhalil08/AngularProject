using ECommerce.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.SellerProfiles.Commands.UpdateSellerProfile
{
    public class UpdateSellerProfileCommand : IRequest<SellerProfileDto>
    {
        public int Id { get; set; }
        public string StoreName { get; set; }
        public string? StoreDescription { get; set; }
        public string? LogoUrl { get; set; }
        public IFormFile? Logo { get; set; }
    }
}
