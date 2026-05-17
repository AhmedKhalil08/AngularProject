using ECommerce.Application.DTOs;
using MediatR;

namespace ECommerce.Application.Features.Reviews.Commands.UpdateReview
{
    public class UpdateReviewCommand : IRequest<ReviewDto>
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
    }
}
