using ECommerce.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.SellerProfiles.Queries.GetAllSellerProfiles
{
    public class GetAllSellerProfilesQuery : IRequest<PagedResult<SellerProfileDto>>
    {
        public string? Search { get; set; }
        public string? Status { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 9;
    }
}
