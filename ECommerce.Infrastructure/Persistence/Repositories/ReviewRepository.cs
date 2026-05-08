using ECommerce.Application.Interfaces.Persistence;
using ECommerce.Domain.Entites;
using ECommerce.Infrastructure.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Infrastructure.Persistence.Repositories
{
    public class ReviewRepository : GenericRepository<Review, int>, IReviewRepository
    {
        public ReviewRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
