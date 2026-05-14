using ECommerce.Application.DTOs;
using ECommerce.Application.Features.Banners.Commands.UpdateBanner;
using ECommerce.Application.Interfaces.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.OrderItems.Commands.UpdateOrderItem
{
    public class UpdateOrderItemHandler: IRequestHandler<UpdateOrderItemCommand, OrderItemDto>
    {
        private readonly IOrderItemRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateOrderItemHandler(IOrderItemRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<OrderItemDto> Handle(UpdateOrderItemCommand request, CancellationToken cancellationToken)
        {
            var item = await _repository.GetByIdAsync(request.Id);
            if (item == null) return null;
            item.Quantity = request.Quantity;
           
            await _repository.UpdateAsync(item);
            await _unitOfWork.SaveChangesAsync();
            return new OrderItemDto
            {
                //Id = item.Id,
                Quantity = item.Quantity,

            };
        }
    }
}
