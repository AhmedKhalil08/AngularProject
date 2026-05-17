using MediatR;

namespace ECommerce.Application.Features.Reviews.Commands.DeleteReview
{
    public class DeleteReviewCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
