using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Carts.Queries
{
    public class GetCartByUserIdQueryHandler : IRequestHandler<GetCartByUserIdQuery, CartDto>
    {
        private readonly ICartRepository _repository;

        public GetCartByUserIdQueryHandler(ICartRepository repository)
        {
            _repository = repository;
        }

        public async Task<CartDto> Handle(GetCartByUserIdQuery request, CancellationToken cancellationToken)
        {
            var carts = await _repository.GetAllAsync();
            var cart = carts.FirstOrDefault(c => c.UserId == request.UserId && !c.IsDeleted);

            if (cart == null)
                return null;

            return new CartDto
            {
                Id = cart.Id,
                Items = cart.CartItems?.Select(i => new CartItemDto
                {
                    Id = i.Id,
                    CartId = i.CartId,
                    ProductId = i.ProductId,
                    Quantity = i.Quantity
                }).ToList() ?? new List<CartItemDto>(),
                TotalPrice = cart.CartItems?.Sum(i => i.Quantity * i.Product?.Price ?? 0) ?? 0
            };
        }
    }
}
