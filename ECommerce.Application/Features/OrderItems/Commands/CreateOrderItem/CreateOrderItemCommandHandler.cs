using ECommerce.Application.DTOs;
using ECommerce.Application.Features.CartItems.Commands.CreateCartItem;
using ECommerce.Application.Interfaces.Persistence;
using ECommerce.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.OrderItems.Commands.CreateOrderItem
{
    public class CreateOrderItemCommandHandler: IRequestHandler<CreateOrderItemCommand, OrderItemDto>
    {
        private readonly IOrderItemRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductRepository _productrepo;

        public CreateOrderItemCommandHandler(IOrderItemRepository repository, IUnitOfWork unitOfWork , IProductRepository productrepo)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _productrepo= productrepo;
        }

        public async Task<OrderItemDto> Handle(CreateOrderItemCommand request, CancellationToken cancellationToken)
        {
            var item = new OrderItem
            {
                OrderId = request.OrderId,
                ProductId = request.ProductId,
                Product= new Product { Id = request.ProductId },
                Quantity = request.Quantity
            };
            var product = await _productrepo.GetByIdAsync(request.ProductId);
            await _repository.AddAsync(item);
            await _unitOfWork.SaveChangesAsync();
            return new OrderItemDto
            {
                //Id = item.Id,
                ProductId = item.ProductId,
                Quantity = item.Quantity
            };
        }
    }
}

