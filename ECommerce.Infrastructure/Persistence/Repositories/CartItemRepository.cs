using ECommerce.Application.Interfaces.Persistence;
using ECommerce.Domain.Entites;
using ECommerce.Infrastructure.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Infrastructure.Persistence.Repositories
{
    public class CartItemRepository : GenericRepository<CartItem, int>, ICartItemRepository
    {
        public CartItemRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
