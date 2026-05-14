using MediatR;

namespace ECommerce.Application.Features.Carts.Commands.SyncCart
{
    public class SyncCartCommand : IRequest<bool>
    {
        public List<CartItemSyncDto> Items { get; set; } = new();
    }
}
