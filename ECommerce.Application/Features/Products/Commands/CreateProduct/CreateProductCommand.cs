using ECommerce.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace ECommerce.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommand : IRequest<ProductDto>
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public List<IFormFile> Images { get; set; } = null;
    }
}
