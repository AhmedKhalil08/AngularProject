using ECommerce.Application.DTOs;
using MediatR;

namespace ECommerce.Application.Features.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommand : IRequest<CategoryDto>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public int? ParentCategoryId { get; set; }
    }
}

