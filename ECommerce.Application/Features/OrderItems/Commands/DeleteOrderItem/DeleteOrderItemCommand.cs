using MediatR;

namespace ECommerce.Application.Features.OrderItems.Commands.DeleteOrderItem
{
    public class DeleteOrderItemCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
