using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Interfaces.Persistence
{
    public interface IContactMessageRepository : IGenericRepository<ContactMessage, int>
    {
        Task<List<ContactMessage>> GetAllOrderedAsync();
    }
}
