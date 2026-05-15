using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Persistence;
using ECommerce.Application.Interfaces.Services;
using Mapster;
using MediatR;

namespace ECommerce.Application.Features.Orders.Queries.GetSellerShipments
{
    public class GetSellerShipmentsQueryHandler : IRequestHandler<GetSellerShipmentsQuery, List<SellerShipmentDto>>
    {
        private readonly IShipmentRepository _shipmentRepository;
        private readonly ICurrentUserService _currentUserService;

        public GetSellerShipmentsQueryHandler(IShipmentRepository shipmentRepository, ICurrentUserService currentUserService)
        {
            _shipmentRepository = shipmentRepository;
            _currentUserService = currentUserService;
        }

        public async Task<List<SellerShipmentDto>> Handle(GetSellerShipmentsQuery request, CancellationToken cancellationToken)
        {
            // السيلر الحالي اللي عامل Login
            var sellerId = _currentUserService.UserId;

            // بنجيب الشحنات الخاصة بيه هو بس، ونجيب معاها الأوردر الأصلي والمنتجات
            var shipments = await _shipmentRepository.GetByConditionAsync(
                s => s.SellerId == sellerId,
                includeProperties: "Order,OrderItems,OrderItems.Product"
            );

            // Mapster هيقوم بالواجب ويحولها لـ DTO
            var shipmentDtos = shipments.Adapt<List<SellerShipmentDto>>();

            return shipmentDtos;
        }
    }
}
