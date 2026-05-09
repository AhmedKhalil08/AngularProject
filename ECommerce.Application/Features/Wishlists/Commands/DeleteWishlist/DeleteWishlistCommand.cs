using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Wishlists.Commands.DeleteWishlist
{
    public class DeleteWishlistCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
