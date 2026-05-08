using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Application.DTOs;



namespace ECommerce.Application.Features.Banners.Commands.DeleteBanner
{
    public class DeleteBannerCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
