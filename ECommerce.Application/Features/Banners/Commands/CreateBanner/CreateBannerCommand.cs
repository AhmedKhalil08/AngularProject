using ECommerce.Application.DTOs;
using MediatR;

namespace ECommerce.Application.Features.Banners.Commands.CreateBanner
{
    public class CreateBannerCommand : IRequest<BannerDto>
    {
        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public string? Link { get; set; }

        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
    }
}
