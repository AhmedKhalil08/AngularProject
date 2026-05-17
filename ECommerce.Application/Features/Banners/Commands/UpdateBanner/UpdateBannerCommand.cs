using ECommerce.Application.DTOs;
using MediatR;

namespace ECommerce.Application.Features.Banners.Commands.UpdateBanner
{
    public class UpdateBannerCommand : IRequest<BannerDto>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string? Link { get; set; }
        public bool IsActive { get; set; }
        public int DisplayOrder { get; set; }
    }
}
