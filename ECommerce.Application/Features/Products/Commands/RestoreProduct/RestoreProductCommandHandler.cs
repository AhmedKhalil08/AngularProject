using ECommerce.Application.Exceptions;
using ECommerce.Application.Interfaces.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Products.Commands.RestoreProduct
{
    public class RestoreProductCommandHandler : IRequestHandler<RestoreProductCommand, bool>
    {
        private readonly IProductRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public RestoreProductCommandHandler(IProductRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(RestoreProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _repository.GetByIdAsync(request.Id);
            if (product == null) throw new NotFoundException("Product not found");
            product.IsDeleted = false;
            await _repository.UpdateAsync(product);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
