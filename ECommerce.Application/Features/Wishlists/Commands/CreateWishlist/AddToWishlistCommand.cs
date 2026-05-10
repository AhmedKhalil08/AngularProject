using ECommerce.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Wishlists.Commands.CreateWishlist
{
    public class AddToWishlistCommand : IRequest<WishlistDto>
    {
        //public string UserId { get; set; }
        public int ProductId { get; set; }
    }
}
