using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.DTOs
{
    public class BannerDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public string? Link { get; set; }

        public bool IsActive { get; set; }

        public int DisplayOrder { get; set; }
    }
}
