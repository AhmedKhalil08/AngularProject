using ECommerce.Application.DTOs;
using MediatR;

namespace ECommerce.Application.Features.Reviews.Commands.CreateReview
{
    public class CreateReviewCommand : IRequest<ReviewDto>
    {
        public int ProductId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
    }
}
