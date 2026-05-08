using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace ECommerce.Application.Features.Banners.Queries.GetAllBanners
{
    public class GetAllBannersQuery: IRequest<List<BannerDto>>
    {
    }
}
