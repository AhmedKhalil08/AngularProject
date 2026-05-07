using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Interfaces.Persistence
{
    public interface ISellerProfileRepository : IGenericRepository<SellerProfile, int>
    {
    }
}
