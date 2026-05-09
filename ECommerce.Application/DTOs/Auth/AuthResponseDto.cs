using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.DTOs.Auth
{
    public class AuthResponseDto
    {
        public string Token { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public DateTime Expiration { get; set; }
    }
}
