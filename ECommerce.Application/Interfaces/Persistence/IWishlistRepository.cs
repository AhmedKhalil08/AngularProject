using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Interfaces.Persistence
{
    public interface IWishlistRepository : IGenericRepository<Wishlist, int>
    {
        Task ClearAllAsync(string userId);
    }
}
