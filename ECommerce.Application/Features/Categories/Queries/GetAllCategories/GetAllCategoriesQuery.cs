using ECommerce.Application.DTOs;
using MediatR;
using System.Collections.Generic;

namespace ECommerce.Application.Features.Categories.Queries.GetAllCategories
{
    public class GetAllCategoriesQuery : IRequest<List<CategoryDto>>
    {
    }
}

