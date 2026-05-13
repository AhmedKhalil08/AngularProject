using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.DTOs.Auth
{
    public class CreateAdminDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
