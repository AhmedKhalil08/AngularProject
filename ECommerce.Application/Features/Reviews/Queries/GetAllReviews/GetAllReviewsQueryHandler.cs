using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Persistence;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Reviews.Queries.GetAllReviews
{
    public class GetAllReviewsQueryHandler : IRequestHandler<GetAllReviewsQuery, List<ReviewDto>>
    {
        private readonly IReviewRepository _repository;

        public GetAllReviewsQueryHandler(IReviewRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ReviewDto>> Handle(GetAllReviewsQuery request, CancellationToken cancellationToken)
        {
            var reviews = await _repository.GetAllAsync();
            return reviews.Select(r => new ReviewDto
            {
                Id = r.Id,
                ProductId = r.ProductId,
                UserId = r.UserId,
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt
            }).ToList();
        }
    }
}
