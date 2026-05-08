using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Application.DTOs;
using MediatR;

namespace ECommerce.Application.Features.Banners.Queries.GetAllBanners
{
    public class GetAllBannersQuery: IRequest<List<BannerDto>>
    {
    }
}
