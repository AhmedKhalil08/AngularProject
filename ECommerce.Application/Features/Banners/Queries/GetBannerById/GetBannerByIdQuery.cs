using ECommerce.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Banners.Queries.GetBannerById
{
    public class GetBannerByIdQuery: IRequest<BannerDto>
    {
        public int Id { get; set; }
    }
}
