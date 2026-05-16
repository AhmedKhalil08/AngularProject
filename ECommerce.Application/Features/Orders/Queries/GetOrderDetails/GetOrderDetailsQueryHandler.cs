using ECommerce.Application.DTOs;
using ECommerce.Application.Features.Orders.Queries.GetOrderDetails;
using ECommerce.Application.Interfaces.Persistence;
using ECommerce.Application.Interfaces.Services;
using Mapster;
using MediatR;

public class GetOrderDetailsQueryHandler : IRequestHandler<GetOrderDetailsQuery, OrderDetailsDto>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetOrderDetailsQueryHandler(IOrderRepository orderRepository, ICurrentUserService currentUserService)
    {
        _orderRepository = orderRepository;
        _currentUserService = currentUserService;
    }

    public async Task<OrderDetailsDto> Handle(GetOrderDetailsQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;

        // بنجيب الأوردر ونجيب معاه الشحنات، والمنتجات اللي جوه الشحنات
        var orders = await _orderRepository.GetByConditionAsync(
            o => o.Id == request.OrderId && o.UserId == userId,
            includeProperties: "Shipments,Shipments.OrderItems,Shipments.OrderItems.Product"
        );

        var order = orders.FirstOrDefault();

        if (order == null)
            throw new Exception("Order not found.");

        // بنحول الـ Entity لـ DTO (ممكن تستخدم Mapster هنا بـ Adapt)
        var orderDto = order.Adapt<OrderDetailsDto>();

        return orderDto;
    }
}