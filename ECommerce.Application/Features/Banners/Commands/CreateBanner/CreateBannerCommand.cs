using ECommerce.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Banners.Commands.CreateBanner
{
    public class CreateBannerCommand: IRequest<BannerDto>
    {
        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public string? Link { get; set; }

        public int DisplayOrder { get; set; }
    }
}
