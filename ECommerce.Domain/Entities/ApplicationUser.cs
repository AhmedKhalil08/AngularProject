using ECommerce.Domain.Entites;
using ECommerce.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ECommerce.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        //-------------------------------------------------------------------------
        //                             SELF PROPS
        //-------------------------------------------------------------------------
        public string FullName { get; set; }
        public string? ProfileImageUrl { get; set; }
        public UserRole Role { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }

        /*-------------------------------------------------------------------------*/
        //                          Navigation Props 
        /*-------------------------------------------------------------------------*/
        public Cart? Cart { get; set; }
        public SellerProfile? SellerProfile { get; set; }
        public ICollection<Address> Addresses { get; set; } = new List<Address>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
