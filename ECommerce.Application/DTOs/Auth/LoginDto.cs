using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.DTOs.Auth
{
    public class LoginDto
    {
        public string EmailOrUserName { get; set; }
        public string Password { get; set; }
    }
}
