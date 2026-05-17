using ECommerce.Application.DTOs;
using MediatR;

namespace ECommerce.Application.Features.Reviews.Queries.GetReviewById
{
    public class GetReviewByIdQuery : IRequest<ReviewDto>
    {
        public int Id { get; set; }
    }
}
