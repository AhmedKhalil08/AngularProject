using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.DTOs.Auth
{
    public class RegisterSellerDto
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string StoreName { get; set; }
        public string? StoreDescription { get; set; }
    }
}
