using ECommerce.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.SellerProfiles.Queries.GetSellerProfileById
{
    public class GetSellerProfileByIdQuery : IRequest<SellerProfileDto>
    {
        public int Id { get; set; }
    }
}
