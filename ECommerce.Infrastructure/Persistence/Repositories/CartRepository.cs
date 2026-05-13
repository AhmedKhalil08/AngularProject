using ECommerce.Application.Interfaces.Persistence;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Infrastructure.Persistence.Repositories
{
    internal class CartRepository : GenericRepository<Cart,int>,ICartRepository
    {
        public CartRepository(ApplicationDbContext context) : base(context)
        {
            
        }
    }
}
