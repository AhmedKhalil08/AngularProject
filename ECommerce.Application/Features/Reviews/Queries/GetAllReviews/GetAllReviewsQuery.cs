using ECommerce.Application.DTOs;
using MediatR;
using System.Collections.Generic;

namespace ECommerce.Application.Features.Reviews.Queries.GetAllReviews
{
    public class GetAllReviewsQuery : IRequest<List<ReviewDto>>
    {
    }
}
