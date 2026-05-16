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
    private readonly IUserRepository _userManager;
    public GetOrderDetailsQueryHandler(IOrderRepository orderRepository, ICurrentUserService currentUserService, IUserRepository userManager )
    {
        _orderRepository = orderRepository;
        _currentUserService = currentUserService;
        _userManager = userManager;
    }

    public async Task<OrderDetailsDto> Handle(GetOrderDetailsQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;

        if(_currentUserService.Role == "Admin")
        {
            var orders = await _orderRepository.GetByConditionAsync(
                o => o.Id == request.OrderId,
                includeProperties: "Shipments,Shipments.OrderItems,Shipments.OrderItems.Product,Shipments.Seller,Shipments.Seller.User"
            );
            var order = orders.FirstOrDefault();

            if (order == null)
                throw new Exception("Order not found.");

            var orderDto = order.Adapt<OrderDetailsDto>();
            foreach (var shipmentDto in orderDto.Shipments)
            {
                var user = await _userManager.GetByIdAsync(shipmentDto.SellerId);
                if (user != null)
                {
                    shipmentDto.SellerName = user.FullName; 
                }
            }
            return orderDto;
        }
        else
        {
            var orders = await _orderRepository.GetByConditionAsync(
                o => o.Id == request.OrderId && o.UserId == userId,
                includeProperties: "Shipments,Shipments.OrderItems,Shipments.OrderItems.Product"
            );
            var order = orders.FirstOrDefault();

            if (order == null)
                throw new Exception("Order not found.");

            var orderDto = order.Adapt<OrderDetailsDto>();

            return orderDto;
        }

       
    }
}